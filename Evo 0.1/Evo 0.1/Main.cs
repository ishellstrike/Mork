﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mork.Generators;
using Mork.Local_Map;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using WButton = System.Windows.Forms.Button;
using Button = TomShane.Neoforce.Controls.Button;
using TomShane.Neoforce.Controls;
using Color = Microsoft.Xna.Framework.Color;
using Label = TomShane.Neoforce.Controls.Label;
using ListBox = TomShane.Neoforce.Controls.ListBox;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using TrackBar = TomShane.Neoforce.Controls.TrackBar;


namespace Mork
{
    public partial class Main : Game
    {
        #region Variables and constants

        public enum Gmapreshim
        {
            Normal,
            Height,
            Tempirature
        }

        //public enum GroundTexes
        //{
        //    None,
        //    IsoDirt1,
        //    IsoSand1,
        //    IsoSandwall,
        //    IsoGround1,
        //    IsoGround2,
        //    IsoGround3,
        //    IsoGround4,
        //    IsoGround5,
        //    IsoGround6,
        //    IsoGround7,
        //    IsoGround8,
        //    IsoGround9,
        //    IsoGround10,
        //    IsoGrass1,
        //    IsoGrass2,
        //    IsoGrass3,
        //    IsoGrass4,
        //    IsoGrass5,
        //    iso_grass_6,
        //    iso_grass_7,
        //    iso_grass_8,
        //    iso_grass_9,
        //    iso_grass_10,
        //    iso_water_base,
        //    iso_water_new1,
        //    iso_water_new2,
        //    iso_water_new3,
        //    iso_water_base1,
        //    iso_water_base2,
        //    iso_water_base3,
        //    iso_water_base4,
        //    iso_water_base5,
        //    iso_water_base6,
        //    iso_water_base7,
        //    iso_water_base8,
        //    iso_water_base9,
        //    iso_water_base10,
        //    iso_water_base11,
        //    iso_water_base12,
        //    iso_water_base13
        //}

        public enum Gstate
        {
            CloseGame,
            OldGame,
            NewGame,
            InGameMessage,
            MainMenu,
            MainOptions,
            GenerationScreen,
            GenerationOptions,
            OpenGMap,
            OpenGame
        }

        public enum LClickAction
        {
            Nothing,
            Dig,
            MarkStore,
            Crop
        }

        public enum OnStoreTexes
        {
            Wood_log,
            Stone
        }

        public const int ssizex = 50, ssizey = 35;
        public const int resx = 1280, resy = 768;

        public static string OurName = "";
        public static string OurVer = "";

        public static bool AllowClose;

        public static IAsyncResult Savear;

        public static IAsyncResult Generatear;
        public static int generate_d_fase;

        public static Gstate gstate = Gstate.MainMenu;
        private static string _messagestring = " ";

        public static string CurrentMap;

        public static int MeTexoncurA = 1;
        public static int MeTexoncurB = 1;
        public static int MeAct = 1;
        public static int MeObjoncur = 1;

        public static Vector3 gmap_region = new Vector3();

        //public static Hero mh = new Hero(); //главный герой
        //public static MFloat mfloat = new MFloat(); //плавающий текст
        public static MMap mmap = new MMap();
        public static MCre mcea = new MCre();
        public static Heroes heroes = new Heroes();
        //public static MBut mbut = new MBut();

        private delegate void GTDelegate(GameTime gt);
        private static GTDelegate asynccore;
        private static IAsyncResult asynccorear;

        public static Orders orders = new Orders();

        public static LClickAction lclickaction = LClickAction.Nothing;

        public static MouseState mousestate = new MouseState();

        //public static Dictionary<GroundTexes, Texture2D> ground_tex = new Dictionary<GroundTexes, Texture2D>();
                                                         // текстуры земли
        public static Texture2D object_tex;


        //public static Dictionary<ObjectTexes, Texture2D> object_tex = new Dictionary<ObjectTexes, Texture2D>();
                                                         // текстуры объектов

        public static Dictionary<OnStoreTexes, Texture2D[]> onstore_tex = new Dictionary<OnStoreTexes, Texture2D[]>();
                                                            //текстуры предметов на складе

        public static List<Texture2D> unit_tex = new List<Texture2D>(); // текстуры юнитов
        public static List<Texture2D> interface_tex = new List<Texture2D>(); // текстуры интрефейса

        public static DBMaterial dbmaterial;
        public static DB_LMO dbobject;
        public static DBOnStore dbonstore;
        public static Stores buildings;
        public static GMap gmap;

        public static Gmapreshim gmapreshim = Gmapreshim.Normal;

        public static Vector3 camera = new Vector3(0, 0, 0);
        public static Vector3 Selector = new Vector3();
        public static Vector3 midscreen = new Vector3();

        public static bool buttonhelper_L;
        public static bool buttonhelper_R;
        public static bool buttonhelper_M;
        public static bool click_L;
        public static bool click_R;
        public static bool click_M;

        private const string Asa = "asdasdasdasdasdasd"; 

        public static SpriteBatch spriteBatch;
        private readonly GraphicsDeviceManager graphics;
        public static LineBatch lineBatch;

        public static Action DrawProc;
        public static Action CalcProc;

        public static Vector2 mousepos;
        public static SpriteFont Font1, Font2;   
        private static Vector3 ramka_2 = new Vector3();
        public Vector2 MousePos;
        private static bool PAUSE;
        public float SavingDeg;
        private Texture2D _curTex; // стандартный курсор

        private Color _fpsCol;
        private int _fpsCur;
        private int _fpsN;
        private TimeSpan _fpsTime;

        private static int _titleAnimation = 1;
        private static int _titlePhase = -1;
        private static int _wheellast;
        private static Vector3 ramka_1 = new Vector3(-1, 0, 0);
        private Random rnd = new Random();
        public bool space, space_helper;

        private int tick_of_5;
        private Vector3 vcalk_now = new Vector3(), vcalk_pre = new Vector3();


        private const int Textlogmax = 10;

        private static readonly String[] textlog = {
                                                       "-|-", "-|-", "-|-", "-|-", "-|-", "-|-", "-|-", "-|-", "-|-",
                                                       "-|-"
                                                   };
#endregion

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            WindowsDesigner();

            base.Initialize();

            Assembly currentAssembly = Assembly.Load("Mork");
            OurName = AssemblyName.GetAssemblyName(currentAssembly.Location).Name;
            OurVer = "v" + AssemblyName.GetAssemblyName(currentAssembly.Location).Version;

            graphics.IsFullScreen = false; //true
            graphics.PreferredBackBufferHeight = resy;
            graphics.PreferredBackBufferWidth = resx;
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = true;
            graphics.ApplyChanges();

            Directory.CreateDirectory(@"Maps");
            Directory.CreateDirectory(@"Data");
            Directory.CreateDirectory(@"Data/Levels");

            Window.Title = OurName + " " + OurVer;

            //if (gstate == Gstate.MainMenu) mbut.MainMenuCreate();

            asynccore = AsyncCore;

            //mmap.RandomRoom(0);

            DrawProc = MainMenuDraw;
            CalcProc = () => {};
        }

        #region Window Designer
        private Manager Manager;
        private Skin basicskin;

        private Window Ingamemesages;
        private Button IngamemesagesOk;
        private Label Ingamemesageslabel;

        private Window mainmenu;
        private Button mainmenucloseB;
        private Button mainmenugeneratenewB;
        private Button mainmenuloadmapB;
        private Button mainmenuloadgameB;
        private Button mainmenuoptionsB;

        private Window generateoptionsmenu;
        private Button generateoptionnextB;
        private Button generateoptionbackB;
        private TrackBar generateoption;
        private Label generateoptionlabel;

        private Window generationMenu;
        private Button generationbegingameB;
        private Button generationgenerateB;
        private Button generationNormalMapB;
        private Button generationheightmapB;
        private Button generationtempmapB;
        private Button generationbackB;

        private Window ingameUIpartLeft;
        private ListBox ingameUIpartLeftlistbox;
        private ListBox ingameUIpartLeftlistbox2;
        private Button ingameshowOrdersB;
        private Button ingameshowZonesB;

        private Window orderssubmenu;
        private Button digorder;
        private Button cutorder;
        private Label orderslabel;

        private Window zonessubmenu;

        private Window maploadmenu;
        private ListBox maploadmenulistbox;
        private Button maploadmenuLoad;
        private Button maploadmenuLoadNext;
        /// <summary>
        /// Код дизайнера всех базовых окон
        /// </summary>
        private void WindowsDesigner()
        {
            Manager = new Manager(this, graphics, "Default");
            Manager.Initialize();

            #region mainmenu

            mainmenu = new Window(Manager) {BackColor = Color.Black};
            mainmenu.Init();
            mainmenu.Text = "";
            mainmenu.SetPosition(resx/3,resy/4);
            mainmenu.Width = resx / 3;
            mainmenu.Height = resy / 2;
            mainmenu.Visible = true;
            mainmenu.BorderVisible = false;
            mainmenu.Movable = false;
            mainmenu.Resizable = false;

            mainmenucloseB = new Button(Manager);
            mainmenucloseB.Init();
            mainmenucloseB.Text = "Quit";
            mainmenucloseB.Width = resx / 5;
            mainmenucloseB.Height = 25;
            mainmenucloseB.Left = (resx/3-resx/5)/2;
            mainmenucloseB.Top = mainmenu.ClientHeight - mainmenucloseB.Height - 8;
            mainmenucloseB.Anchor = Anchors.Bottom;
            mainmenucloseB.Parent = mainmenu;
            mainmenucloseB.Click += button_Click;


            mainmenugeneratenewB = new Button(Manager);
            mainmenugeneratenewB.Init();
            mainmenugeneratenewB.Text = "Создать новый мир и начать игру";
            mainmenugeneratenewB.Width = resx / 5;
            mainmenugeneratenewB.Height = 25;
            mainmenugeneratenewB.Left = (resx / 3 - resx / 5) / 2;
            mainmenugeneratenewB.Top = 50;
            mainmenugeneratenewB.Anchor = Anchors.Bottom;
            mainmenugeneratenewB.Parent = mainmenu;
            mainmenugeneratenewB.Click += mainmenugeneratenewB_Click;

            mainmenuloadmapB = new Button(Manager) {Text = "Начать игру в созданном мире", Width = resx / 5 };
            mainmenuloadmapB.Init();
            mainmenuloadmapB.Height = 25;
            mainmenuloadmapB.Left = (resx / 3 - resx / 5) / 2;
            mainmenuloadmapB.Top = 100;
            mainmenuloadmapB.Anchor = Anchors.Bottom;
            mainmenuloadmapB.Parent = mainmenu;
            mainmenuloadmapB.Click += mainmenuloadmapB_Click;

            mainmenuloadgameB = new Button(Manager);
            mainmenuloadgameB.Init();
            mainmenuloadgameB.Text = "Загрузить игру";
            mainmenuloadgameB.Width = resx / 5;
            mainmenuloadgameB.Height = 25;
            mainmenuloadgameB.Left = (resx / 3 - resx / 5) / 2;
            mainmenuloadgameB.Top = 150;
            mainmenuloadgameB.Anchor = Anchors.Bottom;
            mainmenuloadgameB.Parent = mainmenu;
            mainmenuloadgameB.Click += mainmenuloadgameB_Click;

            mainmenuoptionsB = new Button(Manager);
            mainmenuoptionsB.Init();
            mainmenuoptionsB.Text = "Опции";
            mainmenuoptionsB.Width = resx / 5;
            mainmenuoptionsB.Height = 25;
            mainmenuoptionsB.Left = (resx / 3 - resx / 5) / 2;
            mainmenuoptionsB.Top = 200;
            mainmenuoptionsB.Anchor = Anchors.Bottom;
            mainmenuoptionsB.Parent = mainmenu;
            mainmenuoptionsB.Click += mainmenuoptionsB_Click;

            Manager.Add(mainmenu);
            #endregion

            #region generateoptionsmenu

            generateoptionsmenu = new Window(Manager) {BackColor = Color.Black};
            generateoptionsmenu.Init();
            generateoptionsmenu.Text = "";
            generateoptionsmenu.SetPosition(resx / 3, resy / 4);
            generateoptionsmenu.Width = resx / 3;
            generateoptionsmenu.Height = resy / 2;
            generateoptionsmenu.Visible = false;
            generateoptionsmenu.BorderVisible = false;
            generateoptionsmenu.Movable = false;
            generateoptionsmenu.Resizable = false;

            generateoptionnextB = new Button(Manager);
            generateoptionnextB.Init();
            generateoptionnextB.Text = "Продолжить";
            generateoptionnextB.Width = resx / 5;
            generateoptionnextB.Height = 25;
            generateoptionnextB.Left = (resx / 3 - resx / 5) / 2;
            generateoptionnextB.Top = 50;
            generateoptionnextB.Anchor = Anchors.Bottom;
            generateoptionnextB.Parent = generateoptionsmenu;
            generateoptionnextB.Click += new TomShane.Neoforce.Controls.EventHandler(generateoptionnextB_Click);

            generateoptionbackB = new Button(Manager);
            generateoptionbackB.Init();
            generateoptionbackB.Text = "Назад";
            generateoptionbackB.Width = resx / 5;
            generateoptionbackB.Height = 25;
            generateoptionbackB.Left = (resx / 3 - resx / 5) / 2;
            generateoptionbackB.Top = generateoptionsmenu.ClientHeight - generateoptionbackB.Height - 8;
            generateoptionbackB.Anchor = Anchors.Bottom;
            generateoptionbackB.Parent = generateoptionsmenu;
            generateoptionbackB.Click += new TomShane.Neoforce.Controls.EventHandler(generateoptionbackB_Click);       

            Manager.Add(generateoptionsmenu);
            #endregion

            #region generationMenu

            generationMenu = new Window(Manager) {BackColor = Color.Black};
            generationMenu.Init();
            generationMenu.Text = "";
            generationMenu.SetPosition(20,20);
            generationMenu.Width = resx / 6;
            generationMenu.Height = 300;
            generationMenu.Visible = false;
            generationMenu.BorderVisible = false;
            generationMenu.Movable = false;
            generationMenu.Resizable = false;

            generationbegingameB = new Button(Manager);
            generationbegingameB.Init();
            generationbegingameB.Text = "Начать игру";
            generationbegingameB.Width = resx/8;
            generationbegingameB.Height = 25;
            generationbegingameB.Left = (resx / 6 - resx / 8) / 2;
            generationbegingameB.Top = 50;
            generationbegingameB.Anchor = Anchors.Bottom;
            generationbegingameB.Parent = generationMenu;
            generationbegingameB.Click += new TomShane.Neoforce.Controls.EventHandler(generationbegingameB_Click);

            generationgenerateB = new Button(Manager);
            generationgenerateB.Init();
            generationgenerateB.Text = "Генерировать";
            generationgenerateB.Width = resx / 8;
            generationgenerateB.Height = 25;
            generationgenerateB.Left = (resx / 6 - resx / 8) / 2;
            generationgenerateB.Top = 80;
            generationgenerateB.Anchor = Anchors.Bottom;
            generationgenerateB.Parent = generationMenu;
            generationgenerateB.Click += new TomShane.Neoforce.Controls.EventHandler(generationgenerateB_Click);

            generationNormalMapB = new Button(Manager);
            generationNormalMapB.Init();
            generationNormalMapB.Text = "Обычная карта";
            generationNormalMapB.Width = resx / 8;
            generationNormalMapB.Height = 25;
            generationNormalMapB.Left = (resx / 6 - resx / 8) / 2;
            generationNormalMapB.Top = 110;
            generationNormalMapB.Anchor = Anchors.Bottom;
            generationNormalMapB.Parent = generationMenu;
            generationNormalMapB.Click += new TomShane.Neoforce.Controls.EventHandler(generationnormalmapB_Click);

            generationheightmapB = new Button(Manager);
            generationheightmapB.Init();
            generationheightmapB.Text = "Карта высот";
            generationheightmapB.Width = resx / 8;
            generationheightmapB.Height = 25;
            generationheightmapB.Left = (resx / 6 - resx / 8) / 2;
            generationheightmapB.Top = 140;
            generationheightmapB.Anchor = Anchors.Bottom;
            generationheightmapB.Parent = generationMenu;
            generationheightmapB.Click += new TomShane.Neoforce.Controls.EventHandler(generationheightmapB_Click);

            generationtempmapB = new Button(Manager);
            generationtempmapB.Init();
            generationtempmapB.Text = "Карта температур";
            generationtempmapB.Width = resx / 8;
            generationtempmapB.Height = 25;
            generationtempmapB.Left = (resx / 6 - resx / 8) / 2;
            generationtempmapB.Top = 170;
            generationtempmapB.Anchor = Anchors.Bottom;
            generationtempmapB.Parent = generationMenu;
            generationtempmapB.Click += new TomShane.Neoforce.Controls.EventHandler(generationtempmapB_Click);

            generationbackB = new Button(Manager);
            generationbackB.Init();
            generationbackB.Text = "Назад";
            generationbackB.Width = resx / 8;
            generationbackB.Height = 25;
            generationbackB.Left = (resx / 6 - resx / 8) / 2;
            generationbackB.Top = 200;
            generationbackB.Anchor = Anchors.Bottom;
            generationbackB.Parent = generationMenu;
            generationbackB.Click += new TomShane.Neoforce.Controls.EventHandler(generationbackB_Click);

            generateoption = new TrackBar(Manager);
            generateoption.Init();
            generateoption.Top = 230;
            generateoption.Left = (resx / 6 - resx / 8) / 2;
            generateoption.Width = resx / 8;
            generateoption.Height = 25;
            generateoption.Parent = generationMenu;
            generateoption.ValueChanged += new TomShane.Neoforce.Controls.EventHandler(generateoption_ValueChanged);

            generateoptionlabel = new Label(Manager);
            generateoptionlabel.Init();
            generateoptionlabel.Top = 250;
            generateoptionlabel.Left = (resx / 6 - resx / 8) / 2;
            generateoptionlabel.Width = resx / 8;
            generateoptionlabel.Height = 25;
            generateoptionlabel.Parent = generationMenu;

            Manager.Add(generationMenu);
            #endregion

            #region mapload

            maploadmenu = new Window(Manager) {Color = Color.Black};
            maploadmenu.Init();
            maploadmenu.Text = "";
            maploadmenu.SetPosition(resx / 3, resy / 4);
            maploadmenu.Width = resx / 3;
            maploadmenu.Height = resy / 2;
            maploadmenu.Visible = false;
            maploadmenu.BorderVisible = false;
            maploadmenu.Movable = false;
            maploadmenu.Resizable = false;

            maploadmenuLoad = new Button(Manager);
            maploadmenuLoad.Init();
            maploadmenuLoad.Text = "Назад";
            maploadmenuLoad.Width = resx / 5;
            maploadmenuLoad.Height = 25;
            maploadmenuLoad.Left = (resx / 3 - resx / 5) / 2;
            maploadmenuLoad.Top = maploadmenu.ClientHeight - maploadmenuLoad.Height - 8;
            maploadmenuLoad.Anchor = Anchors.Bottom;
            maploadmenuLoad.Parent = maploadmenu;
            maploadmenuLoad.Click += new TomShane.Neoforce.Controls.EventHandler(maploadmenuLoad_Click);

            maploadmenuLoadNext = new Button(Manager);
            maploadmenuLoadNext.Init();
            maploadmenuLoadNext.Text = "Далее";
            maploadmenuLoadNext.Width = resx / 5;
            maploadmenuLoadNext.Height = 25;
            maploadmenuLoadNext.Left = (resx / 3 - resx / 5) / 2;
            maploadmenuLoadNext.Top = maploadmenu.ClientHeight - maploadmenuLoad.Height - 8 - 30;
            maploadmenuLoadNext.Anchor = Anchors.Bottom;
            maploadmenuLoadNext.Parent = maploadmenu;
            maploadmenuLoadNext.Click += new TomShane.Neoforce.Controls.EventHandler(maploadmenuLoadNext_Click);

            maploadmenulistbox = new ListBox(Manager);
            maploadmenulistbox.Init();
            maploadmenulistbox.Text = "";
            maploadmenulistbox.Width = resx / 5;
            maploadmenulistbox.Height = resy / 3;
            maploadmenulistbox.Left = (resx / 3 - resx / 5) / 2;
            maploadmenulistbox.Top = 50;
            maploadmenulistbox.Anchor = Anchors.Bottom;
            maploadmenulistbox.Parent = maploadmenu;
            maploadmenulistbox.DoubleClick += new TomShane.Neoforce.Controls.EventHandler(maploadmenulistbox_DoubleClick);

            Manager.Add(maploadmenu);
            #endregion

            #region ingameUIpartLeft
            ingameUIpartLeft = new Window(Manager) { Color = Color.Black };
            ingameUIpartLeft.Init();
            ingameUIpartLeft.Text = "";
            ingameUIpartLeft.SetPosition(resx / 5*4, 0);
            ingameUIpartLeft.Width = resx / 5;
            ingameUIpartLeft.Height = resy;
            ingameUIpartLeft.Visible = false;
            ingameUIpartLeft.BorderVisible = false;
            ingameUIpartLeft.Movable = false;
            ingameUIpartLeft.Resizable = false;

            ingameshowOrdersB = new Button(Manager);
            ingameshowOrdersB.Init();
            ingameshowOrdersB.Text = "Приказы (O)";
            ingameshowOrdersB.Width = resx/5-20;
            ingameshowOrdersB.Height = 25;
            ingameshowOrdersB.Left = (20)/2;
            ingameshowOrdersB.Top = 10;
            ingameshowOrdersB.Anchor = Anchors.Bottom;
            ingameshowOrdersB.Parent = ingameUIpartLeft;
            ingameshowOrdersB.Click += new TomShane.Neoforce.Controls.EventHandler(ingameshowOrdersB_Click);

            ingameshowZonesB = new Button(Manager);
            ingameshowZonesB.Init();
            ingameshowZonesB.Text = "Зоны (Z)";
            ingameshowZonesB.Width = resx / 5 - 20;
            ingameshowZonesB.Height = 25;
            ingameshowZonesB.Left = (20) / 2;
            ingameshowZonesB.Top = 40;
            ingameshowZonesB.Anchor = Anchors.Bottom;
            ingameshowZonesB.Parent = ingameUIpartLeft;
            ingameshowZonesB.Click += new TomShane.Neoforce.Controls.EventHandler(ingameshowZonesB_Click);

            ingameUIpartLeftlistbox = new ListBox(Manager);
            ingameUIpartLeftlistbox.Init();
            ingameUIpartLeftlistbox.Text = "";
            ingameUIpartLeftlistbox.Width = resx / 5-20;
            ingameUIpartLeftlistbox.Height = 90;
            ingameUIpartLeftlistbox.Left = 10;
            ingameUIpartLeftlistbox.Top = 200;
            ingameUIpartLeftlistbox.Anchor = Anchors.Bottom;
            ingameUIpartLeftlistbox.Parent = ingameUIpartLeft;
            ingameUIpartLeftlistbox.DoubleClick += new TomShane.Neoforce.Controls.EventHandler(maploadmenulistbox_DoubleClick);

            ingameUIpartLeftlistbox2 = new ListBox(Manager);
            ingameUIpartLeftlistbox2.Init();
            ingameUIpartLeftlistbox2.Text = "";
            ingameUIpartLeftlistbox2.Width = resx / 5-20;
            ingameUIpartLeftlistbox2.Height = 200;
            ingameUIpartLeftlistbox2.Left = 10;
            ingameUIpartLeftlistbox2.Top = 300;
            ingameUIpartLeftlistbox2.Anchor = Anchors.Bottom;
            ingameUIpartLeftlistbox2.Parent = ingameUIpartLeft;

            Manager.Add(ingameUIpartLeft);
            #endregion

            #region orderssubmenu

            orderssubmenu = new Window(Manager);
            orderssubmenu.Init();
            orderssubmenu.Text = "";
            orderssubmenu.Width = 150;
            orderssubmenu.Height = 200;
            orderssubmenu.Center();
            orderssubmenu.Visible = false;
            orderssubmenu.Resizable = false;

            digorder = new Button(Manager);
            digorder.Init();
            digorder.Text = "Выкопать";
            digorder.Width = orderssubmenu.Width-40;
            digorder.Height = 24;
            digorder.Left = 20;
            digorder.Top = 20;
            digorder.Anchor = Anchors.Bottom;
            digorder.Parent = orderssubmenu;
            digorder.Click += new TomShane.Neoforce.Controls.EventHandler(digorder_Click);

            cutorder = new Button(Manager);
            cutorder.Init();
            cutorder.Text = "Срубить";
            cutorder.Width = orderssubmenu.Width - 40;
            cutorder.Height = 24;
            cutorder.Left = 20;
            cutorder.Top = 50;
            cutorder.Anchor = Anchors.Bottom;
            cutorder.Parent = orderssubmenu;
            cutorder.Click += new TomShane.Neoforce.Controls.EventHandler(cutorder_Click);

            orderslabel = new Label(Manager);
            orderslabel.Left = 5;
            orderslabel.Top = 5;
            orderslabel.Text = "Приказы";
            orderslabel.Parent = orderssubmenu;

            Manager.Add(orderssubmenu);
            #endregion

            #region Ingamemesages
            Ingamemesages = new Window(Manager);
            Ingamemesages.Init();
            Ingamemesages.Text = "";
            Ingamemesages.Width = 480;
            Ingamemesages.Height = 200;
            Ingamemesages.Center();
            Ingamemesages.Visible = false;
            Ingamemesages.Resizable = false;

            IngamemesagesOk = new Button(Manager);
            IngamemesagesOk.Init();
            IngamemesagesOk.Text = "OK";
            IngamemesagesOk.Width = 72;
            IngamemesagesOk.Height = 24;
            IngamemesagesOk.Left = (Ingamemesages.ClientWidth / 2) - (IngamemesagesOk.Width / 2);
            IngamemesagesOk.Top = Ingamemesages.ClientHeight - IngamemesagesOk.Height - 8;
            IngamemesagesOk.Anchor = Anchors.Bottom;
            IngamemesagesOk.Parent = Ingamemesages;
            IngamemesagesOk.Click += new TomShane.Neoforce.Controls.EventHandler(IngamemesagesOk_Click);

            Ingamemesageslabel = new Label(Manager);
            Ingamemesageslabel.Left = 5;
            Ingamemesageslabel.Top = 5;
            Ingamemesageslabel.Text = "Text";
            Ingamemesageslabel.Parent = Ingamemesages;

            Manager.Add(Ingamemesages);
            #endregion
        }

        void cutorder_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            Main.lclickaction = Main.LClickAction.Crop;
        }

        void digorder_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            Main.lclickaction = Main.LClickAction.Dig;
        }

        void IngamemesagesOk_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            Ingamemesages.Close();
        }

        void ingameshowZonesB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            
        }

        void ingameshowOrdersB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            orderssubmenu.Show();
        }

        Action<string> sp = LoadGMapFromDisc;
        void maploadmenulistbox_DoubleClick(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            
            if(Savear != null && !Savear.IsCompleted) sp.EndInvoke(Savear);
            Main.Savear = sp.BeginInvoke(string.Format(@"{0}", files[maploadmenulistbox.ItemIndex]), null, null);
            Main.SetPhase(Main.Gstate.GenerationScreen);
            generationgenerateB.Hide();
        }

        void maploadmenuLoadNext_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            generationMenu.Show();
            maploadmenu.Close();
        }

        void maploadmenuLoad_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            maploadmenu.Close();
            mainmenu.Show();
            SetPhase(Gstate.MainMenu);
        }

        void generationbackB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            SetPhase(Gstate.MainMenu);
            generationMenu.Close();
            generateoptionsmenu.Show();
        }

        void generationtempmapB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            gmapreshim = Main.Gmapreshim.Tempirature;
        }

        void generationheightmapB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            gmapreshim = Main.Gmapreshim.Height;
        }

        void generationnormalmapB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            gmapreshim = Main.Gmapreshim.Normal;
        }

        void generateoption_ValueChanged(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            generateoptionlabel.Text = generateoption.Value.ToString();
        }

        void generationgenerateB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (Generatear == null || Generatear.IsCompleted)
            {
                GMap.GenerateProc proc = gmap.GenerateGWorld;
                Generatear = proc.BeginInvoke(generateoption.Value, null, null);
            }
            //gmap.GenerateGWorld();
        }

        void generationbegingameB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (Generatear == null || Generatear.IsCompleted)
            {
                if (generationgenerateB.Visible)
                {
                    Action ss = SaveGMapToDisc;
                    Main.Savear = ss.BeginInvoke(null, null);
                }
                SetPhase(Gstate.NewGame);
                generationMenu.Close();
                ingameUIpartLeft.Show();
                ToGame();
            }
        }

        void generateoptionnextB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            generateoptionsmenu.Close();
            generationMenu.Show();
            SetPhase(Gstate.GenerationScreen);
        }

        void generateoptionbackB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            mainmenu.Show();
            generateoptionsmenu.Close();
            SetPhase(Gstate.GenerationOptions);
        }

        void mainmenugeneratenewB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            generateoptionsmenu.Show();
            mainmenu.Close();
            generationgenerateB.Show();
            SetPhase(Gstate.MainMenu);
        }

        void mainmenuoptionsB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            
        }

        void mainmenuloadgameB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            
        }

        private List<string> files = new List<string>();
        void mainmenuloadmapB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            mainmenu.Close();
            maploadmenu.Show();
            files.Clear();
            files.AddRange(Directory.GetFiles(@"Maps\","*.zmm"));
            string[] datas = new string[files.Count];
            for (int index = 0; index < files.Count; index++)
            {
                datas[index] += new FileInfo(files[index]).CreationTime.ToString()+" "+index.ToString();
            }
            Dictionary<string,string> items = new Dictionary<string, string>();
            for (int i = 0; i < files.Count; i++)
            {
                items.Add(datas[i], files[i]);
            }
            items = items.OrderByDescending(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            for (int i = 0; i < files.Count; i++)
            {
                files[i] = items.ElementAt(i).Value;
            }

            maploadmenulistbox.Items.Clear();
            foreach (var item in items)
            {
                maploadmenulistbox.Items.Add(item.Value.ToString() + " - " + item.Key.ToString());
            }
            maploadmenulistbox.ItemIndex = 1;
        }

        void button_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            SetPhase(Gstate.CloseGame);
        }

        #endregion

        private static void SaveGMapToDisc()
        {
            var k = 0;
            var findCorrect = false;
            do { k++; findCorrect = File.Exists(string.Format(@"Maps\World{0}.zmm", k)); } while (findCorrect);

            var f = File.Create(string.Format(@"Maps\World{0}.zmm", k));
            CurrentMap = string.Format(@"Maps\World{0}.zmm", k);
            var gz = new GZipStream(f, CompressionMode.Compress, false);

            for (int i = 0; i <= GMap.size - 1; i++)
                for (int j = 0; j <= GMap.size - 2; j += 2)
                {
                    gz.Write(BitConverter.GetBytes(gmap.n[i, j]), 0, sizeof(float));
                    gz.Write(BitConverter.GetBytes((byte)gmap.obj[i, j]), 0, sizeof(byte));
                    gz.Write(BitConverter.GetBytes(gmap.t[i, j]), 0, sizeof(byte));

                    gz.Write(BitConverter.GetBytes(gmap.n[i, j + 1]), 0, sizeof(float));
                    gz.Write(BitConverter.GetBytes((byte)gmap.obj[i, j + 1]), 0, sizeof(byte));
                    gz.Write(BitConverter.GetBytes(gmap.t[i, j + 1]), 0, sizeof(byte));
                }
        }

        private static void LoadGMapFromDisc(string s)
        {
            var f = File.OpenRead(s);
            var gz = new GZipStream(f, CompressionMode.Decompress, false);

            for (int i = 0; i <= GMap.size - 1; i++)
                for (int j = 0; j <= GMap.size - 2; j += 2)
                {
                    var m1 = new byte[sizeof(float)];
                    gz.Read(m1, 0, sizeof(float));
                    gmap.n[i, j] = BitConverter.ToSingle(m1, 0);

                    var m2 = new byte[sizeof(byte)];
                    gz.Read(m2, 0, sizeof(byte));
                    gmap.obj[i, j] = (GMapObj)m2[0];

                    var m3 = new byte[sizeof(byte)];
                    gz.Read(m3, 0, sizeof(byte));
                    gmap.t[i, j] = m3[0];

                    m1 = new byte[sizeof(float)];
                    gz.Read(m1, 0, sizeof(float));
                    gmap.n[i, j + 1] = BitConverter.ToSingle(m1, 0);

                    m2 = new byte[sizeof(byte)];
                    gz.Read(m2, 0, sizeof(byte));
                    gmap.obj[i, j + 1] = (GMapObj)m2[0];

                    m3 = new byte[sizeof(byte)];
                    gz.Read(m3, 0, sizeof(byte));
                    gmap.t[i, j + 1] = m3[0];
                }
        }

        private void ToGame()
        {
            SetPhase(Main.Gstate.NewGame);
            Action ingamegen = InGameGeneration;
            Generatear = ingamegen.BeginInvoke(null, null);

            WorldLife.Age = NamesGenerator.GetAgeName();
            WorldLife.Day = rnd.Next(1, 30);
            WorldLife.Month = 3;
            WorldLife.Year = rnd.Next(1, 1000);

            StringBuilder ss = new StringBuilder();
            ss.Append(string.Format("Добро пожаловать в {0} {1} early alpha!", Main.OurName, Main.OurVer));

            if (WorldLife.Year <= 100) ss.Append(string.Format("\n\n" + "Начало эпохи {0}", WorldLife.Age));
            else if (WorldLife.Year >= 700) ss.Append(string.Format("\n\n" + "Закат эпохи {0}", WorldLife.Age));
            else ss.Append(string.Format("\n\n" + "Эпоха {0}", WorldLife.Age));
            ss.Append(string.Format(", год {0}, {1} {2}", WorldLife.Year, WorldLife.Day, NamesGenerator.MonthNameR(WorldLife.Month)));

            ss.Append("\n\nВ вашем отряде: ");
            for (var i = 0; i <= 5; i++)
            {
                ss.Append(NamesGenerator.GetRandomName(Races.Human));
                if (i != 5) ss.Append(", ");
            }
            ss.Append(".");

            ss.Append("\n\nВсе вопросы и предложения просьба отправлять на ishellstrike@gmail.com");

            mmap.SetMapTag("name", "test_name");
            mmap.SetMapTag("start_age", WorldLife.Age);
            mmap.SetMapTag("start_year",WorldLife.Year);
            mmap.SetMapTag("start_month", WorldLife.Month);
            mmap.SetMapTag("start_day", WorldLife.Day);
            mmap.SetMapTag("start_hour", WorldLife.Hour);
            mmap.SetMapTag("start_min", WorldLife.Min);
            mmap.SetMapTag("start_sec", WorldLife.Sec);

            Program.game.ShowIngameMessage(ss.ToString());
        }

        /// <summary>
        /// внутриигровая генерация игровой карты из глобальной карты
        /// </summary>
        private void InGameGeneration()
        {
            Main.mmap.SimpleGeneration_bygmap();

            for (var i = 0; i <= 9; i++)
            {
                Main.heroes.n.Add(new Hero());
                var temp = Main.heroes.n[Main.heroes.n.Count - 1];
                var k = 0;
                temp.pos = new Vector3(temp.pos.X + rnd.Next(0, 6) - 3, temp.pos.Y + rnd.Next(0, 6) - 3, k);
                for (k = 0; k <= MMap.mz - 1; k++)
                {
                    if (MMap.GoodVector3(temp.pos.X, temp.pos.Y, k) && Main.mmap.n[(int)temp.pos.X, (int)temp.pos.Y, k].blockID == 0) continue;
                    k--;
                    goto here;
                }
            here: ;
                temp.pos = new Vector3(temp.pos.X, temp.pos.Y, k);
            }
        }
        
        /// <summary>
        /// Перехват закрытия окна игры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void f_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (!AllowClose)
            //{
            //    e.Cancel = true;
                //mbut.OnClose();
            //    gstate = Gstate.MainMenu;
            //}
        }

        

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            MousePos.X = Mouse.GetState().X;
            MousePos.Y = Mouse.GetState().Y;

            mousestate = Mouse.GetState();

            mousepos.X = MousePos.X;
            mousepos.Y = MousePos.Y;

            _fpsTime += gameTime.ElapsedGameTime;

            if (_fpsTime > TimeSpan.FromSeconds(1))
            {
                _fpsTime -= TimeSpan.FromSeconds(1);
                _fpsCur = _fpsN;
                _fpsN = 0;
            }

            click_L = false;
            click_M = false;
            click_R = false;

            if (buttonhelper_L && mousestate.LeftButton == ButtonState.Released)
            {
                buttonhelper_L = false;
                click_L = true;
            }
            if (buttonhelper_R && mousestate.RightButton == ButtonState.Released)
            {
                buttonhelper_R = false;
                click_R = true;
            }

            if (buttonhelper_M && mousestate.MiddleButton == ButtonState.Released)
            {
                buttonhelper_M = false;
                click_M = true;
            }

            if (mousestate.LeftButton == ButtonState.Pressed)
                buttonhelper_L = true;
            if (mousestate.RightButton == ButtonState.Pressed)
                buttonhelper_R = true;
            if (mousestate.MiddleButton == ButtonState.Pressed)
                buttonhelper_M = true;

            switch (gstate)
            {
                case Gstate.CloseGame:
                    Exit();
                    break;
                case Gstate.InGameMessage:
                    break;
                case Gstate.NewGame:
                    MapEditorUpdate(gameTime);
                    break;
                case Gstate.OldGame:
                    GameUpdate(gameTime);
                    break;
                case Gstate.GenerationScreen:
                    GenerationScreen();
                    break;
            }

            _wheellast = mousestate.ScrollWheelValue;

            //mbut.ActButtons();

            Manager.Update(gameTime);

            base.Update(gameTime);
            FrameRateCounter.Update(gameTime);
        }

        private static void GenerationScreen()
        {
            if (mousestate.LeftButton == ButtonState.Pressed && mousepos.X >= 250 && mousepos.X <= 655 &&
                mousepos.Y >= 50 && mousepos.Y <= 455)
            {
                gmap_region.X = (int) (mousepos.X - 250)*13/5;
                gmap_region.Y = (int) (mousepos.Y - 50)*13/5;
            }
        }

        /// <summary>
        /// на доработку
        /// </summary>
        /// <param name="gs"></param>
        public static void SetPhase(Gstate gs)
        {
            switch (gs)
            {
                case Gstate.CloseGame:
                    break;
                case Gstate.NewGame:
                    DrawProc = GameDraw;
                    break;
                case Gstate.OldGame:
                    DrawProc = GameDraw;
                    break;
                case Gstate.InGameMessage:
                    DrawProc = InGameMessageDraw;
                    break;
                case Gstate.MainMenu:
                    DrawProc = MainMenuDraw;
                    break;
                case Gstate.GenerationScreen:
                    DrawProc = DrawGMapGenRects;
                    break; 
            }

            gstate = gs;
        }

        /// <summary>
        /// ужас! на переработку
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            _fpsN++;
            tick_of_5++;
            tick_of_5 = tick_of_5 > 5 ? 1 : tick_of_5;

            Manager.BeginDraw(gameTime);
            Manager.Draw(gameTime);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            DrawProc();

            _fpsCol = Color.Lerp(Color.Lime, Color.Red, 1 - ((_fpsCur - 30)/30.0f));

            spriteBatch.DrawString(Font2, string.Format("FPS: {0}, sel: {1}, mou: {2}", _fpsCur, Selector, mousepos),
                                   new Vector2(400, 5), _fpsCol);

            //int kk = 0;
            //foreach (Hero h in heroes.n)
            //{
            //    if (h.order != null) spriteBatch.DrawString(Font2, h.pos.ToString()+" "+ h.orderid.ToString() + " "+h.ininv_material.ToString()+" "+ h.item_ininv.ToString()+" phase "+h.orderphase.ToString()+" ord_"+h.order.ToString(), new Vector2(30, 100 + kk * 10), Color.Red);

            //    kk++;
            //}

            //kk = 0;

            //foreach (KeyValuePair<string, int> temp in buildings.GetSummary())
            //{
            //    spriteBatch.DrawString(Font2, temp.Value.ToString()+" "+temp.Key, new Vector2(530, 100 + kk * 10), Color.Red);

            //    kk++;
            //}

            //mbut.DrawButtons();

            if (Savear != null && !Savear.IsCompleted)
            {
                spriteBatch.Draw(interface_tex[16], new Vector2(60, 40), null, Color.White, SavingDeg,
                                 new Vector2(32, 32), Vector2.One, SpriteEffects.None, 0);
                SavingDeg += (float) Math.PI/10;
                if (SavingDeg >= Math.PI*2) SavingDeg = 0;
            }

            if (Generatear != null && !Generatear.IsCompleted)
            {
                spriteBatch.Draw(interface_tex[17], new Vector2(130, 40), new Rectangle(generate_d_fase*41, 0, 41, 28),
                                 Color.White, 0, new Vector2(32, 32), Vector2.One, SpriteEffects.None, 0);

                if (tick_of_5 == 1) generate_d_fase++;
                if (generate_d_fase > 15) generate_d_fase = 0;
            }
            else generate_d_fase = 0;

            
            spriteBatch.End();

            Manager.EndDraw();

            FrameRateCounter.Draw(gameTime, Font2, spriteBatch, lineBatch, resx, resy);
            lineBatch.Draw();
            lineBatch.Clear();

            base.Draw(gameTime);
        }

        public void ShowIngameMessage(string s)
        {
            //_messagestring = s;
            //mbut.ClearButtons();
            //MBut.From = Gstate.NewGame;
            //SetPhase(Gstate.InGameMessage);
            //mbut.CreateButton(new Vector2(150, 400), "OK", ButtonsActionID.CloseBack);
            Ingamemesages.Show();
            Ingamemesageslabel.Text = s;
            Ingamemesageslabel.Width = (int)Manager.Skin.Fonts[0].Resource.MeasureString(s).X;
            Ingamemesageslabel.Height = (int)Manager.Skin.Fonts[0].Resource.MeasureString(s).Y;
            Ingamemesages.Height = Ingamemesageslabel.Height + 50;
            Ingamemesages.Width = Ingamemesageslabel.Width + 120;
            IngamemesagesOk.Visible = false;
        }


        private static Vector2 ToIsometricPos(int x, int y)
        {
            return new Vector2(camera.X + (x - y)*20, camera.Y + (x + y)*10);
        }

        private static float ToIsometricX(int x, int y)
        {
            return camera.X + (x - y)*20;
        }

        private static float ToIsometricY(int x, int y)
        {
            return camera.Y + (x + y)*10;
        }

        private static Vector2 ToIsometricFloat(float x, float y)
        {
            return new Vector2(camera.X + (x - y) * 20f, camera.Y + (x + y)*10f);
        }

        public static void AddToLog(string mess)
        {
            for (int i = 1; i <= Textlogmax - 1; i++)
            {
                textlog[i - 1] = textlog[i];
            }
            textlog[Textlogmax - 1] = mess;
        }

        #region Draws

        private static void DrawGMapGenRects()
        {
            for (int i = 0; i <= GMap.size - 14; i += 13)
                for (int j = 0; j <= GMap.size - 14; j += 13)
                {
                    switch (gmapreshim)
                    {
                        case Gmapreshim.Normal:
                            spriteBatch.Draw(GMap.data[gmap.obj[i, j]].tex,
                                             new Vector2(250 + i / 13 * 5, 50 + j / 13 * 5),
                                             GMap.data[gmap.obj[i, j]].col);
                            break;
                        case Gmapreshim.Height:
                            int a = Convert.ToInt32(gmap.n[i, j] * 255);
                            spriteBatch.Draw(interface_tex[12], new Vector2(250 + i / 13 * 5, 50 + j / 13 * 5),
                                             new Color(a, a, a));
                            break;
                        case Gmapreshim.Tempirature:
                            a = Convert.ToInt32(gmap.t[i, j]);
                            spriteBatch.Draw(interface_tex[12], new Vector2(250 + i / 13 * 5, 50 + j / 13 * 5),
                                             new Color(a, 0, 255 - a));
                            break;
                    }
                }

            for (int i = 0; i <= 77; i++)
                for (int j = 0; j <= 77; j++)
                {
                    if (i + gmap_region.X < GMap.size - 1 && j + gmap_region.Y < GMap.size - 1)
                    {
                        switch (gmapreshim)
                        {
                            case Gmapreshim.Normal:
                                spriteBatch.Draw(GMap.data[gmap.obj[i + (int)gmap_region.X, j + (int)gmap_region.Y]].tex,
                                                 new Vector2(700 + i * 5, 50 + j * 5),
                                                 GMap.data[gmap.obj[i + (int)gmap_region.X, j + (int)gmap_region.Y]].col);
                                break;
                            case Gmapreshim.Tempirature:
                                int a = Convert.ToInt32(gmap.t[i + (int)gmap_region.X, j + (int)gmap_region.Y]);
                                spriteBatch.Draw(interface_tex[12], new Vector2(700 + i * 5, 50 + j * 5),
                                                 new Color(a, 0, 255 - a));
                                break;
                            case Gmapreshim.Height:
                                a = Convert.ToInt32(gmap.n[i + (int)gmap_region.X, j + (int)gmap_region.Y] * 255);
                                spriteBatch.Draw(interface_tex[12], new Vector2(700 + i * 5, 50 + j * 5),
                                                 new Color(a, a, a));
                                break;
                        }
                    }
                }

            for (int i = 5; i <= GMap.data.Count - 1; i++)
            {
                spriteBatch.Draw(GMap.data.ElementAt(i).Value.tex, new Vector2(250, 450 + (i - 5) * 12),
                                 GMap.data.ElementAt(i).Value.col);
                spriteBatch.DrawString(Font2, GMap.data.ElementAt(i).Value.name,
                                       new Vector2(250 + 10, 445 + (i - 5) * 12), Color.White);
            }
        }

        private static void InGameMessageDraw()
        {
            spriteBatch.DrawString(Font1, _messagestring, new Vector2(50, 50), Color.White);
        }

        private static void MainMenuDraw()
        {
            _titleAnimation += _titlePhase;
            if (_titleAnimation >= 255 || _titleAnimation <= 0) _titlePhase *= -1;
            spriteBatch.Draw(interface_tex[15], new Vector2(470, 20),
                             new Color(255, _titleAnimation, _titleAnimation));
        }

        private static void BasicAllDraw()
        {
            int a = (int)midscreen.X - 33;
            if (a < 0) a = 0;
            int b = (int)midscreen.Y - 33;
            if (b < 0) b = 0;
            int aa = (int)midscreen.X + 33;
            if (aa > MMap.mx - 1) aa = MMap.mx - 1;
            int bb = (int)midscreen.Y + 33;
            if (bb > MMap.my - 1) bb = MMap.my - 1;

            var  ramka_3 = new Vector3();
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                ramka_2.X = Math.Max(Selector.X, ramka_1.X);
                ramka_2.Y = Math.Max(Selector.Y, ramka_1.Y);
                ramka_2.Z = Math.Max(Selector.Z, ramka_1.Z);

                ramka_3 = new Vector3(Math.Min(Selector.X, ramka_1.X), Math.Min(Selector.Y, ramka_1.Y),
                                  Math.Min(Selector.Z, ramka_1.Z));
            }

            float x_temp2 = 0;
            float y_temp2 = 0;

            int[,] drawed = WhoDrawedCalculate(a, b, aa, bb);

            if (Generatear.IsCompleted)
            for (int i = a; i < aa; i++)
                for (int j = b; j < bb; j++)
                {
                    if (mmap.n[i, j, drawed[i, j]].blockID != 0) /*&& ground_tex_old[mmap.n[i, j].tex].Height > 20*/
                    {
                        x_temp2 = ToIsometricX(i, j);
                        y_temp2 = ToIsometricY(i, j) - 40 + 20 +
                                  (drawed[i, j] - Selector.Z)*20;


                        DrawAllFloarCreators(ramka_3, x_temp2, y_temp2, drawed, i, j, false);

                       //DrawTreesHigher(ramka_3, ref x_temp2, ref y_temp2, drawed, i, j);
                    }

                    //DrawCNodes(ref x_temp2, ref y_temp2, drawed, i, j);

                    //if (Selector.X == i && Selector.Y == j)
                    //{
                    //    x_temp2 = ToIsometricX((int)Selector.X, (int)Selector.Y);
                    //    y_temp2 = ToIsometricY((int)Selector.X, (int)Selector.Y) - 20;

                    //    spriteBatch.Draw(interface_tex[2], new Vector2(x_temp2, y_temp2),
                    //                         MMap.IsWalkable(Selector) ? new Color(255, 255, 255) : new Color(255, 0, 50));
                    //    for (int sel = 1; sel < drawed[i, j] - Selector.Z; sel++)
                    //        spriteBatch.Draw(interface_tex[2], new Vector2(x_temp2, y_temp2+sel*20),
                    //                         MMap.IsWalkable(Selector) ? new Color(255, 255, 255) : new Color(255, 0, 50));
                    //}
                }

            DrawHeroes(ref x_temp2, ref y_temp2, drawed);

            DrawOrders(ref x_temp2, ref y_temp2, drawed);

            string ssss = string.Format("{0} {1}, год {2} эпохи {3}", WorldLife.Day,
                                        NamesGenerator.MonthNameR(WorldLife.Month), WorldLife.Year, WorldLife.Age);
            string sss1 = string.Format("{0}:{1}:{2}", WorldLife.Hour, WorldLife.Min.ToString("00"),
                                        WorldLife.Sec.ToString("00"));

            if (PAUSE)
            {
                spriteBatch.Draw(interface_tex[6], new Vector2(0, 20), Color.Red);
                spriteBatch.Draw(interface_tex[6], new Vector2(0, 15), Color.DarkRed);
                for (int i = 0; i <= 10; i++)
                {
                    spriteBatch.DrawString(Font2, "PAUSE", new Vector2(i*100, 42), Color.White);
                }
            }
            spriteBatch.Draw(interface_tex[6], Vector2.Zero, Color.DarkGray);
            spriteBatch.Draw(interface_tex[6], new Vector2(0, -5), Color.Black);

            spriteBatch.Draw(interface_tex[10], new Vector2(1024, 0), Color.DarkGray);
            spriteBatch.Draw(interface_tex[10], new Vector2(1029, 0), Color.Black);

            //spriteBatch.Draw(interface_tex[8], new Vector2(10,5), null, Color.Black, 0f, Vector2.Zero, new Vector2(Font2.MeasureString(ssss).X/100,1), SpriteEffects.None,0);
            spriteBatch.DrawString(Font2, ssss, new Vector2(10, 5), Color.White);
            spriteBatch.DrawString(Font2, sss1, new Vector2(10, 17), Color.White);
            if (MMap.GoodVector3(Selector) && mmap.n[(int)Selector.X, (int)Selector.Y, (int)Selector.Z].explored)
            {
                if (mmap.n[(int)Selector.X, (int)Selector.Y, (int)Selector.Z].Storing != OnStoreID.Nothing &&
                    mmap.n[(int)Selector.X, (int)Selector.Y, (int)Selector.Z].Storing_num > 0)
                    spriteBatch.DrawString(Font1,
                                           mmap.n[(int)Selector.X, (int)Selector.Y, (int)Selector.Z].Storing_num + " " +
                                           dbmaterial.Data[mmap.n[(int)Selector.X, (int)Selector.Y, (int)Selector.Z].storing_material].
                                               i_name +
                                           dbonstore.data[mmap.n[(int)Selector.X, (int)Selector.Y, (int)Selector.Z].Storing].R_name +
                                           " " +
                                           dbonstore.data[mmap.n[(int)Selector.X, (int)Selector.Y, (int)Selector.Z].Storing].I_name,
                                           new Vector2(1052, 40), Color.White);
                spriteBatch.DrawString(Font1,
                                       dbmaterial.Data[(MaterialID)mmap.GetNodeTagData(Selector, "material")].i_name + " " +
                                       dbobject.Data[mmap.n[(int)Selector.X, (int)Selector.Y, (int)Selector.Z].blockID].I_name + ":" + mmap.n[(int)Selector.X, (int)Selector.Y, (int)Selector.Z].blockID,
                                       new Vector2(1052, 60), Color.White);
            }
        }

        private static void DrawTreesHigher(Vector3 ramka_3, ref float x_temp2, ref float y_temp2, int[,] drawed, int i, int j)
        {
            if (Selector.Z != 0 && mmap.n[i, j, (int)Selector.Z - 1].blockID != 0 &&
                !dbobject.Data[mmap.n[i, j, (int)Selector.Z - 1].blockID].createfloor)
            {
                x_temp2 = ToIsometricX(i, j);
                y_temp2 = ToIsometricY(i, j) - 40;

                if (x_temp2 < resx + 40 && x_temp2 > -40 && y_temp2 < resy + 40 && y_temp2 > -40)
                {
                    DrawAllFloarCreators(ramka_3, x_temp2, y_temp2 + 40, drawed, i, j + 1, true);
                    spriteBatch.Draw(object_tex, new Vector2(x_temp2, y_temp2), GetTexRectFromN(dbobject.Data[mmap.n[i, j, (int)Selector.Z - 1].blockID].metatex_n),
                                     Color.White);
                }
            }
        }

        private static Rectangle GetTexRectFromN(int n)
        {
            return new Rectangle((n%10)*40, (n/10)*40, 40, 40);
        }

        private static void DrawOrders(ref float x_temp2, ref float y_temp2, int[,] drawed)
        {
            if (orders.list.Count > 0)
            {
                for (int i = 0; i <= orders.list.Count - 1; i++)
                {
                    x_temp2 = ToIsometricX((int)orders.list[i].dest.X, (int)orders.list[i].dest.Y) + 17;
                    y_temp2 = ToIsometricY((int)orders.list[i].dest.X, (int)orders.list[i].dest.Y) - 15 +
                              (drawed[(int)orders.list[i].dest.X, (int)orders.list[i].dest.Y] - Selector.Z) * 20;

                    if (x_temp2 < resx + 40 && x_temp2 > -40 && y_temp2 < resy + 20 && y_temp2 > -20 &&
                        orders.list[i].dest.Z == drawed[(int)orders.list[i].dest.X, (int)orders.list[i].dest.Y])
                    {
                        switch (orders.list[i].id)
                        {
                            case OrderID.dig_order:
                                spriteBatch.Draw(interface_tex[4], new Vector2(x_temp2, y_temp2),
                                                 new Color(255, 255, 255));
                                break;
                            case OrderID.digpit_order:
                                spriteBatch.Draw(interface_tex[4], new Vector2(x_temp2, y_temp2),
                                                 new Color(255, 255, 255));
                                break;
                        }
                    }
                }
            }
        }

        private static void DrawHeroes(ref float x_temp2, ref float y_temp2, int[,] drawed)
        {
            if (heroes.n.Count > 0)
                for (int b = 0; b <= heroes.n.Count - 1; b++)
                {
                    //if (heroes.n[b].pos.X == i - 1 && heroes.n[b].pos.Y == j - 1 && (heroes.n[b].pos.Z == drawed[i, j] - 1))
                    if (heroes.n[b].pos.Z >= drawed[(int)heroes.n[b].pos.X, (int)heroes.n[b].pos.Y] - 1)
                    {
                        x_temp2 = ToIsometricFloat((int)heroes.n[b].pos.X, (int)heroes.n[b].pos.Y).X;
                        y_temp2 = ToIsometricFloat((int)heroes.n[b].pos.X, (int)heroes.n[b].pos.Y).Y + (heroes.n[b].pos.Z - Selector.Z - 1)*20 + 15;
                        int aa = 255;

                        if (heroes.n[b].pos.Z <= drawed[(int)heroes.n[b].pos.X, (int)heroes.n[b].pos.Y])
                        {
                            spriteBatch.Draw(unit_tex[heroes.n[b].tex], new Vector2(x_temp2, y_temp2), Color.White);
                            if (heroes.n[b].item_ininv != OnStoreID.Nothing)
                                spriteBatch.Draw(dbonstore.data[heroes.n[b].item_ininv].tex[0],
                                                 new Vector2(x_temp2 - 15, y_temp2 - 20),
                                                 dbmaterial.Data[heroes.n[b].ininv_material].color);
                        }
                    }
                }
        }

        private static void DrawAllFloarCreators(Vector3 ramka_3, float x_temp2, float y_temp2, int[,] drawed, int i, int j,
                                          bool no_condition)
        {
            if (x_temp2 < resx + 40 - 300 && x_temp2 > -40 && y_temp2 < resy + 40 && y_temp2 > -40 + 40)
            {
                int aa = (drawed[i, j] - (int)Selector.Z + 1) * 5;
                if (drawed[i, j] - Selector.Z + 1 > 1) aa += 50;
                if (mmap.n[i, j, drawed[i, j]].subterrain) aa += 60;

                if (!mmap.n[i, j, drawed[i, j]].explored) aa = 255;

                Color tcol = dbmaterial.Data[(MaterialID)mmap.GetNodeTagData(i, j, drawed[i, j],"material")].color;

                int gg;
                int bb;
                int rr;

                gg = tcol.G - aa;
                bb = tcol.B - aa;
                rr = tcol.R - aa;

                if (buttonhelper_R && i >= ramka_3.X && j >= ramka_3.Y && drawed[i, j] >= ramka_3.Z && i <= ramka_2.X &&
                    j <= ramka_2.Y && drawed[i, j] <= ramka_2.Z)
                {
                    rr = 255;
                    gg = 255;
                    bb = 0;
                    if (!mmap.n[i, j, drawed[i, j]].explored)
                    {
                        rr = 100;
                        gg = 100;
                        bb = 0;
                    }
                }


                if (no_condition || mmap.n[i, j, drawed[i, j]].explored && (rr != 0 || gg != 0 || bb != 0))
                {
                    //if (!dbobject.Data[mmap.n[i, j, drawed[i, j]].blockID].createfloor)
                    //    spriteBatch.Draw(object_tex,
                    //                     new Vector2(x_temp2, y_temp2 + 40), GetTexRectFromN(dbobject.Data[mmap.n[i, j, drawed[i, j] + 1].blockID].metatex_n), new Color(rr, gg, bb));
                    spriteBatch.Draw(object_tex, new Vector2(x_temp2, y_temp2), GetTexRectFromN( dbobject.Data[mmap.n[i, j, drawed[i, j]].blockID].metatex_n),
                                     new Color(rr, gg, bb));


                    if (mmap.n[i, j, drawed[i, j]].Storing != OnStoreID.Nothing &&
                        mmap.n[i, j, drawed[i, j]].Storing_num > 0)
                    {
                        tcol = dbmaterial.Data[mmap.n[i, j, drawed[i, j]].storing_material].color;

                        gg = tcol.G - aa;
                        bb = tcol.B - aa;
                        rr = tcol.R - aa;

                        int aaa = 0;
                        if (mmap.n[i, j, drawed[i, j]].blockID == 1000) aaa = 13;
                        spriteBatch.Draw(
                            dbonstore.data[mmap.n[i, j, drawed[i, j]].Storing].tex[
                                mmap.n[i, j, drawed[i, j]].Storing_num - 1], new Vector2(x_temp2, y_temp2 - 20 + aaa),
                            new Color(rr, gg, bb));
                    }
                }
                else spriteBatch.Draw(object_tex, new Vector2(x_temp2, y_temp2), GetTexRectFromN(dbobject.Data[12345].metatex_n),
                                     new Color(rr, gg, bb));
            }
        }

        private static int[,] WhoDrawedCalculate(int a, int b, int aa, int bb)
        {
            var drawed = new int[MMap.mx,MMap.my];
            for (int i = a; i <= aa; i++)
                for (int j = b; j <= bb; j++)
                {
                    drawed[i, j] = (int)Selector.Z;
                    for (int k = (int)Selector.Z; k <= MMap.mz - 1; k++)
                        if (mmap.n[i, j, k].blockID != 0) // && dbobject.data[mmap.n[i, j, k].Obj].createfloor 
                        {
                            drawed[i, j] = k;
                            goto here;
                        }
                    here:
                    ;
                }
            return drawed;
        }

        private static void DrawCNodes(ref float x_temp2, ref float y_temp2, int[,] drawed, int i, int j)
        {
            //foreach (CNode temp in mcea.n)
            //{
            //    if (temp.pos.X == i && temp.pos.Y == j && mmap.n[temp.pos.X, temp.pos.Y, drawed[i, j]].vision == 0)
            //    {
            //        x_temp2 = ToIsometricPos(temp.pos.X, temp.pos.Y).X + ToIsometricFloat(temp.off.X, temp.off.Y).X + 11;
            //        y_temp2 = ToIsometricPos(temp.pos.X, temp.pos.Y).Y + ToIsometricFloat(temp.off.X, temp.off.Y).Y - 7;

            //        spriteBatch.Draw(unit_tex[temp.tex], new Vector2(x_temp2, y_temp2), Color.White);
            //    }
            //}
        }

        private static void GameDraw()
        {
            BasicAllDraw();

            for (int i = 0; i <= Textlogmax - 1; i++) // отрисова лога
            {
                spriteBatch.DrawString(Font1, textlog[i], new Vector2(8, resy - 192 + i*15), Color.White);
            }
        }

        private void MapEditorDraw()
        {
            //MEAllDraw();
        }

        #endregion

        public static void PrepairMapDeleteWrongIDs(ref MMap map)
        {
            for (int i0 = 0; i0 < map.n.GetUpperBound(0); i0++)
                for (int i1 = 0; i1 < map.n.GetUpperBound(1); i1++)
                    for (int i2 = 0; i2 < map.n.GetUpperBound(2); i2++)
                    {
                        if (!dbobject.Data.ContainsKey(map.n[i0, i1, i2].blockID)) map.n[i0, i1, i2].blockID = 666;
                    }
        }

        #region Updates

        private void GameUpdate(GameTime gt)
        {
            Selector.X = Convert.ToInt16(((MousePos.X - camera.X)/2 + (MousePos.Y - camera.Y)/1)/20) - 1;
            Selector.Y = Convert.ToInt16(-((MousePos.X - camera.X)/2 - (MousePos.Y - camera.Y)/1)/20);

            midscreen.X = Convert.ToInt16(((resx/2 - camera.X) /2 + (resy/2 - camera.Y)/1)/20) - 1;
            midscreen.Y = Convert.ToInt16(-((resx/2 - camera.X) /2 - (resy/2 - camera.Y)/1)/20);

            //if (Keyboard.GetState().IsKeyDown(Keys.E) == true && mh.hero_attack_step == 1) { CNode temp = mcea.GetNearCreature(); if (temp != null) mcea.AttackCreature(temp);}

            space = false;

            if (space_helper && Keyboard.GetState().IsKeyDown(Keys.Space) == false)
            {
                space_helper = false;
                space = true;
                PAUSE = !PAUSE;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                space_helper = true;


            //if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            //    heroes.n[0].patch = mmap.FindPatch(heroes.n[0].pos, new Vector3(Selector.X, Selector.Y, Selector.Z));
            //if (Mouse.GetState().RightButton == ButtonState.Pressed)
            //    mmap.n[Selector.X, Selector.Y, Selector.Z].Obj = 666;

            bool LRUD = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) == false &
                    Keyboard.GetState().IsKeyDown(Keys.RightShift) == false) camera.Y -= 4;
                else camera.Y -= 10;

                LRUD = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) == false &
                    Keyboard.GetState().IsKeyDown(Keys.RightShift) == false) camera.Y += 4;
                else camera.Y += 10;

                LRUD = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) == false &
                    Keyboard.GetState().IsKeyDown(Keys.RightShift) == false) camera.X += 4;
                else camera.X += 10;

                LRUD = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) == false &
                    Keyboard.GetState().IsKeyDown(Keys.RightShift) == false) camera.X -= 4;
                else camera.X -= 10;

                LRUD = true;
            }

            //if (mh.pos.X > camera.X + ssizex - 12 && camera.X < MMap.mx - ssizex && !LRUD)
            //{
            //    camerafade_x -= 2;
            //    if (camerafade_x <= -20)
            //    {
            //        camerafade_x = 0;
            //        camera.X++;
            //    }
            //    //if (mh.pos.X > camera.X) camera.X++;
            //}

            //if (mh.pos.Y > camera.Y+ssizey - 12 && camera.Y < MMap.my - ssizey && !LRUD)
            //{
            //    camerafade_y -= 2;
            //    if (camerafade_y <= -20)
            //    {
            //        camerafade_y = 0;
            //        camera.Y++;
            //    }

            //    //if (mh.pos.Y > camera.Y) camera.Y++;
            //}

            //if (mh.pos.Y < camera.Y + 12 && camera.Y > 0 && !LRUD)
            //{
            //    camerafade_y += 2;
            //    if (camerafade_y >= 20)
            //    {
            //        camerafade_y = 0;
            //        camera.Y--;
            //    }

            //    //if (mh.pos.Y < camera.Y) camera.Y--;
            //}

            //if (mh.pos.X < camera.X + 12 && camera.X > 0 && !LRUD)
            //{
            //    camerafade_x += 2;
            //    if (camerafade_x >= 20)
            //    {
            //        camerafade_x = 0;
            //        camera.X--;
            //    }
            //    //if (mh.pos.X < camera.X) camera.X--;
            //}

            //mmap.water_render_step++; //замедление обработки воды
            //if (mmap.water_render_step >= mmap.water_render_freq)
            //{
            //    mmap.water_render_step = 0;
            //    mmap.WaterCalc(camera.Z, rnd.Next(1, 6));
            //}
            if (!PAUSE)
            {
                heroes.hero_move_step++; //замедление обработки перемещения героя
                if (heroes.hero_move_step >= heroes.hero_move_freq)
                {
                    heroes.hero_move_step = 0;
                    foreach (Hero mh in heroes.n)
                        mh.MoveHuman(gt);
                }
                if (heroes.hero_action_step != 1 && !PAUSE)
                    heroes.hero_action_step++; //замедление обработки атаки героя
                if (heroes.hero_action_step >= heroes.hero_attack_freq)
                {
                    heroes.hero_action_step = 0;
                }
                mcea.move_step++;
                if (mcea.move_step >= mcea.move_freq)
                {
                    mcea.move_step = 0;
                    mcea.AIstep();
                    mcea.MoveCrea();
                }

                WorldLife.WorldTick(ref mmap);


                //mfloat.Tick(); // обработка плавающего текста
            }


            //vcalk_pre = new Vector3(vcalk_now);
            //vcalk_now = new Vector3(mh.pos);
            //if (vcalk_now != vcalk_pre) mmap.CalcWision(); // просчет видимости
        }

        private void MapEditorUpdate(GameTime gt)
        {
            {
                if (Mouse.GetState().ScrollWheelValue > _wheellast)
                {
                    if (Selector.Z > 0) Selector.Z--;
                }

                if (Mouse.GetState().ScrollWheelValue < _wheellast)
                {
                    if (Selector.Z < MMap.mz - 2) Selector.Z++;
                }

                Selector.X = Convert.ToInt16(((MousePos.X - camera.X)/2 + (MousePos.Y - camera.Y)/1)/20) - 1;
                Selector.Y = Convert.ToInt16(-((MousePos.X - camera.X)/2 - (MousePos.Y - camera.Y)/1)/20);

                midscreen.X = Convert.ToInt16(((resx/2 - camera.X)/2 + (resy/2 - camera.Y)/1)/20) - 1;
                midscreen.Y = Convert.ToInt16(-((resx/2 - camera.X)/2 - (resy/2 - camera.Y)/1)/20);

                if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) == false &
                        Keyboard.GetState().IsKeyDown(Keys.RightShift) == false) camera.Y -= 4;
                    else camera.Y -= 10;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) == false &
                        Keyboard.GetState().IsKeyDown(Keys.RightShift) == false) camera.Y += 4;
                    else camera.Y += 10;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) == false &
                        Keyboard.GetState().IsKeyDown(Keys.RightShift) == false) camera.X += 4;
                    else camera.X += 10;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.N))
                {
                    for (var i = 0; i <= MMap.mx - 1; i++)
                        for (var j = 0; j <= MMap.my - 1; j++)
                            for (var k = 0; k <= MMap.mz - 1; k++)
                            {
                                Main.mmap.n[i, j, k].explored = true;
                            }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) == false &
                        Keyboard.GetState().IsKeyDown(Keys.RightShift) == false) camera.X -= 4;
                    else camera.X -= 10;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    space = true;
                }
                if (space && Keyboard.GetState().IsKeyDown(Keys.Space) == false)
                {
                    space = false;
                    PAUSE = !PAUSE;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    //mbut.InGameMainMenu();
                }

                if (Mouse.GetState().RightButton == ButtonState.Pressed && ramka_1.X == -1) ramka_1 = Selector;


                if (click_R)
                {
                    var ramka_3 = new Vector3();
                    ramka_2.X = Math.Max(Selector.X, ramka_1.X);
                    ramka_2.Y = Math.Max(Selector.Y, ramka_1.Y);
                    ramka_2.Z = Math.Max(Selector.Z, ramka_1.Z);

                    ramka_3 = new Vector3(Math.Min(Selector.X, ramka_1.X), Math.Min(Selector.Y, ramka_1.Y),
                                      Math.Min(Selector.Z, ramka_1.Z));

                    switch (lclickaction)
                    {
                        case LClickAction.Crop:
                            for (int i = (int)ramka_3.X; i <= ramka_2.X; i++)
                                for (int j = (int)ramka_3.Y; j <= ramka_2.Y; j++)
                                    for (int m = (int)ramka_3.Z; m <= ramka_2.Z; m++)
                                    {
                                        if (MMap.GoodVector3(i, j, m) && dbobject.Data[mmap.n[i, j, m].blockID].is_tree)
                                            orders.NewOrder(new Vector3(i, j, m), OrderID.crop_order);
                                    }
                            break;
                        case LClickAction.Dig:
                            for (int i = (int)ramka_3.X; i <= ramka_2.X; i++)
                                for (int j = (int)ramka_3.Y; j <= ramka_2.Y; j++)
                                    //for (int m = ramka_1.Z; m <= ramka_2.Z; m++)
                                {
                                    if (MMap.GoodVector3(i, j, (int)ramka_2.Z) &&
                                        dbobject.Data[mmap.n[i, j, (int)ramka_2.Z].blockID].is_rock)
                                        orders.NewOrder(new Vector3(i, j, ramka_2.Z), OrderID.dig_order);
                                }
                            break;
                        case LClickAction.MarkStore:
                            for (int i = (int)ramka_3.X; i <= ramka_2.X; i++)
                                for (int j = (int)ramka_3.Y; j <= ramka_2.Y; j++)
                                    //for (int m = ramka_1.Z; m <= ramka_2.Z; m++)
                                {
                                    if (MMap.GoodVector3(i, j, (int)ramka_2.Z) && mmap.n[i, j, (int)ramka_2.Z].blockID == 0 &&
                                        dbobject.Data[mmap.n[i, j, (int)ramka_2.Z + 1].blockID].createfloor)
                                    {
                                        mmap.n[i, j, (int)ramka_2.Z].blockID = 1000;
                                        buildings.list.Add(new Store(new Vector3(i, j, ramka_2.Z)));
                                    }
                                }
                            break;
                    }

                    ramka_1.X = -1;
                }

                foreach (Hero mh in heroes.n)
                {
                    mh.MoveHuman(gt);
                }

                if (!PAUSE && (asynccorear == null || asynccorear.IsCompleted))
                {
                    WorldLife.WorldTick(ref mmap);

                    asynccorear = asynccore.BeginInvoke(gt, null, null);
                }

                ingameUIpartLeftlistbox.Items.Clear();
                ingameUIpartLeftlistbox.Items.AddRange(mmap.GetMapTagsInText());

                ingameUIpartLeftlistbox2.Items.Clear();
                if (MMap.GoodVector3(Selector)) ingameUIpartLeftlistbox2.Items.AddRange(mmap.GetNodeTagsInText(Selector));

                foreach (var h in heroes.n)
                {
                    ingameUIpartLeftlistbox2.Items.Add(string.Format("{0} ord {1} id p {3}", h.pos,h.order, h.orderid, h.orderphase));
                }

                ingameUIpartLeftlistbox2.Refresh();

                //mmap.CalcWision();
            }
        }

        private static void AsyncCore(GameTime gt)
        {
            heroes.hero_move_step++; //замедление обработки перемещения героя
            if (heroes.hero_move_step >= heroes.hero_move_freq)
            {
                heroes.hero_move_step = 0;
                foreach (Hero mh in heroes.n)
                    mh.MoveHuman(gt);
            }

            buildings.BuildingsActions();
            orders.GivemOrders(ref heroes);
        }

        #endregion
    }
}