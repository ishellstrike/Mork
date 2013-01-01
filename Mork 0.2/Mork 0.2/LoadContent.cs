using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mork.Generators;
using Mork.Local_Map;
using Mork.Local_Map.Sector;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Formu = System.Windows.Forms;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Mork
{
    public partial class Main : Game
    {
        private void LoadTexture2D(List<Texture2D> a, string s)
        {
            a.Add(Content.Load<Texture2D>(s));
            a[a.Count - 1].Name = s;
        }

        private Texture2D ContentLoad(string s)
        {
            var temp = Content.Load<Texture2D>(s);
            temp.Name = s;
            return temp;
        }

        public static Model _teapot; 

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            lineBatch = new LineBatch(GraphicsDevice);

            base.LoadContent();
            object_tex = ContentLoad(@"Textures\Objects\Blocks\Blocks");

            _teapot = Content.Load<Model>(@"teapot");

            var temp = new Texture2D[5];
            for (int i = 0; i <= 4; i++)
                temp[i] =
                    ContentLoad(@"Textures\Objects\OnStore\wood_log" + (i + 1).ToString(CultureInfo.InvariantCulture));
            onstore_tex.Add(OnStoreTexes.Wood_log, temp);

            temp = new Texture2D[5];
            for (int i = 0; i <= 4; i++) temp[i] = ContentLoad(@"Textures\Objects\OnStore\stone" + (i + 1));
            onstore_tex.Add(OnStoreTexes.Stone, temp);

            LoadTexture2D(unit_tex, @"Textures\transparent_pixel"); //0
            LoadTexture2D(unit_tex, @"Textures\Units\human1"); //1
            LoadTexture2D(unit_tex, @"Textures\Units\human2"); //2
            LoadTexture2D(unit_tex, @"Textures\Units\human3"); //3

            LoadTexture2D(interface_tex, @"Textures\Interface\Selector2"); //0
            LoadTexture2D(interface_tex, @"Textures\Interface\PointBar"); //1
            LoadTexture2D(interface_tex, @"Textures\Interface\Selector3_1"); //2
            LoadTexture2D(interface_tex, @"Textures\Interface\Selector3_2"); //3
            LoadTexture2D(interface_tex, @"Textures\Interface\dig"); //4
            LoadTexture2D(interface_tex, @"Textures\Interface\digpit"); //5
            LoadTexture2D(interface_tex, @"Textures\Interface\Int_up"); //6
            LoadTexture2D(interface_tex, @"Textures\Interface\Int_10x50"); //7
            LoadTexture2D(interface_tex, @"Textures\Interface\Int_10x100"); //8
            LoadTexture2D(interface_tex, @"Textures\Interface\Int_1x15"); //9
            LoadTexture2D(interface_tex, @"Textures\Interface\Int_left"); //10
            LoadTexture2D(interface_tex, @"Textures\Interface\Int_10x10"); //11
            LoadTexture2D(interface_tex, @"Textures\Interface\Int_5x5"); //12
            LoadTexture2D(interface_tex, @"Textures\Interface\Int_5x5cross"); //13
            LoadTexture2D(interface_tex, @"Textures\Interface\Int_5x5pike"); //14
            LoadTexture2D(interface_tex, @"Textures\Interface\mork1"); //15
            LoadTexture2D(interface_tex, @"Textures\Interface\Saving"); //16
            LoadTexture2D(interface_tex, @"Textures\Interface\Building_41x28_16fr"); //17
            LoadTexture2D(interface_tex, @"Textures\Interface\build"); //18
            LoadTexture2D(interface_tex, @"Textures\Interface\supply"); //19

            Font1 = Content.Load<SpriteFont>(@"Textures\SpriteFont1");
            Font2 = Content.Load<SpriteFont>(@"Textures\SpriteFont2");


            dbmaterial = new DBMaterial(); //!!!!
            dbobject = new DB_LMO(); //!!!!
            //buildings = new Stores(); //!!!!
            gmap = new GMap(); //!!!!

            WindowsDesigner();

            smap = new SectorMap(GraphicsDevice);

            for (int i = 0; i <= MMap.mx - 1; i++)
                for (int j = 0; j <= MMap.my - 1; j++)
                    for (int k = 0; k <= MMap.mz - 1; k += 16)
                    {
                        mmap.n[i, j, k] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 1] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 2] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 3] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 4] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 5] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 6] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 7] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 8] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 9] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 10] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 11] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 12] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 13] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 14] = new MNode {blockID = 0};

                        mmap.n[i, j, k + 15] = new MNode {blockID = 0};
                    }

            MeTexoncurA = 1;
            MeTexoncurB = 5;
        }
    }
}
