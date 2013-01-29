using System.Collections.Generic;

namespace Mork.Local_Map.Dynamic.Units {
    internal class DBUnitInfo {
        private readonly Dictionary<int, UnitInfo> data = new Dictionary<int, UnitInfo>();

        public DBUnitInfo() {
            data.Add(0, new UnitInfo {basic_mass = 0, meta_tex = 0});
        }
    }
}