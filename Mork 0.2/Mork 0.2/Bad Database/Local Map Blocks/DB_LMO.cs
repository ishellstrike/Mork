using System.Collections.Generic;
using System.Linq;

namespace Mork
{
    public enum KnownIDs
    {
        error = 666,
        stoneboulder = 5000,
    }

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
                             basic_hp = 100,
                         });

            Data.Add(101,
                     new LMO(4, false, "����", "����", "�����", "�������")
                         {
                             basic_hp = 100,
                         });

            Data.Add(102,
                     new LMO(5, false, "�����", "�����", "������", "��������")
                         {
                         });

            Data.Add(500,
                     new LMO(1, true, "�������� ����", "�������� ����", "�������� ����", "�������� ����"));
            Data.Add(501,
                     new LMO(3, true, "�������� ����", "�������� ����", "�������� ����", "�������� ����"));

            Data.Add(400,
                     new LMO(4, false, "����", "�����", "������", "����")
                         {
                             createfloor = true,
                         });

            Data.Add(401,
                     new LMO(1, false, "����", "�����", "������", "����")
                         {
                             createfloor = true,
                         });

            Data.Add(402,
                     new LMO(4, false, "����", "�����", "������", "����")
                         {
                             createfloor = true,
                         });

            Data.Add(10,
                     new LMO(32, false, "�����", "����", "����", "����")
                         {
                             createfloor = true,
                         });

            Data.Add(11,
                     new LMO(31, false, "������", "������", "������",
                                    "�� ������")
                         {
                             createfloor = true,
                         });

            Data.Add(12,
                     new LMO(31, false, "������ ������",
                                    "������� �������", "������ ��������", "�� ������� �������")
                         {
                             createfloor = true,
                             dropafterdeath = (int)KnownIDs.stoneboulder
                         });

            Data.Add(13,
                     new LMO(29, false, "������� ������",
                                    "�������� �������", "������� ��������", "�� �������� �������")
                         {
                             createfloor = true,
                             dropafterdeath = (int)KnownIDs.stoneboulder
                         });

            Data.Add(14,
                     new LMO(28, false, "�������", "����", "����",
                                    "����")
                         {
                             createfloor = true,
                             dropafterdeath = (int)KnownIDs.stoneboulder
                         });

            Data.Add(15,
                     new LMO(25, false, "������ �������", "����",
                                    "����", "����")
                         {
                             createfloor = true,
                             dropafterdeath = (int)KnownIDs.stoneboulder
                         });

            Data.Add(16,
                     new LMO(24, false, "������� �������", "����",
                                    "����", "����")
                         {
                             createfloor = true,
                             dropafterdeath = (int)KnownIDs.stoneboulder
                         });

            Data.Add(17,
                     new LMO(23, false, "����������� �������",
                                    "����", "����", "����")
                         {
                             createfloor = true,
                             dropafterdeath = (int)KnownIDs.stoneboulder
                         });

            Data.Add(18,
                     new LMO(22, false, "�����", "����", "����", "����")
                         {
                             createfloor = true,
                         });

            Data.Add(19,
                     new LMO(27, false, "������", "����", "����",
                                    "����")
                         {
                             createfloor = true,
                             dropafterdeath = (int)KnownIDs.stoneboulder
                         });

            Data.Add(20,
                     new LMO(30, false, "����� ������", "����",
                                    "����", "����")
                         {
                             createfloor = true,
                             dropafterdeath = (int)KnownIDs.stoneboulder
                         });

            Data.Add(12345,
                     new LMO(36, false, "�����", "�����", "�����",
                                    "�����")
                         {
                             createfloor = true,
                         });

            Data.Add(22,
                     new LMO(20, false, "��������", "�����", "�����",
                                    "�����")
                         {
                             createfloor = true,
                         });

            Data.Add(23,
                     new LMO(2, false, "�����", "�����", "������",
                                    "����")
                         {
                             createfloor = true,
                         });

            Data.Add(24,
                     new LMO(3, false, "�������", "�����",
                                    "������", "����")
                         {
                             createfloor = true,
                         });

            Data.Add(25,
                     new LMO(4, false, "����", "�����", "������",
                                    "����")
                         {
                             createfloor = true,
                         });

            Data.Add(26,
                     new LMO(5, false, "������ ����", "�����",
                                    "������", "����")
                         {
                             createfloor = true,
                         });

            Data.Add(27,
                     new LMO(2, false, "��������", "�����",
                                    "������", "����")
                         {
                             createfloor = true,
                         });

            Data.Add(28,
                     new LMO(6, false, "������", "�����", "������", "����")
                         {
                             createfloor = true,
                         });

            Data.Add(1000,
                     new LMO(3, false, "�����", "�����", "������", "����"));
            {

                Data.ElementAt(Data.Count - 1).Value.createfloor = true;
                Data.ElementAt(Data.Count - 1).Value.walkable = true;
            }


            Data.Add(30,
                     new LMO(6, false, "����", "�����", "������", "�������")
                         {
                             createfloor = true,
                         });

            Data.Add(31,
                     new LMO(4, false, "����", "�����", "������", "�������")
                         {
                             createfloor = true,
                         });

            Data.Add(32,
                     new LMO(7, false, "������", "", "", "")
                         {
                             createfloor = true,
                         });

            Data.Add(33,
                     new LMO(4, false, "���� ���", "", "", "")
                         {
                             createfloor = true,
                         });

            Data.Add(34,
                     new LMO(7, false, "������", "", "", "")
                         {
                             createfloor = true,
                         });

            Data.Add(35,
                     new LMO(8, false, "��������", "", "", "")
                         {
                             createfloor = true,
                         });

            Data.Add(36,
                     new LMO(4, false, "���������", "", "", "")
                         {
                             createfloor = true,
                         });

            Data.Add(37,
                     new LMO(33, false, "�����", "", "", "")
                         {
                             createfloor = true,
                         });

            Data.Add(103,
                     new LMO(5, false, "���", "����", "�����", "�������")
                         {
                         });

            Data.Add(104,
                     new LMO(3, false, "�����", "����", "�����", "�������")
                         {
                         });

            Data.Add(105,
                     new LMO(4, false, "�����", "����", "�����", "�������")
                         {
                         });

            Data.Add(106,
                     new LMO(1, false, "����", "����", "�����", "�������")
                         {
                         });

            Data.Add(107,
                     new LMO(7, false, "�����������", "����", "�����",
                                    "�������")
                         {
                         });

            Data.Add(108,
                     new LMO(3, false, "���", "����", "�����", "�������")
                         {
                         });

            Data.Add(109,
                     new LMO(6, false, "���", "����", "�����", "�������"));
            {

            }

            Data.Add(110,
                     new LMO(3, false, "����", "����", "�����", "�������"));
            {

            }

            Data.Add(800,
                     new LMO(24, false, "������� ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            }


            Data.Add(801,
                     new LMO(24, false, "����� ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            }

            Data.Add(802,
                     new LMO(24, false, "������� ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            }

            Data.Add(803,
                     new LMO(24, false, "���������� ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            }

            Data.Add(804,
                     new LMO(24, false, "������ ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            }

            Data.Add(805,
                     new LMO(24, false, "������ ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            }

            Data.Add(806,
                     new LMO(24, false, "���������� ��������", "", "", ""));
            {
                Data.ElementAt(Data.Count - 1).Value.createfloor = true;
            }

            Data.Add(807, new LMO(30, false, "������", "", "", "")
            {
                createfloor = true,
                dropafterdeath = (int) KnownIDs.stoneboulder
            });

            Data.Add(808, new LMO(2, false, "�������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add(809, new LMO(3, false, "��������", "", "", "")
            {
                createfloor = true,
                dropafterdeath = (int) KnownIDs.stoneboulder
            });

            Data.Add(810, new LMO(5, false, "���������� ����", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add(811, new LMO(6, false, "���� �����", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add(812, new LMO(7, false, "���������� ����", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add(813, new LMO(5, false, "�������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add(814, new LMO(4, false, "������", "�����", "������", "����"));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add(815, new LMO(6, false, "������", "�����", "������", "����"));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add(816, new LMO(3, false, "�����", "�����", "������", "����"));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add(817, new LMO(5, false, "������� ������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add(818, new LMO(4, false, "���������� ������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add(819, new LMO(7, false, "������ ������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add(820, new LMO(5, false, "������ ������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add(821, new LMO(6, false, "���������� ������", "", "", ""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add(822, new LMO(4, true, "���������� ��������", "����", "", "")
            {
                createfloor = false,
            });

            Data.Add(888, new LMO(34, true, "����", "��", "", ""));
            Data.Last().Value.createfloor = false;
            Data.Last().Value.walkable = false;

            Data.Add(824, new LMO(3, false, "��������", "", "", "")
            {
                createfloor = true,
                dropafterdeath = (int) KnownIDs.stoneboulder
            });

            Data.Add(825, new LMO(5, false, "���������", "", "", "")
            {
                createfloor = true,
                dropafterdeath = (int) KnownIDs.stoneboulder
            });

            Data.Add((int)KnownIDs.error, new LMO(35, false, "������ id", "","",""));
            Data.ElementAt(Data.Count - 1).Value.createfloor = true;

            Data.Add((int) KnownIDs.stoneboulder, new LMO(36, false, "��������", "", "", "")
            {
                placeble = false
            });
        }
    }
}