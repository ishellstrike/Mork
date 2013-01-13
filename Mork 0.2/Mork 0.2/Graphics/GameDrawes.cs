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

        private static Rectangle GetTexRectFromN(int n)
        {
            return new Rectangle((n%10)*40, (n/10)*40, 40, 40);
        }
    }
}