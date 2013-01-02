using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Mork.Local_Map.Dynamic.Local_Items;
using Mork.Bad_Database;

namespace Mork.Local_Map
{
    [Serializable]////////////////////////////////////////////////////////////////////////
    public class MMap
    {
        Random rnd = new Random();

        public const int mx = Commons.mx, my = Commons.my, mz = Commons.mz;
        public MNode[, ,] n = new MNode[Commons.mx, Commons.my, Commons.mz];
        public float[,] wshine = new float[Commons.mx, Commons.my];
        public short[,] whinenapr = new short[Commons.mx, Commons.my];

        public List<Vector3> active = new List<Vector3>(); //active blocks update inner logic every cycle

        public string Name = "test map 1";

        Dictionary<string, object> tags = new Dictionary<string, object>();

        public void SetMapTag(KeyValuePair<string, object> added_tag)
        {
            if (!tags.ContainsKey(added_tag.Key))
                tags.Add(added_tag.Key, added_tag.Value);
            else tags[added_tag.Key] = added_tag.Value;
        }

        public void SetMapTag(string s, object o)
        {
            if (!tags.ContainsKey(s))
                tags.Add(s, o);
            else tags[s] = o;
        }

        public object GetMapTagData(string s)
        {
            if (tags.ContainsKey(s)) return tags[s];
            return 0;
        }

        public List<string> GetMapTagsInText()
        {
            List<string> s = new List<string>();

            foreach (var tag in tags)
            {
                s.Add(tag.Key + " = " + tag.Value);
            }

            return s;
        }


        public void SetNodeTag(int x, int y, int z, KeyValuePair<string, object> added_tag)
        {
            if (!n[x,y,z].tags.ContainsKey(added_tag.Key))
                n[x, y, z].tags.Add(added_tag.Key, added_tag.Value);
            else n[x, y, z].tags[added_tag.Key] = added_tag.Value;
        }

        /// <summary>
        /// Установить новый таг карты
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="s">строковый идентификатор</param>
        /// <param name="o">значение тага</param>
        public void SetNodeTag(int x, int y, int z, string s, object o)
        {
            if (!n[x, y, z].tags.ContainsKey(s))
                n[x, y, z].tags.Add(s, o);
            else n[x, y, z].tags[s] = o;
        }

        /// <summary>
        /// Получить значение тага карты
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="s">строковый идентификатор</param>
        /// <returns>значение тага</returns>
        public object GetNodeTagData(int x, int y, int z, string s)
        {
            if (n[x, y, z].tags.ContainsKey(s)) return n[x, y, z].tags[s];
            return 0;
        }

        /// <summary>
        /// Получить значение тага карты
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s">строковый идентификатор</param>
        /// <returns>значение тага</returns>
        public object GetNodeTagData(Vector3 d, string s)
        {
            if (n[(int)d.X, (int)d.Y, (int)d.Z].tags.ContainsKey(s)) return n[(int)d.X, (int)d.Y, (int)d.Z].tags[s];
            return 0;
        }

        /// <summary>
        /// Получить все таги нода карты в текстовом виде
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>массив строк тагов</returns>
        public List<string> GetNodeTagsInText(int x, int y, int z)
        {
            List<string> s = new List<string>();

            s.Add(String.Format("Pos = {0} {1} {2}",x,y,z));
            s.Add("ID = " + n[x, y, z].blockID + " mtex = " + Main.dbobject.Data[n[x, y, z].blockID].metatex_n);
            s.Add("DBName = " + Main.dbobject.Data[n[x, y, z].blockID].I_name);
            foreach (var tag in n[x, y, z].tags)
            {
                if (tag.Value is LocalItems)
                {
                    s.Add("Storage =");
                    foreach (var o in (tag.Value as LocalItems).n)
                    {
                        s.Add("id " + o.id + " " + o.count);
                    }
                    s.Add("----------------");

                }
                else
                s.Add(tag.Key + " = " + tag.Value);
            }

            return s;
        }

        /// <summary>
        /// Получить все таги нода карты в текстовом виде
        /// </summary>
        /// <param name="pos"></param>
        /// <returns>массив строк тагов</returns>
        public List<string> GetNodeTagsInText(Vector3 pos)
        {
            return GetNodeTagsInText((int)pos.X, (int)pos.Y, (int)pos.Z);
        }

        //public void CalcWision()
        //{
        //    int x_top;
        //    int y_top;
        //    int dx_top = 0;
        //    int dy_top = 0;
        //    int dir;
        //    float dist;
        //    int seen;
        //    int max, min, tmax;
        //    int dx, dy, s_x, s_y, r_x, r_y;
        //    int px, py;
        //    int a;
        //    int x1, x2, y1, y2;
        //    int los_x_null = 0;
        //    int los_y_null = 0;

        //    int see = Main.mh.att.Vision.curent;
        //    int pos_x = Main.mh.pos.X;
        //    int pos_y = Main.mh.pos.Y;

        //    for (int i = 0; i < mx; i++)////////////////////////////////////////////////////////////////////////////////////////////////////////
        //        for (int j = 0; j < my; j++)
        //            for (int k = 0; k < mz; k++)
        //            n[i, j, k].vision = -1; 

        //    x_top = pos_x - see;
        //    y_top = pos_y - see;


        //    for (dir = 0; dir < 4; dir++)
        //    {
        //        if (x_top < 0) x_top = 0;
        //        if (x_top >= mx) x_top = mx - 1;
        //        if (y_top < 0) y_top = 0;
        //        if (y_top >= my) y_top = my - 1;

        //        if (dir == 0) { dx_top = +1; dy_top = 0; }
        //        if (dir == 1) { dx_top = 0; dy_top = +1; }
        //        if (dir == 2) { dx_top = -1; dy_top = 0; }
        //        if (dir == 3) { dx_top = 0; dy_top = -1; }

        //        for (; ; )
        //        {
        //            if (dir == 0 & ((x_top - pos_x) == see || (x_top - pos_x) >= mx - 1)) break;
        //            if (dir == 1 & ((y_top - pos_y) == see || (y_top - pos_x) >= my - 1)) break;
        //            if (dir == 2 & ((pos_x - x_top) == see || (pos_x - x_top) >= mx - 1)) break;
        //            if (dir == 3 & ((pos_y - y_top) == see || (pos_y - y_top) >= my - 1)) break;

        //            x1 = pos_x;
        //            y1 = pos_y;
        //            x2 = x_top;
        //            y2 = y_top;

        //            px = x2 - x1;
        //            py = y2 - y1;
        //            s_x = (px >= 0) ? 1 : -1;
        //            s_y = (py >= 0) ? 1 : -1;
        //            px = (px >= 0) ? px : -px;
        //            py = (py >= 0) ? py : -py;

        //            if (px >= py) { max = px; min = py; r_x = s_x; r_y = 0; }
        //            else { max = py; min = px; r_x = 0; r_y = s_y; }

        //            tmax = max;
        //            a = max >> 1;
        //            while (tmax != 0)
        //            {
        //                a += min;
        //                if ((a - max) < 0) { dx = r_x; dy = r_y; }
        //                else { dx = s_x; dy = s_y; a -= max; }
        //                x1 += dx;
        //                y1 += dy;

        //                if (pos_x == x_top || pos_y == y_top) seen = see - 1;
        //                else seen = see;
        //                dist = Convert.ToInt32(Math.Sqrt(Math.Pow((pos_x - x1), 2) + Math.Pow((pos_y - y1), 2)));
        //                if (dist > seen) break;

        //                if (GoodXY(x1, y1) && (n[x1 - los_x_null, y1 - los_y_null].vision != 0 || n[x1 - los_x_null, y1 - los_y_null].vision == -1))
        //                {
        //                    if (!IsWalkable(x1, y1))
        //                    {
        //                        n[x1 - los_x_null, y1 - los_y_null].vision = 2;
        //                        n[x1 - los_x_null, y1 - los_y_null].explored = true;
        //                        byte temp11 = Convert.ToByte(255 - dist / seen * 205);
        //                        n[x1 - los_x_null, y1 - los_y_null].lightness = new Color(temp11, temp11, temp11);
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        n[x1 - los_x_null, y1 - los_y_null].vision = 0;
        //                        n[x1 - los_x_null, y1 - los_y_null].explored = true;
        //                        byte temp11 = Convert.ToByte(255 - dist / seen * 205);
        //                        n[x1 - los_x_null, y1 - los_y_null].lightness = new Color(temp11, temp11, temp11);
        //                    }
        //                }

        //                tmax--;
        //            }

        //            x_top += dx_top;
        //            y_top += dy_top;
        //        }
        //    }
        //    n[pos_x, pos_y].vision = 0;
        //}

        /// <summary>
        /// Получить границы карты в виде вектора3
        /// </summary>
        /// <returns></returns>
        public Vector3 Bounds()
        {
            return new Vector3(mx - 1, my - 1, mz - 1);
        }

        /// <summary>
        /// генерация базового слоя с нечеткой границей
        /// </summary>
        /// <param name="id">заполнить базовый слой указанным блоком</param>
        public void Generation_BasicLayer(int id)
        {
            for(int i=0;i<=mx-1;i++)
                for (int j = 0; j <= my - 1; j++)
                {
                    int k=0;
                    for (k = mz - 1; k >= 0; k--)
                    {
                        if (n[i, j, k].blockID == 12345)
                            goto here1;
                    }
                    here1: ;
                
                    for (int m = k; m >= rnd.Next(k-5, k); m--)
                    {
                        if(m>0) if (n[i, j, m].blockID == 12345) n[i, j, m].blockID = id;
                    }
                }
        }

        /// <summary>
        /// заколнить слой толщиной count блоком id
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="count">толщина слоя</param>
        public void Generation_FullLayer(int id, int count)
        {
            for (int i = 0; i <= mx - 1; i++)
                for (int j = 0; j <= my - 1; j++)
                {
                    int k = 0;
                    for (k = mz - 1; k >= 0; k--)
                    {
                        if (n[i, j, k].blockID == 12345)
                            goto here1;
                    }
                    here1: ;

                    if (k <= 0) continue;

                    for (int m = k; m >= k-count+1; m--)
                    {
                        if (n[i, j, m].blockID == 12345) n[i, j, m].blockID = id;
                    }
                }
        }

        /// <summary>
        /// заколнить слой толщиной count блоком id с кластерами clust (частотой c_freq) и жилами jila (частотой j_freq и протяженнойстью length)
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="count">толщина слоя</param>
        /// <param name="clust">идентификаторы кластеров</param>
        /// <param name="c_freq">частота встречания кластеров</param>
        /// <param name="jila">идентификаторы жил</param>
        /// <param name="j_freq">частота встречания жил</param>
        /// <param name="length">протяженность жил</param>
        public void GenerationFullLayerCluster(int id, int count, int[] clust, int c_freq, int[] jila, int j_freq, int length)
        {
            for (int i = 0; i <= mx - 1; i++)
                for (int j = 0; j <= my - 1; j++)
                {
                    int k = 0;
                    for (k = mz - 1; k >= 0; k--)
                    {
                        if (n[i, j, k].blockID == 12345)
                            goto here1;
                    }
                    here1: ;

                    if (k <= 0) continue;

                    for (int m = k; m >= k - count + 1; m--)
                    {
                        if (n[i, j, m].blockID == 12345)
                        {
                            n[i, j, m].blockID = id;
                            if (rnd.Next(1, 101 - c_freq) == 1 && clust.Length > 0)
                            {
                                cluster(clust[rnd.Next(0, clust.Length)], i, j, m);
                            }
                            if (rnd.Next(1, 101 - j_freq) == 1 && jila.Length > 0)
                            {
                                cjila(jila[rnd.Next(0, jila.Length)], i, j, m, length);
                            }
                        }
                    }
                }
        }

        private void cluster(int id, int i, int j, int m)
        {
            switch (rnd.Next(0, 7))
            {
                case 0:
                    n[i, j, m].blockID = id;
                    break;
                case 1:
                    if (GoodVector3(i - 1, j, m)) n[i, j, m].blockID = id;
                    if (GoodVector3(i, j + 1, m)) n[i, j + 1, m].blockID = id;
                    if (GoodVector3(i + 1, j, m)) n[i + 1, j, m].blockID = id;
                    if (GoodVector3(i - 1, j, m)) n[i-1, j, m].blockID = id;
                    if (GoodVector3(i, j - 1, m)) n[i, j - 1, m].blockID = id;
                    break;
                case 2:
                    if (GoodVector3(i - 1, j, m)) n[i, j, m].blockID = id;
                    if (GoodVector3(i + 1, j, m)) n[i + 1, j, m].blockID = id;
                    if (GoodVector3(i - 1, j, m)) n[i-1, j, m].blockID = id;
                    if (GoodVector3(i, j - 1, m)) n[i, j - 1, m].blockID = id;
                    break;
                case 3:
                    if (GoodVector3(i - 1, j, m)) n[i, j, m].blockID = id;
                    if (GoodVector3(i, j + 1, m)) n[i, j + 1, m].blockID = id;
                    if (GoodVector3(i - 1, j, m)) n[i-1, j, m].blockID = id;
                    if (GoodVector3(i, j - 1, m)) n[i, j - 1, m].blockID = id;
                    break;
                case 4:
                    if (GoodVector3(i - 1, j, m)) n[i, j, m].blockID = id;
                    if (GoodVector3(i, j + 1, m)) n[i, j + 1, m].blockID = id;
                    if (GoodVector3(i + 1, j, m)) n[i + 1, j, m].blockID = id;
                    if (GoodVector3(i, j - 1, m)) n[i, j - 1, m].blockID = id;
                    break;
                case 5:
                    if (GoodVector3(i - 1, j, m)) n[i, j, m].blockID = id;
                    if (GoodVector3(i, j + 1, m)) n[i, j + 1, m].blockID = id;
                    if (GoodVector3(i + 1, j, m)) n[i + 1, j, m].blockID = id;
                    if (GoodVector3(i - 1, j, m)) n[i-1, j, m].blockID = id;
                    break;
                case 6:
                    if (GoodVector3(i - 1, j, m)) n[i, j, m].blockID = id;
                    if (GoodVector3(i - 1, j, m)) n[i-1, j, m].blockID = id;
                    if (GoodVector3(i, j - 1, m)) n[i, j - 1, m].blockID = id;
                    break;
                case 7:
                    if (GoodVector3(i - 1, j, m)) n[i, j, m].blockID = id;
                    if (GoodVector3(i, j + 1, m)) n[i, j + 1, m].blockID = id;
                    if (GoodVector3(i + 1, j, m)) n[i + 1, j, m].blockID = id;
                    break;
            }
        }

        private void cjila(int id, int i, int j, int m, int lenght)
        {
            int ii = rnd.Next(-1, 1);
            int jj = rnd.Next(-1, 1);

            int i1 = i; int j1 = j;

            for (int k = 0; k <= lenght - 1; k++)
            {
                i1 += ii; j1 += jj;
                if(GoodVector3(i1, j1, m))
                {
                    n[i1, j1, m].blockID = id;
                }

                if(rnd.Next(1,4) == 1)
                    ii = rnd.Next(-1, 1);
                jj = rnd.Next(-1, 1);
            }
        }

        public void Generation_PlaceOnSurface()
        {
            for (int i = 0; i <= mx - 1; i++)
                for (int j = 0; j <= my - 1; j++)
                {
                    for (int m = 0; m <= mz - 1; m++)
                    {
                        if (n[i, j, m].blockID != 0)
                        {
                            if (rnd.Next(1, Convert.ToInt32(101 - GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].tree_freq)) == 1 && m > 0)
                                if (GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].tree.Count > 0)
                                    n[i, j, m - 1].blockID = GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].tree[rnd.Next(0, GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].tree.Count)];
                            goto here2;
                        }
                    }
                here2: ;
                }
        }

        /// <summary>
        /// генерация слоя под поверхностью
        /// </summary>
        /// <param name="count"></param>
        public void Generation_FullLayer_under(int count)
        {
            for (int i = 0; i <= mx - 1; i++)
                for (int j = 0; j <= my - 1; j++)
                {
                    int k = 0;
                    for (k = mz - 1; k >= 0; k--)
                    {
                        if (n[i, j, k].blockID == 0)
                            goto here1;
                    }
                    here1: ;

                    if (k < 0) continue;

                    for (int m = k; m >= k - count + 1; m--)
                    {
                        if (m > 0) n[i, j, m].blockID = GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].under_surf;
                    }
                }
        }

        /// <summary>
        /// генерация слоя под слоем под поверхностью
        /// </summary>
        /// <param name="count"></param>
        public void Generation_FullLayer_under_under(int count)
        {
            for (int i = 0; i <= mx - 1; i++)
                for (int j = 0; j <= my - 1; j++)
                {
                    int k = 0;
                    for (k = mz - 1; k >= 0; k--)
                    {
                        if (n[i, j, k].blockID == 0)
                            goto here1;
                    }
                    here1: ;

                    if (k < 0) continue;

                    for (int m = k; m >= k - count + 1; m--)
                    {
                        if (m > 0) n[i, j, m].blockID = GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].under_under_surf;
                    }
                }
        }

        /// <summary>
        /// генерация верхнего слоя (чаще всего травы)
        /// </summary>
        /// <param name="count"></param>
        public void Generation_FullLayerGrass(int count)
        {
            for (int i = 0; i <= mx - 1; i++)
                for (int j = 0; j <= my - 1; j++)
                {
                    int k = 0;
                    for (k = mz - 1; k >= 0; k--)
                    {
                        if (n[i, j, k].blockID == 0)
                            goto here1;
                    }
                    here1: ;

                    if (k < 0) continue;

                    List<int> id = GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].grass;

                    for (int m = k; m >= k - count + 1; m--)
                    {
                        if (id.Count != 0)
                        {
                            int a = rnd.Next(0, id.Count);
                            if (m > 0) n[i, j, m].blockID = id[a];
                        }
                        else if (m > 0) n[i, j, m].blockID = 10;
                    }
                }
        }

        public void Generation_SmoothTop()
        {

        }

        /// <summary>
        /// полный пересчет значений нодов subterrain
        /// </summary>
        public void RecalcExploredSubterrain()
        {
            for (int i = 0; i <= mx - 1; i++)
                for (int j = 0; j <= my - 1; j++)
                    for (int m = 0; m <= mz - 1; m++)
                    {
                        n[i, j, m].subterrain = true;
                        n[i, j, m].explored = false;
                    }

            for (int i = 0; i <= mx - 1; i++)
                for (int j = 0; j <= my - 1; j++)
                {
                    for (int m = 0; m <= mz - 1; m++)
                    {
                        if (n[i, j, m].blockID == 0)
                        {
                            n[i, j, m].subterrain = false;
                            n[i, j, m].explored = true;
                        }
                        else
                        {
                            n[i, j, m].subterrain = false;
                            n[i, j, m].explored = true;
                            goto here2;
                        }
                    }
                    here2: ;
                }
        }

        /// <summary>
        /// простая генерация локальный карты по данным глобальной карты (базовая версия)
        /// </summary>
        public void SimpleGeneration_bygmap()
        {
            for (int i0 = 0; i0 < wshine.GetUpperBound(0); i0++)
                for (int i1 = 0; i1 < wshine.GetUpperBound(1); i1++)
                {
                    wshine[i0, i1] = (float)rnd.NextDouble();
                    if(rnd.Next(0,1) == 0)
                    whinenapr[i0, i1] = -1;
                    else whinenapr[i0, i1] = 1;
                }

            for (int i = 0; i <= mx - 1; i++)
                for (int j = 0; j <= my - 1; j++)
                    for (int k = 0; k <= mz - 1; k += 8)
                    {
                        n[i, j, k].blockID = 0;
                        n[i, j, k + 1].blockID = 0;
                        n[i, j, k + 2].blockID = 0;
                        n[i, j, k + 3].blockID = 0;
                        n[i, j, k + 4].blockID = 0;
                        n[i, j, k + 5].blockID = 0;
                        n[i, j, k + 6].blockID = 0;
                        n[i, j, k + 7].blockID = 0;
                    }


            for (int i = 0; i <= mx - 1; i++)
                for (int j = 0; j <= my - 1; j++)
                {
                    for (int k = 0; k <= ((Main.gmap.n[(int)Main.gmap_region.X + i, (int)Main.gmap_region.Y + j])) * (mz - 1) * 0.3 + (mz - 1) * 0.7 - 5; k++)
                    {
                        if (GoodVector3(i, j, mz - 1 - k)) Main.mmap.n[i, j, mz - 1 - k].blockID = 12345;
                    }
                }

            int[] metamorf_clust = { 800,801,802,803,804,805,806};
            int[] matamorf_jila = { 810,811,812 };

            Generation_FullLayer(18, 5);
            Generation_FullLayer(17, 2);
            Generation_FullLayer(16, 2);
            Generation_FullLayer(15, 2);
            Generation_FullLayer(14, 18);

            Generation_BasicLayer((int)KnownIDs.Gabro);
            Generation_FullLayer((int)KnownIDs.Gabro, 7);
            Generation_FullLayer((int)KnownIDs.GabroToGranete, 1);
            Generation_FullLayer((int)KnownIDs.GrenFranite, 6);

            int[] granite_clust = metamorf_clust;
            int[] granite_jila = { 55, 55 };
            GenerationFullLayerCluster(11, 12, granite_clust, 10, granite_jila, 5, 20);

            Generation_FullLayer(33, 10);

            int[] marble_clust = metamorf_clust;
            int[] marble_jila = matamorf_jila;
            GenerationFullLayerCluster(22, 10, marble_clust, 10, marble_jila, 5, 20);

            GenerationFullLayerCluster(44, 4, metamorf_clust, 6, matamorf_jila, 2, 25);

            GenerationFullLayerCluster(22, 4, metamorf_clust, 6, matamorf_jila, 2, 25);

            for (int i = 0; i <= 20; i++ )
            {
                Generation_FullLayer(824, 2);
                Generation_FullLayer(828, 2);
            }


            Generation_FullLayer_under(1);
            Generation_FullLayer_under_under(2);
            //Main.mmap.Generation_FullLayerTOP(ObjectID.DirtWall_Grass2, 1);

            Generation_FullLayerGrass(1);


            for (int i = 0; i <= mx - 1; i++)
                for (int j = 0; j <= my - 1; j++)
                {
                    //if(Main.gmap.n[Main.gmap_region.X + i, Main.gmap_region.Y + j] <= 0.4)
                    for (int k = 0; k <= ((0.4)) * (mz - 1) * 0.3 + (mz - 1) * 0.7 + 1; k++)
                    {
                        if (GoodVector3(i, j, mz - 1 - k) && Main.mmap.n[i, j, mz - 1 - k].blockID == 0) Main.mmap.n[i, j, mz - 1 - k].blockID = KnownIDs.water;//вода
                    }
                }

            RecalcExploredSubterrain();

            Generation_PlaceOnSurface();

            Main.PrepairMapDeleteWrongIDs(ref Main.mmap);
        }

        /// <summary>
        /// Проверка, входит ли вектор в границы карты
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static bool GoodVector3(Vector3 loc)
        {
            if (loc != null && loc.X >= 0 && loc.Y >= 0 && loc.X <= mx - 1 && loc.Y <= my - 1 && loc.Z >= 0 && loc.Z <= mz - 1) return true;
            return false;
        }

        /// <summary>
        /// Проверка, входит ли вектор в границы карты
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <returns></returns>
        public static bool GoodVector3(int X, int Y, int Z)
        {
            if (X >= 0 && Y >= 0 && X <= mx - 1 && Y <= my - 1 && Z >= 0 && Z <= mz - 1) return true;
            return false;
        }

        /// <summary>
        /// Проверка, входит ли вектор в границы карты
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <returns></returns>
        public static bool GoodVector3(float X, float Y, float Z)
        {
            if (X >= 0 && Y >= 0 && X <= mx - 1 && Y <= my - 1 && Z >= 0 && Z <= mz - 1) return true;
            return false;
        }

        /// <summary>
        /// Проверка на проходимость блока
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public static bool IsWalkable(Vector3 loc)
        {
            return Main.dbobject.Data[Main.mmap.n[(int)loc.X, (int)loc.Y, (int)loc.Z].blockID].walkable;
        }

        public static bool IsWalkable(int X, int Y, int Z)
        {
            if (GoodVector3(X, Y, Z))
                return Main.dbobject.Data[Main.mmap.n[X, Y, Z].blockID].walkable;
            else return false;
        }

        public static bool InScreen(Vector3 loc)
        {
            if ((loc.X >= Main.camera.X) && (loc.Y >= Main.camera.Y) && (loc.X <= Main.camera.X + Main.ssizex) && (loc.Y <= Main.camera.Y + Main.ssizey))
                return true;
            return false;
        }
        public static bool InScreen(int X, int Y)
        {
            if ((X >= Main.camera.X) && (Y >= Main.camera.Y) && (X <= Main.camera.X + Main.ssizex) && (Y <= Main.camera.Y + Main.ssizey))
                return true;
            return false;
        }

        public Stack<Vector3> FindPatch(Vector3 loc_ref, Vector3 dest_ref)
        {
            short[,,] patch_step = new short[mx + 1, my + 1, mz + 1];

            Vector3 loc = loc_ref;
            Vector3 dest = dest_ref;
            if (loc == dest)
                return new Stack<Vector3>();

            Queue<Vector3> temp_q = new Queue<Vector3>();
            Vector3 temp = new Vector3();

            short counter = 1;

            dest.X = dest.X > 127 ? 127 : dest.X;
            dest.Y = dest.Y > 127 ? 127 : dest.Y;

            temp_q.Enqueue(loc);
            patch_step[(int)loc.X, (int)loc.Y, (int)loc.Z] = counter;

            if (GoodVector3((int)dest.X, (int)dest.Y, (int)dest.Z))
                while (patch_step[(int)dest.X, (int)dest.Y, (int)dest.Z] == 0 && temp_q.Count > 0)
                {
                    temp = temp_q.Dequeue();
                    if (counter >= 999)
                        return new Stack<Vector3>();

                    counter = Convert.ToInt16(patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] + 1);

                    // via z

                    if (temp.X != mx - 1 && patch_step[(int)temp.X + 1, (int)temp.Y, (int)temp.Z] == 0 && IsWalkable((int)temp.X + 1, (int)temp.Y, (int)temp.Z))
                    {
                        patch_step[(int)temp.X + 1, (int)temp.Y, (int)temp.Z] = counter;
                        temp_q.Enqueue(new Vector3(temp.X + 1, temp.Y, temp.Z));
                    }

                    if (temp.X >=1 && patch_step[(int)temp.X - 1, (int)temp.Y, (int)temp.Z] == 0 && IsWalkable((int)temp.X - 1, (int)temp.Y, (int)temp.Z))
                    {
                        patch_step[(int)temp.X - 1, (int)temp.Y, (int)temp.Z] = counter;
                        temp_q.Enqueue(new Vector3(temp.X - 1, temp.Y, temp.Z));
                    }

                    if (temp.Y != my - 1 && patch_step[(int)temp.X, (int)temp.Y + 1, (int)temp.Z] == 0 && IsWalkable((int)temp.X, (int)temp.Y + 1, (int)temp.Z))
                    {
                        patch_step[(int)temp.X, (int)temp.Y + 1, (int)temp.Z] = counter;
                        temp_q.Enqueue(new Vector3(temp.X, temp.Y + 1, temp.Z));
                    }

                    if (temp.Y >= 1 && patch_step[(int)temp.X, (int)temp.Y - 1, (int)temp.Z] == 0 && IsWalkable((int)temp.X, (int)temp.Y - 1, (int)temp.Z))
                    {
                        patch_step[(int)temp.X, (int)temp.Y - 1, (int)temp.Z] = counter;
                        temp_q.Enqueue(new Vector3(temp.X, temp.Y - 1, temp.Z));
                    }
                    
                    // via z+1

                    if (temp.Z < mz - 1)
                    {
                        if (temp.X != mx - 1 && patch_step[(int)temp.X + 1, (int)temp.Y, (int)temp.Z + 1] == 0 && IsWalkable((int)temp.X + 1, (int)temp.Y, (int)temp.Z + 1))
                        {
                            patch_step[(int)temp.X + 1, (int)temp.Y, (int)temp.Z + 1] = counter;
                            temp_q.Enqueue(new Vector3(temp.X + 1, temp.Y, temp.Z + 1));
                        }

                        if (temp.X >= 1 && patch_step[(int)temp.X - 1, (int)temp.Y, (int)temp.Z + 1] == 0 && IsWalkable((int)temp.X - 1, (int)temp.Y, (int)temp.Z + 1))
                        {
                            patch_step[(int)temp.X - 1, (int)temp.Y, (int)temp.Z + 1] = counter;
                            temp_q.Enqueue(new Vector3(temp.X - 1, temp.Y, temp.Z + 1));
                        }

                        if (temp.Y != my - 1 && patch_step[(int)temp.X, (int)temp.Y + 1, (int)temp.Z + 1] == 0 && IsWalkable((int)temp.X, (int)temp.Y + 1, (int)temp.Z + 1))
                        {
                            patch_step[(int)temp.X, (int)temp.Y + 1, (int)temp.Z + 1] = counter;
                            temp_q.Enqueue(new Vector3(temp.X, temp.Y + 1, temp.Z + 1));
                        }

                        if (temp.Y >= 1 && patch_step[(int)temp.X, (int)temp.Y - 1, (int)temp.Z + 1] == 0 && IsWalkable((int)temp.X, (int)temp.Y - 1, (int)temp.Z + 1))
                        {
                            patch_step[(int)temp.X, (int)temp.Y - 1, (int)temp.Z + 1] = counter;
                            temp_q.Enqueue(new Vector3(temp.X, temp.Y - 1, temp.Z + 1));
                        }

                        if (temp.Y >= 1 && patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z + 1] == 0 && IsWalkable((int)temp.X, (int)temp.Y, (int)temp.Z + 1))
                        {
                            patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z + 1] = counter;
                            temp_q.Enqueue(new Vector3(temp.X, temp.Y, temp.Z + 1));
                        }
                    }

                    // via z-1
                    if (temp.Z >= 1)
                    {
                        if (temp.X != mx - 1 && patch_step[(int)temp.X + 1, (int)temp.Y, (int)temp.Z - 1] == 0 && IsWalkable((int)temp.X + 1, (int)temp.Y, (int)temp.Z - 1))
                        {
                            patch_step[(int)temp.X + 1, (int)temp.Y, (int)temp.Z - 1] = counter;
                            temp_q.Enqueue(new Vector3(temp.X + 1, temp.Y, temp.Z - 1));
                        }

                        if (temp.X >= 1 && patch_step[(int)temp.X - 1, (int)temp.Y, (int)temp.Z - 1] == 0 && IsWalkable((int)temp.X - 1, (int)temp.Y, (int)temp.Z - 1))
                        {
                            patch_step[(int)temp.X - 1, (int)temp.Y, (int)temp.Z - 1] = counter;
                            temp_q.Enqueue(new Vector3(temp.X - 1, temp.Y, temp.Z - 1));
                        }

                        if (temp.Y != my - 1 && patch_step[(int)temp.X, (int)temp.Y + 1, (int)temp.Z - 1] == 0 && IsWalkable((int)temp.X, (int)temp.Y + 1, (int)temp.Z - 1))
                        {
                            patch_step[(int)temp.X, (int)temp.Y + 1, (int)temp.Z - 1] = counter;
                            temp_q.Enqueue(new Vector3(temp.X, temp.Y + 1, temp.Z - 1));
                        }

                        if (temp.Y >= 1 && patch_step[(int)temp.X, (int)temp.Y - 1, (int)temp.Z - 1] == 0 && IsWalkable((int)temp.X, (int)temp.Y - 1, (int)temp.Z - 1))
                        {
                            patch_step[(int)temp.X, (int)temp.Y - 1, (int)temp.Z - 1] = counter;
                            temp_q.Enqueue(new Vector3(temp.X, temp.Y - 1, temp.Z - 1));
                        }

                        if (temp.Y >= 1 && patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z - 1] == 0 && IsWalkable((int)temp.X, (int)temp.Y, (int)temp.Z - 1))
                        {
                            patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z - 1] = counter;
                            temp_q.Enqueue(new Vector3(temp.X, temp.Y, temp.Z - 1));
                        }
                    }
                }

            patch_step[(int)loc.X, (int)loc.Y, (int)loc.Z] = Int16.MaxValue;



            Stack<Vector3> revers_q = new Stack<Vector3>();
            revers_q.Push(dest);
            temp = dest;
            if (GoodVector3(dest))
                while (patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] != 1 && counter > 0)
                {
                    counter--;
                    // via z
                    if (patch_step[(int)temp.X + 1, (int)temp.Y, (int)temp.Z] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X + 1, (int)temp.Y, (int)temp.Z] != 0)
                    {
                        revers_q.Push(new Vector3(temp.X + 1, temp.Y, temp.Z));
                        temp = new Vector3(temp.X + 1, temp.Y, temp.Z);
                    }
                    else
                        if (temp.X >= 1 && patch_step[(int)temp.X - 1, (int)temp.Y, (int)temp.Z] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X - 1, (int)temp.Y, (int)temp.Z] != 0)
                        {
                            revers_q.Push(new Vector3(temp.X - 1, temp.Y, temp.Z));
                            temp = new Vector3(temp.X - 1, temp.Y, temp.Z);
                        }
                        else
                            if (patch_step[(int)temp.X, (int)temp.Y + 1, (int)temp.Z] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X, (int)temp.Y + 1, (int)temp.Z] != 0)
                            {
                                revers_q.Push(new Vector3(temp.X, temp.Y + 1, temp.Z));
                                temp = new Vector3(temp.X, temp.Y + 1, temp.Z);
                            }
                            else
                                if (temp.Y >= 1 && patch_step[(int)temp.X, (int)temp.Y - 1, (int)temp.Z] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X, (int)temp.Y - 1, (int)temp.Z] != 0)
                                {
                                    revers_q.Push(new Vector3(temp.X, temp.Y - 1, temp.Z));
                                    temp = new Vector3(temp.X, temp.Y - 1, temp.Z);
                                }
                                
                    // via z+1
                    if (temp.Z < mz - 1)
                    {
                        if (patch_step[(int)temp.X + 1, (int)temp.Y, (int)temp.Z + 1] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X + 1, (int)temp.Y, (int)temp.Z + 1] != 0)
                        {
                            revers_q.Push(new Vector3(temp.X + 1, temp.Y, temp.Z + 1));
                            temp = new Vector3(temp.X + 1, temp.Y, temp.Z + 1);
                        }
                        else
                            if (temp.X >= 1 && patch_step[(int)temp.X - 1, (int)temp.Y, (int)temp.Z + 1] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X - 1, (int)temp.Y, (int)temp.Z + 1] != 0)
                            {
                                revers_q.Push(new Vector3(temp.X - 1, temp.Y, temp.Z + 1));
                                temp = new Vector3(temp.X - 1, temp.Y, temp.Z + 1);
                            }
                            else
                                if (patch_step[(int)temp.X, (int)temp.Y + 1, (int)temp.Z + 1] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X, (int)temp.Y + 1, (int)temp.Z + 1] != 0)
                                {
                                    revers_q.Push(new Vector3(temp.X, temp.Y + 1, temp.Z + 1));
                                    temp = new Vector3(temp.X, temp.Y + 1, temp.Z + 1);
                                }
                                else
                                    if (temp.Y >= 1 && patch_step[(int)temp.X, (int)temp.Y - 1, (int)temp.Z + 1] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X, (int)temp.Y - 1, (int)temp.Z + 1] != 0)
                                    {
                                        revers_q.Push(new Vector3(temp.X, temp.Y - 1, temp.Z + 1));
                                        temp = new Vector3(temp.X, temp.Y - 1, temp.Z + 1);
                                    }
                                    else
                                        if (temp.Y >= 1 && patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z + 1] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z + 1] != 0)
                                        {
                                            revers_q.Push(new Vector3(temp.X, temp.Y, temp.Z + 1));
                                            temp = new Vector3(temp.X, temp.Y, temp.Z + 1);
                                        }
                    }
                    
                    // via z-1
                    if (temp.Z > 0)
                    {
                        if (patch_step[(int)temp.X + 1, (int)temp.Y, (int)temp.Z - 1] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X + 1, (int)temp.Y, (int)temp.Z - 1] != 0)
                        {
                            revers_q.Push(new Vector3(temp.X + 1, temp.Y, temp.Z - 1));
                            temp = new Vector3(temp.X + 1, temp.Y, temp.Z - 1);
                        }
                        else
                            if (temp.X >= 1 && patch_step[(int)temp.X - 1, (int)temp.Y, (int)temp.Z - 1] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X - 1, (int)temp.Y, (int)temp.Z - 1] != 0)
                            {
                                revers_q.Push(new Vector3(temp.X - 1, temp.Y, temp.Z - 1));
                                temp = new Vector3(temp.X - 1, temp.Y, temp.Z - 1);
                            }
                            else
                                if (patch_step[(int)temp.X, (int)temp.Y + 1, (int)temp.Z - 1] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X, (int)temp.Y + 1, (int)temp.Z - 1] != 0)
                                {
                                    revers_q.Push(new Vector3(temp.X, temp.Y + 1, temp.Z - 1));
                                    temp = new Vector3(temp.X, temp.Y + 1, temp.Z - 1);
                                }
                                else
                                    if (temp.Y >= 1 && patch_step[(int)temp.X, (int)temp.Y - 1, (int)temp.Z - 1] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X, (int)temp.Y - 1, (int)temp.Z - 1] != 0)
                                    {
                                        revers_q.Push(new Vector3(temp.X, temp.Y - 1, temp.Z - 1));
                                        temp = new Vector3(temp.X, temp.Y - 1, temp.Z - 1);
                                    }
                                    else
                                        if (temp.Y >= 1 && patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z - 1] < patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z] && patch_step[(int)temp.X, (int)temp.Y, (int)temp.Z - 1] != 0)
                                        {
                                            revers_q.Push(new Vector3(temp.X, temp.Y, temp.Z - 1));
                                            temp = new Vector3(temp.X, temp.Y, temp.Z - 1);
                                        }
                    }

                }

            if (revers_q.Count == 1)
                return new Stack<Vector3>();

            return revers_q;
        }

        public void KillBlock(Vector3 p)
        {
            KillBlock((int) p.X, (int) p.Y, (int) p.Z);
        }

        public void KillBlock(int x, int y, int z)
        {
            if (Main.dbobject.Data[n[x, y, z].blockID].dropafterdeath != (int)KnownIDs.error)
                Main.localitems.n.Add(new LocalItem() { count = Main.dbobject.Data[n[x, y, z].blockID].dropafterdeath_num, id = Main.dbobject.Data[n[x, y, z].blockID].dropafterdeath, pos = new Vector3(x, y, z) });
                //Main.iss.AddItem(Main.dbobject.Data[n[x, y, z].blockID].dropafterdeath,
                //                 Main.dbobject.Data[n[x, y, z].blockID].dropafterdeath_num);
                

            n[x, y, z].blockID = 0;
            n[x, y, z].health = 10;

            for (var i = -1; i <= 1; i++)
                for (var j = -1; j <= 1; j++)
                {
                    if (GoodVector3(x + i, y + j, z))
                        n[x + i, y + j, z].explored = true;
                }

            if (GoodVector3(x, y, z + 1))
                n[x, y, z + 1].explored = true;

            if (n[x, y, z].blockID == KnownIDs.StorageEntrance)
            {
                Main.globalstorage.n.Remove(new Vector3(x, y, z));
            }

            SubterrainPersonaly(new Vector3(x,y,z), ref Main.mmap);
        }

        /// <summary>
        /// установить блок, внести в список складов если складб активировать, если id блока активное
        /// </summary>
        /// <param name="p"></param>
        /// <param name="id"></param>
        public void SetBlock(Vector3 p, int id)
        {
            SetBlock((int)p.X, (int)p.Y, (int)p.Z, id);
        }

        /// <summary>
        /// установить блок, внести в список складов если складб активировать, если id блока активное
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="id"></param>
        public void SetBlock(int x, int y, int z, int id)
        {
            n[x, y, z].blockID = id;
            n[x, y, z].health = Main.dbobject.Data[id].basic_hp;

            if(Main.dbobject.Data[id].activeblock)
                active.Add(new Vector3(x,y,z));

            if(id == KnownIDs.StorageEntrance)
            {
                n[x, y, z].tags.Add("storage", new LocalItems());
                Main.globalstorage.n.Add(new Vector3(x,y,z));
            }

            SubterrainPersonaly(new Vector3(x, y, z + 1), ref Main.mmap);
        }

        /// <summary>
        /// добавить блок в список обновляемых в каждом цикле
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void ActivateBlock(int x, int y, int z)
        {
            active.Add(new Vector3(x,y,z));
        }

        /// <summary>
        /// добавить блок в список обновляемых в каждом цикле
        /// </summary>
        /// <param name="pos"></param>
        public void ActivateBlock(Vector3 pos)
        {
            active.Add(pos);
        }

        /// <summary>
        /// удалить блок из списка обновляемых в каждом цикле
        /// </summary>
        /// <param name="pos"></param>
        public void DeactivateBlock(Vector3 pos)
        {
            if (active.Contains(pos))
                active.Remove(pos); 
        }

        /// <summary>
        /// удалить блок из списка обновляемых в каждом цикле
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void DeactivateBlock(int x, int y, int z)
        {
            Vector3 a = new Vector3(x,y,z);
            if (active.Contains(a))
                active.Remove(a);
        }

        /// <summary>
        /// обновить блочную логику для всех активных блоков
        /// </summary>
        /// <param name="gt"></param>
        public void UpdateActiveblocks(GameTime gt)
        {
            foreach (var act in active)
            {
                
            }
        }

        /// <summary>
        /// пересчитать локально субтерейн для 1го столбца по Z
        /// </summary>
        /// <param name="where"></param>
        /// <param name="mmap"></param>
        public static void SubterrainPersonaly(Vector3 where, ref MMap mmap)
        {
            var i = @where.X;
            var j = @where.Y;
            for (var m = 0; m <= mz - 1; m++)
            {
                mmap.n[(int)i, (int)j, m].subterrain = true;
            }

            for (var m = 0; m <= mz - 1; m++)
            {
                if (mmap.n[(int)i, (int)j, m].blockID == 0)
                {
                    mmap.n[(int)i, (int)j, m].subterrain = false;
                }
                else
                {
                    mmap.n[(int)i, (int)j, m].subterrain = false;
                    goto here;
                }
            }
            here: ;
        }
    }
}