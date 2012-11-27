using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mork.Local_Map.Dynamic.Units.Skills
{
    public class DBSkills
    {
        Dictionary<byte, Skill> data = new Dictionary<byte, Skill>(); 

        public DBSkills()
        {
            data.Add(0, new Skill { name = "ничего" });
            data.Add(1, new Skill { name = "пловец" });
            data.Add(2, new Skill { name = "бегун" });
        }
    }
}
