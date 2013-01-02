using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mork
{
    public partial class Main
    {
        private void LRInit()
        {
        }

        private void LocalMapRenderer(GameTime gt, SpriteBatch sb, GraphicsDevice device)
        {
            var temp = device.RasterizerState;

            smap.DrawAllMap(gt, Camera);
            device.RasterizerState = temp;
        }

        private void LRUpdate(GameTime gt)
        {
        }
    }
}
