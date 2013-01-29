using System.Collections.Generic;
using System.IO;
using Mork.DataParser;

namespace Mork.BadDatabase.Local_Map_Creatures {
    public class DB_LMC {
        public readonly Dictionary<int, LMC> Data = new Dictionary<int, LMC>();

        public DB_LMC() {
            string dir = Directory.GetCurrentDirectory() + "\\Content\\Data\\LMC";
            List<KeyValuePair<int, object>> list = GameDataParserBacis.ParseDirectory(dir, LMC_Parser.LMCParser);

            foreach (var lmo in list) {
                Data.Add(lmo.Key, (LMC) lmo.Value);
            }
        }
    }
}