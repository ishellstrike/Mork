using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mork.Bad_Database;


namespace Mork.Local_Map.Sector
{
    public class MapSector
    {
        public const byte dimS = 16, dimH = 128;

        public MNode[] n = new MNode[dimS * dimS * dimH];
        //bool[] buildedrow = new bool[dimS * dimS];
        VertexPositionNormalTexture[] VertexArray = new VertexPositionNormalTexture[dimS * dimS * dimS * 6];
        public BoundingBox bounding;

        private Action<GraphicsDevice, int> GeoCaller;
        private IAsyncResult ar;

        public int sx, sy;

        public bool builded = true;

        internal VertexBuffer VertexBuffer;
        public bool empty = true;

        public MapSector(int x, int y)
        {
            sx = x;
            sy = y;
            for (int i = 0; i <= dimS*dimS*dimH - 1; i++)
                    {
                        n[i] = new MNode(){blockID =  0};
                    }
            bounding = new BoundingBox(new Vector3(0 + sx * dimS, 0 + sy * dimS, -0), new Vector3(15 + sx * dimS, 15 + sy * dimS, -1));
        }

        public void RebuildSectorGeo(GraphicsDevice gd, int z_cam)
        {
            //if(ar != null && !ar.IsCompleted)
            //{
            //    GeoCaller.EndInvoke(ar);
            //}

            AsyRSG(gd, z_cam);
        }

        public void AsyRSG(GraphicsDevice gd, int z_cam)
        {
            int index = 0;

            int top = 127;
            int low = 0;

            float tsper = 1 / (Commons.TextureAtlas.X / (Commons.TextureAtlasTexSize));
            float tsperh = 1 / (Commons.TextureAtlas.Y / (Commons.TextureAtlasTexSize));

            for (int i = 0; i <= dimS - 1; i++)
                for (int j = 0; j <= dimS - 1; j++)
                {
                    for (int k = z_cam; k <= dimH - 1; k++)
                    {
                        var b = n[i * dimS * dimH + j * dimH + k];

                        bool invisible = true;

                        if(k==z_cam && k!= 0 && n[i * dimS * dimH + j * dimH + k - 1].blockID != 0)
                        {
                            float umovx = 0;
                            float umovy = 0;

                            VertexArray[index] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx, umovy));
                            VertexArray[index + 1] =
                                new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k),
                                                                Vector3.Up,
                                                                new Vector2(umovx, umovy + tsperh));
                            VertexArray[index + 2] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k),
                                                                Vector3.Up,
                                                                new Vector2(umovx + tsper, umovy));
                            VertexArray[index + 3] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k),
                                                                Vector3.Up,
                                                                new Vector2(umovx + tsper, umovy + tsperh));
                            VertexArray[index + 4] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k),
                                                                Vector3.Up,
                                                                new Vector2(umovx + tsper, umovy));
                            VertexArray[index + 5] =
                                new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k),
                                                                Vector3.Up,
                                                                new Vector2(umovx, umovy + tsperh));

                            index += 6;
                        }
                        else
                        if (b.blockID != 0 && b.explored)
                        {
                            float umovx = (Main.dbobject.Data[b.blockID].metatex_n%Commons.TextureAtlasWCount)*tsper*2;
                            float umovy = (Main.dbobject.Data[b.blockID].metatex_n/Commons.TextureAtlasWCount)*tsperh;

                            float smovx = umovx + tsper;
                            float smovy = umovy;

                            if (k == z_cam || n[i*dimS*dimH + j*dimH + k - 1].blockID == 0)
                            {
                                //Up face
                                VertexArray[index] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + sy*dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx, umovy));
                                VertexArray[index + 1] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + 1 + sy*dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx, umovy + tsperh));
                                VertexArray[index + 2] =
                                    new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + sy*dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx + tsper, umovy));
                                VertexArray[index + 3] =
                                    new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx + tsper, umovy + tsperh));
                                VertexArray[index + 4] =
                                    new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + sy*dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx + tsper, umovy));
                                VertexArray[index + 5] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + 1 + sy*dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx, umovy + tsperh));

                                index += 6;
                                invisible = false;
                            }

                        }

                        if (b.blockID != 0 && b.explored)
                        {

                            float umovx = (Main.dbobject.Data[b.blockID].metatex_n % Commons.TextureAtlasWCount) * tsper * 2;
                            float umovy = (Main.dbobject.Data[b.blockID].metatex_n / Commons.TextureAtlasWCount) * tsperh;

                            float smovx = umovx + tsper;
                            float smovy = umovy;

                         if (i == dimS - 1 || n[(i + 1) * dimS * dimH + j * dimH + k].blockID == 0)
                            {
                                //left face
                                VertexArray[index] =
                                    new VertexPositionNormalTexture(new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Left, new Vector2(smovx + tsper, smovy));
                                VertexArray[index + 1] =
                                    new VertexPositionNormalTexture(
                                        new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k - 1),
                                        Vector3.Left, new Vector2(smovx + tsper, smovy + tsperh));
                                VertexArray[index + 2] =
                                    new VertexPositionNormalTexture(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Left, new Vector2(smovx, smovy + tsperh));
                                VertexArray[index + 3] =
                                    new VertexPositionNormalTexture(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Left, new Vector2(smovx, smovy + tsperh));
                                VertexArray[index + 4] =
                                    new VertexPositionNormalTexture(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Left, new Vector2(smovx, smovy));
                                VertexArray[index + 5] =
                                    new VertexPositionNormalTexture(new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Left, new Vector2(smovx + tsper, smovy));

                                index += 6;
                                invisible = false;
                            }

                            if (i == 0 || n[(i - 1) * dimS * dimH + j * dimH + k].blockID == 0)
                            {
                                //right face
                                VertexArray[index] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Right, new Vector2(smovx, smovy));
                                VertexArray[index + 1] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Right,
                                                                    new Vector2(smovx + tsper, smovy));
                                VertexArray[index + 2] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Right,
                                                                    new Vector2(smovx + tsper, smovy + tsperh));
                                VertexArray[index + 3] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Right,
                                                                    new Vector2(smovx + tsper, smovy + tsperh));
                                VertexArray[index + 4] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k - 1),
                                                                    Vector3.Right, new Vector2(smovx, smovy + tsperh));
                                VertexArray[index + 5] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Right, new Vector2(smovx, smovy));

                                index += 6;
                                invisible = false;
                            }

                            if (j == 0 || n[i * dimS * dimH + (j - 1) * dimH + k].blockID == 0)
                            {
                                //Forward face
                                VertexArray[index] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Forward, new Vector2(smovx, smovy));
                                VertexArray[index + 1] =
                                    new VertexPositionNormalTexture(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Forward, new Vector2(smovx + tsper, smovy));
                                VertexArray[index + 2] =
                                    new VertexPositionNormalTexture(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Forward,
                                                                    new Vector2(smovx + tsper, smovy + tsperh));
                                VertexArray[index + 3] =
                                    new VertexPositionNormalTexture(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Forward,
                                                                    new Vector2(smovx + tsper, smovy + tsperh));
                                VertexArray[index + 4] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Forward, new Vector2(smovx, smovy + tsperh));
                                VertexArray[index + 5] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Forward, new Vector2(smovx, smovy));

                                index += 6;
                                invisible = false;
                            }

                            if (j == dimS - 1 || n[i * dimS * dimH + (j + 1) * dimH + k].blockID == 0)
                            {
                                //Backward face
                                VertexArray[index] =
                                    new VertexPositionNormalTexture(new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Forward, new Vector2(smovx, smovy));
                                VertexArray[index + 1] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Forward, new Vector2(smovx + tsper, smovy));
                                VertexArray[index + 2] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k - 1),
                                                                    Vector3.Forward,
                                                                    new Vector2(smovx + tsper, smovy + tsperh));
                                VertexArray[index + 3] =
                                    new VertexPositionNormalTexture(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k - 1),
                                                                    Vector3.Forward,
                                                                    new Vector2(smovx + tsper, smovy + tsperh));
                                VertexArray[index + 4] =
                                    new VertexPositionNormalTexture(
                                        new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k - 1),
                                        Vector3.Forward, new Vector2(smovx, smovy + tsperh));
                                VertexArray[index + 5] =
                                    new VertexPositionNormalTexture(new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Forward, new Vector2(smovx, smovy));
                                index += 6;
                                invisible = false;
                            }

                            if(!invisible)
                            {
                                if (k < top) top = k;
                                if (k > low) low = k;
                            }

                            if(invisible) goto nextrow;
                            ;
                        }
                    }
                nextrow:
                    ;
                }

            builded = true;

            if (index != 0)
            {
                VertexBuffer = new VertexBuffer(gd, typeof(VertexPositionNormalTexture), index, BufferUsage.WriteOnly);
                VertexBuffer.SetData(VertexArray, 0, index);
                empty = false;
                bounding = new BoundingBox(new Vector3(0 + sx * dimS, 0 + sy * dimS, -top), new Vector3(16 + sx * dimS, 16 + sy * dimS, -low));
            }
            else
            {
                empty = true;
                bounding = new BoundingBox(new Vector3(), new Vector3());
            }
        }
    }
}
