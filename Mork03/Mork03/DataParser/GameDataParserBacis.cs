using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Mork.DataParser {
    public class GameDataParserBacis {
        public static Regex stringextractor = new Regex("\".*\"");
        public static Regex intextractor = new Regex("[0-9]+");

        public static List<KeyValuePair<int, object>> ParseFile(string patch,
                                                                Func<string, List<KeyValuePair<int, object>>> parser) {
            try {
                var sr = new StreamReader(patch);
                string a = sr.ReadToEnd();
                sr.Close();
                return parser(a);
            }
            catch (FileNotFoundException) {
                return new List<KeyValuePair<int, object>>();
            }
        }

        public static List<KeyValuePair<int, object>> ParseDirectory(string patch,
                                                                     Func<string, List<KeyValuePair<int, object>>>
                                                                         parser) {
            try {
                string[] a = Directory.GetFiles(patch, "*.txt");
                var temp = new List<KeyValuePair<int, object>>();
                foreach (string s in a) {
                    temp.AddRange(ParseFile(s, parser));
                }
                return temp;
            }
            catch (DirectoryNotFoundException) {
                return new List<KeyValuePair<int, object>>();
            }
        }
    }
}