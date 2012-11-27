using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.CSharp;

namespace Mork
{

    public enum MaterialID
    {
        Basic,
        Sand,
        WhiteSand,
        SandStone,
        Dirt,
        BlackDirt,
        Water,
        Ice,
        Steam,
        Elovi,
        Sosnovi,
        Dubovi,
        Lipovi,
        Berezovi,
        Yasenivi,
        Kedrovi,
        Bukovi,
        Klenovi,
        Pihtovi,
        Listvenizevi,
        Gabbrovi,
        WGranitovi,
        GGranitovi,
        Basaltovi,
        Ryolitovi,
        Sfaleritovi,
        Slatovi,
        Mramorni,
        Microclini,
        Ortoclazi,

        //-----incomplete-----
        /*Coralovi,

        Stalni,
        Medni,
        Bronzi,
        Svinzovi,
        Olovani,
        Shelkovi,
        Linyani,
        Silverni,
        Zolotoi,
        Platinovi,
        Aluminievi,
        Jelezni,
        RjaviJelezni,

        Grafitni,
        Gipsi,*/

    }
    public enum MaterialTypeID
    {
        Unknown,
        Stone,
        Wood,
        Metal,
        Gem,
        Fluid,
        Flowing,
        Gas
    }
    public class Material
    {
        public int BasicCost = 1;
        public int BasicDensity = 1;
        public int MeltT = 1000;
        public int BoilT = 2000;

        public MaterialTypeID typeid = MaterialTypeID.Unknown;

        public string i_name = "стандарт";
        public string ii_name = "стандарта";
        public string iii_name = "стандартом";

        public Color color = Color.White;

        public Material(string _1_name, string _2_name, string _3_name, int _melt, int _boil, MaterialTypeID _tid)
        {
            i_name = _1_name;
            ii_name = _2_name;
            iii_name = _3_name;
            MeltT = _melt;
            BoilT = _boil;
            typeid = _tid;
        }
    }
    public class DBMaterial
    {
        public readonly Dictionary<MaterialID, Material> Data = new Dictionary<MaterialID, Material>();

        public DBMaterial()
        {
            Data.Add(MaterialID.Basic, new Material("", "", "", 10000, 10000, MaterialTypeID.Unknown));
            Data.Add(MaterialID.Sand, new Material("песча", "песка", "песком", 1710, 10000, MaterialTypeID.Flowing) { color = Color.Yellow });

            Data.Add(MaterialID.Water, new Material("водн", "воды", "водой", 0, 100, MaterialTypeID.Fluid) { color = Color.LightBlue });

            Data.Add(MaterialID.Ice, new Material("ледян", "льда", "льдом", 0, 100, MaterialTypeID.Stone));
            Data.Add(MaterialID.Steam, new Material("паров", "пара", "паром", 0, 100, MaterialTypeID.Gas));

            Data.Add(MaterialID.Berezovi, new Material("березов", "березов", "березов", 0, 0, MaterialTypeID.Wood) { color = Color.LightYellow });

            Data.Add(MaterialID.Elovi, new Material("елов", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.Brown });

            Data.Add(MaterialID.Bukovi, new Material("буков", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.Yellow });

            Data.Add(MaterialID.Kedrovi, new Material("кедров", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.DarkGoldenrod });

            Data.Add(MaterialID.Dubovi, new Material("дубов", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.Chocolate });

            Data.Add(MaterialID.Klenovi, new Material("кленов", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.Yellow });

            Data.Add(MaterialID.Lipovi, new Material("липов", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.Wheat });

            Data.Add(MaterialID.Listvenizevi, new Material("лиственицев", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.LightSalmon });

            Data.Add(MaterialID.Pihtovi, new Material("пихтов", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.Wheat });

            Data.Add(MaterialID.Sosnovi, new Material("соснов", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.LightSalmon });

            Data.Add(MaterialID.Yasenivi, new Material("ясенев", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.Wheat });

            Data.Add(MaterialID.Gabbrovi, new Material("габбровы", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.DarkGray });

            Data.Add(MaterialID.GGranitovi, new Material("гранитн", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.DarkSeaGreen });

            Data.Add(MaterialID.WGranitovi, new Material("гранитн", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.LightGray });

            Data.Add(MaterialID.Sfaleritovi, new Material("сфалеритов", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.Cornsilk });

            Data.Add(MaterialID.Ryolitovi, new Material("риолитов", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.LightGray });

            Data.Add(MaterialID.Slatovi, new Material("сланцев", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.Wheat });

            Data.Add(MaterialID.Mramorni, new Material("мраморн", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.White });

            Data.Add(MaterialID.Microclini, new Material("микроклинов", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.LightBlue });

            Data.Add(MaterialID.Ortoclazi, new Material("ортоклазов", "еловые", "еловая", 0, 0, MaterialTypeID.Wood) { color = Color.LightYellow });
        }
    }
    public class ObjectData
    {
        public int metatex_n;

        public bool walkable = true;
        public bool is_tree;
        public bool is_rock;
        public bool createfloor;

        public OnStoreID dropafterdeath = OnStoreID.Nothing;
        public byte dropafterdeath_num = 1;

        public MaterialID base_material = MaterialID.Basic;

        public bool using_material = true;

        public string I_name = "";
        public string R_name = "";
        public string T_name = "";
        public string P_name = "";

        //public Color col = new Color(255, 255, 255);

        public int basic_hp = 10;



        public ObjectData() { }
        public ObjectData(int _metatex, bool _walkable, string _I_name, string _R_name, string _T_name, string _P_name)
        {
            metatex_n = _metatex;
            walkable = _walkable;

            I_name = _I_name;
            R_name = _R_name;
            T_name = _T_name;
            P_name = _P_name;
        }
    }
    public class DBObject
    {
        public readonly Dictionary<int, ObjectData> Data = new Dictionary<int, ObjectData>();

        public DBObject()
        {
            Data.Add(0, new ObjectData(1, true, "", "", "", ""));
            Data.Add(1,
                     new ObjectData(2, false, "дерево", "дерева", "деревом", "древесный"));

            Data.Add(100,
                     new ObjectData(3, false, "береза", "березы", "березой", "березовый")
                         {
                             is_tree = true,
                             basic_hp = 100,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Berezovi
                         });

            Data.Add(101,
                     new ObjectData(4, false, "липа", "липы", "липой", "липовый")
                         {
                             is_tree = true,
                             basic_hp = 100,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Lipovi
                         });

            Data.Add(102,
                     new ObjectData(5, false, "ясень", "ясеня", "ясенем", "ясеневый")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Yasenivi
                         });

            Data.Add(500,
                     new ObjectData(1, true, "источник воды", "источник воды", "источник воды", "источник воды"));
            Data.Add(501,
                     new ObjectData(3, true, "источник воды", "источник воды", "источник воды", "источник воды"));

            Data.Add(400,
                     new ObjectData(4, false, "дерн", "дерна", "дерном", "дерн")
                         {
                             createfloor = true,
                             is_rock = true
                         });

            Data.Add(401,
                     new ObjectData(1, false, "дерн", "дерна", "дерном", "дерн")
                         {
                             createfloor = true,
                             is_rock = true
                         });

            Data.Add(402,
                     new ObjectData(4, false, "дерн", "дерна", "дерном", "дерн")
                         {
                             createfloor = true,
                             is_rock = true
                         });

            Data.Add(10,
                     new ObjectData(32, false, "почва", "дерн", "дерн", "дерн")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

        Data.Add(11,
                     new ObjectData(1, false, "габбро", "габбро", "габбро",
                                    "из габбро")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.Gabbrovi,
                         });

            Data.Add(12,
                     new ObjectData(5, false, "темный гранит",
                                    "темного гранита", "темным гранитом", "из темного гранита")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.GGranitovi,
                         });

            Data.Add(13,
                     new ObjectData(6, false, "зеленый гранит",
                                    "зеленого гранита", "зеленым гранитом", "из зеленого гранита")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.GGranitovi
                         });

            Data.Add(14,
                     new ObjectData(4, false, "базальт", "дерн", "дерн",
                                    "дерн")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.Basaltovi,
                         });

            Data.Add(15,
                     new ObjectData(6, false, "теплый базальт", "дерн",
                                    "дерн", "дерн")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.Basaltovi,
                         });

            Data.Add(16,
                     new ObjectData(3, false, "горячий базальт", "дерн",
                                    "дерн", "дерн")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.Basaltovi,
                         });

            Data.Add(17,
                     new ObjectData(6, false, "раскаленный базальт",
                                    "дерн", "дерн", "дерн")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.Basaltovi,
                         });

            Data.Add(18,
                     new ObjectData(7, false, "магма", "дерн", "дерн", "дерн")
                         {
                             createfloor = true,
                         });

            Data.Add(19,
                     new ObjectData(8, false, "риолит", "дерн", "дерн",
                                    "дерн")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.Ryolitovi,
                         });

            Data.Add(20,
                     new ObjectData(30, false, "белый гранит", "дерн",
                                    "дерн", "дерн")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.WGranitovi,
                         });

            Data.Add(12345,
                     new ObjectData(36, false, "бланк", "бланк", "бланк",
                                    "бланк")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(22,
                     new ObjectData(20, false, "аргиллит", "бланк", "бланк",
                                    "бланк")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(23,
                     new ObjectData(2, false, "горец", "дерна", "дерном",
                                    "дерн")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(24,
                     new ObjectData(3, false, "таволга", "дерна",
                                    "дерном", "дерн")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(25,
                     new ObjectData(4, false, "мята", "дерна", "дерном",
                                    "дерн")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(26,
                     new ObjectData(5, false, "черный осот", "дерна",
                                    "дерном", "дерн")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(27,
                     new ObjectData(2, false, "полевица", "дерна",
                                    "дерном", "дерн")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(28,
                     new ObjectData(6, false, "ковыль", "дерна", "дерном", "дерн")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(1000,
                     new ObjectData(3, false, "склад", "дерна", "дерном", "дерн"));
            {

                Data.ElementAt(Data.Count - 1).Value.createfloor = true;
                Data.ElementAt(Data.Count - 1).Value.is_rock = false;
                Data.ElementAt(Data.Count - 1).Value.walkable = true;
            }


            Data.Add(30,
                     new ObjectData(6, false, "снег", "снега", "снегом", "снежный")
            {
                createfloor = true,
                is_rock = true,
            });

            Data.Add(31,
                     new ObjectData(4, false, "снег", "снега", "снегом", "снежный")
            {
                createfloor = true,
                is_rock = true,
            });

            Data.Add(32,
                     new ObjectData(7, false, "спорыш", "", "", "")
            {
                createfloor = true,
                is_rock = true,
            });

            Data.Add(33,
                     new ObjectData(4, false, "иван чай", "", "", "")
                        {
                            createfloor = true,
                            is_rock = true,
                        });

            Data.Add(34,
                     new ObjectData(7, false, "клевер", "", "", "")
                        {
                            createfloor = true,
                            is_rock = true,
                        });

            Data.Add(35,
                     new ObjectData(8, false, "зверобой", "", "", "")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(36,
                     new ObjectData(4, false, "белоголов", "", "", "")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(37,
                     new ObjectData(33, false, "песок", "", "", "")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(103,
                     new ObjectData(5, false, "ель", "липы", "липой", "липовый")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Elovi,
                         });

            Data.Add(104,
                     new ObjectData(3, false, "сосна", "липы", "липой", "липовый")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Sosnovi,
                         });

            Data.Add(105,
                     new ObjectData(4, false, "пихта", "липы", "липой", "липовый")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Pihtovi,
                         });

            Data.Add(106,
                     new ObjectData(1, false, "кедр", "липы", "липой", "липовый")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Kedrovi,
                         });

            Data.Add(107,
                     new ObjectData(7, false, "лиственница", "липы", "липой",
                                    "липовый")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Listvenizevi,
                         });

            Data.Add(108,
                     new ObjectData(3, false, "дуб", "липы", "липой", "липовый")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Dubovi,
                         });

            Data.Add(109,
                     new ObjectData(6, false, "бук", "липы", "липой", "липовый"));
            {
                Data.ElementAt(Data.Count - 1).Value.is_tree = true;

                Data.ElementAt(Data.Count - 1).Value.dropafterdeath = OnStoreID.WoodLog;
                Data.ElementAt(Data.Count - 1).Value.base_material = MaterialID.Bukovi;
            }

            Data.Add(110,
                     new ObjectData(3, false, "клен", "липы", "липой", "липовый"));
            {
                Data.ElementAt(Data.Count - 1).Value.is_tree = true;

                Data.ElementAt(Data.Count - 1).Value.dropafterdeath = OnStoreID.WoodLog;
                Data.ElementAt(Data.Count - 1).Value.base_material = MaterialID.Klenovi;
            }

            Data.Add(800,
                     new ObjectData(2, false, "зеленый турмалин", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }


            Data.Add(801,
                     new ObjectData(7, false, "синий турмалин", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }

            Data.Add(802,
                     new ObjectData(4, false, "красный турмалин", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }

            Data.Add(803,
                     new ObjectData(3, false, "прозрачный турмалин", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }

            Data.Add(804,
                     new ObjectData(7, false, "черный турмалин", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }

            Data.Add(805,
                     new ObjectData(4, false, "желтый турмалин", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }

            Data.Add(806,
                     new ObjectData(3, false, "коричневый турмалин", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }

            Data.Add(807, new ObjectData(6, false, "мрамор", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;
                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
                Data.ElementAt(Data.Count - 1).Value.dropafterdeath = OnStoreID.StoneBoulder;
                Data.ElementAt(Data.Count - 1).Value.base_material = MaterialID.Mramorni;
            }

            Data.Add(808, new ObjectData(2, false, "галенит", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(809, new ObjectData(3, false, "сфалерит", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            Data.ElementAt(Data.Count - 1).Value.dropafterdeath = OnStoreID.StoneBoulder;
            Data.ElementAt(Data.Count - 1).Value.base_material = MaterialID.Sfaleritovi;

            Data.Add(810, new ObjectData(5, false, "серебряная руда", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(811, new ObjectData(6, false, "руда олова", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(812, new ObjectData(7, false, "висмутовая руда", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(813, new ObjectData(5, false, "криолит", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(814, new ObjectData(4, false, "мятлик", "дерна", "дерном", "дерн"));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(815, new ObjectData(6, false, "сланец", "дерна", "дерном", "дерн"));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(816, new ObjectData(3, false, "гнейс", "дерна", "дерном", "дерн"));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(817, new ObjectData(5, false, "красный циркон", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(818, new ObjectData(4, false, "прозрачный циркон", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(819, new ObjectData(7, false, "черный циркон", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(820, new ObjectData(5, false, "желтый циркон", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(821, new ObjectData(6, false, "коричневый циркон", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(822, new ObjectData(4, true, "мастерская плотника", "овая", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = false;
            Data.ElementAt(Data.Count - 1).Value.using_material = true;

            Data.Add(888, new ObjectData(34, true, "вода", "ая", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = false;
            Data.ElementAt(Data.Count - 1).Value.walkable = false;

            Data.Add(824, new ObjectData(3, false, "ортоклаз", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            Data.ElementAt(Data.Count - 1).Value.dropafterdeath = OnStoreID.StoneBoulder;
            Data.ElementAt(Data.Count - 1).Value.base_material = MaterialID.Ortoclazi;

            Data.Add(825, new ObjectData(5, false, "микроклин", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            Data.ElementAt(Data.Count - 1).Value.dropafterdeath = OnStoreID.StoneBoulder;
            Data.ElementAt(Data.Count - 1).Value.base_material = MaterialID.Microclini;

            Data.Add(666, new ObjectData(35, false, "ошибка id", "","",""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
        }
    }

    public enum OnStoreID
    {
        Nothing,
        WoodLog,
        StoneBoulder
    }
    public class OnStoreData
    {
        public Texture2D[] tex;

        public Color col = new Color(255, 255, 255);

        public string I_name = "";
        public string R_name = "";
        public string T_name = "";
        public string P_name = "";

        public OnStoreData(Texture2D[] _tex, string _I_name, string _R_name, string _T_name, string _P_name)
        {
            tex = _tex;

            I_name = _I_name;
            R_name = _R_name;
            T_name = _T_name;
            P_name = _P_name;
        }
    }
    public class DBOnStore
    {
        public Dictionary<OnStoreID, OnStoreData> data = new Dictionary<OnStoreID, OnStoreData>();

        public DBOnStore()
        {
            data.Add(OnStoreID.Nothing, new OnStoreData(null, "ничего", "", "", ""));
            data.Add(OnStoreID.WoodLog, new OnStoreData(Main.onstore_tex[Main.OnStoreTexes.Wood_log], "бревно", "ое", "", ""));
            data.Add(OnStoreID.StoneBoulder, new OnStoreData(Main.onstore_tex[Main.OnStoreTexes.Stone], "валун", "ый", "", ""));
        }
    }


    //public enum FloarID
    //{
    //    None,
    //    Grass1,
    //    Grass2,
    //    Grass3,
    //    Grass4,
    //    Grass5,
    //    Sandwall,
    //    Sand,
    //    Dirt,
    //    Ground,
    //    Water1,
    //    Water2,
    //    Water3,
    //    Water4,
    //    Water5,
    //    Water6,
    //    Water7,
    //    Water8,
    //    Water9,
    //    Water10,
    //    Water11,
    //    Water12,
    //    Water13,
    //}
    //public class Floar
    //{
    //    public Texture2D tex;
    //    public bool walkable = true;

    //    public string I_name = "";
    //    public string R_name = "";
    //    public string T_name = "";
    //    public string P_name = "";

    //    public Floar() { }
    //    public Floar(Texture2D _tex, string _I_name, string _R_name, string _T_name, string _P_name)
    //    {
    //        tex = _tex;

    //        I_name = _I_name;
    //        R_name = _R_name;
    //        T_name = _T_name;
    //        P_name = _P_name;
    //    }
    //}
    //public class DBFloar
    //{
    //    public Dictionary<FloarID, Floar> data = new Dictionary<FloarID, Floar>();

    //    public DBFloar()
    //    {
    //        data.Add(FloarID.None, new Floar(Main.ground_tex[Main.GroundTexes.None], "пустота", "пустоты", "пустотой", "пустой"));
    //        data.Add(FloarID.Dirt, new Floar(Main.ground_tex[Main.GroundTexes.iso_dirt1], "грязь", "грязи", "грязью", "грязевой"));
    //        data.Add(FloarID.Ground, new Floar(Main.ground_tex[Main.GroundTexes.iso_ground_1], "земля", "земли", "землей", "земляной"));
    //        data.Add(FloarID.Grass1, new Floar(Main.ground_tex[Main.GroundTexes.iso_grass_1], "трава", "травы", "травой", "травяной"));
    //        data.Add(FloarID.Grass2, new Floar(Main.ground_tex[Main.GroundTexes.iso_grass_2], "трава", "травы", "травой", "травяной"));
    //        data.Add(FloarID.Grass3, new Floar(Main.ground_tex[Main.GroundTexes.iso_grass_3], "трава", "травы", "травой", "травяной"));
    //        data.Add(FloarID.Grass4, new Floar(Main.ground_tex[Main.GroundTexes.iso_grass_4], "трава", "травы", "травой", "травяной"));
    //        data.Add(FloarID.Grass5, new Floar(Main.ground_tex[Main.GroundTexes.iso_grass_5], "трава", "травы", "травой", "травяной"));
    //        data.Add(FloarID.Sandwall, new Floar(Main.ground_tex[Main.GroundTexes.iso_sand1], "песок_", "песок_", "песок_", "песок_"));
    //        data.Add(FloarID.Sand, new Floar(Main.ground_tex[Main.GroundTexes.iso_sand1], "песок", "песка", "песком", "песчаный"));
    //          data.Add(FloarID.Water1, new Floar(Main.ground_tex[Main.GroundTexes.iso_water_base1], "вода", "воды", "водой", "водяной"));
    //          data.Add(FloarID.Water2, new Floar(Main.ground_tex[Main.GroundTexes.iso_water_base2], "вода", "воды", "водой", "водяной"));
    //          data.Add(FloarID.Water3, new Floar(Main.ground_tex[Main.GroundTexes.iso_water_base3], "вода", "воды", "водой", "водяной"));
    //          data.Add(FloarID.Water4, new Floar(Main.ground_tex[Main.GroundTexes.iso_water_base4], "вода", "воды", "водой", "водяной"));
    //          data.Add(FloarID.Water5, new Floar(Main.ground_tex[Main.GroundTexes.iso_water_base5], "вода", "воды", "водой", "водяной"));
    //          data.Add(FloarID.Water6, new Floar(Main.ground_tex[Main.GroundTexes.iso_water_base6], "вода", "воды", "водой", "водяной"));
    //          data.Add(FloarID.Water7, new Floar(Main.ground_tex[Main.GroundTexes.iso_water_base7], "вода", "воды", "водой", "водяной"));
    //          data.Add(FloarID.Water8, new Floar(Main.ground_tex[Main.GroundTexes.iso_water_base8], "вода", "воды", "водой", "водяной"));
    //          data.Add(FloarID.Water9, new Floar(Main.ground_tex[Main.GroundTexes.iso_water_base9], "вода", "воды", "водой", "водяной"));
    //        data.Add(FloarID.Water10, new Floar(Main.ground_tex[Main.GroundTexes.iso_water_base10], "вода", "воды", "водой", "водяной"));
    //        data.Add(FloarID.Water11, new Floar(Main.ground_tex[Main.GroundTexes.iso_water_base11], "вода", "воды", "водой", "водяной"));
    //        data.Add(FloarID.Water12, new Floar(Main.ground_tex[Main.GroundTexes.iso_water_base12], "вода", "воды", "водой", "водяной"));
    //        data.Add(FloarID.Water13, new Floar(Main.ground_tex[Main.GroundTexes.iso_water_base13], "вода", "воды", "водой", "водяной"));
    //    }
    //}
}
