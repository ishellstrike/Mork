using System;
using Mork.Local_Map;

namespace Mork
{
    public class LMO
    {
        public int metatex_n;

        //public bool placeble = true;

        public int dropafterdeath = 666;
        public byte dropafterdeath_num = 1;
        public float drop_chanse = 1;

        public string Name = "";

        public int max_hp = 10;

        public Type mnode_prototype = typeof(MNode);

        public LMO() { }
        public LMO(int _metatex)
        {
            metatex_n = _metatex;
        }
    }
}