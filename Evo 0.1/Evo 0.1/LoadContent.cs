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

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            lineBatch = new LineBatch(GraphicsDevice);

            _curTex = Content.Load<Texture2D>(@"Textures\Cur2");

            //_f = Control.FromHandle(Window.Handle) as Form;
            //if (_f != null)
            //{
            //    _f.FormClosing += f_FormClosing;
            //}
            base.LoadContent();

            //ground_tex = ContentLoad(@"Textures\Blocks");

            //ground_tex.Add(GroundTexes.None, ContentLoad(@"Textures\transparent_pixel"));
            //ground_tex.Add(GroundTexes.IsoDirt1, ContentLoad(@"Textures\Ground\iso_dirt1"));
            //ground_tex.Add(GroundTexes.IsoSand1, ContentLoad(@"Textures\Ground\iso_sand1"));
            //ground_tex.Add(GroundTexes.IsoSandwall, ContentLoad(@"Textures\Ground\iso_sandwall"));
            //ground_tex.Add(GroundTexes.IsoGround1, ContentLoad(@"Textures\Ground\iso_ground_1"));
            //ground_tex.Add(GroundTexes.IsoGround2, ContentLoad(@"Textures\Ground\iso_ground_2"));
            //ground_tex.Add(GroundTexes.IsoGround3, ContentLoad(@"Textures\Ground\iso_ground_3"));
            //ground_tex.Add(GroundTexes.IsoGround4, ContentLoad(@"Textures\Ground\iso_ground_4"));
            //ground_tex.Add(GroundTexes.IsoGround5, ContentLoad(@"Textures\Ground\iso_ground_5"));
            //ground_tex.Add(GroundTexes.IsoGround6, ContentLoad(@"Textures\Ground\iso_ground_6"));
            //ground_tex.Add(GroundTexes.IsoGround7, ContentLoad(@"Textures\Ground\iso_ground_7"));
            //ground_tex.Add(GroundTexes.IsoGround8, ContentLoad(@"Textures\Ground\iso_ground_8"));
            //ground_tex.Add(GroundTexes.IsoGround9, ContentLoad(@"Textures\Ground\iso_ground_9"));
            //ground_tex.Add(GroundTexes.IsoGround10, ContentLoad(@"Textures\Ground\iso_ground_10"));
            //ground_tex.Add(GroundTexes.IsoGrass1, ContentLoad(@"Textures\Ground\iso_grass_1"));
            //ground_tex.Add(GroundTexes.IsoGrass2, ContentLoad(@"Textures\Ground\iso_grass_2"));
            //ground_tex.Add(GroundTexes.IsoGrass3, ContentLoad(@"Textures\Ground\iso_grass_3"));
            //ground_tex.Add(GroundTexes.IsoGrass4, ContentLoad(@"Textures\Ground\iso_grass_4"));
            //ground_tex.Add(GroundTexes.IsoGrass5, ContentLoad(@"Textures\Ground\iso_grass_5"));
            //ground_tex.Add(GroundTexes.iso_grass_6, ContentLoad(@"Textures\Ground\iso_grass_6"));
            //ground_tex.Add(GroundTexes.iso_grass_7, ContentLoad(@"Textures\Ground\iso_grass_7"));
            //ground_tex.Add(GroundTexes.iso_grass_8, ContentLoad(@"Textures\Ground\iso_grass_8"));
            //ground_tex.Add(GroundTexes.iso_grass_9, ContentLoad(@"Textures\Ground\iso_grass_9"));
            //ground_tex.Add(GroundTexes.iso_grass_10, ContentLoad(@"Textures\Ground\iso_grass_10"));
            //ground_tex.Add(GroundTexes.iso_water_base, ContentLoad(@"Textures\Ground\iso_water_base"));
            //ground_tex.Add(GroundTexes.iso_water_new1, ContentLoad(@"Textures\Ground\iso_water_new1"));
            //ground_tex.Add(GroundTexes.iso_water_new2, ContentLoad(@"Textures\Ground\iso_water_new2"));
            //ground_tex.Add(GroundTexes.iso_water_new3, ContentLoad(@"Textures\Ground\iso_water_new3"));
            //ground_tex.Add(GroundTexes.iso_water_base1, ContentLoad(@"Textures\Ground\iso_water_base1"));
            //ground_tex.Add(GroundTexes.iso_water_base2, ContentLoad(@"Textures\Ground\iso_water_base2"));
            //ground_tex.Add(GroundTexes.iso_water_base3, ContentLoad(@"Textures\Ground\iso_water_base3"));
            //ground_tex.Add(GroundTexes.iso_water_base4, ContentLoad(@"Textures\Ground\iso_water_base4"));
            //ground_tex.Add(GroundTexes.iso_water_base5, ContentLoad(@"Textures\Ground\iso_water_base5"));
            //ground_tex.Add(GroundTexes.iso_water_base6, ContentLoad(@"Textures\Ground\iso_water_base6"));
            //ground_tex.Add(GroundTexes.iso_water_base7, ContentLoad(@"Textures\Ground\iso_water_base7"));
            //ground_tex.Add(GroundTexes.iso_water_base8, ContentLoad(@"Textures\Ground\iso_water_base8"));
            //ground_tex.Add(GroundTexes.iso_water_base9, ContentLoad(@"Textures\Ground\iso_water_base9"));
            //ground_tex.Add(GroundTexes.iso_water_base10, ContentLoad(@"Textures\Ground\iso_water_base10"));
            //ground_tex.Add(GroundTexes.iso_water_base11, ContentLoad(@"Textures\Ground\iso_water_base11"));
            //ground_tex.Add(GroundTexes.iso_water_base12, ContentLoad(@"Textures\Ground\iso_water_base12"));
            //ground_tex.Add(GroundTexes.iso_water_base13, ContentLoad(@"Textures\Ground\iso_water_base13"));


            object_tex = ContentLoad(@"Textures\Objects\Blocks\Blocks");
            //object_tex.Add(ObjectTexes.None, ContentLoad(@"Textures\transparent_pixel"));
            //object_tex.Add(ObjectTexes.Tree1, ContentLoad(@"Textures\Objects\tree1"));
            //object_tex.Add(ObjectTexes.BlackSlope_se, ContentLoad(@"Textures\Objects\iso_blankslope_se"));
            //object_tex.Add(ObjectTexes.BlackSlope_sw, ContentLoad(@"Textures\Objects\iso_blankslope_sw"));
            //object_tex.Add(ObjectTexes.Bereza, ContentLoad(@"Textures\Objects\bereza"));
            //object_tex.Add(ObjectTexes.Lipa, ContentLoad(@"Textures\Objects\Lime"));
            //object_tex.Add(ObjectTexes.Yasen, ContentLoad(@"Textures\Objects\yasen"));
            //object_tex.Add(ObjectTexes.DirtWall_Grass3, ContentLoad(@"Textures\Objects\Blocks\grasswall"));
            //object_tex.Add(ObjectTexes.DirtWall_Grass2, ContentLoad(@"Textures\Objects\Blocks\grasswall_less"));
            //object_tex.Add(ObjectTexes.DirtWall_Grass1, ContentLoad(@"Textures\Objects\Blocks\grasswall_lesser"));
            //object_tex.Add(ObjectTexes.DirtWall, ContentLoad(@"Textures\Objects\Blocks\dirtwall"));
            //object_tex.Add(ObjectTexes.GabroWall, ContentLoad(@"Textures\Objects\Blocks\gabbro_wall"));
            //object_tex.Add(ObjectTexes.GraniteToGabroWall,
            //               ContentLoad(@"Textures\Objects\Blocks\granite_to_gabbro_wall"));
            //object_tex.Add(ObjectTexes.RyoliteWall, ContentLoad(@"Textures\Objects\Blocks\Ryolite_wall"));
            //object_tex.Add(ObjectTexes.GreenGraniteWall, ContentLoad(@"Textures\Objects\Blocks\green_granite_wall"));
            //object_tex.Add(ObjectTexes.WhiteGraniteWall, ContentLoad(@"Textures\Objects\Blocks\white_granite_wall"));
            //object_tex.Add(ObjectTexes.BasaltWall, ContentLoad(@"Textures\Objects\Blocks\Basalt_wall"));
            //object_tex.Add(ObjectTexes.BlankWall, ContentLoad(@"Textures\Ground\iso_sandwall"));
            //object_tex.Add(ObjectTexes.BasaltWallHot1, ContentLoad(@"Textures\Objects\Blocks\Basalt_wall_t1"));
            //object_tex.Add(ObjectTexes.BasaltWallHot2, ContentLoad(@"Textures\Objects\Blocks\Basalt_wall_t2"));
            //object_tex.Add(ObjectTexes.BasaltWallHot3, ContentLoad(@"Textures\Objects\Blocks\Basalt_wall_t3"));
            //object_tex.Add(ObjectTexes.Magma, ContentLoad(@"Textures\Objects\Blocks\magma"));
            //object_tex.Add(ObjectTexes.Mudstone, ContentLoad(@"Textures\Objects\Blocks\Mudstone"));
            //object_tex.Add(ObjectTexes.DirtWall_gorez, ContentLoad(@"Textures\Objects\Blocks\grasswall_gorez"));
            //object_tex.Add(ObjectTexes.DirtWall_tavolga, ContentLoad(@"Textures\Objects\Blocks\grasswall_tawolga"));
            //object_tex.Add(ObjectTexes.DirtWall_mint, ContentLoad(@"Textures\Objects\Blocks\grasswall_mint"));
            //object_tex.Add(ObjectTexes.DirtWall_blackosot, ContentLoad(@"Textures\Objects\Blocks\grasswall_blackosot"));
            //object_tex.Add(ObjectTexes.DirtWall_poleviza, ContentLoad(@"Textures\Objects\Blocks\grasswall_poleviza"));
            //object_tex.Add(ObjectTexes.DirtWall_kovil, ContentLoad(@"Textures\Objects\Blocks\grasswall_kovil"));
            //object_tex.Add(ObjectTexes.Store_wall, ContentLoad(@"Textures\Objects\Blocks\Store_wall"));
            //object_tex.Add(ObjectTexes.DirtWall_snow, ContentLoad(@"Textures\Objects\Blocks\grasswall_snow"));
            //object_tex.Add(ObjectTexes.DirtWall_snow_less, ContentLoad(@"Textures\Objects\Blocks\grasswall_snow_less"));
            //object_tex.Add(ObjectTexes.DirtWall_sporish, ContentLoad(@"Textures\Objects\Blocks\grasswall_sporish"));
            //object_tex.Add(ObjectTexes.DirtWall_ivanchai, ContentLoad(@"Textures\Objects\Blocks\grasswall_ivanchai"));
            //object_tex.Add(ObjectTexes.DirtWall_klever, ContentLoad(@"Textures\Objects\Blocks\grasswall_klever"));
            //object_tex.Add(ObjectTexes.DirtWall_zveroboi, ContentLoad(@"Textures\Objects\Blocks\grasswall_zveroboi"));
            //object_tex.Add(ObjectTexes.DirtWall_belogolov, ContentLoad(@"Textures\Objects\Blocks\grasswall_belogolov"));
            //object_tex.Add(ObjectTexes.SandWall, ContentLoad(@"Textures\Objects\Blocks\Sand_wall"));
            //object_tex.Add(ObjectTexes.Carpentery, ContentLoad(@"Textures\Objects\Blocks\Carpentery"));
            //object_tex.Add(ObjectTexes.Ortoclaz, ContentLoad(@"Textures\Objects\Blocks\ortoclaz_wall"));
            //object_tex.Add(ObjectTexes.Microcline, ContentLoad(@"Textures\Objects\Blocks\microcline_wall"));

            var temp = new Texture2D[5];
            for (int i = 0; i <= 4; i++)
                temp[i] =
                    ContentLoad(@"Textures\Objects\OnStore\wood_log" + (i + 1).ToString(CultureInfo.InvariantCulture));
            onstore_tex.Add(OnStoreTexes.Wood_log, temp);

            temp = new Texture2D[5];
            for (int i = 0; i <= 4; i++) temp[i] = ContentLoad(@"Textures\Objects\OnStore\stone" + (i + 1).ToString());
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

            Font1 = Content.Load<SpriteFont>(@"Textures\SpriteFont1");
            Font2 = Content.Load<SpriteFont>(@"Textures\SpriteFont2");


            dbmaterial = new DBMaterial(); //!!!!
            dbobject = new DBObject(); //!!!!
            dbonstore = new DBOnStore(); //!!!!
            buildings = new Stores(); //!!!!
            gmap = new GMap(); //!!!!

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
