using System;
using Mork.Local_Map;

namespace Mork {
    public class LMO {
        //public bool placeble = true;

        public string Description = "";
        public float DropChanse = 1;
        public int Dropafterdeath = 666;
        public byte DropafterdeathNum = 1;

        public int MaxHP = 10;
        public int MetatexN;

        public Type MnodePrototype = typeof (MNode);
        public string Name = "";
        public bool NotBlock;
        public bool Transparent;

        public LMO(int metatex) {
            MetatexN = metatex;
        }
    }
}