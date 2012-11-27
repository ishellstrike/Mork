using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mork
{
    public enum OrderID
    {
        none_order,
        walk_order,
        dig_order,
        crop_order, 
        digpit_order,
        transfer_order,
        return_to_store_order
    }
    public class Order
    {
        public Vector3 dest;
        public Vector3 dest_add;
        public OrderID id;

        public bool taken = false;
        public bool reachable = true;

        public int lifetime = 0;

        public Order() { }
        public Order(Vector3 _dest, OrderID _id)
        {
            dest = _dest;
            id = _id;
        }
        public Order(Vector3 _dest, Vector3 _destadd, OrderID _id)
        {
            dest = _dest;
            dest_add = _destadd;
            id = _id;
        }
    }
    public class Orders
    {
        Random _rnd = new Random();

        private const int Maxlifetime = 1000;

        private int _lazyOrdering = 0;
        private const int LazyOrderingMax = 5;

        private int _sortMaybe = 0;
        private int _sortMabyeMax = 100;

        public List<Order> list = new List<Order>();

        public void NewOrder(Vector3 dest, OrderID _id)
        {
            if(!MMap1.GoodVector3(dest)) return;
            if (list.Any(t => t.dest == dest))
            {
                return;
            }

            list.Add(new Order(dest, _id));
        }

        public void NewOrder(Vector3 dest, Vector3 destadd, OrderID id)
        {
            if (!MMap1.GoodVector3(dest)) return;
            if (list.Any(t => t.dest == dest))
            {
                return;
            }

            list.Add(new Order(dest, destadd, id));
        }

        private static bool IsReachable(Vector3 loc)
        {
            if (!MMap1.GoodVector3(loc)) return false;
            return (MMap1.IsWalkable((int)loc.X + 1, (int)loc.Y, (int)loc.Z) ||
                    MMap1.IsWalkable((int)loc.X - 1, (int)loc.Y, (int)loc.Z) ||
                    MMap1.IsWalkable((int)loc.X, (int)loc.Y + 1, (int)loc.Z) ||
                    MMap1.IsWalkable((int)loc.X, (int)loc.Y - 1, (int)loc.Z) ||
                    MMap1.IsWalkable((int)loc.X + 1, (int)loc.Y, (int)loc.Z - 1) ||
                    MMap1.IsWalkable((int)loc.X - 1, (int)loc.Y, (int)loc.Z - 1) ||
                    MMap1.IsWalkable((int)loc.X, (int)loc.Y + 1, (int)loc.Z - 1) ||
                    MMap1.IsWalkable((int)loc.X, (int)loc.Y - 1, (int)loc.Z - 1) ||
                    MMap1.IsWalkable((int)loc.X, (int)loc.Y, (int)loc.Z - 1));
        }

        private static bool IsNear(Vector3 loc, Vector3 loc2)
        {
            return (Math.Abs(loc.X - loc2.X) <= 2 && Math.Abs(loc.Y - loc2.Y) <= 2 && Math.Abs(loc.Z - loc2.Z) <= 1);
        }

        private static Vector3 GetNear(Vector3 loc)
        {
            if (MMap1.IsWalkable((int)loc.X + 1, (int)loc.Y, (int)loc.Z)) return new Vector3(loc.X + 1, loc.Y, loc.Z);
            if (MMap1.IsWalkable((int)loc.X - 1, (int)loc.Y, (int)loc.Z)) return new Vector3(loc.X - 1, loc.Y, loc.Z);
            if (MMap1.IsWalkable((int)loc.X, (int)loc.Y + 1, (int)loc.Z)) return new Vector3(loc.X, loc.Y + 1, loc.Z);
            if (MMap1.IsWalkable((int)loc.X, (int)loc.Y - 1, (int)loc.Z)) return new Vector3(loc.X, loc.Y - 1, loc.Z);

            if (MMap1.IsWalkable((int)loc.X + 1, (int)loc.Y, (int)loc.Z - 1)) return new Vector3(loc.X + 1, loc.Y, loc.Z - 1);
            if (MMap1.IsWalkable((int)loc.X - 1, (int)loc.Y, (int)loc.Z - 1)) return new Vector3(loc.X - 1, loc.Y, loc.Z - 1);
            if (MMap1.IsWalkable((int)loc.X, (int)loc.Y + 1, (int)loc.Z - 1)) return new Vector3(loc.X, loc.Y + 1, loc.Z - 1);
            if (MMap1.IsWalkable((int)loc.X, (int)loc.Y - 1, (int)loc.Z - 1)) return new Vector3(loc.X, loc.Y - 1, loc.Z - 1);

            return MMap1.IsWalkable((int)loc.X, (int)loc.Y - 1, (int)loc.Z - 1) ? new Vector3(loc.X, loc.Y, loc.Z - 1) : new Vector3();
        }

        public void GivemOrders(ref Heroes heroes)
        {
            _lazyOrdering++;
            //sort_maybe++;
            if (_lazyOrdering >= LazyOrderingMax) ClearTrashOrders();

            //if (sort_maybe >= sort_mabye_max)
            //{
            //    list.Sort(ByReachibleComparison);
            //    sort_maybe = 0;
            //}

            
            if (_lazyOrdering >= LazyOrderingMax)
            {
                GivemIt(heroes);
                _lazyOrdering = 0;
            }

            ControllFinishing(heroes);
        }

/*
        private static int ByReachibleComparison(Order a, Order b)
        {
            if (a.reachable && !b.reachable) return 1;
            return !a.reachable && b.reachable ? -1 : 0;
        }
*/

        private static void ControllFinishing(Heroes heroes)
        {
            foreach (var hero in heroes.n)
            {
                if (hero.patch.Count == 0 && hero.orderid == OrderID.dig_order && heroes.hero_action_step == 0 && IsNear(hero.pos, hero.order))
                {
                    if (hero.orderid == OrderID.dig_order) Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].health -= 1;
                    if (Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].health <= 0)
                    {
                        if (Main.dbobject.Data[Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].blockID].dropafterdeath != OnStoreID.Nothing)
                        {
                            Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z + 1].Storing = Main.dbobject.Data[Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].blockID].dropafterdeath;
                            Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z + 1].Storing_num = Main.dbobject.Data[Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].blockID].dropafterdeath_num;
                            Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z + 1].storing_material = Main.dbobject.Data[Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].blockID].base_material;
                        }
                        Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].blockID = 0;

                        for (var i = -1; i <= 1; i++)
                            for (var j = -1; j <= 1; j++)
                            {
                                if (MMap1.GoodVector3((int)hero.order.X + i, (int)hero.order.Y + j, (int)hero.order.Z))
                                    Main.mmap.n[(int)hero.order.X + i, (int)hero.order.Y + j, (int)hero.order.Z].explored = true;
                            }
                        if (MMap1.GoodVector3((int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z + 1))
                            Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z + 1].explored = true;

                        WorldLife.SubterrainPersonaly(hero.order, ref Main.mmap);

                        hero.order = new Vector3();
                        hero.orderid = OrderID.none_order;
                    }
                }
                if (hero.patch.Count == 0 && hero.orderid == OrderID.crop_order && heroes.hero_action_step == 0 && IsNear(hero.pos, hero.order))
                {
                    if (hero.orderid == OrderID.crop_order) Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].health -= 1;
                    if (Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].health <= 0)
                    {
                        if (Main.dbobject.Data[Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].blockID].dropafterdeath != OnStoreID.Nothing)
                        {
                            Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z + 1].Storing = Main.dbobject.Data[Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].blockID].dropafterdeath;
                            Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z + 1].Storing_num = Main.dbobject.Data[Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].blockID].dropafterdeath_num;
                            Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z + 1].storing_material = Main.dbobject.Data[Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].blockID].base_material;
                        }
                        Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].blockID = 0;

                        WorldLife.SubterrainPersonaly(hero.order, ref Main.mmap);

                        hero.order = new Vector3();
                        hero.orderid = OrderID.none_order;
                    }
                }
                if (hero.patch.Count == 0 && hero.orderid == OrderID.transfer_order && heroes.hero_action_step == 0 && ((hero.orderphase == 0 && IsNear(hero.pos, hero.order)) || (hero.orderphase == 1 && IsNear(hero.pos, hero.order_add))))
                {
                    if (hero.orderphase == 1)
                    {
                        if (Main.mmap.n[(int)hero.order_add.X, (int)hero.order_add.Y, (int)hero.order_add.Z].Storing == OnStoreID.Nothing || (Main.mmap.n[(int)hero.order_add.X, (int)hero.order_add.Y, (int)hero.order_add.Z].Storing_num < 5 && Main.mmap.n[(int)hero.order_add.X, (int)hero.order_add.Y, (int)hero.order_add.Z].Storing == hero.item_ininv && Main.mmap.n[(int)hero.order_add.X, (int)hero.order_add.Y, (int)hero.order_add.Z].storing_material == hero.ininv_material))
                        {
                            Main.mmap.n[(int)hero.order_add.X, (int)hero.order_add.Y, (int)hero.order_add.Z].Storing_num++;
                            Main.mmap.n[(int)hero.order_add.X, (int)hero.order_add.Y, (int)hero.order_add.Z].Storing = hero.item_ininv;
                            Main.mmap.n[(int)hero.order_add.X, (int)hero.order_add.Y, (int)hero.order_add.Z].storing_material = hero.ininv_material;
                            hero.ininv_material = MaterialID.Basic;
                            hero.item_ininv = OnStoreID.Nothing;
                        }
                        else
                        {
                            Main.AddToLog(string.Format("Store error {0} at {1}. Order canceled", hero.orderid, hero.order_add));
                        }
                            
                        hero.order_add = new Vector3();hero.order = new Vector3();
                        
                        hero.orderid = OrderID.none_order;
                        hero.orderphase = 0;
                    }
                    if (hero.orderphase == 0 )//&& Main.mmap.n[hero.order.X, hero.order.Y, hero.order.Z].Storing_num > 0
                    {
                        if (Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].Storing != OnStoreID.Nothing)
                        {
                            hero.item_ininv = Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].Storing;
                            hero.ininv_material = Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].storing_material;
                            Main.mmap.n[(int)hero.order.X, (int)hero.order.Y, (int)hero.order.Z].Storing_num--;
                            hero.orderphase = 1;

                            hero.order = new Vector3(-1, -1, -1);

                            hero.patch = Main.mmap.FindPatch(hero.pos, new Vector3(hero.order_add.X, hero.order_add.Y, hero.order_add.Z));
                        }
                        else
                        {
                            Main.AddToLog(string.Format("Missing order {0} at {1}. Order canceled", hero.orderid, hero.order));

                            hero.orderid = OrderID.none_order;
                            hero.order = new Vector3();
                            hero.order_add = new Vector3();
                        }
                        
                        
                    }
                }
                //if (hero.patch.Count == 0 && hero.orderid == OrderID.return_to_store_order && heroes.hero_action_step == 0 && IsNear(hero.pos, hero.order))
                //{
                //    if (Main.mmap.n[hero.order.X, hero.order.Y, hero.order.Z].Storing_num < 5)
                //    {
                //        Main.mmap.n[hero.order.X, hero.order.Y, hero.order.Z].Storing_num++;
                //        Main.mmap.n[hero.order.X, hero.order.Y, hero.order.Z].Storing = hero.item_ininv;
                //        hero.item_ininv = OnStoreID.Nothing;
                //    }

                //    hero.order_add = new Vector3(); hero.order = new Vector3();

                //    hero.orderid = OrderID.none_order;                  
                //}
            }
        }

        private void GivemIt(Heroes heroes)
        {
            if (list.Count > 0)
                for (int i = 0; i <= list.Count - 1; i++)
                {
                    if (!list[i].taken && list[i].reachable)
                    {
                        if (heroes.n.Count > 0)
                            for (int j = 0; j <= heroes.n.Count - 1; j++)
                            {
                                if (heroes.n[j].orderid == OrderID.none_order)
                                {
                                    heroes.n[j].patch = Main.mmap.FindPatch(heroes.n[j].pos, GetNear(list[i].dest));
                                    if (heroes.n[j].patch.Count == 0 && !IsNear(heroes.n[j].pos, list[i].dest)) //goto here2;
                                    list[i].taken = true;
                                    heroes.n[j].orderid = list[i].id;
                                    heroes.n[j].order_add = list[i].dest_add;
                                    heroes.n[j].order = list[i].dest;
                                    if (list[i].id != OrderID.dig_order) list.RemoveAt(i);
                                    goto here1;
                                }
                            here2: ;
                            }
                    }
                }
        here1: ;
        }

/*
        private void GivemItRandom(Heroes heroes)
        {
            if (list.Count > 0)
            {
                var i = rnd.Next(0, list.Count - 1);
                {
                    if (!list[i].taken && IsReachable(list[i].dest))
                        if (heroes.n.Count > 0)
                            for (var j = 0; j <= heroes.n.Count - 1; j++)
                            {
                                if (heroes.n[j].orderid == OrderID.none_order)
                                {
                                    heroes.n[j].patch = Main.mmap.FindPatch(heroes.n[j].pos, GetNear(list[i].dest));
                                    if (heroes.n[j].patch.Count == 0 && !IsNear(heroes.n[j].pos, list[i].dest))
                                        goto here2;
                                    list[i].taken = true;
                                    heroes.n[j].orderid = list[i].id;
                                    heroes.n[j].order = list[i].dest;
                                    goto here1;
                                }
                                here2:
                                ;
                            }
                }
            here1: ;
            }
        }
*/

        private void ClearTrashOrders()
        {
            if (list.Count > 0)
                for (var i = 0; i <= list.Count - 1; i++)
                {
                    list[i].lifetime++;
                    list[i].reachable = IsReachable(list[i].dest);
                    if (Main.mmap.n[(int)list[i].dest.X, (int)list[i].dest.Y, (int)list[i].dest.Z].blockID == 0)
                    {
                        list.RemoveAt(i);
                        i--; goto here4;
                    }
                    if (list[i].lifetime >= Maxlifetime)
                    {
                        list[i].taken = false;
                    }
                    if (list[i].id == OrderID.transfer_order && Main.mmap.n[(int)list[i].dest.X, (int)list[i].dest.Y, (int)list[i].dest.Z].Storing_num <= 0)
                    {
                        list.RemoveAt(i);
                        i--;
                    }

                here4: ;
                }
        }
    }
}
