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
            Data.Add(MaterialID.Sand, new Material("�����", "�����", "������", 1710, 10000, MaterialTypeID.Flowing) { color = Color.Yellow });

            Data.Add(MaterialID.Water, new Material("����", "����", "�����", 0, 100, MaterialTypeID.Fluid) { color = Color.LightBlue });

            Data.Add(MaterialID.Ice, new Material("�����", "����", "�����", 0, 100, MaterialTypeID.Stone));
            Data.Add(MaterialID.Steam, new Material("�����", "����", "�����", 0, 100, MaterialTypeID.Gas));

            Data.Add(MaterialID.Berezovi, new Material("�������", "�������", "�������", 0, 0, MaterialTypeID.Wood) { color = Color.LightYellow });

            Data.Add(MaterialID.Elovi, new Material("����", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.Brown });

            Data.Add(MaterialID.Bukovi, new Material("�����", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.Yellow });

            Data.Add(MaterialID.Kedrovi, new Material("������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.DarkGoldenrod });

            Data.Add(MaterialID.Dubovi, new Material("�����", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.Chocolate });

            Data.Add(MaterialID.Klenovi, new Material("������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.Yellow });

            Data.Add(MaterialID.Lipovi, new Material("�����", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.Wheat });

            Data.Add(MaterialID.Listvenizevi, new Material("�����������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.LightSalmon });

            Data.Add(MaterialID.Pihtovi, new Material("������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.Wheat });

            Data.Add(MaterialID.Sosnovi, new Material("������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.LightSalmon });

            Data.Add(MaterialID.Yasenivi, new Material("������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.Wheat });

            Data.Add(MaterialID.Gabbrovi, new Material("��������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.DarkGray });

            Data.Add(MaterialID.GGranitovi, new Material("�������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.DarkSeaGreen });

            Data.Add(MaterialID.WGranitovi, new Material("�������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.LightGray });

            Data.Add(MaterialID.Sfaleritovi, new Material("����������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.Cornsilk });

            Data.Add(MaterialID.Ryolitovi, new Material("��������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.LightGray });

            Data.Add(MaterialID.Slatovi, new Material("�������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.Wheat });

            Data.Add(MaterialID.Mramorni, new Material("�������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.White });

            Data.Add(MaterialID.Microclini, new Material("�����������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.LightBlue });

            Data.Add(MaterialID.Ortoclazi, new Material("����������", "������", "������", 0, 0, MaterialTypeID.Wood) { color = Color.LightYellow });
        }
    }
}