using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mork.Local_Map;

namespace Mork.DataParser {
    public static class LMO_Parser {
        public static List<KeyValuePair<int, object>> LMOParser(string s) {
            var temp = new List<KeyValuePair<int, object>>();

            s = Regex.Replace(s, "//.*", "");

            string[] blocks = s.Split('~');
            foreach (string block in blocks) {
                if (block.Length != 0) {
                    string[] lines = Regex.Split(block, "\n");
                    string[] header = lines[0].Split(',');

                    temp.Add(new KeyValuePair<int, object>(Convert.ToInt32(header[1]),
                                                           new LMO(Convert.ToInt32(header[2]))));
                    KeyValuePair<int, object> cur = temp.Last();
                    switch (header[0]) {
                        default:
                            ((LMO) cur.Value).MnodePrototype = typeof (MNode);
                            break;
                    }

                    for (int i = 1; i < lines.Length; i++) {
                        if (lines[i].StartsWith("name=")) {
                            string extractedstring = GameDataParserBacis.stringextractor.Match(lines[i]).ToString();
                            ((LMO) cur.Value).Name = extractedstring.Substring(1, extractedstring.Length - 2);
                        }
                        if (lines[i].StartsWith("transparent")) {
                            ((LMO) cur.Value).Transparent = true;
                        }
                        if (lines[i].StartsWith("notblock")) {
                            ((LMO) cur.Value).NotBlock = true;
                        }
                        if (lines[i].StartsWith("max_hp=")) {
                            string extracterint = GameDataParserBacis.intextractor.Match(lines[i]).ToString();
                            ((LMO) cur.Value).MaxHP = Convert.ToInt32(extracterint);
                        }
                        if (lines[i].StartsWith("description=")) {
                            string extractedstring = GameDataParserBacis.stringextractor.Match(lines[i]).ToString();
                            ((LMO) cur.Value).Description = extractedstring.Substring(1, extractedstring.Length - 2);
                        }
                    }
                }
            }
            return temp;
        }
    }
}