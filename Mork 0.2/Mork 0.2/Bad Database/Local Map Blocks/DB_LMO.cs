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

            Data.Add(10,
                     new LMO(32, false, "�����", "����", "����", "����")
                         {
                         });

            Data.Add((int)KnownIDs.Gabro,
                     new LMO(31, false, "������", "������", "������",
                                    "�� ������")
                         {
                         });

            Data.Add((int)KnownIDs.GabroToGranete,
                     new LMO(31, false, "������ ������",
                                    "������� �������", "������ ��������", "�� ������� �������")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add((int)KnownIDs.GrenFranite,
                     new LMO(29, false, "������� ������",
                                    "�������� �������", "������� ��������", "�� �������� �������")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(14,
                     new LMO(28, false, "�������", "����", "����",
                                    "����")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(15,
                     new LMO(25, false, "������ �������", "����",
                                    "����", "����")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(16,
                     new LMO(24, false, "������� �������", "����",
                                    "����", "����")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(17,
                     new LMO(23, false, "����������� �������",
                                    "����", "����", "����")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(18,
                     new LMO(22, false, "�����", "����", "����", "����")
                         {
                         });

            Data.Add(19,
                     new LMO(27, false, "������", "����", "����",
                                    "����")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(20,
                     new LMO(30, false, "����� ������", "����",
                                    "����", "����")
                         {
                             dropafterdeath = (int)KnownIDs.stoneboulder,
                             dropafterdeath_num = 2
                         });

            Data.Add(12345,
                     new LMO(45, false, "�����", "�����", "�����",
                                    "�����")
                         {
                         });

            Data.Add(22,
                     new LMO(20, false, "��������", "�����", "�����",
                                    "�����")
                         {
                         });

            Data.Add(23,
                     new LMO(2, false, "�����", "�����", "������",
                                    "����")
                         {
                         });

            Data.Add(24,
                     new LMO(3, false, "�������", "�����",
                                    "������", "����")
                         {
                         });

            Data.Add(25,
                     new LMO(4, false, "����", "�����", "������",
                                    "����")
                         {
                         });

            Data.Add(26,
                     new LMO(5, false, "������ ����", "�����",
                                    "������", "����")
                         {
                         });

            Data.Add(27,
                     new LMO(2, false, "��������", "�����",
                                    "������", "����")
                         {
                         });

            Data.Add(28,
                     new LMO(6, false, "������", "�����", "������", "����")
                         {
                         });

            Data.Add(1000,
                     new LMO(3, false, "�����", "�����", "������", "����"));
            {

                Data.ElementAt(Data.Count - 1).Value.walkable = true;
            }


            Data.Add(30,
                     new LMO(6, false, "����", "�����", "������", "�������")
                         {
                         });

            Data.Add(31,
                     new LMO(4, false, "����", "�����", "������", "�������")
                         {
                         });

            Data.Add(32,
                     new LMO(7, false, "������", "", "", "")
                         {
                         });

            Data.Add(33,
                     new LMO(4, false, "���� ���", "", "", "")
                         {
                         });

            Data.Add(34,
                     new LMO(7, false, "������", "", "", "")
                         {
                         });

            Data.Add(35,
                     new LMO(8, false, "��������", "", "", "")
                     {

                         });

            Data.Add(36,
                     new LMO(4, false, "���������", "", "", "")
                         {
                         });

            Data.Add(37,
                     new LMO(33, false, "�����", "", "", "")
                         {
          
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
                
            }


            Data.Add(801,
                     new LMO(24, false, "����� ��������", "", "", ""));
            {
               
            }

            Data.Add(802,
                     new LMO(24, false, "������� ��������", "", "", ""));
            {
               
            }

            Data.Add(803,
                     new LMO(24, false, "���������� ��������", "", "", ""));
            {
              
            }

            Data.Add(804,
                     new LMO(24, false, "������ ��������", "", "", ""));
            {
               
            }

            Data.Add(805,
                     new LMO(24, false, "������ ��������", "", "", ""));
            {
              
            }

            Data.Add(806,
                     new LMO(24, false, "���������� ��������", "", "", ""));
            {
                
            }

            Data.Add(807, new LMO(30, false, "������", "", "", "")
            {

                dropafterdeath = (int)KnownIDs.stoneboulder,
                dropafterdeath_num = 2
            });

            Data.Add(808, new LMO(2, false, "�������", "", "", ""));
       

            Data.Add(809, new LMO(3, false, "��������", "", "", "")
            {
                
                dropafterdeath = (int) KnownIDs.stoneboulder,
                dropafterdeath_num = 2
            });

            Data.Add(810, new LMO(5, false, "���������� ����", "", "", ""));
         

            Data.Add(811, new LMO(6, false, "���� �����", "", "", ""));
           

            Data.Add(812, new LMO(7, false, "���������� ����", "", "", ""));
         

            Data.Add(813, new LMO(5, false, "�������", "", "", ""));
           

            Data.Add(814, new LMO(4, false, "������", "�����", "������", "����"));
 

            Data.Add(815, new LMO(6, false, "������", "�����", "������", "����"));
  

            Data.Add(816, new LMO(3, false, "�����", "�����", "������", "����"));
           

            Data.Add(817, new LMO(5, false, "������� ������", "", "", ""));
         

            Data.Add(818, new LMO(4, false, "���������� ������", "", "", ""));
            

            Data.Add(819, new LMO(7, false, "������ ������", "", "", ""));
       

            Data.Add(820, new LMO(5, false, "������ ������", "", "", ""));
          

            Data.Add(821, new LMO(6, false, "���������� ������", "", "", ""));
       

            Data.Add(822, new LMO(4, true, "���������� ��������", "����", "", "")
            {
               
            });

            Data.Add(KnownIDs.water, new LMO(34, true, "����", "��", "", ""));
       
            Data.Last().Value.walkable = false;

            Data.Add(824, new LMO(3, false, "��������", "", "", "")
            {
              
                dropafterdeath = (int) KnownIDs.stoneboulder
            });

            Data.Add(825, new LMO(5, false, "���������", "", "", "")
            {
               
                dropafterdeath = (int) KnownIDs.stoneboulder
            });

            Data.Add((int)KnownIDs.error, new LMO(35, false, "������ id", "","",""));
        

            Data.Add((int) KnownIDs.stoneboulder, new LMO(36, false, "��������", "", "", "")
            {
                placeble = false
            });

            Data.Add((int)KnownIDs.stonebrickwall, new LMO(43, false, "�������� ������", "", "", "")
            {
                placeble = true,
                basic_hp = 1000,
                dropafterdeath = KnownIDs.stonebrick,
                dropafterdeath_num = 20
            });

            Data.Add((int)KnownIDs.stonebrick, new LMO(44, false, "�������� ������", "", "", "")
            {
                placeble = false,
            });

            Data.Add(KnownIDs.StorageEntrance, new LMO(1, false, "������������� �������", "", "", "")
            {
                placeble = true,
                activeblock = true
            });

            Data.Add(KnownIDs.BrickMaker, new LMO(1, false, "��������������", "", "", "")
            {
                placeble = true,
                activeblock = true
            });
        }
    }
}