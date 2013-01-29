using System;

namespace Mork.BadDatabase.Local_Map_Creatures {
    public enum Race {
        Nothing,

        CivilizatedHumanoid,
        Humanoid,

        Animal,
        Beast,
        Pet,

        Insect,

        Fish,
        AngryFish,

        Plant,
        AngryPlant
    }

    public enum LifeType {
        Nothong,

        Undead,
        Ghost,
        Alive,
    }

    public class LMCAbils {}

    public class LMCStats {
        public byte Agility;
        public byte Dodge;
        public byte Strength;
    }

    public class LMC {
        public bool DamageLight;
        public string Description;
        public bool FearDark;
        public bool FearLight;

        public LifeType LifeType;
        public int MaxHP;
        public int MetatexN;
        public string Name;
        public Race Race;
        public bool Wander;

        public TimeSpan life_long, life_short;

        public LMC(int metatex) {
            MetatexN = metatex;
        }
    }
}