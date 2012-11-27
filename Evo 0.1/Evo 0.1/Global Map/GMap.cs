using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Mork
{
    public delegate void SimpleProc();
    //public static class YearSeason
    //{
    //    public const int Summer = 0;
    //    public const int Winter = 1;
    //    public const int Spring = 2;
    //    public const int Autumn = 3;
    //    public static int GetSeason(int month)
    //    {
    //        if (month >= 1 && month <= 2) return Winter;
    //        if (month >= 3 && month <= 5) return Spring;
    //        if (month >= 6 && month <= 8) return Summer;
    //        if (month >= 9 && month <= 10) return Autumn;
    //        return Winter;
    //    }
    //}
    public enum GMapObj : byte
    {
        Nothing,
        Water,
        WaterDip,
        WaterDipest,
        Mell,
        Lednik,
        ArcticTundra,
        ArcticTundraMountain,
        MoxLishTundra,
        KustTundra,
        TemnihvRedkolese,
        Taiga,
        HvoinieLesa,
        Lesostep,
        Step,
        SuhieStepi,
        Savanna,
        Visokotrav,
        Listopadnie,
        Suhgilei,
        Gilei,
        HighPike,
        Pike
    }

    public class GMap //класс глобальной карты
    {
        public static Dictionary<GMapObj, GObj> data = new Dictionary<GMapObj, GObj>();

        public const int size = 1024;
        public float[,] n;
        public GMapObj[,] obj;
        public byte[,] t;
        public byte[,] wet;
        public byte[,] dre;
        public byte[,] sal;

        byte[] tempa;

        public Random rnd = new Random();

        public int water_factor = 1;

        public GMap()
        {
            obj = new GMapObj[size, size];
            t = new byte[size, size];
            n = new float[size, size];
            tempa = new byte[GMap.size];
            
            data.Add(GMapObj.Water, new GObj(Main.interface_tex[12], new Color(78,187,246), "вода"));
            data.Add(GMapObj.WaterDip, new GObj(Main.interface_tex[12], new Color(26, 169, 248), "вода"));
            data.Add(GMapObj.WaterDipest, new GObj(Main.interface_tex[12], new Color(45,112,219), "вода"));
            data.Add(GMapObj.Mell, new GObj(Main.interface_tex[12], new Color(212,235,247), "вода"));
            data.Add(GMapObj.Nothing, new GObj(Main.interface_tex[12], Color.Black, "ничего"));

            data.Add(GMapObj.Lednik, new GObj(Main.interface_tex[12], new Color(255, 255, 255), "Ледники"));

            data.Add(GMapObj.ArcticTundra, new GObj(Main.interface_tex[12], new Color(209,210,196), "Арктическая тундра"));
            

            data.Add(GMapObj.ArcticTundraMountain, new GObj(Main.interface_tex[13], new Color(209, 210, 196), "Высотная арктическая тундра"));
            data.Add(GMapObj.MoxLishTundra, new GObj(Main.interface_tex[12], new Color(127, 177, 177), "Мохово-лишайниковая тундра"));
            data.Add(GMapObj.KustTundra, new GObj(Main.interface_tex[12], new Color(127, 200, 177), "Кустарниковая тундра"));
            data.ElementAt(data.Count - 1).Value.grass.Add(34);
            data.Add(GMapObj.TemnihvRedkolese, new GObj(Main.interface_tex[12], new Color(160, 145, 145), "Темнохвойное редколесье"));
            data.ElementAt(data.Count - 1).Value.grass.Add(34);

            data.Add(GMapObj.Taiga, new GObj(Main.interface_tex[12], new Color(110, 135, 101), "Тайга"));
            data.ElementAt(data.Count - 1).Value.tree.Add(3);

            data.Add(GMapObj.HvoinieLesa, new GObj(Main.interface_tex[12], new Color(49, 132, 42), "Смешанные леса"));
            data.ElementAt(data.Count - 1).Value.grass.Add(34);
            data.ElementAt(data.Count - 1).Value.tree.Add(3);
            data.ElementAt(data.Count - 1).Value.tree_freq = 40;

            data.Add(GMapObj.Lesostep, new GObj(Main.interface_tex[12], new Color(127, 181, 70), "Лесостепи"));
            data.ElementAt(data.Count - 1).Value.tree.Add(3);
            data.ElementAt(data.Count - 1).Value.grass.Add(34);

            data.Add(GMapObj.Step, new GObj(Main.interface_tex[12], new Color(232, 212, 79), "Степи"));
            data.ElementAt(data.Count - 1).Value.grass.Add(34);

            data.Add(GMapObj.SuhieStepi, new GObj(Main.interface_tex[12], new Color(235, 229, 166), "Сухие степи"));

            data.Add(GMapObj.Savanna, new GObj(Main.interface_tex[12], new Color(215, 108, 72), "Пустыни"));
            data.ElementAt(data.Count - 1).Value.grass.Add(34);
            data.ElementAt(data.Count - 1).Value.under_surf = 37;
            data.ElementAt(data.Count - 1).Value.under_under_surf = 37;

            data.Add(GMapObj.Visokotrav, new GObj(Main.interface_tex[12], new Color(207, 70, 95), "Высокотравные саванны"));
            data.Add(GMapObj.Listopadnie, new GObj(Main.interface_tex[12], new Color(125, 27, 66), "Листопадные леса"));
            data.Add(GMapObj.Suhgilei, new GObj(Main.interface_tex[12], new Color(148, 95, 147), "Сезонные гилеи"));
            data.Add(GMapObj.Gilei, new GObj(Main.interface_tex[12], new Color(106, 93, 144), "Гилеи"));

            data.Add(GMapObj.HighPike, new GObj(Main.interface_tex[14], Color.Gray, "высокий горный пик"));
            data.ElementAt(data.Count - 1).Value.under_surf = 20;
            data.ElementAt(data.Count - 1).Value.under_under_surf = 20;
            data.ElementAt(data.Count - 1).Value.grass.Add(34);

            data.Add(GMapObj.Pike, new GObj(Main.interface_tex[12], Color.Gray, "горный пик"));
            data.ElementAt(data.Count - 1).Value.under_surf = 20;
            data.ElementAt(data.Count - 1).Value.under_under_surf = 20;
            data.ElementAt(data.Count - 1).Value.grass.Add(34);

            for (int i = 0; i <= GMap.size - 1; i++)
            {
                tempa[i] = Convert.ToByte(155 - Math.Pow((i / (double)(size - 1)) * 23.0651 - 11.5325, 2));
            }
        }

        public delegate void GenerateProc(double rou);

        public void GenerateGWorld(double rou)
        {
            generategworld(rou);
        }

        public void GenerateGWorld()
        {
            generategworld(20);
        }

        private void generategworld(double rou)
        {
            Main.gmapreshim = Main.Gmapreshim.Height;
            Main.gmap.n = Main.gmap.DsGenerate(GMap.size, GMap.size, rou);

            for (int i = 0; i <= GMap.size - 1; i++)
                for (int j = 0; j <= GMap.size - 1; j+=8)
                {
                    Main.gmap.obj[i, j] = 0;
                    Main.gmap.obj[i, j + 1] = 0;
                    Main.gmap.obj[i, j + 2] = 0;
                    Main.gmap.obj[i, j + 3] = 0;
                    Main.gmap.obj[i, j + 4] = 0;
                    Main.gmap.obj[i, j + 5] = 0;
                    Main.gmap.obj[i, j + 6] = 0;
                    Main.gmap.obj[i, j + 7] = 0;
                }

            Main.gmapreshim = Main.Gmapreshim.Tempirature;

            for (int i = 0; i <= GMap.size - 1; i++)
                for (int j = 0; j <= GMap.size - 1; j+=8)
                {
                    t[i, j] = Convert.ToByte(tempa[j] + 50);
                    t[i, j + 1] = Convert.ToByte(tempa[j + 1] + 50);
                    t[i, j + 2] = Convert.ToByte(tempa[j + 2] + 50);
                    t[i, j + 3] = Convert.ToByte(tempa[j + 3] + 50);
                    t[i, j + 4] = Convert.ToByte(tempa[j + 4] + 50);
                    t[i, j + 5] = Convert.ToByte(tempa[j + 5] + 50);
                    t[i, j + 6] = Convert.ToByte(tempa[j + 6] + 50);
                    t[i, j + 7] = Convert.ToByte(tempa[j + 7] + 50);
                }

            for(int i = 5; i<= GMap.size - 6; i++)
                for (int j = 5; j <= GMap.size - 6; j ++)
                {              
                    //if (n[i, j] > 0.99) obj[i, j] = GMapObj.pike;
                    //if (n[i, j] > 0.995) obj[i, j] = GMapObj.High_pike;

                    double temp = 0;
                    //for (int k = -3; k <= 3; k++)
                    //    for (int l = -3; l <= 3; l++)
                    //    {

                    //        if (k + i >= 0 && k + i <= size - 1 && l + j >= 0 && l + j <= size - 1)
                    //        {
                    //            temp += n[i + k, j + l];
                    //        }
                    //        else temp += 0.5;

                    //        t[i, j] = Convert.ToByte(100 - temp / 64.0 * 100);

                    //    }

                    temp += n[i, j];
                    temp += n[i + 1, j];
                    temp += n[i, j + 1];
                    temp += n[i - 1, j];
                    temp += n[i, j - 1];
                    temp += n[i + 2, j];
                    temp += n[i, j + 2];
                    temp += n[i - 2, j];
                    temp += n[i, j - 2];
                    temp += n[i + 3, j];
                    temp += n[i, j + 3];
                    temp += n[i - 3, j];
                    temp += n[i, j - 3];
                    temp += n[i + 4, j];
                    temp += n[i, j + 4];
                    temp += n[i - 4, j];
                    temp += n[i, j - 4];
                    temp += n[i + 5, j];
                    temp += n[i, j + 5];
                    temp += n[i - 5, j];
                    temp += n[i, j - 5];

                    t[i, j] = Convert.ToByte(temp / 21 * 100);

                    t[i, j] += tempa[j];
  
                }

            Main.gmapreshim = Main.Gmapreshim.Height;
            Main.gmap.n = Main.gmap.DsGenerate(GMap.size, GMap.size, rou);

            Main.gmapreshim = Main.Gmapreshim.Normal;

            for (var i = 0; i <= GMap.size - 1; i++)
                for (var j = 0; j <= GMap.size - 1; j++)
                {
                    if (n[i, j] <= 0.45 && n[i, j] > 0.4) { obj[i, j] = GMapObj.Mell; }

                    if (n[i, j] <= 0.4) { obj[i, j] = GMapObj.Water; }

                    if (n[i, j] <= 0.3) { obj[i, j] = GMapObj.WaterDip; }

                    if (n[i, j] <= 0.09) { obj[i, j] = GMapObj.WaterDipest; }
                }

            for(var i = 0; i<= size - 1; i++)
                for (var j = 0; j <= size - 1; j++)
                {
                    if (t[i, j] <= 70) obj[i, j] = GMapObj.Lednik;

                    if (n[i, j] > 0.90) obj[i, j] = GMapObj.Pike;

                    if (n[i, j] > 0.99) obj[i, j] = GMapObj.HighPike;

                    if (obj[i, j] == GMapObj.Nothing)
                    {
                        if (t[i, j] > 70 && t[i, j] <= 80) obj[i, j] = GMapObj.ArcticTundra;
                        else
                            if (t[i, j] > 80 && t[i, j] <= 90) obj[i, j] = GMapObj.MoxLishTundra;
                            else
                                if (t[i, j] > 90 && t[i, j] <= 100) obj[i, j] = GMapObj.KustTundra;
                                else
                                    if (t[i, j] > 100 && t[i, j] <= 110) obj[i, j] = GMapObj.TemnihvRedkolese;
                                    else
                                        if (t[i, j] > 110 && t[i, j] <= 120) obj[i, j] = GMapObj.Taiga;
                                        else
                                            if (t[i, j] > 120 && t[i, j] <= 150) obj[i, j] = GMapObj.HvoinieLesa;
                                            else
                                                if (t[i, j] > 150 && t[i, j] <= 160) obj[i, j] = GMapObj.Lesostep;
                                                else
                                                    if (t[i, j] > 160 && t[i, j] <= 180) obj[i, j] = GMapObj.Step;
                                                    else
                                                        if (t[i, j] > 180 && t[i, j] <= 190) obj[i, j] = GMapObj.SuhieStepi;
                                                        else
                                                            if (t[i, j] > 190 && t[i, j] <= 210) obj[i, j] = GMapObj.Savanna;
                                                            else
                                                                if (t[i, j] > 210 && t[i, j] <= 220) obj[i, j] = GMapObj.Visokotrav;
                                                                else
                                                                    if (t[i, j] > 220 && t[i, j] <= 230) obj[i, j] = GMapObj.Listopadnie;
                                                                    else
                                                                        if (t[i, j] > 230 && t[i, j] <= 245) obj[i, j] = GMapObj.Suhgilei;
                                                                        else
                                                                            if (t[i, j] > 245 && t[i, j] <= 255) obj[i, j] = GMapObj.Gilei;
                    }
                }

            int a, b, amax, bmax, xof, yof;
            double max;

            //for (int river = 0; river <= 30; river++)
            //{
            //    amax = rnd.Next(0, GMap.size - 1);
            //    bmax = rnd.Next(0, GMap.size - 1);
            //    max = Main.gmap.n[amax, bmax];

            //    for (int i = 0; i <= 100; i++)
            //    {
            //        a = rnd.Next(0, GMap.size - 1);
            //        b = rnd.Next(0, GMap.size - 1);
            //        if (Main.gmap.n[a, b] > max)
            //        {
            //            amax = a;
            //            bmax = b;
            //            max = Main.gmap.n[amax, bmax];
            //        }
            //    }

            //    //Main.gmap_region.X = amax - 10; if (Main.gmap_region.X < 0) Main.gmap_region.X = 0;
            //    //Main.gmap_region.Y = bmax - 10; if (Main.gmap_region.Y < 0) Main.gmap_region.Y = 0;

            //    int temp = 0; 
            //    while(Main.gmap.n[amax, bmax] > 0.4 && temp < 2000) 
            //    {
            //        int var = 0;
            //        Main.gmap.obj[amax, bmax] = GMapObj.Mell;

            //        if (amax + 1 <= GMap.size - 1 && amax + 1 >= 0)
            //        Main.gmap.obj[amax + 1, bmax] = GMapObj.Mell;

            //        if (amax - 1 <= GMap.size - 1 && amax - 1 >= 0)
            //            Main.gmap.obj[amax + 1, bmax] = GMapObj.Mell;

            //        if (bmax + 1 <= GMap.size - 1 && bmax + 1 <= 0)
            //            Main.gmap.obj[amax, bmax + 1] = GMapObj.Mell;

            //        if (bmax - 1 <= GMap.size - 1 && bmax - 1 <= 0)
            //            Main.gmap.obj[amax, bmax - 1] = GMapObj.Mell;

            //        if(amax > 0 && bmax > 0 && amax <= size-1 && bmax <= size-1) max = Main.gmap.n[amax, bmax];

            //        if (amax + 1 <= GMap.size - 1 && Main.gmap.n[amax + 1, bmax] < max)
            //        {
            //            max = Main.gmap.n[amax + 1, bmax];
            //            var = 1;
            //        }
            //        if (amax - 1 >= 0 && Main.gmap.n[amax - 1, bmax] < max)
            //        {
            //            max = Main.gmap.n[amax - 1, bmax];
            //            var = 2;
            //        }
            //        if (bmax + 1 <= GMap.size - 1 && Main.gmap.n[amax, bmax + 1] < max)
            //        {
            //            max = Main.gmap.n[amax, bmax + 1];
            //            var = 3;
            //        }
            //        if (bmax - 1 >= 0 && Main.gmap.n[amax, bmax - 1] < max)
            //        {
            //            max = Main.gmap.n[amax, bmax - 1];
            //            var = 4;
            //        }
            //        switch (var)
            //        {
            //            case 1:
            //                if (Main.gmap.n[amax + 1, bmax] > Main.gmap.n[amax, bmax]) Main.gmap.n[amax + 1, bmax] = Main.gmap.n[amax, bmax];
            //                amax++;
            //                break;
            //            case 2:
            //                if (Main.gmap.n[amax - 1, bmax] > Main.gmap.n[amax, bmax]) Main.gmap.n[amax - 1, bmax] = Main.gmap.n[amax, bmax];
            //                amax--;
            //                break;
            //            case 3:
            //                if (Main.gmap.n[amax, bmax + 1] > Main.gmap.n[amax, bmax]) Main.gmap.n[amax, bmax + 1] = Main.gmap.n[amax, bmax];
            //                bmax++;
            //                break;
            //            case 4:
            //                if (Main.gmap.n[amax, bmax - 1] > Main.gmap.n[amax, bmax]) Main.gmap.n[amax, bmax - 1] = Main.gmap.n[amax, bmax];
            //                bmax--;
            //                break;
            //            default:
            //                xof = rnd.Next(-1, 1); yof = Math.Abs(xof) == 1 ? 0 : rnd.Next(-1, 1);

            //                if (amax + xof > 0 && amax + xof < GMap.size - 1 && bmax + yof > 0 && bmax + yof < GMap.size - 1)
            //                {
            //                    Main.gmap.n[amax + xof, bmax + yof] = Main.gmap.n[amax, bmax];

            //                    amax += xof; bmax += yof;
            //                }

            //                break;
            //        }

            //        temp++;
            //    }
            //}
            for(int i = 0; i<= size - 1; i++)
                for (int j = 0; j <= size - 1; j++)
                {
                    if (Main.gmap.obj[i, j] == GMapObj.Water || Main.gmap.obj[i, j] == GMapObj.WaterDip || Main.gmap.obj[i, j] == GMapObj.WaterDipest || Main.gmap.obj[i, j] == GMapObj.Mell) Main.gmap.n[i, j] -= 0.1f;
                    if (Main.gmap.n[i, j] <= 0) Main.gmap.n[i, j] = 0;
                }
        }

        #region Diamond-square

        private double _dRoughness;
        private double _dBigSize;

        private float[,] DsGenerate(int iWidth, int iHeight, double iRoughness)
        {
            var points = new float[iWidth + 1, iHeight + 1];

            var c1 = rnd.NextDouble();
            var c2 = rnd.NextDouble();
            var c3 = rnd.NextDouble();
            var c4 = rnd.NextDouble();
            _dRoughness = iRoughness;
            _dBigSize = iWidth + iHeight;
            DsDivide(ref points, 0, 0, iWidth, iHeight, c1, c2, c3, c4);
            return points;
        }

        private void DsDivide(ref float[,] points, double x, double y, double width, double height, double c1, double c2, double c3, double c4)
        {
            double newWidth = Math.Floor(width / 2);
            double newHeight = Math.Floor(height / 2);

            if (width > 1 || height > 1)
            {
                var middle = ((c1 + c2 + c3 + c4) / 4) + PMove(newWidth + newHeight);
                var edge1 = ((c1 + c2) / 2);
                var edge2 = ((c2 + c3) / 2);
                var edge3 = ((c3 + c4) / 2);
                var edge4 = ((c4 + c1) / 2);
                middle = Normalize(middle);
                edge1 = Normalize(edge1);
                edge2 = Normalize(edge2);
                edge3 = Normalize(edge3);
                edge4 = Normalize(edge4);
                DsDivide(ref points, x, y, newWidth, newHeight, c1, edge1, middle, edge4);
                DsDivide(ref points, x + newWidth, y, width - newWidth, newHeight, edge1, c2, edge2, middle);
                DsDivide(ref points, x + newWidth, y + newHeight, width - newWidth, height - newHeight, middle, edge2, c3, edge3);
                DsDivide(ref points, x, y + newHeight, newWidth, height - newHeight, edge4, middle, edge3, c4);
            }
            else
            {
                float c = (float)(c1 + c2 + c3 + c4) / 4f;

                points[(int)(x), (int)(y)] = c;
                if (width == 2)
                {
                    points[(int)(x + 1), (int)(y)] = c;
                }
                if (height == 2)
                {
                    points[(int)(x), (int)(y + 1)] = c;
                }
                if ((width == 2) && (height == 2))
                {
                    points[(int)(x + 1), (int)(y + 1)] = c;
                }
            }
        }
        private static double Normalize(double iNum)
        {
            if (iNum < 0)
            {
                iNum = 0;
            }
            else if (iNum > 1.0)
            {
                iNum = 1.0;
            }
            return iNum;
        }

        private double PMove(double smallSize)
        {
            return (rnd.NextDouble() - 0.5) * smallSize / _dBigSize * _dRoughness;
        }
        #endregion
    }
}