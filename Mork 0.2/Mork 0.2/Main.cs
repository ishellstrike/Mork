using System;
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
using Mork.Bad_Database;
using Mork.Generators;
using Mork.Local_Map;
using Mork.Local_Map.Dynamic.Actions;
using Mork.Local_Map.Dynamic.Local_Items;
using Mork.Local_Map.Dynamic.PlayerOrders;
using Mork.Local_Map.Dynamic.Units;
using Mork.Local_Map.Dynamic.Units.Actions;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using WButton = System.Windows.Forms.Button;
using Button = TomShane.Neoforce.Controls.Button;
using TomShane.Neoforce.Controls;
using Color = Microsoft.Xna.Framework.Color;
using Label = TomShane.Neoforce.Controls.Label;
using ListBox = TomShane.Neoforce.Controls.ListBox;
using Orientation = TomShane.Neoforce.Controls.Orientation;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using ScrollBar = TomShane.Neoforce.Controls.ScrollBar;
using TextBox = TomShane.Neoforce.Controls.TextBox;
using ToolTip = TomShane.Neoforce.Controls.ToolTip;
using TrackBar = TomShane.Neoforce.Controls.TrackBar;
using Mork.Graphics.MapEngine;
using Mork.Local_Map.Sector;


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
            Build,
            Supply,
            Cancel,
            Collect,
            MarkStorage,
            Info
        }

        public enum OnStoreTexes
        {
            Wood_log,
            Stone
        }

        public const int ssizex = 50, ssizey = 35;
        public const int resx = 1280, resy = 768;

        public static bool debug;

        public static string OurName = "";
        public static string OurVer = "";

        public static bool AllowClose;

        public static IAsyncResult Savear;

        public static IAsyncResult Generatear;
        public static float generate_d_fase;

        public static Gstate gstate = Gstate.MainMenu;
        private static string _messagestring = " ";

        public static string CurrentMap;

        public static int MeTexoncurA = 1;
        public static int MeTexoncurB = 1;
        public static int MeAct = 1;
        public static int MeObjoncur = 1;

        public static Vector3 gmap_region = new Vector3();

        //public static MMap mmap = new MMap();
        public static LocalUnits lunits = new LocalUnits();
        public static LocalHeroes lheroes = new LocalHeroes();

        public static SectorMap smap;
        public static IntersectMap imap;

        private delegate void GTDelegate(GameTime gt);
        private static GTDelegate asynccore;
        private static IAsyncResult asynccorear;

        public static LClickAction lclickaction = LClickAction.Nothing;
        public static int buildingselect;

        public static MouseState mousestate = new MouseState();

        public static KeyboardState ks;
        public static KeyboardState lks;

        public static Texture2D object_tex;


        public static Dictionary<OnStoreTexes, Texture2D[]> onstore_tex = new Dictionary<OnStoreTexes, Texture2D[]>();
                                                            //текстуры предметов на складе

        public static List<Texture2D> unit_tex = new List<Texture2D>(); // текстуры юнитов
        public static List<Texture2D> interface_tex = new List<Texture2D>(); // текстуры интрефейса

        public static DBMaterial dbmaterial;
        public static DB_LMO dbobject;
        //public static Stores buildings;
        public static GMap gmap;
        //public static PlayerOrders playerorders = new PlayerOrders();
        public static LocalItems localitems = new LocalItems();
        public static ItemStorageSystem iss = new ItemStorageSystem();
        public static Storages globalstorage = new Storages();

        public static Gmapreshim gmapreshim = Gmapreshim.Normal;

        public static FreeCamera Camera;
        public static Vector3 camera = new Vector3(0, 0, 0);
        public static Vector3 Selector;
        public static Vector3 midscreen;

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

        public static Action<GameTime, SpriteBatch, GraphicsDevice> DrawProc;
        public static Action CalcProc;

        public static Vector2 mousepos;
        public static SpriteFont Font1, Font2;   
        private static Vector3 ramka_2;
        public Vector2 MousePos;
        private static bool PAUSE;
        public float SavingDeg;

        private Color _fpsCol;
        private int _fpsCur;
        private int _fpsN;
        private TimeSpan _fpsTime;

        private static float _titleAnimation = 1;
        private static int _titlePhase = -1;
        private static int _wheellast;
        private static Vector3 ramka_1 = new Vector3(-1, 0, 0);
        private Random rnd = new Random();
        public bool space, space_helper;

        private int tick_of_5;
        private Vector3 vcalk_now, vcalk_pre;


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

            Vector3 a = new Vector3(100,100,500);
            //a = Vector3.Transform(a, Matrix.CreateLookAt(new Vector3(100, 100, 100), Vector3.Zero, Vector3.Up));
            Camera = new FreeCamera(a, MathHelper.ToRadians(45), MathHelper.ToRadians(45), GraphicsDevice);
            Camera.Target = new Vector3(100,100,0);

            Directory.CreateDirectory(@"Maps");
            Directory.CreateDirectory(@"Data");
            Directory.CreateDirectory(@"Data/Levels");

            Window.Title = OurName + " " + OurVer;

            asynccore = AsyncCore;

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
        private Button ingameshowBuildingsB;
        private Button ingameshowallinfo;

        private Window orderssubmenu;
        private Button digorder;
        private Button supplyorder;
        private Button cancelorder;
        private Button collectorder;
        private Label orderslabel;

        private Window buildinsgwindow;
        private Button[] buildingsbuttons;
        private ScrollBar buildingssb;
        private Label[] buildingsbuttonslabel;
        private int lastvalue;

        private Window SummaryWindow;
        private TextBox summarytb;

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

            #region Summary Window
            SummaryWindow = new Window(Manager) { Color = Color.Black };
            SummaryWindow.Init();
            SummaryWindow.Text = "";
            SummaryWindow.SetPosition(100, 100);
            SummaryWindow.Width = resx / 4;
            SummaryWindow.Height = resy / 4;
            SummaryWindow.Visible = false;
            SummaryWindow.BorderVisible = true;
            SummaryWindow.Movable = true;
            SummaryWindow.Resizable = false;

            summarytb = new TextBox(Manager);
            summarytb.Init();
            summarytb.Text = "";
            summarytb.Width = resx / 4 - 30;
            summarytb.Height = resy / 4 - 60;
            summarytb.Left = (20) / 2;
            summarytb.CaretVisible = false;
            summarytb.Passive = true;
            summarytb.Mode = TextBoxMode.Multiline;
            summarytb.Top = (20) / 2;
            summarytb.Anchor = Anchors.Bottom;
            summarytb.Parent = SummaryWindow;

            Manager.Add(SummaryWindow);
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

            ingameshowBuildingsB = new Button(Manager);
            ingameshowBuildingsB.Init();
            ingameshowBuildingsB.Text = "Постройки (Z)";
            ingameshowBuildingsB.Width = resx / 5 - 20;
            ingameshowBuildingsB.Height = 25;
            ingameshowBuildingsB.Left = (20) / 2;
            ingameshowBuildingsB.Top = 70;
            ingameshowBuildingsB.Anchor = Anchors.Bottom;
            ingameshowBuildingsB.Parent = ingameUIpartLeft;
            ingameshowBuildingsB.Click += new TomShane.Neoforce.Controls.EventHandler(ingameshowBuildingsB_Click);

            ingameshowallinfo = new Button(Manager);
            ingameshowallinfo.Init();
            ingameshowallinfo.Text = "Подробная информация";
            ingameshowallinfo.Width = resx / 5 - 20;
            ingameshowallinfo.Height = 25;
            ingameshowallinfo.Left = (20) / 2;
            ingameshowallinfo.Top = 100;
            ingameshowallinfo.Anchor = Anchors.Bottom;
            ingameshowallinfo.Parent = ingameUIpartLeft;
            ingameshowallinfo.Click += new TomShane.Neoforce.Controls.EventHandler(ingameshowallinfo_Click);

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

            supplyorder = new Button(Manager);
            supplyorder.Init();
            supplyorder.Text = "Обеспечить ресурсами";
            supplyorder.Width = orderssubmenu.Width - 40;
            supplyorder.Height = 24;
            supplyorder.Left = 20;
            supplyorder.Top = 50;
            supplyorder.Anchor = Anchors.Bottom;
            supplyorder.Parent = orderssubmenu;
            supplyorder.Click += new TomShane.Neoforce.Controls.EventHandler(supplyorder_Click);

            cancelorder = new Button(Manager);
            cancelorder.Init();
            cancelorder.Text = "Отменить все приказы";
            cancelorder.Width = orderssubmenu.Width - 40;
            cancelorder.Height = 24;
            cancelorder.Left = 20;
            cancelorder.Top = 80;
            cancelorder.Anchor = Anchors.Bottom;
            cancelorder.Parent = orderssubmenu;
            cancelorder.Click += new TomShane.Neoforce.Controls.EventHandler(cancelorder_Click);

            collectorder = new Button(Manager);
            collectorder.Init();
            collectorder.Text = "Cобрать";
            collectorder.Width = orderssubmenu.Width - 40;
            collectorder.Height = 24;
            collectorder.Left = 20;
            collectorder.Top = 110;
            collectorder.Anchor = Anchors.Bottom;
            collectorder.Parent = orderssubmenu;
            collectorder.Click += new TomShane.Neoforce.Controls.EventHandler(collectorder_Click);


            orderslabel = new Label(Manager);
            orderslabel.Left = 5;
            orderslabel.Top = 5;
            orderslabel.Text = "Приказы";
            orderslabel.Parent = orderssubmenu;

            Manager.Add(orderssubmenu);
            #endregion

            #region Buildings window
            buildinsgwindow = new Window(Manager) { BackColor = Color.Black };
            buildinsgwindow.Init();
            buildinsgwindow.Text = "";
            buildinsgwindow.SetPosition(20, 20);
            buildinsgwindow.Width = 42*6 + 20;
            buildinsgwindow.Height = 300;
            buildinsgwindow.Visible = false;
            buildinsgwindow.Resizable = false;

            buildingssb = new ScrollBar(Manager, Orientation.Vertical);
            buildingssb.Init();
            buildingssb.Top = 0;
            buildingssb.Width = 20;
            buildingssb.Left = buildinsgwindow.Width - buildingssb.Width - 20;
            buildingssb.Height = buildinsgwindow.Height - 40;
            buildingssb.Parent = buildinsgwindow;
            buildingssb.ValueChanged += new TomShane.Neoforce.Controls.EventHandler(buildingssb_ValueChanged);

            buildingsbuttons = new Button[dbobject.Data.Count];
            buildingsbuttonslabel = new Label[dbobject.Data.Count];
            int i = 0;
            foreach (var dbo in dbobject.Data)
            {
                buildingsbuttons[i] = new Button(Manager);
                buildingsbuttons[i].Init();
                buildingsbuttons[i].Text = dbo.Value.Name;
                buildingsbuttons[i].Width = 40;
                buildingsbuttons[i].Height = 40;
                buildingsbuttons[i].Left = i%5*42;
                buildingsbuttons[i].Top = i/5*42;
                int[] tg = { buildingsbuttons[i].Top, dbo.Key};
                buildingsbuttons[i].Tag = tg;
                buildingsbuttons[i].Anchor = Anchors.Bottom;
                buildingsbuttons[i].Parent = buildinsgwindow;
                buildingsbuttons[i].Glyph = new Glyph(object_tex, GetTexRectFromN(dbo.Value.metatex_n));
                buildingsbuttons[i].ToolTip = new ToolTip(Manager);
                buildingsbuttons[i].ToolTip.Text = dbo.Value.Name + " id " + dbo.Key;
                buildingsbuttons[i].Click += new TomShane.Neoforce.Controls.EventHandler(Buildingsbutton_Click);
                iss.n.Add(dbo.Key, new LocalItem() { id = dbo.Key, count = 0});

                buildingsbuttonslabel[i] = new Label(Manager);
                buildingsbuttonslabel[i].Init();
                buildingsbuttonslabel[i].Text = "0";
                buildingsbuttonslabel[i].Width = 40;
                buildingsbuttonslabel[i].Height = 40;
                buildingsbuttonslabel[i].Left = i % 5 * 42;
                buildingsbuttonslabel[i].Top = i / 5 * 42;
                buildingsbuttonslabel[i].Parent = buildinsgwindow;

                i++;
            }

            Manager.Add(buildinsgwindow);
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

        void ingameshowallinfo_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            lclickaction = LClickAction.Info; 
        }

        void collectorder_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            lclickaction = LClickAction.Collect;
        }

        void supplyorder_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            lclickaction = LClickAction.Supply;
        }

        void cancelorder_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            lclickaction = LClickAction.Cancel;
        }

        void buildingssb_ValueChanged(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            for (int index = 0; index < buildingsbuttons.Length; index++)
            {
                var button = buildingsbuttons[index];
                button.Top = ((int[]) (button.Tag))[0] - (int) (buildingssb.Value/50f*42f*dbobject.Data.Count/5);
                buildingsbuttonslabel[index].Top = button.Top;
            }
            lastvalue = buildingssb.Value;
        }

        void ingameshowBuildingsB_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            buildinsgwindow.Show();
            orderssubmenu.Close();
            lclickaction = LClickAction.Build;
        }

        void Buildingsbutton_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            buildingselect = ((sender as Button).Tag as int[])[1];
        }

        void cutorder_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            Main.lclickaction = Main.LClickAction.Dig;
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
            buildinsgwindow.Close();
        }

        Action<string> sp = LoadGMapFromDisc;
        void maploadmenulistbox_DoubleClick(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            
            if(Savear != null && !Savear.IsCompleted) sp.EndInvoke(Savear);
            Main.Savear = sp.BeginInvoke(string.Format(@"{0}", files[maploadmenulistbox.ItemIndex]), null, null);
            SetPhase(Main.Gstate.GenerationScreen);
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

        /// <summary>
        /// сохранить глобальную карту в виде сжатого двоичного файла
        /// </summary>
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

        /// <summary>
        /// загрузить глобальную карту из сжатого двоичного файла
        /// </summary>
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
            iss.AddItem(KnownIDs.StorageEntrance, 100000);

            SetPhase(Main.Gstate.NewGame);
            Action ingamegen = InGameGeneration;
            Generatear = ingamegen.BeginInvoke(null, null);

            StringBuilder ss = new StringBuilder();
            ss.Append(string.Format("Добро пожаловать в {0} {1} early alpha!", Main.OurName, Main.OurVer));

            ss.Append("\n\nВ вашем отряде: ");
            for (var i = 0; i <= 5; i++)
            {
                ss.Append(NamesGenerator.GetRandomName(Races.Human));
                if (i != 5) ss.Append(", ");
            }
            ss.Append(".");

            ss.Append("\n\nВсе вопросы и предложения просьба отправлять на ishellstrike@gmail.com");

            Program.game.ShowIngameMessage(ss.ToString());
        }

        /// <summary>
        /// внутриигровая генерация игровой карты из глобальной карты
        /// </summary>
        private void InGameGeneration()
        {
            Main.smap.SimpleGeneration_bygmap();

            for (var i = 0; i <= 9; i++)
            {
                Main.lheroes.n.Add(new LocalHero(){pos = new Vector3(20,20,0)});
                var temp = Main.lheroes.n[Main.lheroes.n.Count - 1];
                var k = 0;
                temp.pos = new Vector3(temp.pos.X + rnd.Next(0, 6) - 3, temp.pos.Y + rnd.Next(0, 6) - 3, k);
                for (k = 0; k <= MMap.mz - 1; k++)
                {
                    if (MMap.GoodVector3(temp.pos.X, temp.pos.Y, k) && Main.smap.At(temp.pos.X, temp.pos.Y, k).BlockID == 0) continue;
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

        /// <summary>
        /// базовый тик обновления игры
        /// </summary>
        /// <param name="gameTime"></param>
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

            switch (gstate)
            {
                case Gstate.CloseGame:
                    Exit();
                    break;
                case Gstate.InGameMessage:
                    break;
                case Gstate.NewGame:
                    GameUpdate(gameTime);
                    break;
                case Gstate.GenerationScreen:
                    GenerationScreen();
                    break;
            }

            _wheellast = mousestate.ScrollWheelValue;

            if (ks.IsKeyDown(Keys.F4) && !lks.IsKeyDown(Keys.F4))
            {
                debug = !debug;
            }

            if (ks.IsKeyDown(Keys.F5) && !lks.IsKeyDown(Keys.F5))
            {
                smap.RebuildAllMapGeo();
            }

            //mbut.ActButtons();

            Manager.Update(gameTime);

            base.Update(gameTime);

            if(debug)
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
        public void SetPhase(Gstate gs)
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
            //_fpsN++;
            //tick_of_5++;
            //tick_of_5 = tick_of_5 > 5 ? 1 : tick_of_5;

            Manager.BeginDraw(gameTime);
            Manager.Draw(gameTime);
            GraphicsDevice.Clear(Commons.skycolor);

            DrawProc(gameTime, spriteBatch, GraphicsDevice);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);


            if (debug)
            {
                string outp = "";
                outp += string.Format("mpos = {0}", mousepos)+Environment.NewLine;
                outp += string.Format("selector = {0}", Selector) + Environment.NewLine;
                //outp += string.Format("ord = {0}", playerorders.n.Count) + Environment.NewLine;
                outp += string.Format("act = {0}", smap.Active.Count) + Environment.NewLine;
                outp += string.Format("time = {0}", gameTime.TotalGameTime) + Environment.NewLine;
                outp += string.Format("srg = {0}", globalstorage.n.Count) + Environment.NewLine;
                outp += string.Format("cam = {0}\nrot = {1}", Camera.Target, camerarotation) + Environment.NewLine;
                outp += string.Format("verts = {0}, sect = {1}", drawed_verts, drawed_sects) + Environment.NewLine;
                outp += string.Format("total rebuild = {0}", sectrebuild) + Environment.NewLine;

                spriteBatch.DrawString(Font2, outp, new Vector2(800, 5), Color.White);
            }

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
                SavingDeg += (float) Math.PI * (float)gameTime.ElapsedGameTime.TotalSeconds*5;
                if (SavingDeg >= Math.PI*2) SavingDeg = 0;
            }

            if (Generatear != null && !Generatear.IsCompleted)
            {
                spriteBatch.Draw(interface_tex[17], new Vector2(130, 40), new Rectangle((int)generate_d_fase*41, 0, 41, 28),
                                 Color.White, 0, new Vector2(32, 32), Vector2.One, SpriteEffects.None, 0);

                generate_d_fase += (float)gameTime.ElapsedGameTime.TotalSeconds*20;
                if (generate_d_fase > 15) generate_d_fase = 0;
            }
            else generate_d_fase = 0;

            
            spriteBatch.End();

            Manager.EndDraw();

            if(debug)
            FrameRateCounter.Draw(gameTime, Font2, spriteBatch, lineBatch, resx, resy);

            lineBatch.Draw();
            lineBatch.Clear();

            base.Draw(gameTime);
        }

        /// <summary>
        /// показать окно с внутриигровым сообщением
        /// </summary>
        /// <param name="s">сообщение</param>
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

        public void ShowIngameSummary(string s)
        {
            SummaryWindow.Show();
            summarytb.Text = s;
            summarytb.Refresh();
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

        private void GameDraw(GameTime gameTime, SpriteBatch sb, GraphicsDevice GraphicsDevice)
        {
            smap.DrawAllMap(gameTime, Camera);

            sb.Begin();

            for (int i = 0; i <= Textlogmax - 1; i++) // отрисова лога
            {
                spriteBatch.DrawString(Font1, textlog[i], new Vector2(8, resy - 192 + i*15), Color.White);
            }
            sb.End();
        }

        #endregion

        /// <summary>
        /// заменить всем id, которых нет в базе, но есть на карте, на блоки ошибки, которые ничего не делают, но не убивают игру
        /// </summary>
        /// <param name="map"></param>
        public static void PrepairMapDeleteWrongIDs(ref SectorMap map)
        {
            for (int i = 0; i < map.N.Length; i++)
            {
                for (int j = 0; j < map.N[i].N.Length; j++)
                {
                    if (!dbobject.Data.ContainsKey(map.N[i].N[j].BlockID)) map.N[i].N[j].BlockID = KnownIDs.error;
                }
            }
        }

        #region Updates

        private float camerarotation = 0.7f;
        public static int drawed_verts;
        public static int drawed_sects;
        private float cameradistance = 30;
        public static int z_cam;
        private int notfastcam = 0;
        public static int sectrebuild = 0;

        private Ray MouseRay;

        private void GameUpdate(GameTime gt)
        {
            Vector3 moving = Vector3.Zero;

            KeyboardUpdate(gt, ref moving);

            moving = Vector3.Transform(moving, Matrix.CreateRotationZ(MathHelper.ToRadians(camerarotation)));

            Camera.Target += moving;
            Camera.View = FreeCamera.BuildViewMatrix(Camera.Target, (float)Math.PI/5, 0, MathHelper.ToRadians(camerarotation), cameradistance);
            Camera.generateFrustum();

            Vector3 near = new Vector3(MousePos.X, MousePos.Y, 0);
            Vector3 far = new Vector3(MousePos.X, MousePos.Y, 1);

            Vector3 nun = GraphicsDevice.Viewport.Unproject(near, Camera.Projection, Camera.View, Matrix.Identity);
            Vector3 fun = GraphicsDevice.Viewport.Unproject(far, Camera.Projection, Camera.View, Matrix.Identity);

            Vector3 raydir = fun - nun;
            raydir.Normalize();

            MouseRay = new Ray(nun, raydir);

            bool tempb = true;
            for (int i = 0; i < imap.N.Length && tempb; i++)
            {
                var nn = imap.N[i];
                var f = MouseRay.Intersects(nn);

                if (f.HasValue)
                {
                    Selector.X = i /(SectorMap.Sectn * MapSector.dimS);
                    Selector.Y = i % (SectorMap.Sectn * MapSector.dimS);
                    Selector.Z = z_cam;

                    tempb = false;
                }
            }

            if (Mouse.GetState().ScrollWheelValue > _wheellast)
            {
                cameradistance -= (float)gt.ElapsedGameTime.TotalSeconds * 250;
            }

            if (Mouse.GetState().ScrollWheelValue < _wheellast)
            {
                cameradistance += (float)gt.ElapsedGameTime.TotalSeconds * 250;
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed && ramka_1.X == -1) ramka_1 = Selector;


            if (click_R)
            {
                Ramka(gt);
            }

            if (!PAUSE)
            {
                //playerorders.OrdersUpdate(gt, lheroes);
                lheroes.Update(gt);
                lunits.Update(gt);

                //asynccorear = asynccore.BeginInvoke(gt, null, null);
            }

            ingameUIpartLeftlistbox.Items.Clear();
            //ingameUIpartLeftlistbox.Items.AddRange(smap.GetMapTagsInText());


            ingameUIpartLeftlistbox2.Items.Clear();
                

            if (MMap.GoodVector3(Selector))
            {
                ingameUIpartLeftlistbox2.Items.Add("hp = " +
                                                    smap.At(Selector.X, Selector.Y, Selector.Z).Health);
                ingameUIpartLeftlistbox2.Items.Add("explored = " +
                                                    smap.At(Selector.X, Selector.Y, Selector.Z).Explored);
                ingameUIpartLeftlistbox2.Items.Add("subterrain = " +
                                                    smap.At(Selector.X,Selector.Y, Selector.Z).Subterrain);
            }
            //if (MMap.GoodVector3(Selector)) ingameUIpartLeftlistbox2.Items.AddRange(smap.GetNodeTagsInText(Selector));

            ingameUIpartLeftlistbox2.Refresh();

            for (int index = 0; index < buildingsbuttons.Length; index++)
            {
                var button = buildingsbuttons[index];
                button.Visible = iss.n[(button.Tag as int[])[1]].count != 0;
                buildingsbuttonslabel[index].Text = iss.n[(button.Tag as int[])[1]].count.ToString();
            }
            //buildingsbuttons = new Button[iss.n.Count];
            //for (int i = 0; i < iss.n.Count; i++)
            //{
            //        buildingsbuttons[i] = new Button(Manager);
            //        buildingsbuttons[i].Init();
            //        buildingsbuttons[i].Text = dbobject.Data[iss.n[i].id].Name;
            //        buildingsbuttons[i].Width = 40;
            //        buildingsbuttons[i].Height = 40;
            //        buildingsbuttons[i].Left = i%5*42;
            //        buildingsbuttons[i].Top = i/5*42;
            //        int[] tg = { buildingsbuttons[i].Top - (int)(buildingssb.Value/50f*42f*iss.n.Count/5), iss.n[i].id };
            //        buildingsbuttons[i].Tag = tg;
            //        buildingsbuttons[i].Anchor = Anchors.Bottom;
            //        buildingsbuttons[i].Parent = buildinsgwindow;
            //        buildingsbuttons[i].Glyph = new Glyph(object_tex, GetTexRectFromN(dbobject.Data[iss.n[i].id].metatex_n));
            //        buildingsbuttons[i].ToolTip = new ToolTip(Manager);
            //        buildingsbuttons[i].ToolTip.Text = dbobject.Data[iss.n[i].id].Name + " id " + iss.n[i].id;
            //        buildingsbuttons[i].Click += new TomShane.Neoforce.Controls.EventHandler(Buildingsbutton_Click);
            //}
            //buildinsgwindow.Refresh();
           
        }

        private void KeyboardUpdate(GameTime gt, ref Vector3 moving)
        {
            lks = ks;
            ks = Keyboard.GetState();

            if (ks[Keys.W] == KeyState.Down)
            {
                moving += Vector3.Up * (float)gt.ElapsedGameTime.TotalSeconds * 8;
            }
            if (ks[Keys.S] == KeyState.Down)
            {
                moving += Vector3.Down * (float)gt.ElapsedGameTime.TotalSeconds * 8;
            }
            if (ks[Keys.A] == KeyState.Down)
            {
                moving += Vector3.Left * (float)gt.ElapsedGameTime.TotalSeconds * 8;
            }
            if (ks[Keys.D] == KeyState.Down)
            {
                moving += Vector3.Right * (float)gt.ElapsedGameTime.TotalSeconds * 8;
            }

            float roll = 0;
            if (ks[Keys.Q] == KeyState.Down)
            {
                camerarotation += 1 * (float)gt.ElapsedGameTime.TotalSeconds * 80;
                if (camerarotation > 360) camerarotation -= 360;
            }
            if (ks[Keys.E] == KeyState.Down)
            {
                camerarotation -= 1 * (float)gt.ElapsedGameTime.TotalSeconds * 80;
                if (camerarotation < 0) camerarotation += 360;
            }

            if (notfastcam > 0) notfastcam--;

            if (ks[Keys.OemComma] == KeyState.Down && lks[Keys.OemComma] == KeyState.Up)
            {
                if (z_cam < 127)
                {
                    z_cam++;
                    imap.MoveIntersectMap(new Vector3(0, 0, 1));
                    if (z_cam < 127 - 5)
                        if (ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift))
                        {
                            z_cam += 4;
                            imap.MoveIntersectMap(new Vector3(0, 0, 4));
                        }
                    smap.RebuildAllMapGeo();
                }
            }
            if (ks[Keys.OemPeriod] == KeyState.Down && lks[Keys.OemPeriod] == KeyState.Up)
            {
                if (z_cam > 0)
                {
                    z_cam--;
                    imap.MoveIntersectMap(new Vector3(0, 0, -1));
                    if(z_cam > 5)
                        if (ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift))
                        {
                            z_cam -= 4;
                            imap.MoveIntersectMap(new Vector3(0, 0, -4));
                        }

                    smap.RebuildAllMapGeo();
                }
            }

            if (ks[Keys.LeftShift] == KeyState.Down)
            {
                moving *= 3;
            }

            if (ks.IsKeyDown(Keys.Down) || ks.IsKeyDown(Keys.S))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) == false &
                    Keyboard.GetState().IsKeyDown(Keys.RightShift) == false)
                    camera.Y -= (float) (400*gt.ElapsedGameTime.TotalSeconds);
                else camera.Y -= (float) (1000*gt.ElapsedGameTime.TotalSeconds);
            }

            if (ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) == false &
                    Keyboard.GetState().IsKeyDown(Keys.RightShift) == false)
                    camera.Y += (float) (400*gt.ElapsedGameTime.TotalSeconds);
                else camera.Y += (float) (1000*gt.ElapsedGameTime.TotalSeconds);
            }

            if (ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.A))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) == false &
                    Keyboard.GetState().IsKeyDown(Keys.RightShift) == false)
                    camera.X += (float) (400*gt.ElapsedGameTime.TotalSeconds);
                else camera.X += (float) (1000*gt.ElapsedGameTime.TotalSeconds);
            }

            if (ks.IsKeyDown(Keys.Right) || ks.IsKeyDown(Keys.D))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) == false &
                    Keyboard.GetState().IsKeyDown(Keys.RightShift) == false)
                    camera.X -= (float) (400*gt.ElapsedGameTime.TotalSeconds);
                else camera.X -= (float) (1000*gt.ElapsedGameTime.TotalSeconds);
            }

            if (ks.IsKeyDown(Keys.N) && !lks.IsKeyDown(Keys.N))
            {
                for (var i = 0; i <= MMap.mx - 1; i++)
                    for (var j = 0; j <= MMap.my - 1; j++)
                        for (var k = 0; k <= MMap.mz - 1; k++)
                        {
                            smap.At(i, j, k).Explored = true;
                        }
            }

            if (ks.IsKeyDown(Keys.Space) && !lks.IsKeyDown(Keys.Space))
            {
                PAUSE = !PAUSE;
            }
        }

        /// <summary>
        /// раздача приказов для выделенной рамки, приказ зависит от Main.lclickaction
        /// </summary>
        /// <param name="gt"></param>
        private void Ramka(GameTime gt)
        {
            var ramka_3 = new Vector3();
            ramka_2.X = Math.Max(Selector.X, ramka_1.X);
            ramka_2.Y = Math.Max(Selector.Y, ramka_1.Y);
            ramka_2.Z = Math.Max(Selector.Z, ramka_1.Z);

            ramka_3 = new Vector3(Math.Min(Selector.X, ramka_1.X), Math.Min(Selector.Y, ramka_1.Y),
                                  Math.Min(Selector.Z, ramka_1.Z));

            //switch (lclickaction)
            //{
            //    case LClickAction.Dig:
            //        for (int i = (int) ramka_3.X; i <= ramka_2.X; i++)
            //            for (int j = (int) ramka_3.Y; j <= ramka_2.Y; j++)
            //                //for (int m = ramka_1.Z; m <= ramka_2.Z; m++)
            //            {
            //                if (MMap.GoodVector3(i, j, (int) ramka_2.Z))
            //                    playerorders.n.Add(new DestroyOrder {dest = new Vector3(i, j, ramka_2.Z)});
            //            }
            //        break;

            //    case LClickAction.Collect:
            //        for (int i = (int)ramka_3.X; i <= ramka_2.X; i++)
            //            for (int j = (int)ramka_3.Y; j <= ramka_2.Y; j++)
            //            //for (int m = ramka_1.Z; m <= ramka_2.Z; m++)
            //            {
            //                List<LocalItem> t = new List<LocalItem>();
            //                foreach (var a in localitems.n)
            //                {
            //                    if (a.pos.X == i && a.pos.Y == j && a.pos.Z == ramka_2.Z) playerorders.n.Add(new CollectOrder { dest = new Vector3(i, j, ramka_2.Z), tocollect = a});
            //                }
            //            }
            //        break;

            //        case LClickAction.Build:
            //        for (int i = (int)ramka_3.X; i <= ramka_2.X; i++)
            //            for (int j = (int)ramka_3.Y; j <= ramka_2.Y; j++)
            //            //for (int m = ramka_1.Z; m <= ramka_2.Z; m++)
            //            {
            //                if (MMap.GoodVector3(i, j, (int)ramka_2.Z) && mmap.n[i, j, (int)ramka_2.Z+1].blockID != 0 && iss.n[buildingselect].count >= 1)
            //                    playerorders.n.Add(new BuildOrder { dest = new Vector3(i, j, ramka_2.Z), blockID = buildingselect});
            //            }
            //        break;

            //        case LClickAction.Supply:
            //        for (int i = (int)ramka_3.X; i <= ramka_2.X; i++)
            //            for (int j = (int)ramka_3.Y; j <= ramka_2.Y; j++)
            //            //for (int m = ramka_1.Z; m <= ramka_2.Z; m++)
            //            {
            //                if (MMap.GoodVector3(i, j, (int)ramka_2.Z) && mmap.n[i, j, (int)ramka_2.Z].tags.ContainsKey("convert"))
            //                    playerorders.n.Add(new SupplyOrder { dest = new Vector3(i, j, ramka_2.Z)});
            //            }
            //        break;

            //        case LClickAction.Info:
            //        if(MMap.GoodVector3(Selector))
            //            {
            //                string s = "";
            //                var t = mmap.GetNodeTagsInText((int) Selector.X, (int) Selector.Y, (int) Selector.Z);
            //                foreach (var ss in t)
            //                {
            //                    s += ss + Environment.NewLine;
            //                }
            //                ShowIngameSummary(s);
            //            }
            //        break;

            //        case LClickAction.Cancel:
            //        for (int i = (int)ramka_3.X; i <= ramka_2.X; i++)
            //            for (int j = (int)ramka_3.Y; j <= ramka_2.Y; j++)
            //            //for (int m = ramka_1.Z; m <= ramka_2.Z; m++)
            //            {
            //                for (int index = 0; index < playerorders.n.Count; index++)
            //                {
            //                    var o = playerorders.n[index];
            //                    if (o.dest.X == i && o.dest.Y == j && ramka_2.Z == o.dest.Z)
            //                    {
            //                        if(o.unit_owner != null)
            //                        o.unit_owner.current_order = new NothingOrder();
            //                        playerorders.n.Remove(o);
            //                    }
            //                }
            //            }
            //        break;

            //}

            ramka_1.X = -1;
        }

        private static void AsyncCore(GameTime gt)
        {

        }

        #endregion
    }
}