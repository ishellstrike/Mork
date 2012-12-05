using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Mork.Local_Map.Dynamic.Actions;
using Mork.Local_Map.Dynamic.Local_Items;
using Mork.Local_Map.Dynamic.Units;
using Mork.Local_Map.Dynamic.Units.Actions;

namespace Mork.Local_Map.Dynamic.PlayerOrders
{
    public class PlayerOrders
    {
        Random rnd = new Random();

        public List<Order> n = new List<Order>();

        public TimeSpan s10;

        public void OrdersUpdate(GameTime gt, LocalHeroes lh)
        {
            ClearingOrders(lh);
            GivingOrders(lh);
            MakingOrders(gt, lh);

            s10 += gt.ElapsedGameTime;
            if(s10.TotalSeconds >= 1)
            {
                s10 -= TimeSpan.FromSeconds(1);
                foreach (var order in n)
                {
                    order.sleep = false;
                }
            }
        }

        void MakingOrders(GameTime gt, LocalHeroes lh)
        {
            for (int o = 0; o < lh.n.Count; o++)
            {
                var h = lh.n[o];
                if (h.current_order is DestroyOrder && IsNear(h.pos, h.current_order.dest))
                {
                    Main.mmap.n[(int) h.current_order.dest.X, (int) h.current_order.dest.Y, (int) h.current_order.dest.Z
                        ].health -= (float) (10*gt.TotalGameTime.TotalSeconds);
                    if (
                        Main.mmap.n[
                            (int) h.current_order.dest.X, (int) h.current_order.dest.Y, (int) h.current_order.dest.Z].
                            health <= 0)
                    {
                        Main.mmap.KillBlock((int)h.current_order.dest.X, (int)h.current_order.dest.Y, (int)h.current_order.dest.Z);
                        h.current_order.complete = true;
                        h.current_order = new NothingOrder();
                    }
                }

                if (h.current_order is CollectOrder && IsNear(h.pos, h.current_order.dest))
                {
                    var temp = Main.localitems.GetNearItem(h.pos);
                    if (temp.id != 0 && h.carry.id == 0)
                    {
                        h.carry.id = temp.id;
                        h.carry.count = temp.count;

                        Main.localitems.n.Remove(temp);

                        if(Main.globalstorage.n.Count > 0)
                        {
                            Vector3 st = Main.globalstorage.GetFreeStorage();
                            if(st != new Vector3(-1))
                            {
                                h.current_order = new ToStoreOrder() {dest = st};
                                Main.AddToLog("ToStoreOrder patch find for " + h.pos + " " + h.patch.Count);
                                h.patch = Main.mmap.FindPatch(h.pos, st);
                                (h.current_order as ToStoreOrder).storagepos = st;
                            }
                        }
                    }
                }

                if (h.current_order is ToStoreOrder && IsNear(h.pos, h.current_order.dest))
                {
                    if (h.carry.id != 0)
                    {
                        Vector3 st = (h.current_order as ToStoreOrder).storagepos;
                        LocalItems tempstor = Main.mmap.n[(int)st.X, (int)st.Y, (int)st.Z].tags["storage"] as LocalItems;
                        if (tempstor.n.Count < tempstor.carp)
                        {
                            tempstor.n.Add(new LocalItem() {count = h.carry.count, id = h.carry.id});
                            h.carry = new LocalItem();
                            h.current_order = new NothingOrder();
                        }
                        else
                        {
                            Main.localitems.n.Add(new LocalItem() { count = h.carry.count, id = h.carry.id });
                            h.carry = new LocalItem();
                            h.current_order = new NothingOrder();
                        }
                    }
                }

                if (h.current_order is BuildOrder && IsNear(h.pos, h.current_order.dest))
                {
                    var allow = true;
                    foreach (var he in lh.n)
                    {
                        if ((int)he.pos.X == (int)h.current_order.dest.X && (int)he.pos.Z == (int)h.current_order.dest.Z && (int)he.pos.Z == (int)h.current_order.dest.Z)
                            allow = false;
                    }
                    if (Main.mmap.n[(int)h.current_order.dest.X, (int)h.current_order.dest.Y, (int)h.current_order.dest.Z].blockID == 0 && allow && Main.iss.n[(h.current_order as BuildOrder).blockID].count >= 1)
                    {
                        Main.mmap.SetBlock((int) h.current_order.dest.X, (int) h.current_order.dest.Y,
                                           (int) h.current_order.dest.Z, (h.current_order as BuildOrder).blockID);
                        Main.iss.n[(h.current_order as BuildOrder).blockID].count--;
                        h.current_order.complete = true;
                        h.current_order = new NothingOrder();
                    }
                }

                if (h.iddle_time >= TimeSpan.FromSeconds(10))
                {
                    h.current_order.taken = false;
                    h.current_order.sleep = true;
                    h.current_order = new NothingOrder();
                }
            }
        }

        void ClearingOrders(LocalHeroes lh)
        {
            //foreach (var h in lh.n)
            //{
            //    if (h.current_order.complete)
            //    {
            //        h.previous_order = h.current_order;
            //        h.current_order = new NothingOrder();
            //    }

            //    if(h.current_order is MoveOrder && h.pos == h.current_order.dest)
            //    {
            //        h.previous_order = h.current_order;
            //        h.current_order.complete = true;
            //        h.current_order = new NothingOrder();
            //    }

            //    if(h.current_order is DestroyOrder && Main.mmap.n[(int)h.current_order.dest.X, (int)h.current_order.dest.Y, (int)h.current_order.dest.Z].blockID == 0)
            //    {
            //        h.previous_order = h.current_order;
            //        h.current_order.complete = true;
            //        h.current_order = new NothingOrder();
            //    }

            //    if (h.current_order is BuildOrder && Main.mmap.n[(int)h.current_order.dest.X, (int)h.current_order.dest.Y, (int)h.current_order.dest.Z].blockID != 0)
            //    {
            //        h.previous_order = h.current_order;
            //        h.current_order.complete = true;
            //        h.current_order = new NothingOrder();
            //    }
            //}

            for (int index = 0; index < n.Count; index++)
            {
                for (int o = 0; o < lh.n.Count; o++)
                {
                    var h = lh.n[o];
                    if (h.current_order is BuildOrder && Main.iss.n[(h.current_order as BuildOrder).blockID].count < 1)
                    {
                        h.current_order.complete = true;
                        h.current_order = new NothingOrder();
                    }

                    if (h.current_order is MoveOrder && IsNear(h.current_order.dest, h.pos))
                    {
                        h.current_order.complete = true;
                        h.current_order = new NothingOrder();
                    }
                }

                if(n[index].complete)n.RemoveAt(index);
            }
        }

        void GivingOrders(LocalHeroes lh)
        {
            foreach (var order in n)
            {
                if (order is BuildOrder && Main.mmap.n[(int)order.dest.X, (int)order.dest.Y, (int)order.dest.Z].blockID != 0)
                {
                    order.complete = true;
                    goto ordertaken;
                }

                if (order is DestroyOrder && Main.mmap.n[(int)order.dest.X, (int)order.dest.Y, (int)order.dest.Z].blockID == 0)
                {
                    order.complete = true;
                    goto ordertaken;
                }

                if(!order.taken && !order.sleep && IsReachable(order.dest))
                {
                    foreach (var h in lh.n)
                    {
                        if(h.current_order is NothingOrder)
                        {
                            h.current_order = order;
                           
                            order.unit_owner = h;
                            h.patch = Main.mmap.FindPatch(h.pos, GetNear(order.dest));
                            Main.AddToLog(order+" patch find for "+h.pos+" "+h.patch.Count);

                            if (h.patch.Count > 0) { order.taken = true; }
                            else order.sleep = true;
                                goto ordertaken;
                        }
                    }
                }
            ordertaken:
                ;
            }
        }

        private static bool IsReachable(Vector3 loc)
        {
            if (!MMap.GoodVector3(loc)) return false;
            return (MMap.IsWalkable((int)loc.X + 1, (int)loc.Y, (int)loc.Z) ||
                    MMap.IsWalkable((int)loc.X - 1, (int)loc.Y, (int)loc.Z) ||
                    MMap.IsWalkable((int)loc.X, (int)loc.Y + 1, (int)loc.Z) ||
                    MMap.IsWalkable((int)loc.X, (int)loc.Y - 1, (int)loc.Z) ||
                    MMap.IsWalkable((int)loc.X, (int)loc.Y, (int)loc.Z + 1) ||
                    MMap.IsWalkable((int)loc.X, (int)loc.Y, (int)loc.Z - 1));
        }

        private static bool IsNear(Vector3 loc, Vector3 loc2)
        {
            return (Math.Abs(loc.X - loc2.X) <= 2 && Math.Abs(loc.Y - loc2.Y) <= 2 && Math.Abs(loc.Z - loc2.Z) <= 1);
        }

        private static Vector3 GetNear(Vector3 loc)
        {
            if (MMap.IsWalkable((int)loc.X + 1, (int)loc.Y, (int)loc.Z)) return new Vector3(loc.X + 1, loc.Y, loc.Z);
            if (MMap.IsWalkable((int)loc.X - 1, (int)loc.Y, (int)loc.Z)) return new Vector3(loc.X - 1, loc.Y, loc.Z);
            if (MMap.IsWalkable((int)loc.X, (int)loc.Y + 1, (int)loc.Z)) return new Vector3(loc.X, loc.Y + 1, loc.Z);
            if (MMap.IsWalkable((int)loc.X, (int)loc.Y - 1, (int)loc.Z)) return new Vector3(loc.X, loc.Y - 1, loc.Z);

            if (MMap.IsWalkable((int)loc.X, (int)loc.Y, (int)loc.Z + 1)) return new Vector3(loc.X, loc.Y, loc.Z + 1);
            if (MMap.IsWalkable((int)loc.X, (int)loc.Y, (int)loc.Z - 1)) return new Vector3(loc.X, loc.Y, loc.Z - 1);

            return new Vector3();
        }
    }
}
