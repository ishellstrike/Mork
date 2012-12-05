using System.Collections.Generic;
using System.Linq;

namespace Mork
{
    public static class KnownIDs
    {
        public static int BrickMaker = 5010;
        public const int error = 666;

        public const int stoneboulder = 5000;
        public const int stonebrick = 5001;
        public const int stonebrickwall = 5002;

        public const int Gabro = 11;
               
        public const int GabroToGranete = 12;
        public const int GrenFranite = 13;

        public const int StorageEntrance = 20000;

        public const int water = 888;
    }

    public class DB_LMO
    {
        public readonly Dictionary<int, LMO> Data = new Dictionary<int, LMO>();

        public DB_LMO()
        {
            Data.Add(0, new LMO(1, true, "", "", "", ""));
            Data.Add(1,
                     new LMO(2, false, "дерево", "дерева", "деревом", "древесный"));

            Data.Add(100,
                     new LMO(3, false, "береза", "березы", "березой", "березовый")
                         {
                             basic_hp = 100,
                         });

            Data.Add(101,
                     new LMO(4, false, "липа", "липы", "липой", "липовый")
                         {
                             basic_hp = 100,
                         });

            Data.Add(102,
                     new LMO(5, false, "ясень", "ясеня", "ясенем", "ясеневый")
                         {
                         });

            Data.Add(500,
                     new LMO(1, true, "источник воды", "источник воды", "источник воды", "источник воды"));
            Data.Add(501,
                     new LMO(3, true, "источник воды", "источник воды", "источник воды", "источник воды"));

            Data.Add(10,
                     new LMO(32, false, "почва", "дерн", "дерн", "дерн")
                         {
                         });

            Data.Add((int)KnownIDs.Gabro,
                     new LMO(31, false, "габбро", "габбро", "габбро",
                                    "из габбро")
                         {
                         });

            Data.Add((int)KnownIDs.GabroToGranete,
                     new LMO(31, false, "темный гранит",
                                    "темного гранита", "темным гранитом", "из темного гранита")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add((int)KnownIDs.GrenFranite,
                     new LMO(29, false, "зеленый гранит",
                                    "зеленого гранита", "зеленым гранитом", "из зеленого гранита")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(14,
                     new LMO(28, false, "базальт", "дерн", "дерн",
                                    "дерн")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(15,
                     new LMO(25, false, "теплый базальт", "дерн",
                                    "дерн", "дерн")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(16,
                     new LMO(24, false, "горячий базальт", "дерн",
                                    "дерн", "дерн")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(17,
                     new LMO(23, false, "раскаленный базальт",
                                    "дерн", "дерн", "дерн")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(18,
                     new LMO(22, false, "магма", "дерн", "дерн", "дерн")
                         {
                         });

            Data.Add(19,
                     new LMO(27, false, "риолит", "дерн", "дерн",
                                    "дерн")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(20,
                     new LMO(30, false, "белый гранит", "дерн",
                                    "дерн", "дерн")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(12345,
                     new LMO(45, false, "бланк", "бланк", "бланк",
                                    "бланк")
                         {
                         });

            Data.Add(22,
                     new LMO(20, false, "аргиллит", "бланк", "бланк",
                                    "бланк")
                         {
                         });

            Data.Add(23,
                     new LMO(2, false, "горец", "дерна", "дерном",
                                    "дерн")
                         {
                         });

            Data.Add(24,
                     new LMO(3, false, "таволга", "дерна",
                                    "дерном", "дерн")
                         {
                         });

            Data.Add(25,
                     new LMO(4, false, "мята", "дерна", "дерном",
                                    "дерн")
                         {
                         });

            Data.Add(26,
                     new LMO(5, false, "черный осот", "дерна",
                                    "дерном", "дерн")
                         {
                         });

            Data.Add(27,
                     new LMO(2, false, "полевица", "дерна",
                                    "дерном", "дерн")
                         {
                         });

            Data.Add(28,
                     new LMO(6, false, "ковыль", "дерна", "дерном", "дерн")
                         {
                         });

            Data.Add(1000,
                     new LMO(3, false, "склад", "дерна", "дерном", "дерн"));
            {

                Data.ElementAt(Data.Count - 1).Value.walkable = true;
            }


            Data.Add(30,
                     new LMO(6, false, "снег", "снега", "снегом", "снежный")
                         {
                         });

            Data.Add(31,
                     new LMO(4, false, "снег", "снега", "снегом", "снежный")
                         {
                         });

            Data.Add(32,
                     new LMO(7, false, "спорыш", "", "", "")
                         {
                         });

            Data.Add(33,
                     new LMO(4, false, "иван чай", "", "", "")
                         {
                         });

            Data.Add(34,
                     new LMO(7, false, "клевер", "", "", "")
                         {
                         });

            Data.Add(35,
                     new LMO(8, false, "зверобой", "", "", "")
                     {

                         });

            Data.Add(36,
                     new LMO(4, false, "белоголов", "", "", "")
                         {
                         });

            Data.Add(37,
                     new LMO(33, false, "песок", "", "", "")
                         {
          
                         });

            Data.Add(103,
                     new LMO(5, false, "ель", "липы", "липой", "липовый")
                         {
                         });

            Data.Add(104,
                     new LMO(3, false, "сосна", "липы", "липой", "липовый")
                         {
                         });

            Data.Add(105,
                     new LMO(4, false, "пихта", "липы", "липой", "липовый")
                         {
                         });

            Data.Add(106,
                     new LMO(1, false, "кедр", "липы", "липой", "липовый")
                         {
                         });

            Data.Add(107,
                     new LMO(7, false, "лиственница", "липы", "липой",
                                    "липовый")
                         {
                         });

            Data.Add(108,
                     new LMO(3, false, "дуб", "липы", "липой", "липовый")
                         {
                         });

            Data.Add(109,
                     new LMO(6, false, "бук", "липы", "липой", "липовый"));
            {

            }

            Data.Add(110,
                     new LMO(3, false, "клен", "липы", "липой", "липовый"));
            {

            }

            Data.Add(800,
                     new LMO(24, false, "зеленый турмалин", "", "", ""));
            {
                
            }


            Data.Add(801,
                     new LMO(24, false, "синий турмалин", "", "", ""));
            {
               
            }

            Data.Add(802,
                     new LMO(24, false, "красный турмалин", "", "", ""));
            {
               
            }

            Data.Add(803,
                     new LMO(24, false, "прозрачный турмалин", "", "", ""));
            {
              
            }

            Data.Add(804,
                     new LMO(24, false, "черный турмалин", "", "", ""));
            {
               
            }

            Data.Add(805,
                     new LMO(24, false, "желтый турмалин", "", "", ""));
            {
              
            }

            Data.Add(806,
                     new LMO(24, false, "коричневый турмалин", "", "", ""));
            {
                
            }

            Data.Add(807, new LMO(30, false, "мрамор", "", "", "")
            {

                dropafterdeath = (int)KnownIDs.stoneboulder,
                dropafterdeath_num = 2
            });

            Data.Add(808, new LMO(2, false, "галенит", "", "", ""));
       

            Data.Add(809, new LMO(3, false, "сфалерит", "", "", "")
            {
                
                dropafterdeath = (int) KnownIDs.stoneboulder,
                dropafterdeath_num = 2
            });

            Data.Add(810, new LMO(5, false, "серебряная руда", "", "", ""));
         

            Data.Add(811, new LMO(6, false, "руда олова", "", "", ""));
           

            Data.Add(812, new LMO(7, false, "висмутовая руда", "", "", ""));
         

            Data.Add(813, new LMO(5, false, "криолит", "", "", ""));
           

            Data.Add(814, new LMO(4, false, "мятлик", "дерна", "дерном", "дерн"));
 

            Data.Add(815, new LMO(6, false, "сланец", "дерна", "дерном", "дерн"));
  

            Data.Add(816, new LMO(3, false, "гнейс", "дерна", "дерном", "дерн"));
           

            Data.Add(817, new LMO(5, false, "красный циркон", "", "", ""));
         

            Data.Add(818, new LMO(4, false, "прозрачный циркон", "", "", ""));
            

            Data.Add(819, new LMO(7, false, "черный циркон", "", "", ""));
       

            Data.Add(820, new LMO(5, false, "желтый циркон", "", "", ""));
          

            Data.Add(821, new LMO(6, false, "коричневый циркон", "", "", ""));
       

            Data.Add(822, new LMO(4, true, "мастерская плотника", "овая", "", "")
            {
               
            });

            Data.Add(KnownIDs.water, new LMO(34, true, "вода", "ая", "", ""));
       
            Data.Last().Value.walkable = false;

            Data.Add(824, new LMO(3, false, "ортоклаз", "", "", "")
            {
              
                dropafterdeath = (int) KnownIDs.stoneboulder
            });

            Data.Add(825, new LMO(5, false, "микроклин", "", "", "")
            {
               
                dropafterdeath = (int) KnownIDs.stoneboulder
            });

            Data.Add((int)KnownIDs.error, new LMO(35, false, "ошибка id", "","",""));
        

            Data.Add((int) KnownIDs.stoneboulder, new LMO(36, false, "булыжник", "", "", "")
            {
                placeble = false
            });

            Data.Add((int)KnownIDs.stonebrickwall, new LMO(43, false, "каменная кладка", "", "", "")
            {
                placeble = true,
                basic_hp = 1000,
                dropafterdeath = KnownIDs.stonebrick,
                dropafterdeath_num = 20
            });

            Data.Add((int)KnownIDs.stonebrick, new LMO(44, false, "каменный кирпич", "", "", "")
            {
                placeble = false,
            });

            Data.Add(KnownIDs.StorageEntrance, new LMO(1, false, "сортировочная станция", "", "", "")
            {
                placeble = true,
                activeblock = true
            });

            Data.Add(KnownIDs.BrickMaker, new LMO(1, false, "камнедробитель", "", "", "")
            {
                placeble = true,
                activeblock = true
            });
        }
    }
}