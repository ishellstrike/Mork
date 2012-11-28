using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mork
{
    public class OnStoreData
    {
        public Texture2D[] tex;

        public Color col = new Color(255, 255, 255);

        public string I_name = "";
        public string R_name = "";
        public string T_name = "";
        public string P_name = "";

        public OnStoreData(Texture2D[] _tex, string _I_name, string _R_name, string _T_name, string _P_name)
        {
            tex = _tex;

            I_name = _I_name;
            R_name = _R_name;
            T_name = _T_name;
            P_name = _P_name;
        }
    }
}