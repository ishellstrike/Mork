using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mork.Graphics.MapEngine;
using Mork.Local_Map.Dynamic.Local_Items;

namespace Mork.Local_Map.Sector {
    public class IntersectMap {
        public BoundingBox[] N = new BoundingBox[SectorMap.Sectn*SectorMap.Sectn*MapSector.dimS*MapSector.dimS];

        public IntersectMap() {
            for (int i = 0; i < N.Length; i++) {
                N[i] =
                    new BoundingBox(
                        new Vector3(i/(SectorMap.Sectn*MapSector.dimS), i%(SectorMap.Sectn*MapSector.dimS), 0),
                        new Vector3(i/(SectorMap.Sectn*MapSector.dimS) + 1, i%(SectorMap.Sectn*MapSector.dimS) + 1, -1));
            }
        }

        public void MoveIntersectMap(Vector3 mover) {
            for (int index = 0; index < N.Length; index++) {
                N[index].Max -= mover;
                N[index].Min -= mover;
            }
        }
    }

    [Serializable]
    public class SectorMap {
        public const int Sectn = 8;
        private readonly BasicEffect _basice;
        private readonly BasicEffect _basice2;
        private readonly Effect _be;
        private readonly GraphicsDevice _gd;
        private readonly Random rnd = new Random();
        private readonly List<VertexPositionColor> vl = new List<VertexPositionColor>();
        private readonly List<VertexPositionColor> vl1 = new List<VertexPositionColor>();
        public List<Vector3> Active = new List<Vector3>();
        public MapSector[] N = new MapSector[Sectn*Sectn];
        private int _passn;

        public int drawed_sects, drawed_verts;
        public int sectrebuild;
        public short[] whinenapr = new short[Sectn*MapSector.dimS*Sectn*MapSector.dimS];
        public float[] wshine = new float[Sectn*MapSector.dimS*Sectn*MapSector.dimS];

        public SectorMap(GraphicsDevice gd, Effect mapeffect) {
            for (int i = 0; i <= Sectn - 1; i++) {
                for (int j = 0; j <= Sectn - 1; j++) {
                    N[i*Sectn + j] = new MapSector(i, j);
                }
            }

            _gd = gd;
            _be = mapeffect;
            _basice = new BasicEffect(gd);
            _basice2 = new BasicEffect(gd);
        }

        public void RebuildAllMapGeo() {
            for (int i = 0; i < N.Length; i++) {
                N[i].builded = false;
            }
        }

        public void AsyRAMG(int z_cam, FreeCamera Camera) {}

        public MNode At(int x, int y, int z) {
            return
                N[x/MapSector.dimS*Sectn + y/MapSector.dimS].N[
                    x%MapSector.dimS*MapSector.dimS*MapSector.dimH + y%MapSector.dimS*MapSector.dimH + z];
        }

        public MNode At(float x, float y, float z) {
            return N[(int) x/MapSector.dimS*Sectn + (int) y/MapSector.dimS].N[
                (int) x%MapSector.dimS*MapSector.dimS*MapSector.dimH + (int) y%MapSector.dimS*MapSector.dimH + (int) z];
        }

        public MNode At(Vector3 ve) {
            return N[(int) ve.X/MapSector.dimS*Sectn + (int) ve.Y/MapSector.dimS].N[
                (int) ve.X%MapSector.dimS*MapSector.dimS*MapSector.dimH + (int) ve.Y%MapSector.dimS*MapSector.dimH +
                (int) ve.Z];
        }

        public void DrawAllMap(GameTime gt, Camera cam) {
            _passn++;
            if (_passn > Sectn*Sectn - 1) {
                _passn = 0;
            }

            _be.Parameters["worldMatrix"].SetValue(Matrix.Identity);
            _be.Parameters["viewMatrix"].SetValue(Main.Camera.View);
            _be.Parameters["projectionMatrix"].SetValue(Main.Camera.Projection);
            _be.Parameters["diffuseColor"].SetValue(Color.White.ToVector4());
            _be.Parameters["ambientColor"].SetValue(Color.DarkGray.ToVector4());
            var ld = new Vector3(0.5f, -1, -1.2f);
            ld.Normalize();
            _be.Parameters["lightDirection"].SetValue(ld);
            _be.Parameters["shaderTexture"].SetValue(Main.texatlas);

            _gd.RasterizerState = RasterizerState.CullCounterClockwise;
            _gd.DepthStencilState = DepthStencilState.Default;
            _gd.BlendState = BlendState.AlphaBlend;

            drawed_sects = 0;
            drawed_verts = 0;

            foreach (EffectPass pass in _be.CurrentTechnique.Passes) {
                pass.Apply();
                foreach (MapSector a in N) {
                    if (cam.Frustum.Contains(new BoundingBox(a.bounding.Min, a.bounding.Max)) !=
                        ContainmentType.Disjoint) {
                        if (!a.builded) {
                            a.RebuildSectorGeo(_gd, Main.z_cam);
                            sectrebuild++;
                        }
                        if (!a.empty) {
                            drawed_sects++;
                            _gd.DrawUserPrimitives(PrimitiveType.TriangleList, a.VertexArray, 0, a.index/3);
                            drawed_verts += a.index/3;
                        }
                    }
                }

                foreach (MapSector a in N) {
                    if (a.indextransparent > 0) {
                        _gd.DrawUserPrimitives(PrimitiveType.TriangleList, a.VertexArrayTransparent, 0,
                                               a.indextransparent/3);
                        drawed_verts += a.indextransparent/3;
                    }
                }
            }

            _basice2.VertexColorEnabled = true;
            _basice2.Alpha = 0.5f;
            _basice2.Projection = cam.Projection;
            _basice2.View = cam.View;

            //_gd.BlendState = BlendState.AlphaBlend;


            Color greentop = At(Main.Selector).BlockID == 0
                                 ? At(Main.Selector.X, Main.Selector.Y, Main.Selector.Z + 1).BlockID != 0
                                       ? Color.Yellow
                                       : Color.Red
                                 : Color.Green;
            greentop.A = 128;
            Color greencube = greentop*0.5f;

            Color selecttop = Color.LightGray;
            selecttop.A = 128;
            Color selectcube = selecttop*0.5f;

            vl1.Clear();

            var ramka_3 = new Vector3();
            if (Mouse.GetState().RightButton == ButtonState.Pressed) {
                Main.ramka_2.X = Math.Max(Main.Selector.X, Main.ramka_1.X);
                Main.ramka_2.Y = Math.Max(Main.Selector.Y, Main.ramka_1.Y);
                Main.ramka_2.Z = Math.Max(Main.Selector.Z, Main.ramka_1.Z);

                ramka_3 = new Vector3(Math.Min(Main.Selector.X, Main.ramka_1.X),
                                      Math.Min(Main.Selector.Y, Main.ramka_1.Y),
                                      Math.Min(Main.Selector.Z, Main.ramka_1.Z));
            }

            foreach (EffectPass pass in _basice2.CurrentTechnique.Passes) {
                pass.Apply();
                AddCubeverts(Main.Selector, new Vector3(Main.Selector.X + 1, Main.Selector.Y + 1, Main.Selector.Z + 127),
                             greencube, greentop, 0.02f);
                if (Mouse.GetState().RightButton == ButtonState.Pressed) {
                    AddCubeverts(ramka_3, new Vector3(Main.ramka_2.X + 1, Main.ramka_2.Y + 1, Main.ramka_2.Z + 1),
                                 selectcube, selecttop, 0.01f);
                }

                //VertexBuffer vb1 = new VertexBuffer(gd, typeof (VertexPositionColor), 18, BufferUsage.WriteOnly);
                //vb1.SetData(vl1,0,18);

                //gd.SetVertexBuffer(vb1);
                _gd.DrawUserPrimitives(PrimitiveType.TriangleList, vl1.ToArray(), 0, vl1.Count()/3);
            }


            _basice.VertexColorEnabled = false;
            _basice.Projection = cam.Projection;
            _basice.View = cam.View;

            vl.Clear();
            if (Main.debug) {
                foreach (EffectPass pass in _basice.CurrentTechnique.Passes) {
                    pass.Apply();
                    foreach (MapSector a in N) {
                        var vv = new Vector3[24];
                        vv[0] = a.bounding.Min;
                        vv[11] = a.bounding.Max;
                        vv[1] = new Vector3(vv[11].X, vv[0].Y, vv[0].Z);


                        vv[2] = new Vector3(vv[11].X, vv[0].Y, vv[0].Z);
                        vv[3] = new Vector3(vv[11].X, vv[11].Y, vv[0].Z);


                        vv[4] = new Vector3(vv[11].X, vv[11].Y, vv[0].Z);
                        vv[5] = new Vector3(vv[0].X, vv[11].Y, vv[0].Z);


                        vv[6] = new Vector3(vv[0].X, vv[11].Y, vv[0].Z);
                        vv[7] = new Vector3(vv[0].X, vv[0].Y, vv[0].Z);


                        vv[8] = new Vector3(vv[0].X, vv[0].Y, vv[11].Z);
                        vv[9] = new Vector3(vv[11].X, vv[0].Y, vv[11].Z);


                        vv[10] = new Vector3(vv[11].X, vv[0].Y, vv[11].Z);


                        vv[12] = a.bounding.Max;
                        vv[13] = new Vector3(vv[0].X, vv[11].Y, vv[11].Z);


                        vv[14] = new Vector3(vv[0].X, vv[11].Y, vv[11].Z);
                        vv[15] = new Vector3(vv[0].X, vv[0].Y, vv[11].Z);

                        vv[16] = vv[0];
                        vv[17] = vv[8];
                        vv[18] = vv[1];
                        vv[19] = vv[9];
                        vv[20] = vv[3];
                        vv[21] = vv[11];
                        vv[22] = vv[5];
                        vv[23] = vv[13];
                        vl.AddRange(vv.Select(vector3 => new VertexPositionColor(vector3, Color.Red)));
                    }

                    if (vl.Count > 0) {
                        //VertexBuffer vb = new VertexBuffer(gd, typeof (VertexPositionColor), vl.Count,
                        //                                   BufferUsage.WriteOnly);
                        //vb.SetData(vl.ToArray());

                        //gd.SetVertexBuffer(vb);
                        _gd.DrawUserPrimitives(PrimitiveType.LineList, vl.ToArray(), 0, vl.Count/2);
                    }
                }
            }
        }

        private void AddCubeverts(Vector3 min, Vector3 max, Color greencube, Color greentop, float delta) {
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, min.Y - delta, -min.Z + delta),
                     greentop));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, max.Y + delta, -min.Z + delta),
                     greentop));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, min.Y - delta, -min.Z + delta),
                     greentop));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, max.Y + delta, -min.Z + delta),
                     greentop));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, min.Y - delta, -min.Z + delta),
                     greentop));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, max.Y + delta, -min.Z + delta),
                     greentop));

            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, max.Y + delta, -min.Z + delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, max.Y + delta, -max.Z - delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, min.Y - delta, -max.Z - delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, min.Y - delta, -max.Z - delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, min.Y - delta, -min.Z + delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, max.Y + delta, -min.Z + delta),
                     greencube));

            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, max.Y + delta, -min.Z + delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, min.Y - delta, -min.Z + delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, min.Y - delta, -max.Z - delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, min.Y - delta, -max.Z - delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, max.Y + delta, -max.Z - delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, max.Y + delta, -min.Z + delta),
                     greencube));

            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, min.Y - delta, -min.Z + delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, min.Y - delta, -min.Z + delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, min.Y - delta, -max.Z - delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, min.Y - delta, -max.Z - delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, min.Y - delta, -max.Z - delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, min.Y - delta, -min.Z + delta),
                     greencube));


            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, max.Y + delta, -min.Z + delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, max.Y + delta, -min.Z + delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, max.Y + delta, -max.Z - delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(min.X - delta, max.Y + delta, -max.Z - delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, max.Y + delta, -max.Z - delta),
                     greencube));
            vl1.Add
                (new VertexPositionColor(
                     new Vector3(max.X + delta, max.Y + delta, -min.Z + delta),
                     greencube));
        }

        public void KillBlock(int x, int y, int z) {
            MNode with = At(x, y, z);

            if (Main.dbobject.Data[with.BlockID].Dropafterdeath != 666) {
                Main.localitems.n.Add(new LocalItem {
                                                        count = Main.dbobject.Data[At(x, y, z).BlockID].DropafterdeathNum,
                                                        id = Main.dbobject.Data[with.BlockID].Dropafterdeath,
                                                        pos = new Vector3(x, y, z)
                                                    });
            }
            //Main.iss.AddItem(Main.dbobject.Data[n[x, y, z].blockID].dropafterdeath,
            //                 Main.dbobject.Data[n[x, y, z].blockID].dropafterdeath_num);


            with.BlockID = 0;
            with.Health = 10;

            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    if (GoodVector3(x + i, y + j, z)) {
                        At(x + i, y + j, z).Explored = true;
                    }
                }
            }

            if (GoodVector3(x, y, z + 1)) {
                At(x, y, z + 1).Explored = true;
            }

            if (with.BlockID == KnownIDs.StorageEntrance) {
                Main.globalstorage.n.Remove(new Vector3(x, y, z));
            }

            SubterrainPersonaly(new Vector3(x, y, z));

            N[x/MapSector.dimS*Sectn + y/MapSector.dimS].builded = false;
        }

        public void SubterrainPersonaly(Vector3 where) {
            float i = where.X;
            float j = where.Y;
            for (int m = 0; m <= MapSector.dimH - 1; m++) {
                At(i, j, m).Subterrain = true;
            }

            for (int m = 0; m <= MapSector.dimH - 1; m++) {
                if (At(i, j, m).BlockID == 0) {
                    At(i, j, m).Subterrain = false;
                }
                else {
                    At(i, j, m).Subterrain = false;
                    goto here;
                }
            }
            here:
            ;
        }

        /// <summary>
        /// Проверка, входит ли вектор в границы карты
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static bool GoodVector3(Vector3 loc) {
            if (loc != null && loc.X >= 0 && loc.Y >= 0 && loc.X <= Sectn*MapSector.dimS - 1 &&
                loc.Y <= Sectn*MapSector.dimS - 1 && loc.Z >= 0 && loc.Z <= MapSector.dimH - 1) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка, входит ли вектор в границы карты
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <returns></returns>
        public static bool GoodVector3(int X, int Y, int Z) {
            if (X >= 0 && Y >= 0 && X <= Sectn*MapSector.dimS - 1 && Y <= Sectn*MapSector.dimS - 1 && Z >= 0 &&
                Z <= MapSector.dimH - 1) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка, входит ли вектор в границы карты
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <returns></returns>
        public static bool GoodVector3(float X, float Y, float Z) {
            if (X >= 0 && Y >= 0 && X <= Sectn*MapSector.dimS - 1 && Y <= Sectn*MapSector.dimS - 1 && Z >= 0 &&
                Z <= MapSector.dimH - 1) {
                return true;
            }
            return false;
        }


        /// <summary>
        /// генерация базового слоя с нечеткой границей
        /// </summary>
        /// <param name="id">заполнить базовый слой указанным блоком</param>
        public void Generation_BasicLayer(int id) {
            for (int i = 0; i <= Sectn*MapSector.dimS - 1; i++) {
                for (int j = 0; j <= Sectn*MapSector.dimS - 1; j++) {
                    int k = 0;
                    for (k = 128 - 1; k >= 0; k--) {
                        if (At(i, j, k).BlockID == 12345) {
                            goto here1;
                        }
                    }
                    here1:
                    ;

                    for (int m = k; m >= rnd.Next(k - 5, k); m--) {
                        if (m > 0) {
                            if (At(i, j, m).BlockID == 12345) {
                                At(i, j, m).BlockID = id;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// заколнить слой толщиной count блоком id
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="count">толщина слоя</param>
        public void Generation_FullLayer(int id, int count) {
            for (int i = 0; i <= Sectn*MapSector.dimS - 1; i++) {
                for (int j = 0; j <= Sectn*MapSector.dimS - 1; j++) {
                    int k = 0;
                    for (k = 128 - 1; k >= 0; k--) {
                        if (At(i, j, k).BlockID == 12345) {
                            goto here1;
                        }
                    }
                    here1:
                    ;

                    if (k <= 0) {
                        continue;
                    }

                    for (int m = k; m >= k - count + 1; m--) {
                        if (At(i, j, m).BlockID == 12345) {
                            At(i, j, m).BlockID = id;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// заколнить слой толщиной count блоком id с кластерами clust (частотой c_freq) и жилами jila (частотой j_freq и протяженнойстью length)
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="count">толщина слоя</param>
        /// <param name="clust">идентификаторы кластеров</param>
        /// <param name="c_freq">частота встречания кластеров</param>
        /// <param name="jila">идентификаторы жил</param>
        /// <param name="j_freq">частота встречания жил</param>
        /// <param name="length">протяженность жил</param>
        public void GenerationFullLayerCluster(int id, int count, int[] clust, int c_freq, int[] jila, int j_freq,
                                               int length) {
            for (int i = 0; i <= Sectn*MapSector.dimS - 1; i++) {
                for (int j = 0; j <= Sectn*MapSector.dimS - 1; j++) {
                    int k = 0;
                    for (k = MapSector.dimH - 1; k >= 0; k--) {
                        if (At(i, j, k).BlockID == 12345) {
                            goto here1;
                        }
                    }
                    here1:
                    ;

                    if (k <= 0) {
                        continue;
                    }

                    for (int m = k; m >= k - count + 1; m--) {
                        if (At(i, j, m).BlockID == 12345) {
                            At(i, j, m).BlockID = id;
                            if (rnd.Next(1, 101 - c_freq) == 1 && clust.Length > 0) {
                                cluster(clust[rnd.Next(0, clust.Length)], i, j, m);
                            }
                            if (rnd.Next(1, 101 - j_freq) == 1 && jila.Length > 0) {
                                cjila(jila[rnd.Next(0, jila.Length)], i, j, m, length);
                            }
                        }
                    }
                }
            }
        }

        private void cluster(int id, int i, int j, int m) {
            switch (rnd.Next(0, 7)) {
                case 0:
                    At(i, j, m).BlockID = id;
                    break;
                case 1:
                    if (GoodVector3(i - 1, j, m)) {
                        At(i, j, m).BlockID = id;
                    }
                    if (GoodVector3(i, j + 1, m)) {
                        At(i, j + 1, m).BlockID = id;
                    }
                    if (GoodVector3(i + 1, j, m)) {
                        At(i + 1, j, m).BlockID = id;
                    }
                    if (GoodVector3(i - 1, j, m)) {
                        At(i - 1, j, m).BlockID = id;
                    }
                    if (GoodVector3(i, j - 1, m)) {
                        At(i, j - 1, m).BlockID = id;
                    }
                    break;
                case 2:
                    if (GoodVector3(i - 1, j, m)) {
                        At(i, j, m).BlockID = id;
                    }
                    if (GoodVector3(i + 1, j, m)) {
                        At(i + 1, j, m).BlockID = id;
                    }
                    if (GoodVector3(i - 1, j, m)) {
                        At(i - 1, j, m).BlockID = id;
                    }
                    if (GoodVector3(i, j - 1, m)) {
                        At(i, j - 1, m).BlockID = id;
                    }
                    break;
                case 3:
                    if (GoodVector3(i - 1, j, m)) {
                        At(i, j, m).BlockID = id;
                    }
                    if (GoodVector3(i, j + 1, m)) {
                        At(i, j + 1, m).BlockID = id;
                    }
                    if (GoodVector3(i - 1, j, m)) {
                        At(i - 1, j, m).BlockID = id;
                    }
                    if (GoodVector3(i, j - 1, m)) {
                        At(i, j - 1, m).BlockID = id;
                    }
                    break;
                case 4:
                    if (GoodVector3(i - 1, j, m)) {
                        At(i, j, m).BlockID = id;
                    }
                    if (GoodVector3(i, j + 1, m)) {
                        At(i, j + 1, m).BlockID = id;
                    }
                    if (GoodVector3(i + 1, j, m)) {
                        At(i + 1, j, m).BlockID = id;
                    }
                    if (GoodVector3(i, j - 1, m)) {
                        At(i, j - 1, m).BlockID = id;
                    }
                    break;
                case 5:
                    if (GoodVector3(i - 1, j, m)) {
                        At(i, j, m).BlockID = id;
                    }
                    if (GoodVector3(i, j + 1, m)) {
                        At(i, j + 1, m).BlockID = id;
                    }
                    if (GoodVector3(i + 1, j, m)) {
                        At(i + 1, j, m).BlockID = id;
                    }
                    if (GoodVector3(i - 1, j, m)) {
                        At(i - 1, j, m).BlockID = id;
                    }
                    break;
                case 6:
                    if (GoodVector3(i - 1, j, m)) {
                        At(i, j, m).BlockID = id;
                    }
                    if (GoodVector3(i - 1, j, m)) {
                        At(i - 1, j, m).BlockID = id;
                    }
                    if (GoodVector3(i, j - 1, m)) {
                        At(i, j - 1, m).BlockID = id;
                    }
                    break;
                case 7:
                    if (GoodVector3(i - 1, j, m)) {
                        At(i, j, m).BlockID = id;
                    }
                    if (GoodVector3(i, j + 1, m)) {
                        At(i, j + 1, m).BlockID = id;
                    }
                    if (GoodVector3(i + 1, j, m)) {
                        At(i + 1, j, m).BlockID = id;
                    }
                    break;
            }
        }

        private void cjila(int id, int i, int j, int m, int lenght) {
            int ii = rnd.Next(-1, 1);
            int jj = rnd.Next(-1, 1);

            int i1 = i;
            int j1 = j;

            for (int k = 0; k <= lenght - 1; k++) {
                i1 += ii;
                j1 += jj;
                if (GoodVector3(i1, j1, m)) {
                    At(i1, j1, m).BlockID = id;
                }

                if (rnd.Next(1, 4) == 1) {
                    ii = rnd.Next(-1, 1);
                }
                jj = rnd.Next(-1, 1);
            }
        }

        public void Generation_PlaceOnSurface() {
            for (int i = 0; i <= Sectn*MapSector.dimS - 1; i++) {
                for (int j = 0; j <= Sectn*MapSector.dimS - 1; j++) {
                    for (int m = 0; m <= MapSector.dimH - 1; m++) {
                        if (At(i, j, m).BlockID != 0) {
                            if (
                                rnd.Next(1,
                                         Convert.ToInt32(101 -
                                                         GMap.data[
                                                             Main.gmap.obj[
                                                                 i + (int) Main.gmap_region.X,
                                                                 j + (int) Main.gmap_region.Y]].tree_freq)) == 1 &&
                                m > 0) {
                                if (
                                    GMap.data[Main.gmap.obj[i + (int) Main.gmap_region.X, j + (int) Main.gmap_region.Y]]
                                        .tree.Count > 0) {
                                    At(i, j, m - 1).BlockID =
                                        GMap.data[
                                            Main.gmap.obj[i + (int) Main.gmap_region.X, j + (int) Main.gmap_region.Y]].
                                            tree[
                                                rnd.Next(0,
                                                         GMap.data[
                                                             Main.gmap.obj[
                                                                 i + (int) Main.gmap_region.X,
                                                                 j + (int) Main.gmap_region.Y]].tree.Count)];
                                }
                            }
                            goto here2;
                        }
                    }
                    here2:
                    ;
                }
            }
        }

        /// <summary>
        /// генерация слоя под поверхностью
        /// </summary>
        /// <param name="count"></param>
        public void Generation_FullLayer_under(int count) {
            for (int i = 0; i <= Sectn*MapSector.dimS - 1; i++) {
                for (int j = 0; j <= Sectn*MapSector.dimS - 1; j++) {
                    int k = 0;
                    for (k = MapSector.dimH - 1; k >= 0 && At(i, j, k).BlockID != 0; k--) {
                        ;
                    }
                    if (k < 0) {
                        continue;
                    }

                    for (int m = k; m >= k - count + 1; m--) {
                        if (m > 0) {
                            At(i, j, m).BlockID =
                                GMap.data[Main.gmap.obj[i + (int) Main.gmap_region.X, j + (int) Main.gmap_region.Y]].
                                    under_surf;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// генерация слоя под слоем под поверхностью
        /// </summary>
        /// <param name="count"></param>
        public void Generation_FullLayer_under_under(int count) {
            for (int i = 0; i <= Sectn*MapSector.dimS - 1; i++) {
                for (int j = 0; j <= Sectn*MapSector.dimS - 1; j++) {
                    int k = 0;
                    for (k = MapSector.dimH - 1; k >= 0; k--) {
                        if (At(i, j, k).BlockID == 0) {
                            goto here1;
                        }
                    }
                    here1:
                    ;

                    if (k < 0) {
                        continue;
                    }

                    for (int m = k; m >= k - count + 1; m--) {
                        if (m > 0) {
                            At(i, j, m).BlockID =
                                GMap.data[Main.gmap.obj[i + (int) Main.gmap_region.X, j + (int) Main.gmap_region.Y]].
                                    under_under_surf;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// генерация верхнего слоя (чаще всего травы)
        /// </summary>
        /// <param name="count"></param>
        public void Generation_FullLayerGrass(int count) {
            for (int i = 0; i <= Sectn*MapSector.dimS - 1; i++) {
                for (int j = 0; j <= Sectn*MapSector.dimS - 1; j++) {
                    int k = 0;
                    for (k = MapSector.dimH - 1; k >= 0; k--) {
                        if (At(i, j, k).BlockID == 0) {
                            goto here1;
                        }
                    }
                    here1:
                    ;

                    if (k < 0) {
                        continue;
                    }

                    List<int> id =
                        GMap.data[Main.gmap.obj[i + (int) Main.gmap_region.X, j + (int) Main.gmap_region.Y]].grass;

                    for (int m = k; m >= k - count + 1; m--) {
                        if (id.Count != 0) {
                            int a = rnd.Next(0, id.Count);
                            if (m > 0) {
                                At(i, j, m).BlockID = id[a];
                            }
                        }
                        else if (m > 0) {
                            At(i, j, m).BlockID = 10;
                        }
                    }
                }
            }
        }

        public void Generation_SmoothTop() {}

        /// <summary>
        /// полный пересчет значений нодов subterrain
        /// </summary>
        public void RecalcExploredSubterrain() {
            for (int i = 0; i <= Sectn*MapSector.dimS - 1; i++) {
                for (int j = 0; j <= Sectn*MapSector.dimS - 1; j++) {
                    for (int m = 0; m <= MapSector.dimH - 1; m++) {
                        At(i, j, m).Subterrain = true;
                        At(i, j, m).Explored = false;
                    }
                }
            }

            for (int i = 1; i <= Sectn*MapSector.dimS - 2; i++) {
                for (int j = 1; j <= Sectn*MapSector.dimS - 2; j++) {
                    for (int m = 0; m <= MapSector.dimH - 1; m++) {
                        if (Main.dbobject.Data[At(i, j, m).BlockID].Transparent) {
                            At(i, j, m).Subterrain = false;
                            At(i, j, m).Explored = true;

                            At(i + 1, j, m).Subterrain = false;
                            At(i + 1, j, m).Explored = true;
                            At(i, j + 1, m).Subterrain = false;
                            At(i, j + 1, m).Explored = true;
                            At(i - 1, j, m).Subterrain = false;
                            At(i - 1, j, m).Explored = true;
                            At(i, j - 1, m).Subterrain = false;
                            At(i, j - 1, m).Explored = true;
                        }
                        else {
                            At(i, j, m).Subterrain = false;
                            At(i, j, m).Explored = true;
                            goto here2;
                        }
                    }
                    here2:
                    ;
                }
            }
        }

        /// <summary>
        /// простая генерация локальный карты по данным глобальной карты (базовая версия)
        /// </summary>
        public void SimpleGeneration_bygmap() {
            for (int i0 = 0; i0 < wshine.GetUpperBound(0); i0++) {
                wshine[i0] = (float) rnd.NextDouble();
                whinenapr[i0] = rnd.Next(0, 1) == 0 ? (short) -1 : (short) 1;
            }

            for (int i = 0; i <= Sectn*MapSector.dimS - 1; i++) {
                for (int j = 0; j <= Sectn*MapSector.dimS - 1; j++) {
                    for (int k = 0; k <= MapSector.dimH - 1; k += 2) {
                        At(i, j, k).BlockID = 0;
                        At(i, j, k + 1).BlockID = 0;
                    }
                }
            }


            for (int i = 0; i <= Sectn*MapSector.dimS - 1; i++) {
                for (int j = 0; j <= Sectn*MapSector.dimS - 1; j++) {
                    for (int k = 0;
                         k <=
                         ((Main.gmap.n[(int) Main.gmap_region.X + i, (int) Main.gmap_region.Y + j]))*
                         (MapSector.dimH - 1)*0.3 + (MapSector.dimH - 1)*0.7 - 5;
                         k++) {
                        if (GoodVector3(i, j, MapSector.dimH - 1 - k)) {
                            At(i, j, MapSector.dimH - 1 - k).BlockID = 12345;
                        }
                    }
                }
            }

            int[] metamorf_clust = {800, 801, 802, 803, 804, 805, 806};
            int[] matamorf_jila = {810, 811, 812};

            Generation_FullLayer(18, 5);
            Generation_FullLayer(17, 2);
            Generation_FullLayer(16, 2);
            Generation_FullLayer(15, 2);
            Generation_FullLayer(14, 18);

            Generation_BasicLayer(KnownIDs.Gabro);
            Generation_FullLayer(KnownIDs.Gabro, 7);
            Generation_FullLayer(KnownIDs.GabroToGranete, 1);
            Generation_FullLayer(KnownIDs.GrenFranite, 6);

            int[] granite_clust = metamorf_clust;
            int[] granite_jila = {55, 55};
            GenerationFullLayerCluster(11, 12, granite_clust, 10, granite_jila, 5, 20);

            Generation_FullLayer(33, 10);

            int[] marble_clust = metamorf_clust;
            int[] marble_jila = matamorf_jila;
            GenerationFullLayerCluster(22, 10, marble_clust, 10, marble_jila, 5, 20);

            GenerationFullLayerCluster(44, 4, metamorf_clust, 6, matamorf_jila, 2, 25);

            GenerationFullLayerCluster(22, 4, metamorf_clust, 6, matamorf_jila, 2, 25);

            for (int i = 0; i <= 20; i++) {
                Generation_FullLayer(824, 2);
                Generation_FullLayer(828, 2);
            }


            Generation_FullLayer_under(1);
            Generation_FullLayer_under_under(2);
            //Main.mmap.Generation_FullLayerTOP(ObjectID.DirtWall_Grass2, 1);

            Generation_FullLayerGrass(1);


            for (int i = 0; i <= Sectn*MapSector.dimS - 1; i++) {
                for (int j = 0; j <= Sectn*MapSector.dimS - 1; j++) {
                    //if(Main.gmap.n[Main.gmap_region.X + i, Main.gmap_region.Y + j] <= 0.4)
                    for (int k = 0; k <= ((0.4))*(MapSector.dimH - 1)*0.3 + (MapSector.dimH - 1)*0.7 + 1; k++) {
                        if (GoodVector3(i, j, MapSector.dimH - 1 - k) && At(i, j, MapSector.dimH - 1 - k).BlockID == 0) {
                            At(i, j, MapSector.dimH - 1 - k).BlockID = KnownIDs.water; //вода
                        }
                    }
                }
            }

            Generation_PlaceOnSurface();

            Main.PrepairMapDeleteWrongIDs(ref Main.smap);

            RecalcExploredSubterrain();

            Main.smap.RebuildAllMapGeo();
        }

        private int atint(Vector3 ve) {
            return (int) ve.X*Sectn*MapSector.dimS*MapSector.dimH + (int) ve.Y*MapSector.dimH + (int) ve.Z;
        }

        public Stack<Vector3> FindPatch(Vector3 loc_ref, Vector3 dest_ref) {
            var final_patch = new Stack<Vector3>();

            var lpf = new int[MapSector.dimS*MapSector.dimS*MapSector.dimH*Sectn*Sectn];

            var walk1 = new Queue<Vector3>();
            walk1.Enqueue(loc_ref);
            lpf[atint(loc_ref)] = 1;

            Vector3 cur;
            int cur_v = 2;
            int cur_lpf;

            int dest_lpf = atint(dest_ref);

            Vector3 ve1, ve2, ve3, ve4, ve5, ve6;

            while (walk1.Count > 0 && lpf[dest_lpf] == 0) {
                cur = walk1.Dequeue();
                cur_lpf = (int) cur.X*Sectn*MapSector.dimS*MapSector.dimH + (int) cur.Y*MapSector.dimH + (int) cur.Z;
                cur_v = lpf[cur_lpf];

                //ve1 = cur + Vector3.Left;
                if (cur.X - 1 >= 0 && lpf[cur_lpf - Sectn*MapSector.dimS*MapSector.dimH] == 0 &&
                    N[(int) (cur.X - 1)/MapSector.dimS*Sectn + (int) cur.Y/MapSector.dimS].N[
                        (int) (cur.X + 1)%MapSector.dimS*MapSector.dimS*MapSector.dimH +
                        (int) cur.Y%MapSector.dimS*MapSector.dimH + (int) cur.Z].BlockID == 0) {
                    lpf[cur_lpf - Sectn*MapSector.dimS*MapSector.dimH] = cur_v + 1;
                    walk1.Enqueue(new Vector3(cur.X - 1, cur.Y, cur.Z));
                }

                //ve2 = cur + Vector3.Right;
                if (cur.X + 1 < Sectn*MapSector.dimS && lpf[cur_lpf + Sectn*MapSector.dimS*MapSector.dimH] == 0 &&
                    N[(int) (cur.X + 1)/MapSector.dimS*Sectn + (int) cur.Y/MapSector.dimS].N[
                        (int) (cur.X + 1)%MapSector.dimS*MapSector.dimS*MapSector.dimH +
                        (int) cur.Y%MapSector.dimS*MapSector.dimH + (int) cur.Z].BlockID == 0) {
                    lpf[cur_lpf + Sectn*MapSector.dimS*MapSector.dimH] = cur_v + 1;
                    walk1.Enqueue(new Vector3(cur.X + 1, cur.Y, cur.Z));
                }

                //ve3 = cur + Vector3.Down;
                if (cur.Y - 1 >= 0 && lpf[cur_lpf - MapSector.dimH] == 0 &&
                    N[(int) cur.X/MapSector.dimS*Sectn + (int) (cur.Y - 1)/MapSector.dimS].N[
                        (int) cur.X%MapSector.dimS*MapSector.dimS*MapSector.dimH +
                        (int) (cur.Y - 1)%MapSector.dimS*MapSector.dimH + (int) cur.Z].BlockID == 0) {
                    lpf[cur_lpf - MapSector.dimH] = cur_v + 1;
                    walk1.Enqueue(new Vector3(cur.X, cur.Y - 1, cur.Z));
                }

                //ve4 = cur + Vector3.Up;
                if (cur.Y + 1 < Sectn*MapSector.dimS && lpf[cur_lpf + MapSector.dimH] == 0 &&
                    N[(int) cur.X/MapSector.dimS*Sectn + (int) (cur.Y + 1)/MapSector.dimS].N[
                        (int) cur.X%MapSector.dimS*MapSector.dimS*MapSector.dimH +
                        (int) (cur.Y + 1)%MapSector.dimS*MapSector.dimH + (int) cur.Z].BlockID == 0) {
                    lpf[cur_lpf + MapSector.dimH] = cur_v + 1;
                    walk1.Enqueue(new Vector3(cur.X, cur.Y + 1, cur.Z));
                }

                //ve5 = cur + Vector3.Forward;
                if (cur.Z - 1 >= 0 && lpf[cur_lpf - 1] == 0 &&
                    N[(int) cur.X/MapSector.dimS*Sectn + (int) cur.Y/MapSector.dimS].N[
                        (int) cur.X%MapSector.dimS*MapSector.dimS*MapSector.dimH +
                        (int) cur.Y%MapSector.dimS*MapSector.dimH + (int) cur.Z - 1].BlockID == 0) {
                    lpf[cur_lpf - 1] = cur_v + 1;
                    walk1.Enqueue(new Vector3(cur.X, cur.Y, cur.Z - 1));
                }

                //ve6 = cur + Vector3.Backward;
                if (cur.Z + 1 < MapSector.dimH && lpf[cur_lpf + 1] == 0 &&
                    N[(int) cur.X/MapSector.dimS*Sectn + (int) cur.Y/MapSector.dimS].N[
                        (int) cur.X%MapSector.dimS*MapSector.dimS*MapSector.dimH +
                        (int) cur.Y%MapSector.dimS*MapSector.dimH + (int) cur.Z + 1].BlockID == 0) {
                    lpf[cur_lpf + 1] = cur_v + 1;
                    walk1.Enqueue(new Vector3(cur.X, cur.Y, cur.Z + 1));
                }
            }

            final_patch.Push(dest_ref);
            cur = dest_ref;

            int max = cur_v;

            for (int i = 0; i <= max; i++) {
                cur_lpf = (int) cur.X*Sectn*MapSector.dimS*MapSector.dimH + (int) cur.Y*MapSector.dimH + (int) cur.Z;
                cur_v = lpf[cur_lpf];

                if (cur.X - 1 >= 0 && lpf[cur_lpf - Sectn*MapSector.dimS*MapSector.dimH] < cur_v &&
                    lpf[cur_lpf - Sectn*MapSector.dimS*MapSector.dimH] != 0) {
                    cur.X--;
                    final_patch.Push(cur);
                }
                else if (cur.X + 1 < MapSector.dimS && lpf[cur_lpf + Sectn*MapSector.dimS*MapSector.dimH] < cur_v &&
                         lpf[cur_lpf + Sectn*MapSector.dimS*MapSector.dimH] != 0) {
                    cur.X++;
                    final_patch.Push(cur);
                }
                else if (cur.Y - 1 >= 0 && lpf[cur_lpf - MapSector.dimH] < cur_v &&
                         lpf[cur_lpf - MapSector.dimH] != 0) {
                    cur.Y--;
                    final_patch.Push(cur);
                }
                else if (cur.Y + 1 < MapSector.dimS && lpf[cur_lpf + MapSector.dimH] < cur_v &&
                         lpf[cur_lpf + MapSector.dimH] != 0) {
                    cur.Y++;
                    final_patch.Push(cur);
                }
                else if (cur.Z - 1 >= 0 && lpf[cur_lpf - 1] < cur_v && lpf[cur_lpf - 1] != 0) {
                    cur.Z--;
                    final_patch.Push(cur);
                }
                else if (cur.Z + 1 < MapSector.dimH && lpf[cur_lpf + 1] < cur_v && lpf[cur_lpf + 1] != 0) {
                    cur.Z++;
                    final_patch.Push(cur);
                }
            }

            return final_patch;
        }

        public bool IsWalkable(int p0, int p1, int p2) {
            return
                N[p0/MapSector.dimS*Sectn + p1/MapSector.dimS].N[
                    p0%MapSector.dimS*MapSector.dimS*MapSector.dimH + p1%MapSector.dimS*MapSector.dimH + p2].BlockID ==
                0;
                /*&& 
                (N[p0/MapSector.dimS*Sectn + p1/MapSector.dimS].N[p0%MapSector.dimS*MapSector.dimS*MapSector.dimH + p1%MapSector.dimS*MapSector.dimH + p2 + 1].BlockID != 0 || 
                    N[p0/MapSector.dimS*Sectn + p1/MapSector.dimS].N[p0%MapSector.dimS*MapSector.dimS*MapSector.dimH + p1%MapSector.dimS*MapSector.dimH + p2 + 2].BlockID != 0);*/
        }

        public bool IsWalkable(Vector3 ve1) {
            return
                N[(int) ve1.X/MapSector.dimS*Sectn + (int) ve1.Y/MapSector.dimS].N[
                    (int) ve1.X%MapSector.dimS*MapSector.dimS*MapSector.dimH + (int) ve1.Y%MapSector.dimS*MapSector.dimH +
                    (int) ve1.Z].BlockID == 0;
                /* && 
                (N[(int)ve1.X / MapSector.dimS * Sectn + (int)ve1.Y / MapSector.dimS].N[(int)ve1.X % MapSector.dimS * MapSector.dimS * MapSector.dimH + (int)ve1.Y % MapSector.dimS * MapSector.dimH + (int)(ve1.Z + 1)].BlockID != 0 || 
                    N[(int)ve1.X / MapSector.dimS * Sectn + (int)ve1.Y / MapSector.dimS].N[(int)ve1.X % MapSector.dimS * MapSector.dimS * MapSector.dimH + (int)ve1.Y % MapSector.dimS * MapSector.dimH + (int)(ve1.Z + 2)].BlockID != 0);*/
        }

        public void SetBlock(Vector3 dest, int blockId) {
            At(dest).BlockID = blockId;

            N[(int) dest.X/MapSector.dimS*Sectn + (int) dest.Y/MapSector.dimS].builded = false;
        }
    }
}