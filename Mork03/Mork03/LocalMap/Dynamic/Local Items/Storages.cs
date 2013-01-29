using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Mork.Local_Map.Dynamic.Local_Items {
    public class Storages {
        public List<Vector3> n = new List<Vector3>();

        public Vector3 GetFreeStorage() {
            foreach (Vector3 v in n) {
                //LocalItems tempstor = Main.smap.At(v.X,v.Y,v.Z).tags["storage"] as LocalItems;
                //if (tempstor.n.Count < tempstor.carp) return v;
            }

            return new Vector3(-1);
        }
    }
}