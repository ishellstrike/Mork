﻿namespace Mork.Local_Map.Dynamic.Units.Skills {
    public class Skill {
        public byte level;
        public string name;

        public string GetLevel() {
            switch (level) {
                case 0:
                    return "";
                case 1:
                    return "";
                case 2:
                    return "";
                case 3:
                    return "";
                case 4:
                    return "";
            }
            return level.ToString();
        }
    }
}