using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mork.Local_Map.Dynamic.Units
{
    public class LocalHeroes
    {
        public List<LocalHero> n = new List<LocalHero>(); 

        public void Update(GameTime gt)
        {
            foreach (var lh in n)
            {
                lh.pre_pos = lh.pos;
                lh.MoveUnit(gt);
                if (lh.pos == lh.pre_pos) lh.iddle_time += gt.ElapsedGameTime;
                else lh.iddle_time = TimeSpan.Zero;
            }
        }
    }
}
