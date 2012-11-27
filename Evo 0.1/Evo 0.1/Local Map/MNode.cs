using System;
using System.Collections.Generic;

namespace Mork.Local_Map
{
    [Serializable]////////////////////////////////////////////////////////////////////////
    public class MNode
    {
        public int blockID;

        public Dictionary<string, object> tags = new Dictionary<string, object>();

        //public byte water = 0; // 0 - сухая 1 - влажная 2 - мокрая. 3-13 - заполненость водой. 13+ - под давлением

        public bool subterrain = true;

        private OnStoreID storing = OnStoreID.Nothing;

        public OnStoreID Storing
        {
            get { return storing; }
            set { storing = value; }
        }

        private byte storing_num = 0;

        public MaterialID storing_material = MaterialID.Basic;

        public byte Storing_num
        {
            get { return storing_num; }
            set { storing_num = value; if (storing_num == 0) storing = OnStoreID.Nothing; }
        }

        public bool explored = true;

        public byte health = 10;
    }
}