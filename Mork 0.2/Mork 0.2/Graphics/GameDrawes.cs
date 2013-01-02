using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mork.Generators;
using Mork.Local_Map;
using Mork.Local_Map.Dynamic;
using Mork.Local_Map.Dynamic.Actions;
using Mork.Local_Map.Dynamic.Units.Actions;

namespace Mork
{
    public partial class Main
    {
        private static void DrawGMapGenRects(GameTime gameTime, SpriteBatch sb, GraphicsDevice GraphicsDevice)
        {
            sb.Begin();
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
            sb.End();
        }

        private static void InGameMessageDraw(GameTime gameTime, SpriteBatch sb, GraphicsDevice GraphicsDevice)
        {
            sb.Begin();
            spriteBatch.DrawString(Font1, _messagestring, new Vector2(50, 50), Color.White);
            sb.End();
        }

        private static void MainMenuDraw(GameTime gt, SpriteBatch sb, GraphicsDevice GraphicsDevice)
        {
            sb.Begin();
            _titleAnimation += (float)(_titlePhase*gt.ElapsedGameTime.TotalSeconds);
            if (_titleAnimation >= 1 || _titleAnimation <= 0) _titlePhase *= -1;
            spriteBatch.Draw(interface_tex[15], new Vector2(470, 20),
                             new Color(255, _titleAnimation, _titleAnimation));
            sb.End();
        }

        private static void DrawLocalItems(int[,] drawed)
        {
            foreach (var i in localitems.n)
            {
                if (MMap.GoodVector3(i.pos) && i.pos.Z >= drawed[(int)i.pos.X, (int)i.pos.Y] - 1)
                {
                    Vector2 pos = ToIsometricFloat(i.pos.X, i.pos.Y);
                    pos.Y = pos.Y + (i.pos.Z - Selector.Z - 1) * 20 + 1;

                    spriteBatch.Draw(object_tex, new Vector2(pos.X, pos.Y),
                                     GetTexRectFromN(dbobject.Data[i.id].metatex_n), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1 - (i.pos.X + i.pos.Y + 1) / (MMap.mx + MMap.my));
                }
            }
        }

        private static void GameInterface()
        {
            //string ssss = String.Format("{0} {1}, год {2} эпохи {3}", WorldLife.Day,
            //                            NamesGenerator.MonthNameR(WorldLife.Month), WorldLife.Year, WorldLife.Age);
            //string sss1 = String.Format("{0}:{1}:{2}", WorldLife.Hour, WorldLife.Min.ToString("00"),
            //                            WorldLife.Sec.ToString("00"));

            if (PAUSE)
            {
                spriteBatch.Draw(interface_tex[6], new Vector2(0, 20), null, Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0.03f);
                spriteBatch.Draw(interface_tex[6], new Vector2(0, 15), null, Color.DarkRed, 0, Vector2.Zero, 1, SpriteEffects.None, 0.02f);
                for (int i = 0; i <= 10; i++)
                {
                    spriteBatch.DrawString(Font2, "PAUSE", new Vector2(i * 100, 42), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                }
            }

        }

        private static void DrawSelector(int[,] drawed)
        {
            if (MMap.GoodVector3(Selector))
            {
                float x_temp2 = ToIsometricX((int) Selector.X, (int) Selector.Y);
                float y_temp2 = ToIsometricY((int) Selector.X, (int) Selector.Y) - 20;
                Color col = MMap.IsWalkable(Selector) ? new Color(255, 255, 255) : new Color(255, 0, 50);

                spriteBatch.Draw(interface_tex[2], new Vector2(x_temp2, y_temp2), null, col, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None,
                                     1 - (Selector.X + Selector.Y+2) / (MMap.mx + MMap.my));
                for (int sel = 1; sel < drawed[(int) Selector.X, (int) Selector.Y] - Selector.Z; sel++)
                    spriteBatch.Draw(interface_tex[2], new Vector2(x_temp2, y_temp2 + sel*20), null, col, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None,
                                     1 - (Selector.X + Selector.Y+2) / (MMap.mx + MMap.my));
            }
        }

        private static Rectangle GetTexRectFromN(int n)
        {
            return new Rectangle((n%10)*40, (n/10)*40, 40, 40);
        }
    }
}