using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Mork.Local_Map;

namespace Mork.Data_Parser
{
    public static class LMO_Parser
    {
        public static List<KeyValuePair<int, LMO>> ParseString(string s)
        {
            List<KeyValuePair<int, LMO>> temp = new List<KeyValuePair<int, LMO>>();

            Regex stringextractor = new Regex("\".*\"");
            Regex intextractor = new Regex("[0-9]+");

            s = Regex.Replace(s, "//.*\n*", "");

            string[] blocks = s.Split('~');
            foreach (var block in blocks)
            {
                if (block.Length != 0)
                {
                    string[] lines = Regex.Split(block, "\r\n");
                    string[] header = lines[0].Split(',');

                    temp.Add(new KeyValuePair<int, LMO>(Convert.ToInt32(header[1]), new LMO(Convert.ToInt32(header[2]))));
                    var cur = temp.Last();
                    switch (header[0])
                    {
                        default:
                            cur.Value.MnodePrototype = typeof(MNode);
                            break;
                    }

                    for (int i = 1; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith("name="))
                        {
                            string extractedstring = stringextractor.Match(lines[i]).ToString();
                            cur.Value.Name = extractedstring.Substring(1, extractedstring.Length - 2);
                        }
                        if (lines[i].StartsWith("transparent"))
                        {
                            cur.Value.Transparent = true;
                        }
                        if (lines[i].StartsWith("max_hp="))
                        {
                            string extracterint = intextractor.Match(lines[i]).ToString(); 
                            cur.Value.MaxHp = Convert.ToInt32(extracterint);
                        }
                    }
                }
            }
            return temp;
        }

        private static bool NewElPredicate(char c)
        {
            return c == '~';
        }

        public static List<KeyValuePair<int, LMO>> ParseFile(string patch)
        {
            try
            {
                StreamReader sr = new StreamReader(patch);
                string a = sr.ReadToEnd();
                sr.Close();
                return ParseString(a);
            }
            catch (FileNotFoundException)
            {

                return new List<KeyValuePair<int, LMO>>();
            }

        }

        public static List<KeyValuePair<int, LMO>> ParseDirectory(string patch)
        {
            try
            {
                string[] a = Directory.GetFiles(patch, "*.txt");
                List<KeyValuePair<int, LMO>> temp = new List<KeyValuePair<int, LMO>>();
                foreach (var s in a)
                {
                    temp.AddRange(ParseFile(s));
                }
                return temp;
            }
            catch (DirectoryNotFoundException)
            {

                return new List<KeyValuePair<int, LMO>>();
            }

        }
    }
}
