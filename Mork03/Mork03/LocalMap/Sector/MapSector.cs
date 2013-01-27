using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mork.Bad_Database;
using Mork.Graphics;


namespace Mork.Local_Map.Sector
{
    public class MapSector
    {
        public const byte dimS = 16, dimH = 128;

        public MNode[] N = new MNode[dimS * dimS * dimH];
        //bool[] buildedrow = new bool[dimS * dimS];
        public VertexPositionNormalTextureShade[] VertexArray = new VertexPositionNormalTextureShade[dimS * dimS * dimS * 6];
        public VertexPositionNormalTextureShade[] VertexArrayTransparent = new VertexPositionNormalTextureShade[dimS * dimS * dimS];
        public BoundingBox bounding;

        private Action<GraphicsDevice, int> GeoCaller;
        private IAsyncResult ar;

        public int index = 0;
        public int indextransparent = 0;

        public int sx, sy;

        public bool builded = true;

        //internal VertexBuffer VertexBuffer;
        public bool empty = true;

        public MapSector(int x, int y)
        {
            sx = x;
            sy = y;
            for (int i = 0; i <= dimS * dimS * dimH - 1; i++)
            {
                N[i] = new MNode() { BlockID = 0 };
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
            indextransparent = 0;

            float tsper = 1 / (Commons.TextureAtlas.X / (Commons.TextureAtlasTexSize));
            float tsperh = 1 / (Commons.TextureAtlas.Y / (Commons.TextureAtlasTexSize));

            for (int i = 0; i <= dimS - 1; i++)
                for (int j = 0; j <= dimS - 1; j++)
                {
                    for (int k = z_cam; k <= dimH - 1; k++)
                    {
                        var b = N[i * dimS * dimH + j * dimH + k];

                        float light = !b.Subterrain ? 1 : 0.5f;

                        bool invisible = true;

                        if(b.BlockID!=0 && Main.dbobject.Data[b.BlockID].NotBlock)
                        {
                            #region billb
                            if (b.Explored)
                            {
                                float umovx = (Main.dbobject.Data[b.BlockID].metatex_n%Commons.TextureAtlasWCount)*tsper*2;
                                float umovy = (Main.dbobject.Data[b.BlockID].metatex_n/Commons.TextureAtlasWCount)*tsperh;

                                float smovx = umovx + tsper;
                                float smovy = umovy;

                                LeftFaceTree(smovy, tsper, tsperh, light, j, i, smovx, k);

                                RightFaceTree(light, smovy, tsperh, tsper, j, i, smovx, k);

                                UpFaceTree(light, smovy, tsperh, tsper, j, i, smovx, k);

                                DownFaceTree(light, smovy, tsperh, tsper, j, i, smovx, k);

                            }

                            #endregion
                        }
                        else
                        {
                            #region block

                            if (k == z_cam && k != 0 && b.BlockID != 0 && !Main.dbobject.Data[N[i * dimS * dimH + j * dimH + k - 1].BlockID].Transparent)
                            // верхняя черная грань, если сверху непрозрачный блок
                            {
                                float umovx = 0;
                                float umovy = 0;

                                ForwardFaceBlack(umovy, tsperh, tsper, umovx, i, j, k);
                            }
                            else if (b.BlockID != 0 && b.Explored)
                            {
                                float umovx = (Main.dbobject.Data[b.BlockID].metatex_n % Commons.TextureAtlasWCount) * tsper * 2;
                                float umovy = (Main.dbobject.Data[b.BlockID].metatex_n / Commons.TextureAtlasWCount) * tsperh;

                                float smovx = umovx + tsper;
                                float smovy = umovy;

                                if (k == z_cam || Main.dbobject.Data[N[i * dimS * dimH + j * dimH + k - 1].BlockID].Transparent)
                                {
                                    ForwardFace(light, umovy, tsper, tsperh, j, i, umovx, k);
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

                                if (i == dimS - 1 || Main.dbobject.Data[N[(i + 1) * dimS * dimH + j * dimH + k].BlockID].Transparent)
                                {
                                    LeftFace(smovy, tsper, tsperh, light, j, i, smovx, k);
                                    invisible = false;
                                }

                                if (i == 0 || Main.dbobject.Data[N[(i - 1) * dimS * dimH + j * dimH + k].BlockID].Transparent)
                                {
                                    RightFace(light, smovy, tsperh, tsper, j, i, smovx, k);
                                    invisible = false;
                                }

                                if (j == 0 || Main.dbobject.Data[N[i * dimS * dimH + (j - 1) * dimH + k].BlockID].Transparent)
                                {
                                    UpFace(light, smovy, tsperh, tsper, j, i, smovx, k);
                                    invisible = false;
                                }

                                if (j == dimS - 1 || Main.dbobject.Data[N[i * dimS * dimH + (j + 1) * dimH + k].BlockID].Transparent)
                                {
                                    DownFace(light, smovy, tsperh, tsper, j, i, smovx, k);
                                    invisible = false;
                                }

                                if (!invisible)
                                {
                                    if (k < top) top = k;
                                    if (k > low) low = k;
                                }

                                if (invisible) goto nextrow;
                                ;
                            }

                            #endregion
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

        private void ForwardFace(float light, float umovy, float tsper, float tsperh, int j, int i, float umovx, int k)
        {
//Up face
            VertexArray[index] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + sy*dimS, -k),
                                                     Vector3.Backward,
                                                     new Vector2(umovx, umovy), light);
            VertexArray[index + 1] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + 1 + sy*dimS, -k),
                                                     Vector3.Backward,
                                                     new Vector2(umovx, umovy + tsperh), light);
            VertexArray[index + 2] =
                new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx*dimS, j + sy*dimS, -k),
                                                     Vector3.Backward,
                                                     new Vector2(umovx + tsper, umovy), light);
            VertexArray[index + 3] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, -k),
                    Vector3.Backward,
                    new Vector2(umovx + tsper, umovy + tsperh), light);
            VertexArray[index + 4] =
                new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx*dimS, j + sy*dimS, -k),
                                                     Vector3.Backward,
                                                     new Vector2(umovx + tsper, umovy), light);
            VertexArray[index + 5] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + 1 + sy*dimS, -k),
                                                     Vector3.Backward,
                                                     new Vector2(umovx, umovy + tsperh), light);

            index += 6;
        }

        private void ForwardFaceBlack(float umovy, float tsperh, float tsper, float umovx, int i, int j, int k)
        {
            VertexArray[index] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + sy*dimS, -k),
                                                     Vector3.Backward,
                                                     new Vector2(umovx, umovy), 0);
            VertexArray[index + 1] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + 1 + sy*dimS, -k),
                                                     Vector3.Backward,
                                                     new Vector2(umovx, umovy + tsperh), 0);
            VertexArray[index + 2] =
                new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx*dimS, j + sy*dimS, -k),
                                                     Vector3.Backward,
                                                     new Vector2(umovx + tsper, umovy), 0);
            VertexArray[index + 3] =
                new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, -k),
                                                     Vector3.Backward,
                                                     new Vector2(umovx + tsper, umovy + tsperh), 0);
            VertexArray[index + 4] =
                new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx*dimS, j + sy*dimS, -k),
                                                     Vector3.Backward,
                                                     new Vector2(umovx + tsper, umovy), 0);
            VertexArray[index + 5] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + 1 + sy*dimS, -k),
                                                     Vector3.Backward,
                                                     new Vector2(umovx, umovy + tsperh), 0);

            index += 6;
        }

        private void DownFace(float light, float smovy, float tsperh, float tsper, int j, int i, float smovx, int k)
        {
//Backward face
            VertexArray[index] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, -k),
                    Vector3.Down, new Vector2(smovx, smovy), light*0.8f);
            VertexArray[index + 1] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + 1 + sy*dimS, -k),
                                                     Vector3.Down,
                                                     new Vector2(smovx + tsper, smovy), light*0.8f);
            VertexArray[index + 2] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + sx*dimS, j + 1 + sy*dimS, -k - 1),
                    Vector3.Down,
                    new Vector2(smovx + tsper, smovy + tsperh), light*0.8f);
            VertexArray[index + 3] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + sx*dimS, j + 1 + sy*dimS, -k - 1),
                    Vector3.Down,
                    new Vector2(smovx + tsper, smovy + tsperh), light*0.8f);
            VertexArray[index + 4] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, -k - 1),
                    Vector3.Down, new Vector2(smovx, smovy + tsperh), light*0.8f);
            VertexArray[index + 5] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, -k),
                    Vector3.Down, new Vector2(smovx, smovy), light*0.8f);
            index += 6;
        }

        private void UpFace(float light, float smovy, float tsperh, float tsper, int j, int i, float smovx, int k)
        {
//Forward face
            VertexArray[index] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + sy*dimS, -k),
                                                     Vector3.Up, new Vector2(smovx, smovy),
                                                     light*0.8f);
            VertexArray[index + 1] =
                new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx*dimS, j + sy*dimS, -k),
                                                     Vector3.Up,
                                                     new Vector2(smovx + tsper, smovy), light*0.8f);
            VertexArray[index + 2] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx*dimS, j + sy*dimS, -k - 1),
                    Vector3.Up,
                    new Vector2(smovx + tsper, smovy + tsperh), light*0.8f);
            VertexArray[index + 3] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx*dimS, j + sy*dimS, -k - 1),
                    Vector3.Up,
                    new Vector2(smovx + tsper, smovy + tsperh), light*0.8f);
            VertexArray[index + 4] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + sy*dimS, -k - 1),
                                                     Vector3.Up,
                                                     new Vector2(smovx, smovy + tsperh), light*0.8f);
            VertexArray[index + 5] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + sy*dimS, -k),
                                                     Vector3.Up, new Vector2(smovx, smovy),
                                                     light*0.8f);

            index += 6;
        }

        private void RightFace(float light, float smovy, float tsperh, float tsper, int j, int i, float smovx, int k)
        {
//right face
            VertexArray[index] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + 1 + sy*dimS, -k),
                                                     Vector3.Right, new Vector2(smovx, smovy),
                                                     light*0.8f);
            VertexArray[index + 1] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + sy*dimS, -k),
                                                     Vector3.Right,
                                                     new Vector2(smovx + tsper, smovy), light*0.8f);
            VertexArray[index + 2] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + sy*dimS, -k - 1),
                                                     Vector3.Right,
                                                     new Vector2(smovx + tsper, smovy + tsperh),
                                                     light*0.8f);
            VertexArray[index + 3] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + sy*dimS, -k - 1),
                                                     Vector3.Right,
                                                     new Vector2(smovx + tsper, smovy + tsperh),
                                                     light*0.8f);
            VertexArray[index + 4] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + sx*dimS, j + 1 + sy*dimS, -k - 1),
                    Vector3.Right, new Vector2(smovx, smovy + tsperh), light*0.8f);
            VertexArray[index + 5] =
                new VertexPositionNormalTextureShade(new Vector3(i + sx*dimS, j + 1 + sy*dimS, -k),
                                                     Vector3.Right, new Vector2(smovx, smovy),
                                                     light*0.8f);

            index += 6;
        }

        private void LeftFace(float smovy, float tsper, float tsperh, float light, int j, int i, float smovx, int k)
        {
//left face
            VertexArray[index] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, -k),
                    Vector3.Left, new Vector2(smovx + tsper, smovy), light*0.8f);
            VertexArray[index + 1] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, -k - 1),
                    Vector3.Left, new Vector2(smovx + tsper, smovy + tsperh), light*0.8f);
            VertexArray[index + 2] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx*dimS, j + sy*dimS, -k - 1),
                    Vector3.Left, new Vector2(smovx, smovy + tsperh), light*0.8f);
            VertexArray[index + 3] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx*dimS, j + sy*dimS, -k - 1),
                    Vector3.Left, new Vector2(smovx, smovy + tsperh), light*0.8f);
            VertexArray[index + 4] =
                new VertexPositionNormalTextureShade(new Vector3(i + 1 + sx*dimS, j + sy*dimS, -k),
                                                     Vector3.Left, new Vector2(smovx, smovy),
                                                     light*0.8f);
            VertexArray[index + 5] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx*dimS, j + 1 + sy*dimS, -k),
                    Vector3.Left, new Vector2(smovx + tsper, smovy), light*0.8f);

            index += 6;
        }

        private void LeftFaceTree(float smovy, float tsper, float tsperh, float light, int j, int i, float smovx, int k)
        {
//left face
            VertexArrayTransparent[indextransparent] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 0.499f + sx*dimS, j + 1 + sy*dimS, -k + 1),
                    Vector3.Left, new Vector2(smovx + tsper, smovy),
                    light);
            VertexArrayTransparent[indextransparent + 1] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 0.499f + sx*dimS, j + 1 + sy*dimS, -k - 1),
                    Vector3.Left, new Vector2(smovx + tsper, smovy + tsperh),
                    light);
            VertexArrayTransparent[indextransparent + 2] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 0.499f + sx*dimS, j + sy*dimS, -k - 1),
                    Vector3.Left, new Vector2(smovx, smovy + tsperh),
                    light);
            VertexArrayTransparent[indextransparent + 3] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 0.499f + sx*dimS, j + sy*dimS, -k - 1),
                    Vector3.Left, new Vector2(smovx, smovy + tsperh),
                    light);
            VertexArrayTransparent[indextransparent + 4] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 0.499f + sx*dimS, j + sy*dimS, -k + 1),
                    Vector3.Left, new Vector2(smovx, smovy),
                    light);
            VertexArrayTransparent[indextransparent + 5] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 0.499f + sx*dimS, j + 1 + sy*dimS, -k + 1),
                    Vector3.Left, new Vector2(smovx + tsper, smovy),
                    light);

            indextransparent += 6;
        }

        private void DownFaceTree(float light, float smovy, float tsperh, float tsper, int j, int i, float smovx, int k)
        {
//Backward face
            VertexArrayTransparent[indextransparent] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx*dimS, j + 0.499f + sy*dimS, -k + 1),
                    Vector3.Down, new Vector2(smovx, smovy), light);
            VertexArrayTransparent[indextransparent + 1] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + sx * dimS, j + 0.499f + sy * dimS, -k + 1),
                    Vector3.Down,
                    new Vector2(smovx + tsper, smovy), light);
            VertexArrayTransparent[indextransparent + 2] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + sx * dimS, j + 0.499f + sy * dimS, -k - 1),
                    Vector3.Down,
                    new Vector2(smovx + tsper, smovy + tsperh), light);
            VertexArrayTransparent[indextransparent + 3] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + sx * dimS, j + 0.499f + sy * dimS, -k - 1),
                    Vector3.Down,
                    new Vector2(smovx + tsper, smovy + tsperh), light);
            VertexArrayTransparent[indextransparent + 4] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx * dimS, j + 0.499f + sy * dimS, -k - 1),
                    Vector3.Down, new Vector2(smovx, smovy + tsperh), light);
            VertexArrayTransparent[indextransparent + 5] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx * dimS, j + 0.499f + sy * dimS, -k + 1),
                    Vector3.Down, new Vector2(smovx, smovy), light);
            indextransparent += 6;
        }

        private void UpFaceTree(float light, float smovy, float tsperh, float tsper, int j, int i, float smovx, int k)
        {
//Forward face
            VertexArrayTransparent[indextransparent] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + sx * dimS, j + 0.5f + sy * dimS, -k + 1),
                    Vector3.Up, new Vector2(smovx, smovy),
                    light);
            VertexArrayTransparent[indextransparent + 1] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx * dimS, j + 0.5f + sy * dimS, -k + 1),
                    Vector3.Up, new Vector2(smovx + tsper, smovy),
                    light);
            VertexArrayTransparent[indextransparent + 2] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx * dimS, j + 0.5f + sy * dimS, -k - 1),
                    Vector3.Up, new Vector2(smovx + tsper, smovy + tsperh),
                    light);
            VertexArrayTransparent[indextransparent + 3] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 1 + sx * dimS, j + 0.5f + sy * dimS, -k - 1),
                    Vector3.Up, new Vector2(smovx + tsper, smovy + tsperh),
                    light);
            VertexArrayTransparent[indextransparent + 4] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + sx * dimS, j + 0.5f + sy * dimS, -k - 1),
                    Vector3.Up, new Vector2(smovx, smovy + tsperh),
                    light);
            VertexArrayTransparent[indextransparent + 5] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + sx * dimS, j + 0.5f + sy * dimS, -k + 1),
                    Vector3.Up, new Vector2(smovx, smovy),
                    light);

            indextransparent += 6;
        }

        private void RightFaceTree(float light, float smovy, float tsperh, float tsper, int j, int i, float smovx, int k)
        {
//right face
            VertexArrayTransparent[indextransparent] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 0.5f + sx*dimS, j + 1 + sy*dimS, -k + 1),
                    Vector3.Right, new Vector2(smovx, smovy),
                    light);
            VertexArrayTransparent[indextransparent + 1] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 0.5f + sx*dimS, j + sy*dimS, -k + 1),
                    Vector3.Right, new Vector2(smovx + tsper, smovy),
                    light);
            VertexArrayTransparent[indextransparent + 2] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 0.5f + sx*dimS, j + sy*dimS, -k - 1),
                    Vector3.Right,
                    new Vector2(smovx + tsper, smovy + tsperh),
                    light);
            VertexArrayTransparent[indextransparent + 3] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 0.5f + sx*dimS, j + sy*dimS, -k - 1),
                    Vector3.Right,
                    new Vector2(smovx + tsper, smovy + tsperh),
                    light);
            VertexArrayTransparent[indextransparent + 4] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 0.5f + sx*dimS, j + 1 + sy*dimS, -k - 1),
                    Vector3.Right, new Vector2(smovx, smovy + tsperh), light);
            VertexArrayTransparent[indextransparent + 5] =
                new VertexPositionNormalTextureShade(
                    new Vector3(i + 0.5f + sx*dimS, j + 1 + sy*dimS, -k + 1),
                    Vector3.Right, new Vector2(smovx, smovy),
                    light);

            indextransparent += 6;
        }
    }
}
