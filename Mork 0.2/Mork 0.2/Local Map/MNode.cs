using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Mork.Local_Map
{
    [Serializable]////////////////////////////////////////////////////////////////////////
    public class MNode
    {
        public int blockID;
        public bool subterrain = true;
        public bool explored = true;
        public float health = 10;
    }

    public class MActive : MNode
    {
        public Vector3 pos;
    }

    public class MStorage : MActive
    {
        
    }
}