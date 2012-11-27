using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mork.Local_Map.Dynamic.Units
{
    class DBUnitInfo
    {
        private Dictionary<int, UnitInfo> data = new Dictionary<int, UnitInfo>();

        public DBUnitInfo()
        {
            data.Add(0, new UnitInfo {basic_mass = 0, meta_tex = 0});
        }
    }
}
