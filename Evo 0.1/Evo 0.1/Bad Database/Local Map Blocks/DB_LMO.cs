using System.Collections.Generic;
using System.Linq;

namespace Mork
{
    public class DB_LMO
    {
        public readonly Dictionary<int, LMO> Data = new Dictionary<int, LMO>();

        public DB_LMO()
        {
            Data.Add(0, new LMO(1, true, "", "", "", ""));
            Data.Add(1,
                     new LMO(2, false, "������", "������", "�������", "���������"));

            Data.Add(100,
                     new LMO(3, false, "������", "������", "�������", "���������")
                         {
                             is_tree = true,
                             basic_hp = 100,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Berezovi
                         });

            Data.Add(101,
                     new LMO(4, false, "����", "����", "�����", "�������")
                         {
                             is_tree = true,
                             basic_hp = 100,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Lipovi
                         });

            Data.Add(102,
                     new LMO(5, false, "�����", "�����", "������", "��������")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Yasenivi
                         });

            Data.Add(500,
                     new LMO(1, true, "�������� ����", "�������� ����", "�������� ����", "�������� ����"));
            Data.Add(501,
                     new LMO(3, true, "�������� ����", "�������� ����", "�������� ����", "�������� ����"));

            Data.Add(400,
                     new LMO(4, false, "����", "�����", "������", "����")
                         {
                             createfloor = true,
                             is_rock = true
                         });

            Data.Add(401,
                     new LMO(1, false, "����", "�����", "������", "����")
                         {
                             createfloor = true,
                             is_rock = true
                         });

            Data.Add(402,
                     new LMO(4, false, "����", "�����", "������", "����")
                         {
                             createfloor = true,
                             is_rock = true
                         });

            Data.Add(10,
                     new LMO(32, false, "�����", "����", "����", "����")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(11,
                     new LMO(1, false, "������", "������", "������",
                                    "�� ������")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.Gabbrovi,
                         });

            Data.Add(12,
                     new LMO(5, false, "������ ������",
                                    "������� �������", "������ ��������", "�� ������� �������")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.GGranitovi,
                         });

            Data.Add(13,
                     new LMO(6, false, "������� ������",
                                    "�������� �������", "������� ��������", "�� �������� �������")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.GGranitovi
                         });

            Data.Add(14,
                     new LMO(4, false, "�������", "����", "����",
                                    "����")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.Basaltovi,
                         });

            Data.Add(15,
                     new LMO(6, false, "������ �������", "����",
                                    "����", "����")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.Basaltovi,
                         });

            Data.Add(16,
                     new LMO(3, false, "������� �������", "����",
                                    "����", "����")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.Basaltovi,
                         });

            Data.Add(17,
                     new LMO(6, false, "����������� �������",
                                    "����", "����", "����")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.Basaltovi,
                         });

            Data.Add(18,
                     new LMO(7, false, "�����", "����", "����", "����")
                         {
                             createfloor = true,
                         });

            Data.Add(19,
                     new LMO(8, false, "������", "����", "����",
                                    "����")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.Ryolitovi,
                         });

            Data.Add(20,
                     new LMO(30, false, "����� ������", "����",
                                    "����", "����")
                         {
                             createfloor = true,
                             is_rock = true,
                             dropafterdeath = OnStoreID.StoneBoulder,
                             base_material = MaterialID.WGranitovi,
                         });

            Data.Add(12345,
                     new LMO(36, false, "�����", "�����", "�����",
                                    "�����")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(22,
                     new LMO(20, false, "��������", "�����", "�����",
                                    "�����")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(23,
                     new LMO(2, false, "�����", "�����", "������",
                                    "����")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(24,
                     new LMO(3, false, "�������", "�����",
                                    "������", "����")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(25,
                     new LMO(4, false, "����", "�����", "������",
                                    "����")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(26,
                     new LMO(5, false, "������ ����", "�����",
                                    "������", "����")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(27,
                     new LMO(2, false, "��������", "�����",
                                    "������", "����")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(28,
                     new LMO(6, false, "������", "�����", "������", "����")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(1000,
                     new LMO(3, false, "�����", "�����", "������", "����"));
            {

                Data.ElementAt(Data.Count - 1).Value.createfloor = true;
                Data.ElementAt(Data.Count - 1).Value.is_rock = false;
                Data.ElementAt(Data.Count - 1).Value.walkable = true;
            }


            Data.Add(30,
                     new LMO(6, false, "����", "�����", "������", "�������")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(31,
                     new LMO(4, false, "����", "�����", "������", "�������")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(32,
                     new LMO(7, false, "������", "", "", "")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(33,
                     new LMO(4, false, "���� ���", "", "", "")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(34,
                     new LMO(7, false, "������", "", "", "")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(35,
                     new LMO(8, false, "��������", "", "", "")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(36,
                     new LMO(4, false, "���������", "", "", "")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(37,
                     new LMO(33, false, "�����", "", "", "")
                         {
                             createfloor = true,
                             is_rock = true,
                         });

            Data.Add(103,
                     new LMO(5, false, "���", "����", "�����", "�������")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Elovi,
                         });

            Data.Add(104,
                     new LMO(3, false, "�����", "����", "�����", "�������")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Sosnovi,
                         });

            Data.Add(105,
                     new LMO(4, false, "�����", "����", "�����", "�������")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Pihtovi,
                         });

            Data.Add(106,
                     new LMO(1, false, "����", "����", "�����", "�������")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Kedrovi,
                         });

            Data.Add(107,
                     new LMO(7, false, "�����������", "����", "�����",
                                    "�������")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Listvenizevi,
                         });

            Data.Add(108,
                     new LMO(3, false, "���", "����", "�����", "�������")
                         {
                             is_tree = true,
                             dropafterdeath = OnStoreID.WoodLog,
                             base_material = MaterialID.Dubovi,
                         });

            Data.Add(109,
                     new LMO(6, false, "���", "����", "�����", "�������"));
            {
                Data.ElementAt(Data.Count - 1).Value.is_tree = true;

                Data.ElementAt(Data.Count - 1).Value.dropafterdeath = OnStoreID.WoodLog;
                Data.ElementAt(Data.Count - 1).Value.base_material = MaterialID.Bukovi;
            }

            Data.Add(110,
                     new LMO(3, false, "����", "����", "�����", "�������"));
            {
                Data.ElementAt(Data.Count - 1).Value.is_tree = true;

                Data.ElementAt(Data.Count - 1).Value.dropafterdeath = OnStoreID.WoodLog;
                Data.ElementAt(Data.Count - 1).Value.base_material = MaterialID.Klenovi;
            }

            Data.Add(800,
                     new LMO(2, false, "������� ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }


            Data.Add(801,
                     new LMO(7, false, "����� ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }

            Data.Add(802,
                     new LMO(4, false, "������� ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }

            Data.Add(803,
                     new LMO(3, false, "���������� ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }

            Data.Add(804,
                     new LMO(7, false, "������ ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }

            Data.Add(805,
                     new LMO(4, false, "������ ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }

            Data.Add(806,
                     new LMO(3, false, "���������� ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;

                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            }

            Data.Add(807, new LMO(6, false, "������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;
                Data.ElementAt(Data.Count - 1).Value.is_rock = true;
                Data.ElementAt(Data.Count - 1).Value.dropafterdeath = OnStoreID.StoneBoulder;
                Data.ElementAt(Data.Count - 1).Value.base_material = MaterialID.Mramorni;
            }

            Data.Add(808, new LMO(2, false, "�������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(809, new LMO(3, false, "��������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            Data.ElementAt(Data.Count - 1).Value.dropafterdeath = OnStoreID.StoneBoulder;
            Data.ElementAt(Data.Count - 1).Value.base_material = MaterialID.Sfaleritovi;

            Data.Add(810, new LMO(5, false, "���������� ����", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(811, new LMO(6, false, "���� �����", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(812, new LMO(7, false, "���������� ����", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(813, new LMO(5, false, "�������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(814, new LMO(4, false, "������", "�����", "������", "����"));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(815, new LMO(6, false, "������", "�����", "������", "����"));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(816, new LMO(3, false, "�����", "�����", "������", "����"));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(817, new LMO(5, false, "������� ������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(818, new LMO(4, false, "���������� ������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(819, new LMO(7, false, "������ ������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(820, new LMO(5, false, "������ ������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(821, new LMO(6, false, "���������� ������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;

            Data.Add(822, new LMO(4, true, "���������� ��������", "����", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = false;
            Data.ElementAt(Data.Count - 1).Value.using_material = true;

            Data.Add(888, new LMO(34, true, "����", "��", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = false;
            Data.ElementAt(Data.Count - 1).Value.walkable = false;

            Data.Add(824, new LMO(3, false, "��������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            Data.ElementAt(Data.Count - 1).Value.dropafterdeath = OnStoreID.StoneBoulder;
            Data.ElementAt(Data.Count - 1).Value.base_material = MaterialID.Ortoclazi;

            Data.Add(825, new LMO(5, false, "���������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            Data.ElementAt(Data.Count - 1).Value.is_rock = true;
            Data.ElementAt(Data.Count - 1).Value.dropafterdeath = OnStoreID.StoneBoulder;
            Data.ElementAt(Data.Count - 1).Value.base_material = MaterialID.Microclini;

            Data.Add(666, new LMO(35, false, "������ id", "","",""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;
        }
    }
}