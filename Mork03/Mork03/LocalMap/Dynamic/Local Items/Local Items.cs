using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Mork.Local_Map.Dynamic.Local_Items {
    public class LocalItems {
        public byte carp = 10;
        public List<LocalItem> n = new List<LocalItem>();

        public LocalItem GetNearItem(int x, int y, int z) {
            var temp = new LocalItem();
            var dest = new Vector3(x, y, z);

            foreach (LocalItem item in n) {
                if (Vector3.Distance(item.pos, dest) <= 1) {
                    return item;
                }
            }

            return temp;
        }

        public LocalItem GetNearItem(Vector3 dest) {
            var temp = new LocalItem();

            foreach (LocalItem item in n) {
                if (Vector3.Distance(item.pos, dest) <= 1) {
                    return item;
                }
            }

            return temp;
        }
    }
}