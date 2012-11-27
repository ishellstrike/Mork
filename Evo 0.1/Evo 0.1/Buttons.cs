using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mork.Generators;

namespace Mork
{
    //public class MBut //все кнопки в игре
    //{
    //    public static Main.Gstate From = Main.Gstate.MainMenu;

    //    readonly Random _rnd = new Random();
    //    List<BNode> n = new List<BNode>();
    //    public void CreateButton(Vector2 a, string texta, ButtonsActionID act)
    //    {
    //        n.Add(new BNode(a, texta, act));
    //    }
    //    public void ClearButtons()
    //    {
    //        n.Clear();
    //    }
    //    public void DrawButtons()
    //    {
    //        foreach (BNode temp in n)
    //        {
    //            var aimed = Main.mousepos.X >= temp.V1.X && Main.mousepos.X <= temp.V2.X && Main.mousepos.Y >= temp.V1.Y && Main.mousepos.Y <= temp.V2.Y;

    //            if (aimed) 
    //            {
    //                Main.spriteBatch.DrawString(Main.Font1, temp.Text, temp.V1, Color.Tomato);
    //                if (Mouse.GetState().LeftButton == ButtonState.Pressed) Main.spriteBatch.DrawString(Main.Font1, temp.Text, temp.V1, Color.Red);
    //            }
    //            else
    //            Main.spriteBatch.DrawString(Main.Font1, temp.Text, temp.V1, Color.White);
    //        }
    //    }
    //    public void ActButtons()
    //    {
    //        if (Main.click_L)
    //            foreach (var temp in n.Where(temp => Main.mousepos.X >= temp.V1.X && Main.mousepos.X <= temp.V2.X && Main.mousepos.Y >= temp.V1.Y && Main.mousepos.Y <= temp.V2.Y))
    //                switch (temp.Action)
    //                {
    //                    case ButtonsActionID.CloseGame:
    //                        if (Main.Savear == null || Main.Savear.IsCompleted)
    //                        {
    //                            Main.gstate = Main.Gstate.CloseGame;
    //                            Main.AllowClose = true;
    //                        }
    //                        return;
    //                    case ButtonsActionID.ClosingMenu:
    //                        OnClose();
    //                        Main.SetPhase(Main.Gstate.MainMenu);
    //                        return;
    //                    case ButtonsActionID.ToMainOptions:
    //                        Main.SetPhase(Main.Gstate.MainOptions);
    //                        ClearButtons();
    //                        CreateButton(new Vector2(300, 300), "Назад", ButtonsActionID.ToMainMenu);
    //                        return;
    //                    case ButtonsActionID.ToMainMenu:
    //                        Main.SetPhase(Main.Gstate.MainMenu);
    //                        ClearButtons();
    //                        MainMenuCreate();
    //                        return;
    //                    case ButtonsActionID.ToGenerationOptions:
    //                        ClearButtons();
    //                        Main.SetPhase(Main.Gstate.GenerationOptions);
    //                        CreateButton(new Vector2(20, 200), "Далее", ButtonsActionID.ToGeneration);
    //                        CreateButton(new Vector2(20, 220), "Главное меню", ButtonsActionID.ToMainMenu);
    //                        return;
    //                    case ButtonsActionID.ToGeneration:
    //                        ClearButtons();
    //                        Main.SetPhase(Main.Gstate.GenerationScreen);
    //                        CreateButton(new Vector2(20, 80), "Начать игру", ButtonsActionID.ToGameNewMap);
    //                        CreateButton(new Vector2(20, 100), "Генерировать", ButtonsActionID.Generate);
    //                        CreateButton(new Vector2(20, 130), "Обычная карта", ButtonsActionID.TurnNMap);
    //                        CreateButton(new Vector2(20, 150), "Карта высот", ButtonsActionID.TurnHMap);
    //                        CreateButton(new Vector2(20, 170), "Карта температур", ButtonsActionID.TurnTMap);
    //                        CreateButton(new Vector2(20, 300), "Опции", ButtonsActionID.ToGenerationOptions);
    //                        return;
    //                    case ButtonsActionID.ToGameNewMap:
    //                        if (Main.Generatear == null || Main.Generatear.IsCompleted)
    //                        {
    //                            ToGame();
    //                            SimpleProc ss = SaveGMapToDisc;
    //                            Main.Savear = ss.BeginInvoke(null, null);
    //                        }
    //                        return;
    //                    case ButtonsActionID.ToGameOldMap:
    //                        if (Main.Savear == null || Main.Savear.IsCompleted)
    //                        {
    //                            ToGame();
    //                        }
    //                        return;
    //                    case ButtonsActionID.ToLoadMap:
    //                        ClearButtons();

    //                        Main.SetPhase(Main.Gstate.MainMenu);

    //                        var k = 0;
    //                        for (var i = 1; i <= 10; i++)
    //                        {
    //                            if (File.Exists(string.Format(@"Maps\World{0}.zmm", i)))
    //                            {
    //                                string s = "Название утеряно";
    //                                if (File.Exists(string.Format(@"Maps\World{0}.zmn", i)))
    //                                {
    //                                    StreamReader sr = File.OpenText(string.Format(@"Maps\World{0}.zmn", i));
    //                                    s = sr.ReadToEnd();
    //                                }
    //                                CreateButton(new Vector2(320, 200 + k * 25), string.Format("World{0}....{1}", i, s), ButtonsActionID.OpenMap1+(i-1));
    //                            }
    //                            k++;
    //                        }

    //                        CreateButton(new Vector2(300, 500), "Назад", ButtonsActionID.ToMainMenu);
    //                        return;

    //                    case ButtonsActionID.OpenMap1:
    //                    case ButtonsActionID.OpenMap2:
    //                    case ButtonsActionID.OpenMap3:
    //                    case ButtonsActionID.OpenMap4:
    //                    case ButtonsActionID.OpenMap5:
    //                    case ButtonsActionID.OpenMap6:
    //                    case ButtonsActionID.OpenMap7:
    //                    case ButtonsActionID.OpenMap8:
    //                    case ButtonsActionID.OpenMap9:
    //                    case ButtonsActionID.OpenMap10:
    //                        StringProc sp = LoadGMapFromDisc;
    //                        Main.Savear = sp.BeginInvoke(string.Format(@"Maps\World{0}.zmm", (temp.Action - ButtonsActionID.OpenMap1 + 1).ToString()), null, null);

    //                        ClearButtons();
    //                        Main.SetPhase(Main.Gstate.GenerationScreen);
    //                        CreateButton(new Vector2(20, 80), "Начать игру", ButtonsActionID.ToGameOldMap);

    //                        CreateButton(new Vector2(20, 130), "Обычная карта", ButtonsActionID.TurnNMap);
    //                        CreateButton(new Vector2(20, 150), "Карта высот", ButtonsActionID.TurnHMap);
    //                        CreateButton(new Vector2(20, 170), "Карта температур", ButtonsActionID.TurnTMap);

    //                        CreateButton(new Vector2(20, 300), "Назад", ButtonsActionID.ToLoadMap);
    //                        return;

    //                    case ButtonsActionID.IngameMainmenu:
    //                        InGameMainMenu();
    //                        return;
    //                    case ButtonsActionID.IngameZonesSubmenu:
    //                        ZonesSubmenu();
    //                        return;
    //                    case ButtonsActionID.IngameOrdersSubmenu:
    //                        OrdersSubmenu();
    //                        return;
    //                    case ButtonsActionID.IngameOrdersDig:
    //                        ClearButtons();
    //                        Main.lclickaction = Main.LClickAction.Dig;
    //                        return;
    //                    case ButtonsActionID.IngameOrdersCrop:
    //                        ClearButtons();
    //                        Main.lclickaction = Main.LClickAction.Crop;
    //                        return;
    //                    case ButtonsActionID.IngameZonesStore:
    //                        ClearButtons();
    //                        Main.lclickaction = Main.LClickAction.MarkStore;
    //                        return;
    //                    case ButtonsActionID.Generate: //generate
    //                        if (Main.Generatear == null || Main.Generatear.IsCompleted)
    //                        {
    //                            GMap.GenerateProc proc = Main.gmap.GenerateGWorld;
    //                            Main.Generatear = proc.BeginInvoke(10, null, null);
    //                        }
    //                        //Main.gmap.GenerateGWorld();

    //                        return;
    //                    case ButtonsActionID.TurnHMap:
    //                        Main.gmapreshim = Main.Gmapreshim.Height;
    //                        return;
    //                    case ButtonsActionID.TurnNMap:
    //                        Main.gmapreshim = Main.Gmapreshim.Normal;
    //                        return;
    //                    case ButtonsActionID.TurnTMap:
    //                        Main.gmapreshim = Main.Gmapreshim.Tempirature;
    //                        return;
    //                    case ButtonsActionID.CloseBack:
    //                        switch (From)
    //                        {
    //                            case Main.Gstate.NewGame:
    //                                Main.SetPhase(Main.Gstate.NewGame);
    //                                InGameMainMenu();
    //                                break;
    //                            default:
    //                                Main.SetPhase(Main.Gstate.MainMenu);
    //                                MainMenuCreate();
    //                                break;
    //                        }
    //                        return;
    //                }
    //    }

    //    private static void SaveGMapToDisc()
    //    {
    //        var k = 0;
    //        var findCorrect = false;
    //        do { k++; findCorrect = File.Exists(string.Format(@"Maps\World{0}.zmm", k)); } while (findCorrect);

    //        var f = File.Create(string.Format(@"Maps\World{0}.zmm", k));
    //        Main.CurrentMap = string.Format(@"Maps\World{0}.zmm", k);
    //        var gz = new GZipStream(f, CompressionMode.Compress, false);

    //        for (int i = 0; i <= GMap.size - 1; i++)
    //            for (int j = 0; j <= GMap.size - 2; j+=2)
    //            {
    //                gz.Write(BitConverter.GetBytes(Main.gmap.n[i, j]), 0, sizeof(double));
    //                gz.Write(BitConverter.GetBytes((int)Main.gmap.obj[i, j]), 0, sizeof(int));
    //                gz.Write(BitConverter.GetBytes(Main.gmap.t[i, j]), 0, sizeof(byte));

    //                gz.Write(BitConverter.GetBytes(Main.gmap.n[i, j+1]), 0, sizeof(double));
    //                gz.Write(BitConverter.GetBytes((int)Main.gmap.obj[i, j+1]), 0, sizeof(int));
    //                gz.Write(BitConverter.GetBytes(Main.gmap.t[i, j+1]), 0, sizeof(byte));
    //            }
    //    }

    //    private static void LoadGMapFromDisc(string s)
    //    {
    //        var f = File.OpenRead(s);
    //        var gz = new GZipStream(f, CompressionMode.Decompress, false);

    //        for (int i = 0; i <= GMap.size - 1; i++)
    //            for (int j = 0; j <= GMap.size - 2; j += 2)
    //            {
    //                var m1 = new byte[sizeof(double)];
    //                gz.Read(m1, 0, sizeof(double));
    //                Main.gmap.n[i, j] = BitConverter.ToDouble(m1, 0);

    //                var m2 = new byte[sizeof(int)];
    //                gz.Read(m2, 0, sizeof(int));
    //                Main.gmap.obj[i, j] = (GMapObj) BitConverter.ToInt32(m2, 0);

    //                var m3 = new byte[sizeof(byte)];
    //                gz.Read(m3, 0, sizeof(byte));
    //                Main.gmap.t[i, j] = m3[0];

    //                m1 = new byte[sizeof(double)];
    //                gz.Read(m1, 0, sizeof(double));
    //                Main.gmap.n[i, j + 1] = BitConverter.ToDouble(m1, 0);

    //                m2 = new byte[sizeof(int)];
    //                gz.Read(m2, 0, sizeof(int));
    //                Main.gmap.obj[i, j + 1] = (GMapObj)BitConverter.ToInt32(m2, 0);

    //                m3 = new byte[sizeof(byte)];
    //                gz.Read(m3, 0, sizeof(byte));
    //                Main.gmap.t[i, j + 1] = m3[0];
    //            }
    //    }

    //    private void ToGame()
    //    {
    //        Main.SetPhase(Main.Gstate.NewGame);
    //        SimpleProc ingamegen = InGameGeneration;
    //        Main.Generatear = ingamegen.BeginInvoke(null, null);
    //        InGameMainMenu();

    //        WorldLife.Age = NamesGenerator.GetAgeName();
    //        WorldLife.Day = _rnd.Next(1, 30);
    //        WorldLife.Month = 3;
    //        WorldLife.Year = _rnd.Next(1, 1000);

    //        StringBuilder ss = new StringBuilder();
    //        ss.Append(string.Format("Добро пожаловать в {0} {1} early alpha!", Main.OurName, Main.OurVer));

    //        if (WorldLife.Year <= 100) ss.Append(string.Format("\n\n" + "Начало эпохи {0}", WorldLife.Age));
    //        else if (WorldLife.Year >= 700) ss.Append(string.Format("\n\n" + "Закат эпохи {0}", WorldLife.Age));
    //        else ss.Append(string.Format("\n\n" + "Эпоха {0}", WorldLife.Age));
    //        ss.Append(string.Format(", год {0}, {1} {2}", WorldLife.Year, WorldLife.Day, NamesGenerator.MonthNameR(WorldLife.Month)));

    //        ss.Append("\n\nВ вашем отряде: ");
    //        for (var i = 0; i <= 5; i++)
    //        {
    //            ss.Append(NamesGenerator.GetRandomName(Races.Human));
    //            if (i != 5) ss.Append( ", ");
    //        }
    //        ss.Append(".");

    //        ss.Append("\n\nВсе вопросы и предложения просьба отправлять на ishellstrike@gmail.com");

    //        Program.game.ShowIngameMessage(ss.ToString());
    //    }

    //    public void OnClose()
    //    {
    //        Main.SetPhase(Main.gstate);
    //        ClearButtons();

    //        //CreateButton(new Vector2(300, 300), "Создать новый мир", ButtonsActionID.ToGenerationOptions);

    //        CreateButton(new Vector2(500, 300), "Назад", ButtonsActionID.CloseBack);

    //        CreateButton(new Vector2(500, 400), "Сохранить и выйти", ButtonsActionID.CloseGame);
    //    }

    //    private void InGameGeneration()
    //    {
    //        Main.mmap.SimpleGeneration_bygmap();

    //        for (var i = 0; i <= 9; i++)
    //        {
    //            Main.heroes.n.Add(new Hero());
    //            var temp = Main.heroes.n[Main.heroes.n.Count - 1];
    //            var k = 0;
    //            temp.pos = new Vector3(temp.pos.X + _rnd.Next(0, 6) - 3, temp.pos.Y + _rnd.Next(0, 6) - 3, k);
    //            for (k = 0; k <= MMap.mz - 1; k++)
    //            {
    //                if (Main.mmap.n[temp.pos.X, temp.pos.Y, k].Obj == ObjectID.None) continue;
    //                k--;
    //                goto here;
    //            }
    //        here: ;
    //            temp.pos = new Vector3(temp.pos.X, temp.pos.Y, k);
    //        }
    //    }

    //    public void InGameMainMenu()
    //    {
    //        ClearButtons();
    //        CreateButton(new Vector2(1052, 140), "Приказы (O)", ButtonsActionID.IngameOrdersSubmenu);
    //        CreateButton(new Vector2(1052, 160), "Зоны (Z)", ButtonsActionID.IngameZonesSubmenu);

    //        CreateButton(new Vector2(1152, 600), "Выйти", ButtonsActionID.ClosingMenu);
    //        Main.lclickaction = Main.LClickAction.Nothing;
    //    }

    //    public void MainMenuCreate()
    //    {
    //        ClearButtons();
    //        CreateButton(new Vector2(500, 250), "Создать новый мир и начать игру", ButtonsActionID.ToGenerationOptions);

    //        CreateButton(new Vector2(500, 300), "Начать игру в созданном мире", ButtonsActionID.ToLoadMap);

    //        CreateButton(new Vector2(500, 350), "Загрузить игру", ButtonsActionID.ToLoadGame);

    //        CreateButton(new Vector2(500, 400), "Опции", ButtonsActionID.ToMainOptions);

    //        CreateButton(new Vector2(500, 500), "Выйти", ButtonsActionID.CloseGame);

    //        CreateButton(new Vector2(1053, 715), Main.OurVer, 0);
    //    }

    //    private void OrdersSubmenu()
    //    {
    //        ClearButtons();
    //        CreateButton(new Vector2(1052, 140), "Выкопать (G)", ButtonsActionID.IngameOrdersDig);
    //        CreateButton(new Vector2(1052, 160), "Срубить (C)", ButtonsActionID.IngameOrdersCrop);
    //    }

    //    private void ZonesSubmenu()
    //    {
    //        ClearButtons();
    //        CreateButton(new Vector2(1052, 140), "Разметить склад (M)", ButtonsActionID.IngameZonesStore);
    //    }
    //}
    //public delegate void StringProc(string s);
    //public enum ButtonsActionID
    //{
    //    Nothing,
    //    CloseGame,
    //    ClosingMenu,
    //    ToMainMenu,
    //    ToGenerationOptions,
    //    ToGeneration,
    //    Generate,
    //    ToGameNewMap,
    //    ToGameOldMap,
    //    ToMainOptions,
    //    ToLoadGame,
    //    ToLoadMap,
    //    IngameMainmenu,
    //    IngameOrdersSubmenu,
    //    IngameOrdersDig,
    //    IngameOrdersCrop,
    //    IngameZonesSubmenu,
    //    IngameZonesStore,
    //    TurnNMap,
    //    TurnHMap,
    //    TurnTMap,
    //    CloseBack,
    //    OpenMap1,
    //    OpenMap2,
    //    OpenMap3,
    //    OpenMap4,
    //    OpenMap5,
    //    OpenMap6,
    //    OpenMap7,
    //    OpenMap8,
    //    OpenMap9,
    //    OpenMap10,
    //}
    //public class BNode
    //{
    //    public ButtonsActionID Action = ButtonsActionID.Nothing;
    //    public Vector2 V1, V2;
    //    public string Text;
    //    public BNode()
    //    {
    //        V1 = new Vector2(0, 0);
    //        V2 = new Vector2(0, 0);
    //        Text = "";
    //    }
    //    public BNode(float a1, float a2, float b1, float b2, string texta, ButtonsActionID act)
    //    {
    //        V1 = new Vector2(a1, b1);
    //        V2 = new Vector2(a2, b2);
    //        Text = texta;
    //        Action = act;
    //    }
    //    public BNode(Vector2 a, Vector2 b, string texta, ButtonsActionID act)
    //    {
    //        V1 = new Vector2(a.X, a.Y);
    //        V2 = new Vector2(b.X, b.Y);
    //        Text = texta;
    //        Action = act;
    //    }
    //    public BNode(Vector2 a, string texta, ButtonsActionID act)
    //    {
    //        V1 = new Vector2(a.X, a.Y);
    //        V2 = new Vector2(a.X + texta.Length * 9, a.Y + 19);
    //        Text = texta;
    //        Action = act;
    //    }

    //} //класс кнопка

}
