using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mork.Bad_Database;
using Mork.Graphics.MapEngine;
using Mork.Local_Map.Dynamic.Local_Items;

namespace Mork.Local_Map.Sector
{
    public class IntersectMap
    {
        public BoundingBox[] n = new BoundingBox[SectorMap.sectn*SectorMap.sectn*MapSector.dimS*MapSector.dimS];

        public IntersectMap()
        {
            for (int i = 0; i < n.Length; i++)
            {
                n[i] = new BoundingBox(new Vector3(i / (SectorMap.sectn * MapSector.dimS), i % (SectorMap.sectn * MapSector.dimS), 0), new Vector3(i / (SectorMap.sectn * MapSector.dimS) + 1, i % (SectorMap.sectn * MapSector.dimS) + 1, -1));
            }
        }

        public void MoveIntersectMap(Vector3 mover)
        {
            for (int index = 0; index < n.Length; index++)
            {
                n[index].Max -= mover;
                n[index].Min -= mover;
            }
        }
    }
    [Serializable]
    public class SectorMap
    {
        public const int sectn = 8;
        public MapSector[] n = new MapSector[sectn * sectn];
        GraphicsDevice gd;
        Effect be;
        private BasicEffect basice;
        private BasicEffect basice2;
        private int passn = 0;

        public List<Vector3> active = new List<Vector3>();

        public SectorMap(GraphicsDevice _gd, Effect mapeffect)
        {
            for (int i = 0; i <= sectn - 1; i++)
                for (int j = 0; j <= sectn - 1; j++)
                        n[i * sectn + j] = new MapSector(i, j);

            gd = _gd;
            be = mapeffect;
            basice = new BasicEffect(_gd);
            basice2 = new BasicEffect(_gd);
        }

        public void RebuildAllMapGeo(int z_cam, FreeCamera Camera)
        {
            foreach (var a in n)
            {
                a.builded = false;
            }
        }

        public void AsyRAMG(int z_cam, FreeCamera Camera)
        {
        }

        public MNode At(int x, int y, int z)
        {
            return
                n[x/MapSector.dimS*sectn + y/MapSector.dimS].n[
                    x%MapSector.dimS*MapSector.dimS*MapSector.dimH + y%MapSector.dimS*MapSector.dimH + z];
        }

        public MNode At(float x, float y, float z)
        {
            return n[(int)x / MapSector.dimS * sectn + (int)y / MapSector.dimS].n[
                    (int)x % MapSector.dimS * MapSector.dimS * MapSector.dimH + (int)y % MapSector.dimS * MapSector.dimH + (int)z];
        }

        public MNode At(Vector3 ve)
        {
            return n[(int)ve.X / MapSector.dimS * sectn + (int)ve.Y / MapSector.dimS].n[
                    (int)ve.X % MapSector.dimS * MapSector.dimS * MapSector.dimH + (int)ve.Y % MapSector.dimS * MapSector.dimH + (int)ve.Z];
        }

        public void DrawAllMap(GameTime gt, Camera cam)
        {
            passn++;
            if (passn > sectn*sectn - 1) passn = 0;

            be.Parameters["worldMatrix"].SetValue(Matrix.Identity);
            be.Parameters["viewMatrix"].SetValue(Main.Camera.View);
            be.Parameters["projectionMatrix"].SetValue(Main.Camera.Projection);
            be.Parameters["diffuseColor"].SetValue(Color.White.ToVector4());
            be.Parameters["ambientColor"].SetValue(Color.DarkGray.ToVector4());
            var ld = new Vector3(0.5f, -1, 0.5f);
            ld.Normalize();
            be.Parameters["lightDirection"].SetValue(ld);
            be.Parameters["shaderTexture"].SetValue(Main.texatlas);

            gd.RasterizerState = RasterizerState.CullCounterClockwise;
            gd.DepthStencilState = DepthStencilState.Default;
            gd.BlendState = BlendState.Opaque;

            Main.drawed_sects = 0;
            Main.drawed_verts = 0;

            foreach (var pass in be.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (var a in n)
                {
                    if (cam.Frustum.Contains(new BoundingBox(a.bounding.Min, a.bounding.Max)) !=
                        ContainmentType.Disjoint)
                    {
                        if (!a.builded)
                        {
                            a.RebuildSectorGeo(gd, Main.z_cam);
                            Main.sectrebuild++;
                        }
                        if (!a.empty)
                        {
                            Main.drawed_sects++;
                            //gd.SetVertexBuffer(a.VertexBuffer);
                            gd.DrawUserPrimitives(PrimitiveType.TriangleList, a.VertexArray, 0, a.index / 3);
                            Main.drawed_verts += a.index/3;
                        }
                    }
                }
            }

            basice2.VertexColorEnabled = true;
            basice2.Alpha = 0.5f;
            basice2.Projection = cam.Projection;
            basice2.View = cam.View;

            gd.BlendState = BlendState.AlphaBlend;


            Color greencube = At(Main.Selector).blockID == 0 ? Color.Red : Color.GreenYellow;
            greencube.A = 128;

            foreach (var pass in basice2.CurrentTechnique.Passes)
            {
                pass.Apply();
                VertexPositionColor[] vl1 = new VertexPositionColor[30];
                vl1[0] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z + 0.01f),
                        greencube));
                vl1[1] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z + 0.01f),
                        greencube));
                vl1[2] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z + 0.01f),
                        greencube));
                vl1[3] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z + 0.01f),
                        greencube));
                vl1[4] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z + 0.01f),
                        greencube));
                vl1[5] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z + 0.01f),
                        greencube));

                vl1[6] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z + 0.01f),
                        greencube));
                vl1[7] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z - 128.01f),
                        greencube));
                vl1[8] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z - 128.01f),
                        greencube));
                vl1[9] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z - 128.01f),
                        greencube));
                vl1[10] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z + 0.01f),
                        greencube));
                vl1[11] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z + 0.01f),
                        greencube));

                vl1[12] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z + 0.01f),
                        greencube));
                vl1[13] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z + 0.01f),
                        greencube));
                vl1[14] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z - 128.01f),
                        greencube));
                vl1[15] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z - 128.01f),
                        greencube));
                vl1[16] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z - 128.01f),
                        greencube));
                vl1[17] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z + 0.01f),
                        greencube));

                vl1[18] =
                (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z + 0.01f),
                        greencube));
                vl1[19] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z + 0.01f),
                        greencube));
                vl1[20] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z - 128.01f),
                        greencube));
                vl1[21] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z - 128.01f),
                        greencube));
                vl1[22] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z - 128.01f),
                        greencube));
                vl1[23] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y - 0.01f, -Main.Selector.Z + 0.01f),
                        greencube));


                vl1[24] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z + 0.01f),
                        greencube));
                vl1[25] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z + 0.01f),
                        greencube));
                vl1[26] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z - 128.01f),
                        greencube));
                vl1[27] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X - 0.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z - 128.01f),
                        greencube));
                vl1[28] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z - 128.01f),
                        greencube));
                vl1[29] =
                    (new VertexPositionColor(
                        new Vector3(Main.Selector.X + 1.01f, Main.Selector.Y + 1.01f, -Main.Selector.Z + 0.01f),
                        greencube));

                //VertexBuffer vb1 = new VertexBuffer(gd, typeof (VertexPositionColor), 18, BufferUsage.WriteOnly);
                //vb1.SetData(vl1,0,18);

                //gd.SetVertexBuffer(vb1);
                gd.DrawUserPrimitives(PrimitiveType.TriangleList, vl1, 0, vl1.Count()/3);
            }


            basice.VertexColorEnabled = false;
            basice.Projection = cam.Projection;
            basice.View = cam.View;

            if(Main.debug)
            foreach (var pass in basice.CurrentTechnique.Passes)
            {
                pass.Apply();
                List<VertexPositionColor> vl = new List<VertexPositionColor>();
                foreach (var a in n)
                {
                    Vector3[] vv = new Vector3[24];
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

                if (vl.Count > 0)
                {
                    //VertexBuffer vb = new VertexBuffer(gd, typeof (VertexPositionColor), vl.Count,
                    //                                   BufferUsage.WriteOnly);
                    //vb.SetData(vl.ToArray());

                    //gd.SetVertexBuffer(vb);
                    gd.DrawUserPrimitives(PrimitiveType.LineList, vl.ToArray(), 0, vl.Count/2);
                }
            }
        }

        public void KillBlock(int x, int y, int z)
        {
            MNode with = At(x, y, z);

            if (Main.dbobject.Data[with.blockID].dropafterdeath != (int)KnownIDs.error)
                Main.localitems.n.Add(new LocalItem() { count = Main.dbobject.Data[At(x, y, z).blockID].dropafterdeath_num, id = Main.dbobject.Data[with.blockID].dropafterdeath, pos = new Vector3(x, y, z) });
            //Main.iss.AddItem(Main.dbobject.Data[n[x, y, z].blockID].dropafterdeath,
            //                 Main.dbobject.Data[n[x, y, z].blockID].dropafterdeath_num);


            with.blockID = 0;
            with.health = 10;

            for (var i = -1; i <= 1; i++)
                for (var j = -1; j <= 1; j++)
                {
                    if (GoodVector3(x + i, y + j, z))
                        At(x + i, y + j, z).explored = true;
                }

            if (GoodVector3(x, y, z + 1))
                At(x, y, z + 1).explored = true;

            if (with.blockID == KnownIDs.StorageEntrance)
            {
                Main.globalstorage.n.Remove(new Vector3(x, y, z));
            }

            SubterrainPersonaly(new Vector3(x, y, z));
        }

        public void SubterrainPersonaly(Vector3 where)
        {
            var i = where.X;
            var j = where.Y;
            for (var m = 0; m <= MapSector.dimH - 1; m++)
            {
                At(i, j, m).subterrain = true;
            }

            for (var m = 0; m <= MapSector.dimH - 1; m++)
            {
                if (At(i, j, m).blockID == 0)
                {
                    At(i, j, m).subterrain = false;
                }
                else
                {
                    At(i, j, m).subterrain = false;
                    goto here;
                }
            }
        here: ;
        }

        /// <summary>
        /// Проверка, входит ли вектор в границы карты
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static bool GoodVector3(Vector3 loc)
        {
            if (loc != null && loc.X >= 0 && loc.Y >= 0 && loc.X <= sectn * MapSector.dimS - 1 && loc.Y <= sectn * MapSector.dimS - 1 && loc.Z >= 0 && loc.Z <= MapSector.dimH - 1) return true;
            return false;
        }

        /// <summary>
        /// Проверка, входит ли вектор в границы карты
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <returns></returns>
        public static bool GoodVector3(int X, int Y, int Z)
        {
            if (X >= 0 && Y >= 0 && X <= sectn * MapSector.dimS - 1 && Y <= sectn * MapSector.dimS - 1 && Z >= 0 && Z <= MapSector.dimH - 1) return true;
            return false;
        }

        /// <summary>
        /// Проверка, входит ли вектор в границы карты
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <returns></returns>
        public static bool GoodVector3(float X, float Y, float Z)
        {
            if (X >= 0 && Y >= 0 && X <= sectn * MapSector.dimS - 1 && Y <= sectn * MapSector.dimS - 1 && Z >= 0 && Z <= MapSector.dimH - 1) return true;
            return false;
        }











        Random rnd = new Random();
        /// <summary>
        /// генерация базового слоя с нечеткой границей
        /// </summary>
        /// <param name="id">заполнить базовый слой указанным блоком</param>
        public void Generation_BasicLayer(int id)
        {
            for (int i = 0; i <= sectn*MapSector.dimS - 1; i++)
                for (int j = 0; j <= sectn * MapSector.dimS - 1; j++)
                {
                    int k = 0;
                    for (k = 128 - 1; k >= 0; k--)
                    {
                        if (At(i, j, k).blockID == 12345)
                            goto here1;
                    }
                here1: ;

                    for (int m = k; m >= rnd.Next(k - 5, k); m--)
                    {
                        if (m > 0) if (At(i, j, m).blockID == 12345) At(i, j, m).blockID = id;
                    }
                }
        }

        /// <summary>
        /// заколнить слой толщиной count блоком id
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="count">толщина слоя</param>
        public void Generation_FullLayer(int id, int count)
        {
            for (int i = 0; i <= sectn * MapSector.dimS - 1; i++)
                for (int j = 0; j <= sectn * MapSector.dimS - 1; j++)
                {
                    int k = 0;
                    for (k = 128 - 1; k >= 0; k--)
                    {
                        if (At(i, j, k).blockID == 12345)
                            goto here1;
                    }
                here1: ;

                    if (k <= 0) continue;

                    for (int m = k; m >= k - count + 1; m--)
                    {
                        if (At(i, j, m).blockID == 12345) At(i, j, m).blockID = id;
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
        public void GenerationFullLayerCluster(int id, int count, int[] clust, int c_freq, int[] jila, int j_freq, int length)
        {
            for (int i = 0; i <= sectn * MapSector.dimS - 1; i++)
                for (int j = 0; j <= sectn * MapSector.dimS - 1; j++)
                {
                    int k = 0;
                    for (k = sectn * MapSector.dimS - 1; k >= 0; k--)
                    {
                        if (At(i, j, k).blockID == 12345)
                            goto here1;
                    }
                here1: ;

                    if (k <= 0) continue;

                    for (int m = k; m >= k - count + 1; m--)
                    {
                        if (At(i, j, m).blockID == 12345)
                        {
                            At(i, j, m).blockID = id;
                            if (rnd.Next(1, 101 - c_freq) == 1 && clust.Length > 0)
                            {
                                cluster(clust[rnd.Next(0, clust.Length)], i, j, m);
                            }
                            if (rnd.Next(1, 101 - j_freq) == 1 && jila.Length > 0)
                            {
                                cjila(jila[rnd.Next(0, jila.Length)], i, j, m, length);
                            }
                        }
                    }
                }
        }

        private void cluster(int id, int i, int j, int m)
        {
            switch (rnd.Next(0, 7))
            {
                case 0:
                    At(i, j, m).blockID = id;
                    break;
                case 1:
                    if (GoodVector3(i - 1, j, m)) At(i, j, m).blockID = id;
                    if (GoodVector3(i, j + 1, m)) At(i, j + 1, m).blockID = id;
                    if (GoodVector3(i + 1, j, m)) At(i + 1, j, m).blockID = id;
                    if (GoodVector3(i - 1, j, m)) At(i - 1, j, m).blockID = id;
                    if (GoodVector3(i, j - 1, m)) At(i, j - 1, m).blockID = id;
                    break;
                case 2:
                    if (GoodVector3(i - 1, j, m)) At(i, j, m).blockID = id;
                    if (GoodVector3(i + 1, j, m)) At(i + 1, j, m).blockID = id;
                    if (GoodVector3(i - 1, j, m)) At(i - 1, j, m).blockID = id;
                    if (GoodVector3(i, j - 1, m)) At(i, j - 1, m).blockID = id;
                    break;
                case 3:
                    if (GoodVector3(i - 1, j, m)) At(i, j, m).blockID = id;
                    if (GoodVector3(i, j + 1, m)) At(i, j + 1, m).blockID = id;
                    if (GoodVector3(i - 1, j, m)) At(i - 1, j, m).blockID = id;
                    if (GoodVector3(i, j - 1, m)) At(i, j - 1, m).blockID = id;
                    break;
                case 4:
                    if (GoodVector3(i - 1, j, m)) At(i, j, m).blockID = id;
                    if (GoodVector3(i, j + 1, m)) At(i, j + 1, m).blockID = id;
                    if (GoodVector3(i + 1, j, m)) At(i + 1, j, m).blockID = id;
                    if (GoodVector3(i, j - 1, m)) At(i, j - 1, m).blockID = id;
                    break;
                case 5:
                    if (GoodVector3(i - 1, j, m)) At(i, j, m).blockID = id;
                    if (GoodVector3(i, j + 1, m)) At(i, j + 1, m).blockID = id;
                    if (GoodVector3(i + 1, j, m)) At(i + 1, j, m).blockID = id;
                    if (GoodVector3(i - 1, j, m)) At(i - 1, j, m).blockID = id;
                    break;
                case 6:
                    if (GoodVector3(i - 1, j, m)) At(i, j, m).blockID = id;
                    if (GoodVector3(i - 1, j, m)) At(i - 1, j, m).blockID = id;
                    if (GoodVector3(i, j - 1, m)) At(i, j - 1, m).blockID = id;
                    break;
                case 7:
                    if (GoodVector3(i - 1, j, m)) At(i, j, m).blockID = id;
                    if (GoodVector3(i, j + 1, m)) At(i, j + 1, m).blockID = id;
                    if (GoodVector3(i + 1, j, m)) At(i + 1, j, m).blockID = id;
                    break;
            }
        }

        private void cjila(int id, int i, int j, int m, int lenght)
        {
            int ii = rnd.Next(-1, 1);
            int jj = rnd.Next(-1, 1);

            int i1 = i; int j1 = j;

            for (int k = 0; k <= lenght - 1; k++)
            {
                i1 += ii; j1 += jj;
                if (GoodVector3(i1, j1, m))
                {
                    At(i1, j1, m).blockID = id;
                }

                if (rnd.Next(1, 4) == 1)
                    ii = rnd.Next(-1, 1);
                jj = rnd.Next(-1, 1);
            }
        }

        public void Generation_PlaceOnSurface()
        {
            for (int i = 0; i <= sectn * MapSector.dimS - 1; i++)
                for (int j = 0; j <= sectn * MapSector.dimS - 1; j++)
                {
                    for (int m = 0; m <= sectn * MapSector.dimS - 1; m++)
                    {
                        if (At(i, j, m).blockID != 0)
                        {
                            if (rnd.Next(1, Convert.ToInt32(101 - GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].tree_freq)) == 1 && m > 0)
                                if (GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].tree.Count > 0)
                                    At(i, j, m - 1).blockID = GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].tree[rnd.Next(0, GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].tree.Count)];
                            goto here2;
                        }
                    }
                here2: ;
                }
        }

        /// <summary>
        /// генерация слоя под поверхностью
        /// </summary>
        /// <param name="count"></param>
        public void Generation_FullLayer_under(int count)
        {
            for (int i = 0; i <= sectn * MapSector.dimS - 1; i++)
                for (int j = 0; j <= sectn * MapSector.dimS - 1; j++)
                {
                    int k = 0;
                    for (k = sectn * MapSector.dimS - 1; k >= 0; k--)
                    {
                        if (At(i, j, k).blockID == 0)
                            goto here1;
                    }
                here1: ;

                    if (k < 0) continue;

                    for (int m = k; m >= k - count + 1; m--)
                    {
                        if (m > 0) At(i, j, m).blockID = GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].under_surf;
                    }
                }
        }

        /// <summary>
        /// генерация слоя под слоем под поверхностью
        /// </summary>
        /// <param name="count"></param>
        public void Generation_FullLayer_under_under(int count)
        {
            for (int i = 0; i <= sectn * MapSector.dimS - 1; i++)
                for (int j = 0; j <= sectn * MapSector.dimS - 1; j++)
                {
                    int k = 0;
                    for (k = sectn * MapSector.dimS - 1; k >= 0; k--)
                    {
                        if (At(i, j, k).blockID == 0)
                            goto here1;
                    }
                here1: ;

                    if (k < 0) continue;

                    for (int m = k; m >= k - count + 1; m--)
                    {
                        if (m > 0) At(i, j, m).blockID = GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].under_under_surf;
                    }
                }
        }

        /// <summary>
        /// генерация верхнего слоя (чаще всего травы)
        /// </summary>
        /// <param name="count"></param>
        public void Generation_FullLayerGrass(int count)
        {
            for (int i = 0; i <= sectn * MapSector.dimS - 1; i++)
                for (int j = 0; j <= sectn * MapSector.dimS - 1; j++)
                {
                    int k = 0;
                    for (k = sectn * MapSector.dimS - 1; k >= 0; k--)
                    {
                        if (At(i, j, k).blockID == 0)
                            goto here1;
                    }
                here1: ;

                    if (k < 0) continue;

                    List<int> id = GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].grass;

                    for (int m = k; m >= k - count + 1; m--)
                    {
                        if (id.Count != 0)
                        {
                            int a = rnd.Next(0, id.Count);
                            if (m > 0) At(i, j, m).blockID = id[a];
                        }
                        else if (m > 0) At(i, j, m).blockID = 10;
                    }
                }
        }

        public void Generation_SmoothTop()
        {

        }

        /// <summary>
        /// полный пересчет значений нодов subterrain
        /// </summary>
        public void RecalcExploredSubterrain()
        {
            for (int i = 0; i <= sectn * MapSector.dimS - 1; i++)
                for (int j = 0; j <= sectn * MapSector.dimS - 1; j++)
                    for (int m = 0; m <= sectn * MapSector.dimS - 1; m++)
                    {
                        At(i, j, m).subterrain = true;
                        At(i, j, m).explored = false;
                    }

            for (int i = 1; i <= sectn * MapSector.dimS - 2; i++)
                for (int j = 1; j <= sectn * MapSector.dimS - 2; j++)
                {
                    for (int m = 0; m <= sectn * MapSector.dimS - 1; m++)
                    {
                        if (At(i, j, m).blockID == 0)
                        {
                            At(i, j, m).subterrain = false;
                            At(i, j, m).explored = true;

                            At(i + 1, j, m).subterrain = false;
                            At(i + 1, j, m).explored = true;
                            At(i, j + 1, m).subterrain = false;
                            At(i, j + 1, m).explored = true;
                            At(i - 1, j, m).subterrain = false;
                            At(i - 1, j, m).explored = true;
                            At(i, j - 1, m).subterrain = false;
                            At(i, j - 1, m).explored = true;
                        }
                        else
                        {
                            At(i, j, m).subterrain = false;
                            At(i, j, m).explored = true;
                            goto here2;
                        }
                    }
                here2: ;
                }
        }

        public float[] wshine = new float[sectn * MapSector.dimS * sectn * MapSector.dimS];
        public short[] whinenapr = new short[sectn * MapSector.dimS * sectn * MapSector.dimS];
        /// <summary>
        /// простая генерация локальный карты по данным глобальной карты (базовая версия)
        /// </summary>
        public void SimpleGeneration_bygmap()
        {
            for (int i0 = 0; i0 < wshine.GetUpperBound(0); i0++)
                {
                    wshine[i0] = (float)rnd.NextDouble();
                    whinenapr[i0] = rnd.Next(0, 1) == 0 ? (short)-1 : (short)1;
                }

            for (int i = 0; i <= sectn * MapSector.dimS - 1; i++)
                for (int j = 0; j <= sectn * MapSector.dimS - 1; j++)
                    for (int k = 0; k <= sectn * MapSector.dimS - 1; k += 8)
                    {
                        At(i, j, k).blockID = 0;
                        At(i, j, k + 1).blockID = 0;
                        At(i, j, k + 2).blockID = 0;
                        At(i, j, k + 3).blockID = 0;
                        At(i, j, k + 4).blockID = 0;
                        At(i, j, k + 5).blockID = 0;
                        At(i, j, k + 6).blockID = 0;
                        At(i, j, k + 7).blockID = 0;
                    }


            for (int i = 0; i <= sectn * MapSector.dimS - 1; i++)
                for (int j = 0; j <= sectn * MapSector.dimS - 1; j++)
                {
                    for (int k = 0; k <= ((Main.gmap.n[(int)Main.gmap_region.X + i, (int)Main.gmap_region.Y + j])) * (sectn * MapSector.dimS - 1) * 0.3 + (sectn * MapSector.dimS - 1) * 0.7 - 5; k++)
                    {
                        if (GoodVector3(i, j, sectn * MapSector.dimS - 1 - k)) At(i, j, sectn * MapSector.dimS - 1 - k).blockID = 12345;
                    }
                }

            int[] metamorf_clust = { 800, 801, 802, 803, 804, 805, 806 };
            int[] matamorf_jila = { 810, 811, 812 };

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
            int[] granite_jila = { 55, 55 };
            GenerationFullLayerCluster(11, 12, granite_clust, 10, granite_jila, 5, 20);

            Generation_FullLayer(33, 10);

            int[] marble_clust = metamorf_clust;
            int[] marble_jila = matamorf_jila;
            GenerationFullLayerCluster(22, 10, marble_clust, 10, marble_jila, 5, 20);

            GenerationFullLayerCluster(44, 4, metamorf_clust, 6, matamorf_jila, 2, 25);

            GenerationFullLayerCluster(22, 4, metamorf_clust, 6, matamorf_jila, 2, 25);

            for (int i = 0; i <= 20; i++)
            {
                Generation_FullLayer(824, 2);
                Generation_FullLayer(828, 2);
            }


            Generation_FullLayer_under(1);
            Generation_FullLayer_under_under(2);
            //Main.mmap.Generation_FullLayerTOP(ObjectID.DirtWall_Grass2, 1);

            Generation_FullLayerGrass(1);


            for (int i = 0; i <= sectn * MapSector.dimS - 1; i++)
                for (int j = 0; j <= sectn * MapSector.dimS - 1; j++)
                {
                    //if(Main.gmap.n[Main.gmap_region.X + i, Main.gmap_region.Y + j] <= 0.4)
                    for (int k = 0; k <= ((0.4)) * (sectn * MapSector.dimS - 1) * 0.3 + (sectn * MapSector.dimS - 1) * 0.7 + 1; k++)
                    {
                        if (GoodVector3(i, j, 128 - 1 - k) && At(i, j, 128 - 1 - k).blockID == 0) At(i, j, sectn * MapSector.dimS - 1 - k).blockID = KnownIDs.water;//вода
                    }
                }

            RecalcExploredSubterrain();

            Generation_PlaceOnSurface();

            Main.PrepairMapDeleteWrongIDs(ref Main.smap);
            Main.smap.RebuildAllMapGeo(0, Main.Camera);
        }
    }
}
