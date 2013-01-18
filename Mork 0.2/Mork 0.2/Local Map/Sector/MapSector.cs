using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mork.Bad_Database;


namespace Mork.Local_Map.Sector
{
    [Serializable]
    public struct VertexPositionNormalTextureShade : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinate;
        public float Shade;
        public VertexPositionNormalTextureShade(Vector3 position, Vector3 normal, Vector2 textureCoordinate, float shade)
        {
            Position = position;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
            Shade = shade;
        }

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0,VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float)*3,VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(sizeof(float)*6,VertexElementFormat.Vector2,  VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(float)*8,VertexElementFormat.Single,  VertexElementUsage.TextureCoordinate, 1));

        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; }}

        public static int SizeInBytes { get { return sizeof(float) * 9; } }
    }

    public class MapSector
    {
        public const byte dimS = 16, dimH = 128;

        public MNode[] N = new MNode[dimS * dimS * dimH];
        //bool[] buildedrow = new bool[dimS * dimS];
        public VertexPositionNormalTextureShade[] VertexArray = new VertexPositionNormalTextureShade[dimS * dimS * dimS * 3];
        public BoundingBox bounding;

        private Action<GraphicsDevice, int> GeoCaller;
        private IAsyncResult ar;

        public int index = 0;

        public int sx, sy;

        public bool builded = true;

        //internal VertexBuffer VertexBuffer;
        public bool empty = true;

        public MapSector(int x, int y)
        {
            sx = x;
            sy = y;
            for (int i = 0; i <= dimS*dimS*dimH - 1; i++)
                    {
                        N[i] = new MNode(){BlockID =  0};
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

            int top = 12345;
            int low = z_cam;

            index = 0;

            float tsper = 1 / (Commons.TextureAtlas.X / (Commons.TextureAtlasTexSize));
            float tsperh = 1 / (Commons.TextureAtlas.Y / (Commons.TextureAtlasTexSize));

            for (int i = 0; i <= dimS - 1; i++)
                for (int j = 0; j <= dimS - 1; j++)
                {
                    for (int k = z_cam; k <= dimH - 1; k++)
                    {
                        var b = N[i * dimS * dimH + j * dimH + k];

                        bool invisible = true;

                        if(k==z_cam && k!= 0 && N[i * dimS * dimH + j * dimH + k - 1].BlockID != 0)
                        {
                            float umovx = 0;
                            float umovy = 0;

                            VertexArray[index] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx, umovy), 0);
                            VertexArray[index + 1] =
                                new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k),
                                                                Vector3.Up,
                                                                new Vector2(umovx, umovy + tsperh),0);
                            VertexArray[index + 2] =
                                new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k),
                                                                Vector3.Up,
                                                                new Vector2(umovx + tsper, umovy),0);
                            VertexArray[index + 3] =
                                new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k),
                                                                Vector3.Up,
                                                                new Vector2(umovx + tsper, umovy + tsperh),0);
                            VertexArray[index + 4] =
                                new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k),
                                                                Vector3.Up,
                                                                new Vector2(umovx + tsper, umovy),0);
                            VertexArray[index + 5] =
                                new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k),
                                                                Vector3.Up,
                                                                new Vector2(umovx, umovy + tsperh),0);

                            index += 6;
                        }
                        else
                        if (b.BlockID != 0 && b.Explored)
                        {
                            float umovx = (Main.dbobject.Data[b.BlockID].metatex_n%Commons.TextureAtlasWCount)*tsper*2;
                            float umovy = (Main.dbobject.Data[b.BlockID].metatex_n/Commons.TextureAtlasWCount)*tsperh;

                            float smovx = umovx + tsper;
                            float smovy = umovy;

                            if (k == z_cam || N[i*dimS*dimH + j*dimH + k - 1].BlockID == 0)
                            {
                                //Up face
                                VertexArray[index] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx, umovy),1);
                                VertexArray[index + 1] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx, umovy + tsperh),1);
                                VertexArray[index + 2] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx + tsper, umovy),1);
                                VertexArray[index + 3] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx + tsper, umovy + tsperh),1);
                                VertexArray[index + 4] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx + tsper, umovy),1);
                                VertexArray[index + 5] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Up,
                                                                    new Vector2(umovx, umovy + tsperh),1);

                                index += 6;
                                invisible = false;
                            }

                            if (!invisible)
                            {
                                if (k < top) top = k;
                                if (k > low) low = k;
                            }

                        }

                        if (b.BlockID != 0 && b.Explored)
                        {

                            float umovx = (Main.dbobject.Data[b.BlockID].metatex_n % Commons.TextureAtlasWCount) * tsper * 2;
                            float umovy = (Main.dbobject.Data[b.BlockID].metatex_n / Commons.TextureAtlasWCount) * tsperh;

                            float smovx = umovx + tsper;
                            float smovy = umovy;

                         if (i == dimS - 1 || N[(i + 1) * dimS * dimH + j * dimH + k].BlockID == 0)
                            {
                                //left face
                                VertexArray[index] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Left, new Vector2(smovx + tsper, smovy),0.8f);
                                VertexArray[index + 1] =
                                    new VertexPositionNormalTextureShade(
                                        new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k - 1),
                                        Vector3.Left, new Vector2(smovx + tsper, smovy + tsperh), 0.8f);
                                VertexArray[index + 2] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Left, new Vector2(smovx, smovy + tsperh), 0.8f);
                                VertexArray[index + 3] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Left, new Vector2(smovx, smovy + tsperh), 0.8f);
                                VertexArray[index + 4] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Left, new Vector2(smovx, smovy), 0.8f);
                                VertexArray[index + 5] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Left, new Vector2(smovx + tsper, smovy), 0.8f);

                                index += 6;
                                invisible = false;
                            }

                            if (i == 0 || N[(i - 1) * dimS * dimH + j * dimH + k].BlockID == 0)
                            {
                                //right face
                                VertexArray[index] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Right, new Vector2(smovx, smovy), 0.8f);
                                VertexArray[index + 1] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Right,
                                                                    new Vector2(smovx + tsper, smovy), 0.8f);
                                VertexArray[index + 2] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Right,
                                                                    new Vector2(smovx + tsper, smovy + tsperh), 0.8f);
                                VertexArray[index + 3] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Right,
                                                                    new Vector2(smovx + tsper, smovy + tsperh), 0.8f);
                                VertexArray[index + 4] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k - 1),
                                                                    Vector3.Right, new Vector2(smovx, smovy + tsperh), 0.8f);
                                VertexArray[index + 5] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Right, new Vector2(smovx, smovy), 0.8f);

                                index += 6;
                                invisible = false;
                            }

                            if (j == 0 || N[i * dimS * dimH + (j - 1) * dimH + k].BlockID == 0)
                            {
                                //Forward face
                                VertexArray[index] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Forward, new Vector2(smovx, smovy), 0.8f);
                                VertexArray[index + 1] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Forward, new Vector2(smovx + tsper, smovy), 0.8f);
                                VertexArray[index + 2] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Forward,
                                                                    new Vector2(smovx + tsper, smovy + tsperh), 0.8f);
                                VertexArray[index + 3] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Forward,
                                                                    new Vector2(smovx + tsper, smovy + tsperh), 0.8f);
                                VertexArray[index + 4] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + sy * dimS, -k - 1),
                                                                    Vector3.Forward, new Vector2(smovx, smovy + tsperh), 0.8f);
                                VertexArray[index + 5] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + sy * dimS, -k),
                                                                    Vector3.Forward, new Vector2(smovx, smovy), 0.8f);

                                index += 6;
                                invisible = false;
                            }

                            if (j == dimS - 1 || N[i * dimS * dimH + (j + 1) * dimH + k].BlockID == 0)
                            {
                                //Backward face
                                VertexArray[index] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Forward, new Vector2(smovx, smovy), 0.8f);
                                VertexArray[index + 1] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Forward, new Vector2(smovx + tsper, smovy), 0.8f);
                                VertexArray[index + 2] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k - 1),
                                                                    Vector3.Forward,
                                                                    new Vector2(smovx + tsper, smovy + tsperh), 0.8f);
                                VertexArray[index + 3] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + sx * dimS, j + 1 + sy * dimS, -k - 1),
                                                                    Vector3.Forward,
                                                                    new Vector2(smovx + tsper, smovy + tsperh), 0.8f);
                                VertexArray[index + 4] =
                                    new VertexPositionNormalTextureShade(
                                        new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k - 1),
                                        Vector3.Forward, new Vector2(smovx, smovy + tsperh), 0.8f);
                                VertexArray[index + 5] =
                                    new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx * dimS, j + 1 + sy * dimS, -k),
                                                                    Vector3.Forward, new Vector2(smovx, smovy), 0.8f);
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
                //VertexBuffer = new VertexBuffer(gd, typeof(VertexPositionNormalTextureShade), index, BufferUsage.WriteOnly);
                //VertexBuffer.SetData(VertexArray, 0, index);
                empty = false;
                if (top == 12345) top = z_cam + 1;
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
