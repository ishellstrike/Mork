using System;

namespace Mork.Generators
{
    public enum Races
    {
        Human,
/*
        Goblin,

        Elf,
        Gnomes,
        Orcs,
        Demons,
        Nymphs,
        NightCreatures,
        Gods,
        Titans,
        Demigods
 */
    }

    public static class NamesGenerator
    {
        private static readonly string[] HumanNameRot1 = { "Каз", "Оз", "Маз", "Вез", "Кои", "Мер", "Бао" };
        private static readonly string[] HumanNameRot2 = { "дин", "рел", "мал", "вест", "тор", "вил", "дун" };
        private static readonly string[] HumanNameIn = { "о", "а" };
        private static readonly string[] HumanSecondnames = {"Стоун","Блад","Дарк","Фои","Вилоу","Ив","Спайн","Дроуэн","Сивил","Мауен","Ашен","Оукен","Грас","Филд","Боулд","Рокс","Тион","Тиос","Киос","Риол","Брим"};



        static readonly Random rnd = new Random();
        public static string GetAgeName()
        {
            var s = "";
            switch (rnd.Next(0, 12))
            {
                case 0:
                    s = "завоеваний";
                    break;
                case 1:
                    s = "открытий";
                    break;
                case 2:
                    s = "исследований";
                    break;
                case 3:
                    s = "василиска";
                    break;
                case 4:
                    s = "горгоны";
                    break;
                case 5:
                    s = "мантикоры";
                    break;
                case 6:
                    s = "кентавра";
                    break;
                case 7:
                    s = "дракона";
                    break;
                case 8:
                    s = "медузы";
                    break;
                case 9:
                    s = "пламени";
                    break;
                default:
                    s = "легенд";
                    break;
            }
            return s;
        }

        public static string GetRandomName(Races sRaces)
        {
            var s = "";

            if (sRaces == Races.Human)
                s = string.Format("{0}{1}{2}", HumanNameRot1[rnd.Next(0, HumanNameRot1.Length)], HumanNameIn[rnd.Next(0, HumanNameIn.Length)], HumanNameRot2[rnd.Next(0, HumanNameRot2.Length)]);

            return s;
        }

        public static string GetRandomSecondName(Races sRaces)
        {
            string s = "";

            if (sRaces == Races.Human)
                s += string.Format(" {0}", HumanSecondnames[rnd.Next(0, HumanSecondnames.Length)]);

            return s;
        }

        public static string MonthNameI(int n)
        {
            switch (n)
            {
                case 1:
                    return "январь";
                case 2:
                    return "февраль";
                case 3:
                    return "март";
                case 4:
                    return "апрель";
                case 5:
                    return "май";
                case 6:
                    return "июнь";
                case 7:
                    return "июль";
                case 8:
                    return "август";
                case 9:
                    return "сентябрь";
                case 10:
                    return "октябрь";
                case 11:
                    return "ноябрь";
                case 12:
                    return "декабрь";
            }
            return "мартябрь";
        }
        public static string MonthNameR(int n)
        {
            switch (n)
            {
                case 1:
                    return "января";
                case 2:
                    return "февраля";
                case 3:
                    return "марта";
                case 4:
                    return "апреля";
                case 5:
                    return "мая";
                case 6:
                    return "июня";
                case 7:
                    return "июля";
                case 8:
                    return "августа";
                case 9:
                    return "сентября";
                case 10:
                    return "октября";
                case 11:
                    return "ноября";
                case 12:
                    return "декабря";
            }
            return "мартябрь";
        }
    }
}
