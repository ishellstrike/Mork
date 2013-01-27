using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mork.Local_Map.Dynamic.Actions
{
    public abstract class Order
    {
        public Vector3 dest;
        public bool complete;
        public bool sleep = false;
        public bool taken;
        public LocalUnit unit_owner;

        public override string ToString()
        {
            return GetType().ToString().Substring(GetType().ToString().LastIndexOf('.')) + " " + complete + " "  + dest;
        }
    }
}
