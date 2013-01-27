using Microsoft.Xna.Framework;

namespace Mork
{
    public class Material
    {
        public int BasicCost = 1;
        public int BasicDensity = 1;
        public int MeltT = 1000;
        public int BoilT = 2000;

        public MaterialTypeID typeid = MaterialTypeID.Unknown;

        public string i_name = "стандарт";
        public string ii_name = "стандарта";
        public string iii_name = "стандартом";

        public Color color = Color.White;

        public Material(string _1_name, string _2_name, string _3_name, int _melt, int _boil, MaterialTypeID _tid)
        {
            i_name = _1_name;
            ii_name = _2_name;
            iii_name = _3_name;
            MeltT = _melt;
            BoilT = _boil;
            typeid = _tid;
        }
    }
}