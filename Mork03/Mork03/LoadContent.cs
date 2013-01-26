using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mork.Bad_Database;
using Mork.Local_Map.Dynamic.Units;
using Mork.Local_Map.Sector;

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

        public static Texture2D texatlas;
        public static List<Texture2D> gears;
        private static float g1r;
        private static float g2r;
        private static float g3r;
        static float rotation;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            lineBatch = new LineBatch(GraphicsDevice);

            base.LoadContent();
            object_tex = ContentLoad(@"Textures\Objects\Blocks\Blocks");

            texatlas = Content.Load<Texture2D>(@"Textures\BlocktexAtlas");
            Commons.TextureAtlas.Y = texatlas.Height;
            Commons.TextureAtlas.X = texatlas.Width;

            var temp = new Texture2D[5];
            for (int i = 0; i <= 4; i++)
                temp[i] =
                    ContentLoad(@"Textures\Objects\OnStore\wood_log" + (i + 1));
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

            gears = new List<Texture2D>();
            gears.Add(Content.Load<Texture2D>(@"Textures\Interface\g1"));
            gears.Add(Content.Load<Texture2D>(@"Textures\Interface\g2"));
            gears.Add(Content.Load<Texture2D>(@"Textures\Interface\g3"));

            g1r = gears[0].Height / 2;
            g2r = gears[1].Height / 2;
            g3r = gears[2].Height / 2;

            Font1 = Content.Load<SpriteFont>(@"Textures\SpriteFont1");
            Font2 = Content.Load<SpriteFont>(@"Textures\SpriteFont2");


            dbmaterial = new DBMaterial(); //!!!!
            dbobject = new DB_LMO(); //!!!!
            //buildings = new Stores(); //!!!!
            gmap = new GMap(); //!!!!

            WindowsDesigner();

            var blockeffect = Content.Load<Effect>(@"Effects\SolidBlockEffect");

            smap = new SectorMap(GraphicsDevice, blockeffect);
            imap = new IntersectMap();

            lheroes = new LocalHeroes(GraphicsDevice, unit_tex[1]);

            MeTexoncurA = 1;
            MeTexoncurB = 5;
        }
    }
}
