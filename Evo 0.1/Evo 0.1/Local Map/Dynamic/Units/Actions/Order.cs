using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mork.Local_Map.Dynamic.Actions
{
    public abstract class Order
    {
        public Vector3 destination;
        public bool complete;
        public bool taken;
        public LocalUnit unit_owner;
    }
}
