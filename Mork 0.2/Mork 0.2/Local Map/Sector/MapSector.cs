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
        const byte dimS = 16, dimH = 128;

        MNode[] data = new MNode[dimS * dimS * dimH];
        bool[] buildedrow = new bool[dimS * dimS];

        public int sx, sy;

        public bool builded;

        internal VertexBuffer VertexBuffer;

        public MapSector(int x, int y)
        {
            sx = x;
            sy = y;
            for (int i = 0; i <= dimS*dimS*dimH - 1; i++)
                    {
                        data[i] = new MNode();
                        data[i].blockID = i%3 == 0 ? 1 : 0;
                    }
        }

        public void RebuildSectorGeo(GraphicsDevice gd)
        {
            VertexPositionNormalTexture[] VertexArray = new VertexPositionNormalTexture[dimS * dimS * dimS * 6];

            int index = 0;

            float tsper = 1/(Commons.TextureAtlas.X/Commons.TextureAtlasTexSize);

            for (int i = 0; i <= dimS - 1;i++ )
                for (int j = 0; j <= dimS - 1; j++)
                {
                    for (int k = 0; k <= dimH - 1; k++)
                    {
                        int bid = data[i*dimS*dimS + j*dimS + k].blockID;
                        if (bid != 0)
                        {
                            float umovx = (Main.dbobject.Data[bid].metatex_n%Commons.TextureAtlasWCount) * tsper * 2;
                            float umovy = (Main.dbobject.Data[bid].metatex_n /Commons.TextureAtlasWCount) * tsper;

                            float smovx = umovx + tsper;
                            float smovy = umovy;

                            //Up face
                            VertexArray[index] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + sy*dimS, k), Vector3.Up,
                                                                new Vector2(umovx, umovy));
                            VertexArray[index + 1] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + 1 + sy*dimS, k), Vector3.Up,
                                                                new Vector2(umovx, umovy + tsper));
                            VertexArray[index + 2] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + sy*dimS, k), Vector3.Up,
                                                                new Vector2(umovx + tsper, umovy));
                            VertexArray[index + 3] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, k),
                                                                Vector3.Up, new Vector2(umovx + tsper, umovy + tsper));
                            VertexArray[index + 4] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + sy*dimS, k), Vector3.Up,
                                                                new Vector2(umovx + tsper, umovy));
                            VertexArray[index + 5] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + 1 + sy*dimS, k), Vector3.Up,
                                                                new Vector2(umovx, umovy + tsper));

                            index += 6;

                            //left face
                            VertexArray[index] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, k),
                                                                Vector3.Left, new Vector2(smovx + tsper, smovy));
                            VertexArray[index + 1] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, k - 1),
                                                                Vector3.Left, new Vector2(smovx + tsper, smovy + tsper));
                            VertexArray[index + 2] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + sy*dimS, k - 1),
                                                                Vector3.Left, new Vector2(smovx, smovy + tsper));
                            VertexArray[index + 3] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + sy*dimS, k - 1),
                                                                Vector3.Left, new Vector2(smovx, smovy + tsper));
                            VertexArray[index + 4] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + sy*dimS, k),
                                                                Vector3.Left, new Vector2(smovx, smovy));
                            VertexArray[index + 5] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, k),
                                                                Vector3.Left, new Vector2(smovx + tsper, smovy));

                            index += 6;

                            //right face
                            VertexArray[index] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + 1 + sy*dimS, k),
                                                                Vector3.Right, new Vector2(smovx, smovy));
                            VertexArray[index + 1] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + sy*dimS, k), Vector3.Right,
                                                                new Vector2(smovx + tsper, smovy));
                            VertexArray[index + 2] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + sy*dimS, k - 1),
                                                                Vector3.Right, new Vector2(smovx + tsper, smovy + tsper));
                            VertexArray[index + 3] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + sy*dimS, k - 1),
                                                                Vector3.Right, new Vector2(smovx + tsper, smovy + tsper));
                            VertexArray[index + 4] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + 1 + sy*dimS, k - 1),
                                                                Vector3.Right, new Vector2(smovx, smovy + tsper));
                            VertexArray[index + 5] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + 1 + sy*dimS, k),
                                                                Vector3.Right, new Vector2(smovx, smovy));

                            index += 6;

                            //Forward face
                            VertexArray[index] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + sy*dimS, k),
                                                                Vector3.Forward, new Vector2(smovx, smovy));
                            VertexArray[index + 1] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + sy*dimS, k),
                                                                Vector3.Forward, new Vector2(smovx + tsper, smovy));
                            VertexArray[index + 2] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + sy*dimS, k - 1),
                                                                Vector3.Forward, new Vector2(smovx + tsper, smovy + tsper));
                            VertexArray[index + 3] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + sy*dimS, k - 1),
                                                                Vector3.Forward, new Vector2(smovx + tsper, smovy + tsper));
                            VertexArray[index + 4] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + sy*dimS, k - 1),
                                                                Vector3.Forward, new Vector2(smovx, smovy + tsper));
                            VertexArray[index + 5] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + sy*dimS, k),
                                                                Vector3.Forward, new Vector2(smovx, smovy));

                            index += 6;

                            //Backward face
                            VertexArray[index] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, k),
                                                                Vector3.Forward, new Vector2(smovx, smovy));
                            VertexArray[index + 1] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + 1 + sy*dimS, k),
                                                                Vector3.Forward, new Vector2(smovx + tsper, smovy));
                            VertexArray[index + 2] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + 1 + sy*dimS, k - 1),
                                                                Vector3.Forward, new Vector2(smovx + tsper, smovy + tsper));
                            VertexArray[index + 3] =
                                new VertexPositionNormalTexture(new Vector3(i + sx*dimS, j + 1 + sy*dimS, k - 1),
                                                                Vector3.Forward, new Vector2(smovx + tsper, smovy + tsper));
                            VertexArray[index + 4] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, k - 1),
                                                                Vector3.Forward, new Vector2(smovx, smovy + tsper));
                            VertexArray[index + 5] =
                                new VertexPositionNormalTexture(new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, k),
                                                                Vector3.Forward, new Vector2(smovx, smovy));
                            index += 6;

                            goto nextrow;
                            ;
                        }
                    }
                    nextrow:
                    ;
                }



            VertexBuffer = new VertexBuffer(gd, typeof(VertexPositionNormalTexture), VertexArray.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(VertexArray);
            builded = true;
        }
    }
}
