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
    }

    public class MoveOrder : Order
    {
        
    }

    public class BuildOrder : Order
    {
        public int blockID;
    }

    public class DestroyOrder : Order
    {
        
    }
}
