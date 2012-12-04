using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Mork.Local_Map.Dynamic.Local_Items;

namespace Mork.Local_Map
{
    [Serializable]////////////////////////////////////////////////////////////////////////
    public class MMap
    {
        Random rnd = new Random();

        public static Int32 mx = 128, my = 128, mz = 128;
        public MNode[,,] n = new MNode[mx, my, mz];

        public string Name = "test map 1";

        Dictionary<string, object> tags = new Dictionary<string, object>();

        public void SetMapTag(KeyValuePair<string,object> added_tag)
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

        public void SetNodeTag(int x, int y, int z, string s, object o)
        {
            if (!n[x, y, z].tags.ContainsKey(s))
                n[x, y, z].tags.Add(s, o);
            else n[x, y, z].tags[s] = o;
        }

        public object GetNodeTagData(int x, int y, int z, string s)
        {
            if (n[x, y, z].tags.ContainsKey(s)) return n[x, y, z].tags[s];
            return 0;
        }

        public object GetNodeTagData(Vector3 d, string s)
        {
            if (n[(int)d.X, (int)d.Y, (int)d.Z].tags.ContainsKey(s)) return n[(int)d.X, (int)d.Y, (int)d.Z].tags[s];
            return 0;
        }

        public List<string> GetNodeTagsInText(int x, int y, int z)
        {
            List<string> s = new List<string>();

            s.Add(string.Format("Pos = {0} {1} {2}",x,y,z));
            s.Add("ID = " + n[x, y, z].blockID + " mtex = " + Main.dbobject.Data[n[x, y, z].blockID].metatex_n);
            s.Add("DBName = " + Main.dbobject.Data[n[x, y, z].blockID].I_name);
            foreach (var tag in n[x, y, z].tags)
            {
                s.Add(tag.Key + " = " + tag.Value);
            }

            return s;
        }

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

        public Vector3 Bounds()
        {
            return new Vector3(mx - 1, my - 1, mz - 1);
        }

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
            for (int i = 0; i <= MMap.mx - 1; i++)
                for (int j = 0; j <= MMap.my - 1; j++)
                {
                    for (int m = 0; m <= MMap.mz - 1; m++)
                    {
                        if (Main.mmap.n[i, j, m].blockID != 0)
                        {
                            if (rnd.Next(1, Convert.ToInt32(101 - GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].tree_freq)) == 1 && m > 0)
                                if (GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].tree.Count > 0)
                                    Main.mmap.n[i, j, m - 1].blockID = GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].tree[rnd.Next(0, GMap.data[Main.gmap.obj[i + (int)Main.gmap_region.X, j + (int)Main.gmap_region.Y]].tree.Count)];
                            goto here2;
                        }
                    }
                    here2: ;
                }
        }

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

        public void RecalcExploredSubterrain()
        {
            for (int i = 0; i <= mx - 1; i++)
                for (int j = 0; j <= my - 1; j++)
                    for (int m = 0; m <= mz - 1; m++)
                    {
                        n[i, j, m].subterrain = true;
                        n[i, j, m].explored = false;
                    }

            for (int i = 0; i <= MMap.mx - 1; i++)
                for (int j = 0; j <= MMap.my - 1; j++)
                {
                    for (int m = 0; m <= MMap.mz - 1; m++)
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

        public void SimpleGeneration_bygmap()
        {
            for (int i = 0; i <= MMap.mx - 1; i++)
                for (int j = 0; j <= MMap.my - 1; j++)
                    for (int k = 0; k <= MMap.mz - 1; k += 8)
                    {
                        Main.mmap.n[i, j, k].blockID = 0;
                        Main.mmap.n[i, j, k + 1].blockID = 0;
                        Main.mmap.n[i, j, k + 2].blockID = 0;
                        Main.mmap.n[i, j, k + 3].blockID = 0;
                        Main.mmap.n[i, j, k + 4].blockID = 0;
                        Main.mmap.n[i, j, k + 5].blockID = 0;
                        Main.mmap.n[i, j, k + 6].blockID = 0;
                        Main.mmap.n[i, j, k + 7].blockID = 0;
                    }


            for (int i = 0; i <= MMap.mx - 1; i++)
                for (int j = 0; j <= MMap.my - 1; j++)
                {
                    for (int k = 0; k <= ((Main.gmap.n[(int)Main.gmap_region.X + i, (int)Main.gmap_region.Y + j])) * (MMap.mz - 1) * 0.3 + (MMap.mz - 1) * 0.7 - 5; k++)
                    {
                        if (MMap.GoodVector3(i, j, MMap.mz - 1 - k)) Main.mmap.n[i, j, MMap.mz - 1 - k].blockID = 12345;
                    }
                }

            int[] metamorf_clust = { 800,801,802,803,804,805,806};
            int[] matamorf_jila = { 810,811,812 };

            Main.mmap.Generation_FullLayer(18, 5);
            Main.mmap.Generation_FullLayer(17, 2);
            Main.mmap.Generation_FullLayer(16, 2);
            Main.mmap.Generation_FullLayer(15, 2);
            Main.mmap.Generation_FullLayer(14, 18);

            Main.mmap.Generation_BasicLayer((int)KnownIDs.Gabro);
            Main.mmap.Generation_FullLayer((int)KnownIDs.Gabro, 7);
            Main.mmap.Generation_FullLayer((int)KnownIDs.GabroToGranete, 1);
            Main.mmap.Generation_FullLayer((int)KnownIDs.GrenFranite, 6);

            int[] granite_clust = metamorf_clust;
            int[] granite_jila = { 55, 55 };
            Main.mmap.GenerationFullLayerCluster(11, 12, granite_clust, 10, granite_jila, 5, 20);

            Main.mmap.Generation_FullLayer(33, 10);

            int[] marble_clust = metamorf_clust;
            int[] marble_jila = matamorf_jila;
            Main.mmap.GenerationFullLayerCluster(22, 10, marble_clust, 10, marble_jila, 5, 20);

            Main.mmap.GenerationFullLayerCluster(44, 4, metamorf_clust, 6, matamorf_jila, 2, 25);

            Main.mmap.GenerationFullLayerCluster(22, 4, metamorf_clust, 6, matamorf_jila, 2, 25);

            for (int i = 0; i <= 20; i++ )
            {
                Main.mmap.Generation_FullLayer(824, 2);
                Main.mmap.Generation_FullLayer(828, 2);
            }


            Main.mmap.Generation_FullLayer_under(1);
            Main.mmap.Generation_FullLayer_under_under(2);
            //Main.mmap.Generation_FullLayerTOP(ObjectID.DirtWall_Grass2, 1);

            Main.mmap.Generation_FullLayerGrass(1);


            for (int i = 0; i <= MMap.mx - 1; i++)
                for (int j = 0; j <= MMap.my - 1; j++)
                {
                    //if(Main.gmap.n[Main.gmap_region.X + i, Main.gmap_region.Y + j] <= 0.4)
                    for (int k = 0; k <= ((0.4)) * (MMap.mz - 1) * 0.3 + (MMap.mz - 1) * 0.7 + 1; k++)
                    {
                        if (MMap.GoodVector3(i, j, MMap.mz - 1 - k) && Main.mmap.n[i, j, MMap.mz - 1 - k].blockID == 0) Main.mmap.n[i, j, MMap.mz - 1 - k].blockID = 888;//вода
                    }
                }

            Main.mmap.RecalcExploredSubterrain();

            Main.mmap.Generation_PlaceOnSurface();

            Main.PrepairMapDeleteWrongIDs(ref Main.mmap);
        }

        //public void RandomRoom(int type)
        //{
        //    switch (type)
        //    {
        //        case 0: _REmpty();
        //            break;
        //        case 1: _RR1();
        //            break;
        //        case 2: _RR2();
        //            break;
        //        case 3: _RNew();
        //            break;
        //        case 4: _RMy();
        //            break;
        //        case 5:
        //            break;
        //        case 255: _RTest();
        //            break;
        //    }

        //}
        //public void _RTest()
        //{
        //    if (Main.me_texoncur_a > Main.me_texoncur_b)
        //    {
        //        int a = Main.me_texoncur_a;
        //        Main.me_texoncur_a = Main.me_texoncur_b;
        //        Main.me_texoncur_b = a;
        //    }

        //    for (int i = 0; i <= mx - 1; i++)
        //        for (int j = 0; j <= my - 1; j++)
        //            for (int k = 0; k <= MMap.mz - 1; k++)
        //        {
        //            if (i == 1 || j == 1 || i == mx - 1 || j == my - 1) n[i, j,k].obj = FloarID.Sandwall;
        //            n[i, j, k].obj = (ObjID) rnd.Next(Main.me_texoncur_a, Main.me_texoncur_b);
        //            n[i, j, k].explored = false;
        //        }
        //}
        //public void _REmpty()
        //{
        //    for (int i = 0; i <= mx - 1; i++)
        //        for (int j = 0; j <= my - 1; j++)
        //            for (int k = 0; k <= MMap.mz - 1; k++)
        //        {
        //            n[i, j,k].floar = FloarID.Sandwall;
        //            n[i, j,k].explored = false;
        //            n[i, j,k].room_id = (byte)Room_ids.SolidRock;
        //        }
        //}
        //public void _RR1()
        //{
        //    for (int i = 0; i <= mx - 1; i++)
        //        for (int j = 0; j <= my - 1; j++)
        //            for (int k = 0; k <= MMap.mz - 1; k++)
        //        {
        //            n[i, j,k].floar = FloarID.Grass1;
        //            if (i == 1 || j == 1 || i == mx - 1 || j == my - 1) n[i, j,k].floar = FloarID.Sandwall;
        //            n[i, j,k].explored = false;
        //        }

        //    for (int a = 0; a <= rnd.Next(5, 20); a++)
        //    {
        //        int i = rnd.Next(3, mx - 2);
        //        for (int j = 3; j <= my - 2; j++)
        //        {
        //            n[i, j,0].floar = FloarID.Sandwall;
        //        }
        //    }
        //    for (int a = 0; a <= rnd.Next(5, 20); a++)
        //    {
        //        int j = rnd.Next(3, my - 2);
        //        for (int i = 3; i <= mx - 2; i++)
        //        {
        //            n[i, j,0].floar = FloarID.Sandwall;
        //        }
        //    }
        //    for (int a = 0; a <= rnd.Next(5, 20); a++)
        //    {
        //        int i = rnd.Next(3, mx - 2);
        //        for (int j = 3; j <= my - 2; j++)
        //        {
        //            if (rnd.Next(1, 5) == 1) n[i, j,0].floar = FloarID.Dirt;
        //        }
        //    }
        //    for (int a = 0; a <= rnd.Next(5, 20); a++)
        //    {
        //        int j = rnd.Next(3, my - 2);
        //        for (int i = 3; i <= mx - 2; i++)
        //        {
        //            if (rnd.Next(1, 2) == 1) n[i, j,0].floar = FloarID.Dirt;
        //        }
        //    }
        //}
        //public void _RR2()
        //{
        //    _REmpty();

        //    int a = rnd.Next(5000, 20000);
        //    Vector3 cur = new Vector3(mx / 2, my / 2, 0);
        //    for (int i = 0; i <= a - 1; i++)
        //    {
        //        if (GoodXY(cur)) n[cur.X, cur.Y,0].floar = FloarID.Grass1;
        //        if (!GoodXY(cur)) cur = new Vector3(mx / 2, my / 2, 0);
        //        int aa = rnd.Next(1, 6);
        //        switch (aa)
        //        {
        //            case 1:
        //                cur.X++;
        //                break;
        //            case 2:
        //                cur.Y--;
        //                break;
        //            case 3:
        //                cur.Y++;
        //                break;
        //            case 4:
        //                cur.X--;
        //                break;
        //        }
        //    }
        //    for (int i = 0; i <= mx - 1; i++)
        //        for (int j = 0; j <= my - 1; j++)
        //        {
        //            if (i == 2 || j == 2 || i == mx - 1 || j == my - 1) n[i, j,0].floar = FloarID.Sandwall;
        //        }
        //}
        //public void _RNew()
        //{
        //    _REmpty();

        //    int room_num = 50;
        //    int room_max = 5, room_min = 13;

        //    for (int i = 0; i <= room_num - 1; i++)
        //    {
        //        int no_unlimit = 0;
        //    retry:
        //        no_unlimit++;
        //        if (no_unlimit >= 10000) goto unlimit;

        //        Vector3 aa = new Vector3(rnd.Next(0, mx - 1), rnd.Next(0, my),0);
        //        Vector3 bb = new Vector3(aa.X + rnd.Next(room_min, room_min + room_max), aa.Y + rnd.Next(room_min, room_min + room_max),0);

        //        if (!GoodXY(aa) || !GoodXY(bb)) goto retry;
        //        for (int u = aa.X; u <= bb.X; u++)
        //            for (int v = aa.Y; v <= bb.Y; v++)
        //            {
        //                if (n[u, v,0].floar != FloarID.Sandwall)
        //                { goto retry; }
        //            }


        //        MakeRoom(aa, bb, Convert.ToByte(i + 1));
        //    }
        //unlimit:

        //    int a = 5000;
        //    Vector3 cur = new Vector3(mx / 2, my / 2,0);
        //    Vector3 mover = new Vector3();
        //    for (int i = 0; i <= a - 1; i++)
        //    {
        //        if (GoodXY(cur) && n[cur.X, cur.Y,0].room_id != (byte)Room_ids.RoomWall && n[cur.X, cur.Y,0].room_id > 50)
        //        {
        //            n[cur.X, cur.Y,0].floar = FloarID.Grass1;
        //            if (cur.X < mx - 1 && n[cur.X + 1, cur.Y,0].room_id != (byte)Room_ids.RoomWall)
        //                n[cur.X + 1, cur.Y,0].room_id = (byte)Room_ids.Coridor;
        //            if (cur.Y < my - 1 && n[cur.X, cur.Y + 1,0].room_id != (byte)Room_ids.RoomWall)
        //                n[cur.X, cur.Y + 1,0].room_id = (byte)Room_ids.CoridorWall;
        //            if (cur.X < mx - 1 && cur.Y < my - 1 && n[cur.X + 1, cur.Y + 1,0].room_id != (byte)Room_ids.RoomWall)
        //                n[cur.X + 1, cur.Y + 1,0].room_id = (byte)Room_ids.CoridorWall;
        //            if (cur.X < mx - 1 && cur.Y > 0 && n[cur.X + 1, cur.Y - 1,0].room_id != (byte)Room_ids.RoomWall)
        //                n[cur.X + 1, cur.Y - 1,0].room_id = (byte)Room_ids.CoridorWall;
        //            if (cur.X > 0 && cur.Y < my - 1 && n[cur.X - 1, cur.Y + 1,0].room_id != (byte)Room_ids.RoomWall)
        //                n[cur.X - 1, cur.Y + 1,0].room_id = (byte)Room_ids.CoridorWall;
        //            if (cur.X > 0 && n[cur.X - 1, cur.Y,0].room_id != (byte)Room_ids.RoomWall)
        //                n[cur.X - 1, cur.Y,0].room_id = (byte)Room_ids.CoridorWall;
        //            if (cur.Y > 0 && n[cur.X, cur.Y - 1,0].room_id != (byte)Room_ids.RoomWall)
        //                n[cur.X, cur.Y - 1,0].room_id = (byte)Room_ids.CoridorWall;
        //            if (cur.X > 0 && cur.Y > 0 && n[cur.X - 1, cur.Y - 1,0].room_id != (byte)Room_ids.RoomWall)
        //                n[cur.X - 1, cur.Y - 1,0].room_id = (byte)Room_ids.CoridorWall;
        //        }
        //        int aa = rnd.Next(1, 18);
        //        if (cur.X % 2 == 0 || cur.Y % 2 == 0)
        //            switch (aa)
        //            {
        //                case 1:
        //                    mover.X = 1;
        //                    mover.Y = 0;
        //                    break;
        //                case 2:
        //                    mover.X = -1;
        //                    mover.Y = 0;
        //                    break;
        //                case 3:
        //                    mover.X = 0;
        //                    mover.Y = 1;
        //                    break;
        //                case 4:
        //                    mover.X = 0;
        //                    mover.Y = -1;
        //                    break;
        //            }
        //        cur += mover;

        //        if (!GoodXY(cur)) cur = new Vector3(mx / 2, my / 2,0);
        //    }
        //}
        //public void _RMy()
        //{
        //    _REmpty();
        //}

        //public void MakeRoom(Vector3 a, Vector3 b, byte id)
        //{
        //    FillRect(a.X + 1, a.Y + 1, b.X - 1, b.Y - 1, FloarID.Grass1);
        //    //FillRoomID(a, b, id);
        //    n[a.X, rnd.Next(a.Y, b.Y),0].floar = FloarID.Grass1;
        //    n[b.X, rnd.Next(a.Y, b.Y),0].floar = FloarID.Grass1;
        //    n[rnd.Next(a.X, b.X), a.Y,0].floar = FloarID.Grass1;
        //    n[rnd.Next(b.X, b.X), b.Y,0].floar = FloarID.Grass1;
        //}

        //public void FillRect(int x1, int y1, int x2, int y2, FloarID flid)
        //{
        //    for (int i = x1; i <= x2; i++)
        //        for (int j = y1; j <= y2; j++)
        //        {
        //            n[i, j,0].floar = flid;
        //        }
        //}
        //public void FillRect(Vector3 a, Vector3 b, FloarID flid)
        //{
        //    for (int i = a.X; i <= b.X; i++)
        //        for (int j = a.Y; j <= b.Y; j++)
        //            for (int k = a.Z; k <= b.Z; k++)
        //        {
        //            n[i, j, k].floar = flid;
        //        }
        //}

        //public void FillRoomID(int x1, int y1, int x2, int y2, byte id)
        //{
        //    for (int i = x1; i <= x2; i++)
        //        for (int j = y1; j <= y2; j++)
        //        {
        //            n[i, j,0].room_id = id;
        //            if (i == x1 || j == y1 || i == x2 || j == y2) n[i, j,0].room_id = (byte)Room_ids.RoomWall;
        //            if ((i == x1 || j == y1 || i == x2 || j == y2) && n[i, j,0].floar != FloarID.Sandwall) n[i, j,0].room_id = (byte)Room_ids.RoomDoor;
        //        }
        //}
        //public void FillRoomID(XY a, XY b, byte id)
        //{
        //    for (int i = a.X; i <= b.X; i++)
        //        for (int j = a.Y; j <= b.Y; j++)
        //        {
        //            n[i, j,0].room_id = id;
        //            if (i == a.X || j == a.Y || i == b.X || j == b.Y) n[i, j,0].room_id = (byte)Room_ids.RoomWall;
        //            if ((i == a.X || j == a.Y || i == b.X || j == b.Y) && n[i, j,0].floar != FloarID.Sandwall) n[i, j,0].room_id = (byte)Room_ids.RoomDoor;
        //        }
        //}

        public static bool GoodVector3(Vector3 loc)
        {
            if (loc != null && loc.X >= 0 && loc.Y >= 0 && loc.X <= mx - 1 && loc.Y <= my - 1 && loc.Z >= 0 && loc.Z <= mz - 1) return true;
            return false;
        }
        public static bool GoodVector3(int X, int Y, int Z)
        {
            if (X >= 0 && Y >= 0 && X <= mx - 1 && Y <= my - 1 && Z >= 0 && Z <= mz - 1) return true;
            return false;
        }

        public static bool GoodVector3(float X, float Y, float Z)
        {
            if (X >= 0 && Y >= 0 && X <= mx - 1 && Y <= my - 1 && Z >= 0 && Z <= mz - 1) return true;
            return false;
        }

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

            patch_step[(int)loc.X, (int)loc.Y, (int)loc.Z] = short.MaxValue;



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
            if (Main.dbobject.Data[n[x, y, z].blockID].dropafterdeath != (int) KnownIDs.error)
            Main.localitems.n.Add(new LocalItem() { count = Main.dbobject.Data[n[x, y, z].blockID].dropafterdeath_num, id = Main.dbobject.Data[n[x, y, z].blockID].dropafterdeath, pos = new Vector3(x,y,z)});

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

            WorldLife.SubterrainPersonaly(new Vector3(x,y,z), ref Main.mmap);
        }
    }
}