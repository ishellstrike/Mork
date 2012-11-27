using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mork.Generators;
using Mork.Local_Map;

namespace Mork
{
    public partial class Main
    {
        private static void DrawGMapGenRects()
        {
            for (int i = 0; i <= GMap.size - 14; i += 13)
                for (int j = 0; j <= GMap.size - 14; j += 13)
                {
                    switch (gmapreshim)
                    {
                        case Gmapreshim.Normal:
                            spriteBatch.Draw(GMap.data[gmap.obj[i, j]].tex,
                                             new Vector2(250 + i/13*5, 50 + j/13*5),
                                             GMap.data[gmap.obj[i, j]].col);
                            break;
                        case Gmapreshim.Height:
                            int a = Convert.ToInt32(gmap.n[i, j]*255);
                            spriteBatch.Draw(interface_tex[12], new Vector2(250 + i/13*5, 50 + j/13*5),
                                             new Color(a, a, a));
                            break;
                        case Gmapreshim.Tempirature:
                            a = Convert.ToInt32(gmap.t[i, j]);
                            spriteBatch.Draw(interface_tex[12], new Vector2(250 + i/13*5, 50 + j/13*5),
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
                                spriteBatch.Draw(
                                    GMap.data[gmap.obj[i + (int) gmap_region.X, j + (int) gmap_region.Y]].tex,
                                    new Vector2(700 + i*5, 50 + j*5),
                                    GMap.data[gmap.obj[i + (int) gmap_region.X, j + (int) gmap_region.Y]].col);
                                break;
                            case Gmapreshim.Tempirature:
                                int a = Convert.ToInt32(gmap.t[i + (int) gmap_region.X, j + (int) gmap_region.Y]);
                                spriteBatch.Draw(interface_tex[12], new Vector2(700 + i*5, 50 + j*5),
                                                 new Color(a, 0, 255 - a));
                                break;
                            case Gmapreshim.Height:
                                a = Convert.ToInt32(gmap.n[i + (int) gmap_region.X, j + (int) gmap_region.Y]*255);
                                spriteBatch.Draw(interface_tex[12], new Vector2(700 + i*5, 50 + j*5),
                                                 new Color(a, a, a));
                                break;
                        }
                    }
                }

            for (int i = 5; i <= GMap.data.Count - 1; i++)
            {
                spriteBatch.Draw(GMap.data.ElementAt(i).Value.tex, new Vector2(250, 450 + (i - 5)*12),
                                 GMap.data.ElementAt(i).Value.col);
                spriteBatch.DrawString(Font2, GMap.data.ElementAt(i).Value.name,
                                       new Vector2(250 + 10, 445 + (i - 5)*12), Color.White);
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
            int a = (int) midscreen.X - 33;
            if (a < 0) a = 0;
            int b = (int) midscreen.Y - 33;
            if (b < 0) b = 0;
            int aa = (int) midscreen.X + 33;
            if (aa > MMap.mx - 1) aa = MMap.mx - 1;
            int bb = (int) midscreen.Y + 33;
            if (bb > MMap.my - 1) bb = MMap.my - 1;

            var ramka_3 = new Vector3();
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
                        }
                    }

            string ssss = String.Format("{0} {1}, год {2} эпохи {3}", WorldLife.Day,
                                        NamesGenerator.MonthNameR(WorldLife.Month), WorldLife.Year, WorldLife.Age);
            string sss1 = String.Format("{0}:{1}:{2}", WorldLife.Hour, WorldLife.Min.ToString("00"),
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
            if (MMap.GoodVector3(Selector) && mmap.n[(int) Selector.X, (int) Selector.Y, (int) Selector.Z].explored)
            {
                if (mmap.n[(int) Selector.X, (int) Selector.Y, (int) Selector.Z].Storing != OnStoreID.Nothing &&
                    mmap.n[(int) Selector.X, (int) Selector.Y, (int) Selector.Z].Storing_num > 0)
                    spriteBatch.DrawString(Font1,
                                           mmap.n[(int) Selector.X, (int) Selector.Y, (int) Selector.Z].Storing_num +
                                           " " +
                                           dbmaterial.Data[
                                               mmap.n[(int) Selector.X, (int) Selector.Y, (int) Selector.Z].
                                                   storing_material].
                                               i_name +
                                           dbonstore.data[
                                               mmap.n[(int) Selector.X, (int) Selector.Y, (int) Selector.Z].Storing].
                                               R_name +
                                           " " +
                                           dbonstore.data[
                                               mmap.n[(int) Selector.X, (int) Selector.Y, (int) Selector.Z].Storing].
                                               I_name,
                                           new Vector2(1052, 40), Color.White);
                spriteBatch.DrawString(Font1,
                                       dbmaterial.Data[(MaterialID) mmap.GetNodeTagData(Selector, "material")].i_name +
                                       " " +
                                       dbobject.Data[
                                           mmap.n[(int) Selector.X, (int) Selector.Y, (int) Selector.Z].blockID].I_name +
                                       ":" + mmap.n[(int) Selector.X, (int) Selector.Y, (int) Selector.Z].blockID,
                                       new Vector2(1052, 60), Color.White);
            }
        }

        private static void DrawTreesHigher(Vector3 ramka_3, ref float x_temp2, ref float y_temp2, int[,] drawed, int i,
                                            int j)
        {
            if (Selector.Z != 0 && mmap.n[i, j, (int) Selector.Z - 1].blockID != 0 &&
                !dbobject.Data[mmap.n[i, j, (int) Selector.Z - 1].blockID].createfloor)
            {
                x_temp2 = ToIsometricX(i, j);
                y_temp2 = ToIsometricY(i, j) - 40;

                if (x_temp2 < resx + 40 && x_temp2 > -40 && y_temp2 < resy + 40 && y_temp2 > -40)
                {
                    DrawAllFloarCreators(ramka_3, x_temp2, y_temp2 + 40, drawed, i, j + 1, true);
                    spriteBatch.Draw(object_tex, new Vector2(x_temp2, y_temp2),
                                     GetTexRectFromN(dbobject.Data[mmap.n[i, j, (int) Selector.Z - 1].blockID].metatex_n),
                                     Color.White);
                }
            }
        }

        private static Rectangle GetTexRectFromN(int n)
        {
            return new Rectangle((n%10)*40, (n/10)*40, 40, 40);
        }

        private static void DrawAllFloarCreators(Vector3 ramka_3, float x_temp2, float y_temp2, int[,] drawed, int i,
                                                 int j,
                                                 bool no_condition)
        {
            if (x_temp2 < resx + 40 - 300 && x_temp2 > -40 && y_temp2 < resy + 40 && y_temp2 > -40 + 40)
            {
                int aa = (drawed[i, j] - (int) Selector.Z + 1)*5;
                if (drawed[i, j] - Selector.Z + 1 > 1) aa += 50;
                if (mmap.n[i, j, drawed[i, j]].subterrain) aa += 60;

                if (!mmap.n[i, j, drawed[i, j]].explored) aa = 255;

                Color tcol = dbmaterial.Data[(MaterialID) mmap.GetNodeTagData(i, j, drawed[i, j], "material")].color;

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
                    spriteBatch.Draw(object_tex, new Vector2(x_temp2, y_temp2),
                                     GetTexRectFromN(dbobject.Data[mmap.n[i, j, drawed[i, j]].blockID].metatex_n),
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
                else
                    spriteBatch.Draw(object_tex, new Vector2(x_temp2, y_temp2),
                                     GetTexRectFromN(dbobject.Data[12345].metatex_n),
                                     new Color(rr, gg, bb));
            }
        }

        private static int[,] WhoDrawedCalculate(int a, int b, int aa, int bb)
        {
            var drawed = new int[MMap.mx,MMap.my];
            for (int i = a; i <= aa; i++)
                for (int j = b; j <= bb; j++)
                {
                    drawed[i, j] = (int) Selector.Z;
                    for (var k = (int) Selector.Z; k <= MMap.mz - 1; k++)
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
    }
}