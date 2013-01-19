using System;
using Mork.Local_Map;

namespace Mork
{
    public class LMO
    {
        public int metatex_n;

        //public bool placeble = true;

        public int Dropafterdeath = 666;
        public byte DropafterdeathNum = 1;
        public float DropChanse = 1;

        public bool Transparent;

        public string Name = "";

        public int MaxHp = 10;

        public Type MnodePrototype = typeof(MNode);

        public LMO() { }
        public LMO(int metatex)
        {
            metatex_n = metatex;
        }
    }
}