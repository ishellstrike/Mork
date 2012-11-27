using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Mork
{
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
}