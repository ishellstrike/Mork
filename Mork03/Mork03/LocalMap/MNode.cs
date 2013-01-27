using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Mork.Local_Map
{
    [Serializable]////////////////////////////////////////////////////////////////////////
    public class MNode
    {
        public int BlockID;
        public bool Subterrain = true;
        public bool Explored = true;
        public float Health = 10;
    }

    public class MActive : MNode
    {

    }

    public class MStorage : MActive
    {
        
    }

    public class MError : MNode
    {
        public int preBlockID;

        public MError(int id)
        {
            preBlockID = id;
            BlockID = 666;
        }
    }
}