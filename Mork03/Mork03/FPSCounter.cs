using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mork
{
        /// <summary>
        /// Displays the FPS
        /// </summary>
        public static class FrameRateCounter
        {           
            private static TimeSpan _elapsedTime = TimeSpan.Zero;
            private static NumberFormatInfo _format;
            private static int _frameCounter;
            private static int _frameRate;
            private static Vector2 _position = new Vector2(10,50);
            private static Vector2 ofs = new Vector2(0,12);
            private static long memo;
            private static int max_gr = 300;
            private static int[] graph = new int[max_gr];
            private static int curent;
            private static int max = 1,min;

            public static void Update(GameTime gameTime)
            {
                _elapsedTime += gameTime.ElapsedGameTime;

                if (_elapsedTime > TimeSpan.FromMilliseconds(123))
                {
                    _elapsedTime -= TimeSpan.FromMilliseconds(123);
                    _frameRate = (int)(_frameCounter*8.13008);

                    graph[curent] = _frameRate;
                    curent++;
                    if (curent >= max_gr) curent = 0;

                    max = graph.Max();
                    min = graph.Min();

                    _frameCounter = 0;
                    if(curent % 10 == 0)
                    memo = (long)(Process.GetCurrentProcess().WorkingSet64/1024f/1024f);
                }
            }
             
            public static void Draw(GameTime gameTime, SpriteFont fnt, SpriteBatch sb, LineBatch lb, int resx, int resy)
            {
                _frameCounter++;

                string fps = string.Format(_format, "{0}x{1} {2} fps", resx, resy, _frameRate);
                string mem = string.Format(_format, "{0} MiB", memo);

                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null);
                sb.DrawString(fnt, fps,_position + Vector2.One, Color.Black);
                sb.DrawString(fnt, fps,_position, Color.White);

                sb.DrawString(fnt, mem, _position + Vector2.One + ofs, Color.Black);
                sb.DrawString(fnt, mem, _position + ofs, Color.White);


                Vector2 offset = new Vector2(150,15);
                for (int index = 0; index < max_gr; index++)
                {
                    int a = (int)(((float)graph[index]) / (max) * 100);
                    Color col = Color.Lerp(Color.Blue, Color.Red, (float)graph[index] / (max));
                    if (index == curent - 1)
                    {
                        col.G = 255;
                        col.B = 255;
                    }
                    if (index == curent - 2)
                    {
                        col.G = 200;
                        col.B = 200;
                    }
                    if (index == curent - 3)
                    {
                        col.G = 150;
                        col.B = 150;
                    }
                    if (index == curent - 4)
                    {
                        col.G = 75;
                        col.B = 75;
                    }

                    if (a > 0)
                    lb.AddLine(new Vector2(10 + index, 100) + offset, new Vector2(10 + index, 100 - a) + offset, col, 1);
                }

                int average = 0;
                foreach (var i in graph)
                {
                    average += i;
                }
                average /= graph.Length;
                sb.DrawString(fnt, string.Concat("average = ", average), new Vector2(10, 115) + offset, Color.Red);

                sb.End();
            }
        }
}
