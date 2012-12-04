using System;
using System.Collections.Generic;

namespace Mork.Local_Map
{
    [Serializable]////////////////////////////////////////////////////////////////////////
    public class MNode
    {
        public int blockID;

        public Dictionary<string, object> tags = new Dictionary<string, object>();

        //public byte water = 0; // 0 - ����� 1 - ������� 2 - ������. 3-13 - ������������ �����. 13+ - ��� ���������

        public bool subterrain = true;

        private byte storing_num = 0;

        public bool explored = true;

        public float health = 10;
    }
}