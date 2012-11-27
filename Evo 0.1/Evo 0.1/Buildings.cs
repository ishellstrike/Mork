using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Mork.Local_Map;

namespace Mork
{
    public class Store
    {
        public Vector3 pos = new Vector3();

        public bool busy = false;

        public Store(Vector3 _pos)
        {
            pos = _pos;
        }
    }
    public class Stores
    {
        public List<Store> list = new List<Store>();

        public Stores() { }

        public void BuildingsActions()
        {
            const int posmax = 100;
            int ykaz = 0;
            var poslist = new Vector3[posmax];

            for (int i = 0; i <= MMap.mx - 1; i++)
                for (int j = 0; j <= MMap.my - 1; j++)
                {
                    if (Main.mmap.n[i, j, WorldLife.ZProvider].Storing != OnStoreID.Nothing && Main.mmap.n[i, j, WorldLife.ZProvider].blockID != 1000)
                    {
                        poslist[ykaz] = new Vector3(i, j, WorldLife.ZProvider);
                        ykaz++;
                        if (ykaz >= posmax) goto here2;
                    }
                }
            here2:;
            int k = 0;
            if(poslist[0] != null)
            foreach (Store bld in list)
            {
                if (poslist[k].X != -1 && (Main.mmap.n[(int)bld.pos.X, (int)bld.pos.Y, (int)bld.pos.Z].Storing == OnStoreID.Nothing || (Main.mmap.n[(int)bld.pos.X, (int)bld.pos.Y, (int)bld.pos.Z].Storing == Main.mmap.n[(int)poslist[k].X, (int)poslist[k].Y, (int)poslist[k].Z].Storing && Main.mmap.n[(int)bld.pos.X, (int)bld.pos.Y, (int)bld.pos.Z].Storing_num < 5 && Main.mmap.n[(int)bld.pos.X, (int)bld.pos.Y, (int)bld.pos.Z].storing_material == Main.mmap.n[(int)poslist[k].X, (int)poslist[k].Y, (int)poslist[k].Z].storing_material)))
                {

                    Main.orders.NewOrder(poslist[k], new Vector3(bld.pos.X, bld.pos.Y, bld.pos.Z), OrderID.transfer_order);
                    
                    k++;
                    if (k >= ykaz) goto here;
                }

            }
            here:;
        }

        public Dictionary<string, int> GetSummary()
        {
            var cou = new Dictionary<string, int>();

            foreach (var st in list)
            {
                var temp = Main.mmap.n[(int)st.pos.X, (int)st.pos.Y, (int)st.pos.Z];
                if(temp.Storing != OnStoreID.Nothing)
                {
                    var key = Main.dbmaterial.Data[temp.storing_material].i_name + Main.dbonstore.data[temp.Storing].R_name + " " + Main.dbonstore.data[temp.Storing].I_name;
                    if (cou.ContainsKey(key))
                    {
                        cou[key] += temp.Storing_num;
                    }
                    else cou.Add(key, temp.Storing_num);
                }
            }

            return cou;
        }
    }

    public enum BuildingID
    {
        Nothing,
/*      Store,
        Carpentery*/
    }
    public class Building
    {
        private Vector3 pos = new Vector3();
        public BuildingID id = BuildingID.Nothing;

        public bool busy = false;

        public Building(Vector3 _pos, BuildingID _id)
        {
            pos = _pos;
            id = _id;
        }
    }
    public class Buildings
    {
        public List<Building> List = new List<Building>();

        public Buildings(List<Building> list)
        {
            List = list;
        }

        public void BuildingsActions()
        {
        }
    }
}
