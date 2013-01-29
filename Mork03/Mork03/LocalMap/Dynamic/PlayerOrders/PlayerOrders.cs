using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Mork.Local_Map.Dynamic.Actions;
using Mork.Local_Map.Dynamic.Units;
using Mork.Local_Map.Dynamic.Units.Actions;
using Mork.Local_Map.Sector;

namespace Mork.Local_Map.Dynamic.PlayerOrders {
    public class PlayerOrders {
        public List<Order> N = new List<Order>();
        private Random _rnd = new Random();

        public TimeSpan s10;

        public void OrdersUpdate(GameTime gt, LocalUnits lh, SectorMap smap) {
            ClearingOrders(lh);
            GivingOrders(lh);
            MakingOrders(gt, lh, smap);

            s10 += gt.ElapsedGameTime;
            if (s10.TotalSeconds >= 1) {
                s10 -= TimeSpan.FromSeconds(1);
                foreach (Order order in N) {
                    order.sleep = false;
                }
            }
        }

        private void MakingOrders(GameTime gt, LocalUnits lh, SectorMap smap) {
            for (int o = 0; o < lh.N.Count; o++) {
                LocalUnit h = lh.N[o];
                if (h.CurrentOrder is DestroyOrder && IsNear(h.Pos, h.CurrentOrder.dest)) {
                    smap.At(h.CurrentOrder.dest.X, h.CurrentOrder.dest.Y, h.CurrentOrder.dest.Z).Health -=
                        (float) (10*gt.TotalGameTime.TotalSeconds);
                    if (
                        smap.At(h.CurrentOrder.dest.X, h.CurrentOrder.dest.Y, h.CurrentOrder.dest.Z).Health <= 0) {
                        Main.smap.KillBlock((int) h.CurrentOrder.dest.X, (int) h.CurrentOrder.dest.Y,
                                            (int) h.CurrentOrder.dest.Z);
                        h.CurrentOrder.complete = true;
                        h.CurrentOrder = new NothingOrder();
                    }
                }

                if (h.CurrentOrder is CollectOrder && IsNear(h.Pos, h.CurrentOrder.dest)) {
                    //var temp = Main.localitems.GetNearItem(h.pos);
                    if ((h.CurrentOrder as CollectOrder).tocollect.id != 0 && h.Carry.id == 0) {
                        //if (Main.localitems.n.Contains((h.current_order as CollectOrder).tocollect))
                        //{
                        h.Carry.id = (h.CurrentOrder as CollectOrder).tocollect.id;
                        h.Carry.count = (h.CurrentOrder as CollectOrder).tocollect.count;

                        Main.localitems.n.Remove((h.CurrentOrder as CollectOrder).tocollect);

                        h.CurrentOrder.complete = true;

                        if (Main.globalstorage.n.Count > 0) {
                            Vector3 st = Main.globalstorage.GetFreeStorage();
                            if (st != new Vector3(-1)) {
                                h.CurrentOrder = new ToStoreOrder {dest = st};
                                Main.AddToLog("ToStoreOrder patch find for " + h.Pos + " " + h.Patch.Count);
                                h.Patch = Main.smap.FindPatch(h.Pos, st);
                                (h.CurrentOrder as ToStoreOrder).storagepos = st;
                            }
                        }
                        //}
                    }
                }

                //if (h.current_order is ToStoreOrder && IsNear(h.pos, h.current_order.dest))
                //{
                //    if (h.carry.id != 0)
                //    {
                //        Vector3 st = (h.current_order as ToStoreOrder).storagepos;
                //        LocalItems tempstor = Main.mmap.n[(int)st.X, (int)st.Y, (int)st.Z].tags["storage"] as LocalItems;
                //        if (tempstor.n.Count < tempstor.carp)
                //        {
                //            tempstor.n.Add(new LocalItem() { count = h.carry.count, id = h.carry.id });
                //            h.carry = new LocalItem();
                //            h.current_order = new NothingOrder();
                //        }
                //        else
                //        {
                //            Main.localitems.n.Add(new LocalItem() { count = h.carry.count, id = h.carry.id });
                //            h.carry = new LocalItem();
                //            h.current_order = new NothingOrder();
                //        }
                //    }
                //}

                if (h.CurrentOrder is BuildOrder && IsNear(h.Pos, h.CurrentOrder.dest)) {
                    bool allow = true;
                    foreach (LocalUnit he in lh.N) {
                        if ((int) he.Pos.X == (int) h.CurrentOrder.dest.X &&
                            (int) he.Pos.Z == (int) h.CurrentOrder.dest.Z &&
                            (int) he.Pos.Z == (int) h.CurrentOrder.dest.Z) {
                            allow = false;
                        }
                    }
                    if (Main.smap.At(h.CurrentOrder.dest).BlockID == 0 && allow &&
                        Main.iss.n[(h.CurrentOrder as BuildOrder).blockID].count >= 1) {
                        Main.smap.SetBlock(h.CurrentOrder.dest, (h.CurrentOrder as BuildOrder).blockID);
                        Main.iss.n[(h.CurrentOrder as BuildOrder).blockID].count--;
                        h.CurrentOrder.complete = true;
                        h.CurrentOrder = new NothingOrder();
                    }
                }

                if (h.IddleTime >= TimeSpan.FromSeconds(10)) {
                    h.CurrentOrder.taken = false;
                    h.CurrentOrder.sleep = true;
                    h.CurrentOrder = new NothingOrder();
                }
            }
        }

        private void ClearingOrders(LocalUnits lh) {
            for (int index = 0; index < N.Count; index++) {
                for (int o = 0; o < lh.N.Count; o++) {
                    LocalUnit h = lh.N[o];
                    if (h.CurrentOrder is BuildOrder && Main.iss.n[(h.CurrentOrder as BuildOrder).blockID].count < 1) {
                        h.CurrentOrder.complete = true;
                        h.CurrentOrder = new NothingOrder();
                    }

                    if (h.CurrentOrder is MoveOrder && IsNear(h.CurrentOrder.dest, h.Pos)) {
                        h.CurrentOrder.complete = true;
                        h.CurrentOrder = new NothingOrder();
                    }
                }

                if (N[index].complete) {
                    N.RemoveAt(index);
                }
            }
        }

        private void GivingOrders(LocalUnits lh) {
            foreach (Order order in N) {
                if (order is BuildOrder && Main.smap.At(order.dest).BlockID != 0) {
                    order.complete = true;
                    goto ordertaken;
                }

                if (order is DestroyOrder && Main.smap.At(order.dest).BlockID == 0) {
                    order.complete = true;
                    goto ordertaken;
                }

                if (!order.taken && !order.sleep && IsReachable(order.dest)) {
                    foreach (LocalUnit h in lh.N) {
                        if (h.CurrentOrder is NothingOrder) {
                            h.CurrentOrder = order;

                            order.unit_owner = h;
                            h.Patch = Main.smap.FindPatch(h.Pos, GetNear(order.dest));
                            Main.AddToLog(order + " patch find for " + h.Pos + " " + h.Patch.Count);

                            if (h.Patch.Count > 0) {
                                order.taken = true;
                            }
                            else {
                                order.sleep = true;
                            }
                            goto ordertaken;
                        }
                    }
                }
                ordertaken:
                ;
            }
        }

        private static bool IsReachable(Vector3 loc) {
            if (!MMap.GoodVector3(loc)) {
                return false;
            }
            return (Main.smap.IsWalkable((int) loc.X + 1, (int) loc.Y, (int) loc.Z) ||
                    Main.smap.IsWalkable((int) loc.X - 1, (int) loc.Y, (int) loc.Z) ||
                    Main.smap.IsWalkable((int) loc.X, (int) loc.Y + 1, (int) loc.Z) ||
                    Main.smap.IsWalkable((int) loc.X, (int) loc.Y - 1, (int) loc.Z) ||
                    Main.smap.IsWalkable((int) loc.X, (int) loc.Y, (int) loc.Z + 1) ||
                    Main.smap.IsWalkable((int) loc.X, (int) loc.Y, (int) loc.Z - 1));
        }

        private static bool IsNear(Vector3 loc, Vector3 loc2) {
            return (Math.Abs(loc.X - loc2.X) <= 2 && Math.Abs(loc.Y - loc2.Y) <= 2 && Math.Abs(loc.Z - loc2.Z) <= 1);
        }

        private static Vector3 GetNear(Vector3 loc) {
            if (Main.smap.IsWalkable((int) loc.X + 1, (int) loc.Y, (int) loc.Z)) {
                return new Vector3(loc.X + 1, loc.Y, loc.Z);
            }
            if (Main.smap.IsWalkable((int) loc.X - 1, (int) loc.Y, (int) loc.Z)) {
                return new Vector3(loc.X - 1, loc.Y, loc.Z);
            }
            if (Main.smap.IsWalkable((int) loc.X, (int) loc.Y + 1, (int) loc.Z)) {
                return new Vector3(loc.X, loc.Y + 1, loc.Z);
            }
            if (Main.smap.IsWalkable((int) loc.X, (int) loc.Y - 1, (int) loc.Z)) {
                return new Vector3(loc.X, loc.Y - 1, loc.Z);
            }

            if (Main.smap.IsWalkable((int) loc.X, (int) loc.Y, (int) loc.Z + 1)) {
                return new Vector3(loc.X, loc.Y, loc.Z + 1);
            }
            if (Main.smap.IsWalkable((int) loc.X, (int) loc.Y, (int) loc.Z - 1)) {
                return new Vector3(loc.X, loc.Y, loc.Z - 1);
            }

            return new Vector3();
        }
    }
}