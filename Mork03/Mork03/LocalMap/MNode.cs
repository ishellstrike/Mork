using System;

namespace Mork.Local_Map {
    [Serializable] ////////////////////////////////////////////////////////////////////////
    public class MNode {
        public int BlockID;
        public bool Explored = true;
        public float Health = 10;
        public bool Subterrain = true;
    }

    public class MActive : MNode {}

    public class MStorage : MActive {}

    public class MError : MNode {
        public int preBlockID;

        public MError(int id) {
            preBlockID = id;
            BlockID = 666;
        }
    }
}