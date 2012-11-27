using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mork
{

    static class WorldLife
    {
        public static int ZProvider = 0;
        static int _subtCalc = 0;

        public static int Sec;
        public static int Min;
        public static int Hour;
        public static int Day;
        public static int Month;
        public static int Year;
        public static string Age = "";

        public static void SubterrainPersonaly(Vector3 where, ref MMap mmap)
        {
            var i = where.X;
            var j = where.Y;
            for (var m = 0; m <= MMap.mz - 1; m++)
            {
                mmap.n[(int)i, (int)j, (int)m].subterrain = true;
            }

            for (var m = 0; m <= MMap.mz - 1; m++)
            {
                if (mmap.n[(int)i, (int)j, (int)m].blockID == 0)
                {
                    mmap.n[(int)i, (int)j, (int)m].subterrain = false;
                }
                else
                {
                    mmap.n[(int)i, (int)j, (int)m].subterrain = false;
                    goto here;
                }
            }
            here: ;
        }

        public static void WorldTick(ref MMap mmap)
        {
            var rnd = new Random();
            byte t = 0;

            ZProvider++;
            _subtCalc++;
            if (ZProvider > MMap.mz - 1) ZProvider = 0;
            if (_subtCalc > 100) _subtCalc = 0;

            int k = ZProvider;

            if (_subtCalc == 0)
            {

                Sec += 13;
                Min += 41;
                Hour += 7;
                if (Sec >= 60)
                {
                    Sec -= 60;
                    Min++;
                }
                if (Min >= 60)
                {
                    Min -= 60;
                    Hour++;
                }
                if (Hour >= 24)
                {
                    Hour -= 24;
                    Day++;
                    if (Day >= 30)
                    {
                        Day -= 30;
                        Month++;
                        if (Month > 12)
                        {
                            Month -= 12;
                            Year++;
                        }
                    }
                }
            }

            //for (int i = 0; i <= MMap.mx - 1; i++)
                //    for (int j = 0; j <= MMap.my - 1; j++)
                //        for (int m = 0; m <= MMap.mz - 1; m++)
                //        {
                //            mmap.n[i, j, m].subterrain = true;
                //        }

                //for (int i = 0; i <= MMap.mx - 1; i++)
                //    for (int j = 0; j <= MMap.my - 1; j++)
                //    {
                //        for (int m = 0; m <= MMap.mz - 1; m++)
                //        {
                //            if (mmap.n[i, j, m].Obj == ObjectID.None)
                //            {
                //                mmap.n[i, j, m].subterrain = false;
                //            }
                //            else
                //            {
                //                mmap.n[i, j, m].subterrain = false;
                //                goto here;
                //            }
                //        }
                //    here: ;
                //    }
            //}


            //for (var i = 0; i <= MMap.mx - 1; i++)
            //    for (var j = 0; j <= MMap.my - 1; j++)
            //    {
            //        var temp = mmap.n[i, j, k];

            //        if (!temp.subterrain)
            //        {
            //            if (temp.Obj == ObjectID.DirtWall && rnd.Next(0, 400) == 0) temp.Obj = ObjectID.DirtWall_Grass1;
            //            if (temp.Obj == ObjectID.DirtWall_Grass1 && rnd.Next(0, 400) == 0) temp.Obj = ObjectID.DirtWall_Grass2;
            //            if (temp.Obj == ObjectID.DirtWall_Grass2 && rnd.Next(0, 400) == 0)
            //            {

            //            }

            //            if (temp.Obj == GMap.data[Main.gmap.obj[i, j]].under_surf && GMap.data[Main.gmap.obj[i + Main.gmap_region.X, j + Main.gmap_region.Y]].grass.Count > 0 && rnd.Next(0, 400) == 0)
            //            {
            //                temp.Obj = GMap.data[Main.gmap.obj[i + Main.gmap_region.X, j + Main.gmap_region.Y]].grass[rnd.Next(0, GMap.data[Main.gmap.obj[i + Main.gmap_region.X, j + Main.gmap_region.Y]].grass.Count - 1)];
            //            }
            //        }

                    //if (temp.water > 2)
                    //{
                    //    int direction = rnd.Next(1, 15);
                    //    switch (direction)
                    //    {
                    //        case 1:
                    //            if (j != MMap.my - 1 && mmap.n[i, j + 1,k].water < temp.water && mmap.n[i,j + 1,k].Obj == 0)
                    //            {
                    //                t = Convert.ToByte(temp.water / 2);
                    //                mmap.n[i, j + 1,k].water+=t;
                    //                temp.water-=t;
                    //            }
                    //            break;
                    //        case 2:
                    //            if (i != MMap.mx - 1 && mmap.n[i + 1, j,k].water < temp.water && mmap.n[i + 1, j,k].Obj == 0)
                    //            {
                    //                t = Convert.ToByte(temp.water / 2);
                    //                mmap.n[i + 1, j,k].water+=t;
                    //                temp.water -= t;
                    //            }
                    //            break;
                    //        case 3:
                    //            if (j != 0 && mmap.n[i, j - 1,k].water < temp.water && mmap.n[i, j - 1,k].Obj == 0)
                    //            {
                    //                t = Convert.ToByte(temp.water / 2);
                    //                mmap.n[i, j - 1,k].water += t;
                    //                temp.water-=t;
                    //            }
                    //            break;
                    //        case 4:
                    //            if (i != 0 && mmap.n[i - 1, j,k].water < temp.water && mmap.n[i - 1,j,k].Obj == 0)
                    //            {
                    //                t = Convert.ToByte(temp.water / 2);
                    //                mmap.n[i - 1, j,k].water+=t;
                    //                temp.water-=t;
                    //            }
                    //            break;
                    //    }

                    //    if (temp.water > 2)
                    //    {
                    //        direction = rnd.Next(1, 15);
                    //        switch (direction)
                    //        {
                    //            case 1:
                    //                if (j != MMap.my - 1 && mmap.n[i, j + 1, k].water < temp.water && mmap.n[i, j + 1, k].Obj == 0)
                    //                {
                    //                    t = Convert.ToByte(temp.water / 2);
                    //                    mmap.n[i, j + 1, k].water += t;
                    //                    temp.water -= t;
                    //                }
                    //                break;
                    //            case 2:
                    //                if (i != MMap.mx - 1 && mmap.n[i + 1, j, k].water < temp.water && mmap.n[i + 1, j, k].Obj == 0)
                    //                {
                    //                    t = Convert.ToByte(temp.water / 2);
                    //                    mmap.n[i + 1, j, k].water += t;
                    //                    temp.water -= t;
                    //                }
                    //                break;
                    //            case 3:
                    //                if (j != 0 && mmap.n[i, j - 1, k].water < temp.water && mmap.n[i, j - 1, k].Obj == 0)
                    //                {
                    //                    t = Convert.ToByte(temp.water / 2);
                    //                    mmap.n[i, j - 1, k].water += t;
                    //                    temp.water -= t;
                    //                }
                    //                break;
                    //            case 4:
                    //                if (i != 0 && mmap.n[i - 1, j, k].water < temp.water && mmap.n[i - 1, j, k].Obj == 0)
                    //                {
                    //                    t = Convert.ToByte(temp.water / 2);
                    //                    mmap.n[i - 1, j, k].water += t;
                    //                    temp.water -= t;
                    //                }
                    //                break;
                    //        }
                    //    }
                    //}

                    //if (temp.Obj == ObjectID.WaterSource) temp.water += 1;
                    //if (temp.Obj == ObjectID.WaterSourceBig) temp.water += 4;
                
        }
    }
}
