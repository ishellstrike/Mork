using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mork {
    /// <summary>
    /// Displays the FPS
    /// </summary>
    public static class FrameRateCounter {
        private const int max_gr = 300;
        private static TimeSpan _elapsedTime = TimeSpan.Zero;
        private static NumberFormatInfo _format;
        private static int _frameCounter;
        private static int _frameRate;
        private static readonly Vector2 _position = new Vector2(10, 50);
        private static readonly Vector2 ofs = new Vector2(0, 12);
        private static long memo;
        private static readonly int[] graph = new int[max_gr];
        private static int _curent;
        private static int _max = 1, _min;
        private static readonly int[] insec = new int[10];
        private static byte curinsec;

        public static void Update(GameTime gameTime) {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime > TimeSpan.FromMilliseconds(100)) {
                _elapsedTime -= TimeSpan.FromMilliseconds(100);
                _frameRate = _frameCounter;

                insec[curinsec] = _frameRate;
                curinsec++;
                if (curinsec >= 10) {
                    curinsec = 0;
                }
                graph[_curent] = insec.Sum();
                _curent++;
                if (_curent >= max_gr) {
                    _curent = 0;
                }

                _max = graph.Max();
                _min = graph.Min();

                _frameCounter = 0;
                if (_curent%10 == 0) {
                    memo = (long) (Process.GetCurrentProcess().WorkingSet64/1024f/1024f);
                }
            }
        }

        public static void Draw(GameTime gameTime, SpriteFont fnt, SpriteBatch sb, LineBatch lb, int resx, int resy) {
            _frameCounter++;

            string fps = string.Format(_format, "{0}x{1} {2} fps", resx, resy, insec.Sum());
            string mem = string.Format(_format, "{0} MiB", memo);

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
                     DepthStencilState.Default, RasterizerState.CullCounterClockwise, null);
            sb.DrawString(fnt, fps, _position + Vector2.One, Color.Black);
            sb.DrawString(fnt, fps, _position, Color.White);

            sb.DrawString(fnt, mem, _position + Vector2.One + ofs, Color.Black);
            sb.DrawString(fnt, mem, _position + ofs, Color.White);


            var offset = new Vector2(150, 15);
            for (int index = 0; index < max_gr; index++) {
                var a = (int) (((float) graph[index])/(_max)*100);
                Color col = Color.Lerp(Color.Blue, Color.Red, (float) graph[index]/(_max));
                if (index == _curent - 1) {
                    col.G = 255;
                    col.B = 255;
                }
                if (index == _curent - 2) {
                    col.G = 200;
                    col.B = 200;
                }
                if (index == _curent - 3) {
                    col.G = 150;
                    col.B = 150;
                }
                if (index == _curent - 4) {
                    col.G = 75;
                    col.B = 75;
                }

                if (a > 0) {
                    lb.AddLine(new Vector2(10 + index, 100) + offset, new Vector2(10 + index, 100 - a) + offset, col, 1);
                }
            }

            int average = graph.Sum();
            average /= graph.Length;
            sb.DrawString(fnt, string.Concat("average = ", average), new Vector2(10, 115) + offset, Color.Red);

            sb.End();
        }
    }
}