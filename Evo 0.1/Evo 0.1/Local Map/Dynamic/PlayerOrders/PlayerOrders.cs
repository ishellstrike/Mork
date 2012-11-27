using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mork.Local_Map.Dynamic.Actions;
using Mork.Local_Map.Dynamic.Units;
using Mork.Local_Map.Dynamic.Units.Actions;

namespace Mork.Local_Map.Dynamic.PlayerOrders
{
    public class PlayerOrders
    {
        public List<Order> n = new List<Order>();

        public void OrdersUpdate(LocalHeroes lh)
        {
            ClearingOrders(lh);
            GivingOrders(lh);
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
            }
        }

        void GivingOrders(LocalHeroes lh)
        {
            foreach (var order in n)
            {
                if(!order.taken)
                {
                    foreach (var h in lh.n)
                    {
                        if(h.current_order is NothingOrder)
                        {
                            h.current_order = order;
                            order.taken = true;
                        }
                    }
                }
            }
        }
    }
}
