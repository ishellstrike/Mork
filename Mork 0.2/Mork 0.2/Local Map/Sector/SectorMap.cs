using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mork.Graphics.MapEngine;
using Mork.Local_Map.Dynamic.Local_Items;

namespace Mork.Local_Map.Sector
{
    public class SectorMap
    {
        public const int sectn = 8;
        public MapSector[] n = new MapSector[sectn * sectn];
        GraphicsDevice gd;
        BasicEffect be;

        public SectorMap(GraphicsDevice _gd)
        {
            for (int i = 0; i <= sectn - 1; i++)
                for (int j = 0; j <= sectn - 1; j++)
                        n[i * sectn + j] = new MapSector(i, j);

            gd = _gd;
            be = new BasicEffect(gd);
        }

        public void RebuildAllMapGeo()
        {
            foreach(var a in n)
            {
                a.RebuildSectorGeo(gd);
            }
        }

        public MNode At(int x, int y, int z)
        {
            return
                n[x/MapSector.dimS*sectn + y/MapSector.dimS].n[
                    x%MapSector.dimS*MapSector.dimS*MapSector.dimS + y%MapSector.dimS*MapSector.dimS + z%MapSector.dimH];
        }

        public MNode At(float x, float y, float z)
        {
            return n[(int)x / MapSector.dimS * sectn + (int)y / MapSector.dimS].n[
                    (int)x % MapSector.dimS * MapSector.dimS * MapSector.dimS + (int)y % MapSector.dimS * MapSector.dimS + (int)z % MapSector.dimH];
        }

        public MNode At(Vector3 ve)
        {
            return n[(int)ve.X / MapSector.dimS * sectn + (int)ve.Y / MapSector.dimS].n[
                    (int)ve.X % MapSector.dimS * MapSector.dimS * MapSector.dimS + (int)ve.Y % MapSector.dimS * MapSector.dimS + (int)ve.Z % MapSector.dimH];
        }

        public void DrawAllMap(GameTime gt, Camera cam)
        {
            be.World = Matrix.CreateScale(10);
            be.View = cam.View;
            be.Projection = cam.Projection;
            be.AmbientLightColor = new Vector3(0.1F,0.1F,0.1F);
            be.LightingEnabled = true;
            be.AmbientLightColor = new Vector3(1,1,1);
            be.TextureEnabled = true;
            be.Texture = Main.texatlas;

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
                    if (!a.builded) a.RebuildSectorGeo(gd);
                    if (a.VertexBuffer.VertexCount != 0)
                    {
                       Main.drawed_sects++;
                       gd.SetVertexBuffer(a.VertexBuffer);
                       gd.DrawPrimitives(PrimitiveType.TriangleList, 0, a.VertexBuffer.VertexCount / 3);
                        Main.drawed_verts += a.VertexBuffer.VertexCount;
                    }
                }

                //Matrix aa = Matrix.CreateScale(10);
                //Main._teapot.Draw(aa, cam.View, cam.Projection);
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
    }
}
