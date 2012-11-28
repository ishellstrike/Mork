using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mork.Generators
{
    public class Civilizations
    {
        Random rnd = new Random();
        List<Civilization> n = new List<Civilization>();
        public Civilizations()
        {

        }

        public void GenerateAllRandom()
        {
            n.Add(new Civilization());
            n[n.Count - 1].Race = (Races) rnd.Next(0, 4);
        }
    }
    public class Civilization
    {
        public string Name = " ";
        public Religion Religion = new Religion();
        public Gender Arhat = Gender.Male;

        public int Population = 0;
        public Races Race = Races.Human;

        public List<HistoricalFigure> Hf = new List<HistoricalFigure>();

        public string StrArhatR()
        {
            switch (Arhat)
            {
                case Gender.Male:
                    return "Патриархат";
                case Gender.Fermale:
                    return "Матриархат";
                case Gender.Essence:
                    return "Равнорпавие";
                default: 
                    return "";
            }
        }
        public string StrArhatP()
        {
            switch (Arhat)
            {
                case Gender.Male:
                    return "Патриархальное";
                case Gender.Fermale:
                    return "Матриархальное";
                case Gender.Essence:
                    return "Равнорпавное";
                default:
                    return "";
            }
        }
    }

    public class HistoricalFigure
    {
        public string Name = " ";
        public string SName = " ";
    }

    public enum ReligionType
    {
        Monoteistic,
        Panteistic,
        Nonteistic
    }
    public enum Gender
    {
        Male,
        Fermale,
        Essence
    }
    public enum Elements
    {
        HaveNot,
        Fire,
        Water,
        Blood,
        Light,
        Dark,
        Wind,
        Pain
    }
    public enum HistoricalRole
    {
        Leader,
        Champion,
        Prophet
    }

    public class Religion
    {
        public string name = " ";
        public ReligionType type = ReligionType.Nonteistic;
        public List<God> maingod = new List<God>();
        public List<God> secondgod = new List<God>();

        Elements mainelement = Elements.HaveNot;

        public string StrMainElementR()
        {
            switch (mainelement)
            {
                case Elements.Blood:
                    return "кровь";
                case Elements.Dark:
                    return "тьма";
                case Elements.Fire:
                    return "огонь";
                case Elements.Light:
                    return "свет";
                case Elements.Pain:
                    return "боль";
                case Elements.Water:
                    return "вода";
                case Elements.Wind:
                    return "ветер";
            }
            return "";
        }
        public string StrMainElementP()
        {
            switch (mainelement)
            {
                case Elements.Blood:
                    return "крови";
                case Elements.Dark:
                    return "тьмы";
                case Elements.Fire:
                    return "огоня";
                case Elements.Light:
                    return "света";
                case Elements.Pain:
                    return "боли";
                case Elements.Water:
                    return "воды";
                case Elements.Wind:
                    return "ветра";
            }
            return "";
        }
        public string StrMainElementD()
        {
            switch (mainelement)
            {
                case Elements.Blood:
                    return "крови";
                case Elements.Dark:
                    return "тьме";
                case Elements.Fire:
                    return "огоню";
                case Elements.Light:
                    return "свету";
                case Elements.Pain:
                    return "боли";
                case Elements.Water:
                    return "воде";
                case Elements.Wind:
                    return "ветру";
            }
            return "";
        }

        public string StrTypeP()
        {
            if (type == ReligionType.Monoteistic) return "монотеистическая";
            if (type == ReligionType.Panteistic) return "пантеистическая";
            if (type == ReligionType.Nonteistic) return "идеалистическая";
            return "";
        }
    }
    public class God
    {
        public string name = " ";
        public Elements element = Elements.HaveNot;
        public Gender gender = Gender.Essence;
    }
}
