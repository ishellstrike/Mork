namespace Mork
{
    public class LMO
    {
        public int metatex_n;

        public bool walkable = true;
        public bool is_tree;
        public bool is_rock;
        public bool createfloor;

        public OnStoreID dropafterdeath = OnStoreID.Nothing;
        public byte dropafterdeath_num = 1;

        public MaterialID base_material = MaterialID.Basic;

        public bool using_material = true;

        public string I_name = "";
        public string R_name = "";
        public string T_name = "";
        public string P_name = "";

        public int basic_hp = 10;



        public LMO() { }
        public LMO(int _metatex, bool _walkable, string _I_name, string _R_name, string _T_name, string _P_name)
        {
            metatex_n = _metatex;
            walkable = _walkable;

            I_name = _I_name;
            R_name = _R_name;
            T_name = _T_name;
            P_name = _P_name;
        }
    }
}