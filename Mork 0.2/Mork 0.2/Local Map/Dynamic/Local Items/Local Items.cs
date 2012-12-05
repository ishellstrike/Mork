using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mork.Local_Map.Dynamic.Local_Items
{
    public class LocalItems
    {
        public List<LocalItem> n = new List<LocalItem>();
        public byte carp = 100;

        public LocalItem GetNearItem(int x, int y, int z)
        {
            LocalItem temp = new LocalItem();
            Vector3 dest = new Vector3(x,y,z);

            foreach (var item in n)
            {
                if(Vector3.Distance(item.pos, dest) <= 1)
                {
                    return item;
                }
            }

            return temp;
        }

        public LocalItem GetNearItem(Vector3 dest)
        {
            LocalItem temp = new LocalItem();

            foreach (var item in n)
            {
                if (Vector3.Distance(item.pos, dest) <= 1)
                {
                    return item;
                }
            }

            return temp;
        }
    }
}
