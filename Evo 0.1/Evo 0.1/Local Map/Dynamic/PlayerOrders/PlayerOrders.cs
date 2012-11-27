using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Mork.Local_Map.Dynamic.Actions;
using Mork.Local_Map.Dynamic.Units;
using Mork.Local_Map.Dynamic.Units.Actions;

namespace Mork.Local_Map.Dynamic.PlayerOrders
{
    public class PlayerOrders
    {
        public List<Order> n = new List<Order>();

        public void OrdersUpdate(GameTime gt, LocalHeroes lh)
        {
            ClearingOrders(lh);
            GivingOrders(lh);
            MakingOrders(gt, lh);
        }

        void MakingOrders(GameTime gt, LocalHeroes lh)
        {
            foreach (var h in lh.n)
            {
                if (h.current_order is DestroyOrder && IsNear(h.position, h.current_order.destination))
                {
                    Main.mmap.n[(int) h.current_order.destination.X, (int) h.current_order.destination.Y, (int) h.current_order.destination.Z].health -= (float)(10*gt.TotalGameTime.TotalSeconds);
                    if (Main.mmap.n[(int)h.current_order.destination.X, (int)h.current_order.destination.Y, (int)h.current_order.destination.Z].health <= 0)
                    {
                        Main.mmap.n[
                            (int) h.current_order.destination.X, (int) h.current_order.destination.Y,
                            (int) h.current_order.destination.Z].blockID = 0;
                        WorldLife.SubterrainPersonaly(h.current_order.destination, ref Main.mmap);
                    }
                }
            }
        }

        void ClearingOrders(LocalHeroes lh)
        {
            foreach (var h in lh.n)
            {
                if (h.current_order.complete)
                {
                    h.previous_order = h.current_order;
                    h.current_order = new NothingOrder();
                }

                if(h.current_order is MoveOrder && h.position == h.current_order.destination)
                {
                    h.previous_order = h.current_order;
                    h.current_order = new NothingOrder();
                }

                if(h.current_order is DestroyOrder && Main.mmap.n[(int)h.current_order.destination.X, (int)h.current_order.destination.Y, (int)h.current_order.destination.Z].blockID == 0)
                {
                    h.previous_order = h.current_order;
                    h.current_order = new NothingOrder();
                }
            }
        }

        void GivingOrders(LocalHeroes lh)
        {
            foreach (var order in n)
            {
                if(!order.taken && IsReachable(order.destination))
                {
                    foreach (var h in lh.n)
                    {
                        if(h.current_order is NothingOrder)
                        {
                            h.current_order = order;
                           
                            order.unit_owner = h;
                            h.patch = Main.mmap.FindPatch(h.position, GetNear(order.destination));
                            order.taken = true;
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
                    MMap.IsWalkable((int)loc.X + 1, (int)loc.Y, (int)loc.Z - 1) ||
                    MMap.IsWalkable((int)loc.X - 1, (int)loc.Y, (int)loc.Z - 1) ||
                    MMap.IsWalkable((int)loc.X, (int)loc.Y + 1, (int)loc.Z - 1) ||
                    MMap.IsWalkable((int)loc.X, (int)loc.Y - 1, (int)loc.Z - 1) ||
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

            if (MMap.IsWalkable((int)loc.X + 1, (int)loc.Y, (int)loc.Z - 1)) return new Vector3(loc.X + 1, loc.Y, loc.Z - 1);
            if (MMap.IsWalkable((int)loc.X - 1, (int)loc.Y, (int)loc.Z - 1)) return new Vector3(loc.X - 1, loc.Y, loc.Z - 1);
            if (MMap.IsWalkable((int)loc.X, (int)loc.Y + 1, (int)loc.Z - 1)) return new Vector3(loc.X, loc.Y + 1, loc.Z - 1);
            if (MMap.IsWalkable((int)loc.X, (int)loc.Y - 1, (int)loc.Z - 1)) return new Vector3(loc.X, loc.Y - 1, loc.Z - 1);

            return MMap.IsWalkable((int)loc.X, (int)loc.Y - 1, (int)loc.Z - 1) ? new Vector3(loc.X, loc.Y, loc.Z - 1) : new Vector3();
        }
    }
}
