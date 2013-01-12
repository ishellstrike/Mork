using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Mork.Local_Map.Sector;

namespace Mork.Local_Map.Dynamic.Local_Items
{
    public class Storages
    {
        public List<Vector3> n = new List<Vector3>();

        public Vector3 GetFreeStorage()
        {
            foreach (var v in n)
            {
                //LocalItems tempstor = Main.smap.At(v.X,v.Y,v.Z).tags["storage"] as LocalItems;
                //if (tempstor.n.Count < tempstor.carp) return v;
            }

            return new Vector3(-1);
        }
    }
}
