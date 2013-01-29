using Microsoft.Xna.Framework;

namespace Mork.Local_Map.Dynamic.Actions {
    public abstract class Order {
        public bool complete;
        public Vector3 dest;
        public bool sleep;
        public bool taken;
        public LocalUnit unit_owner;

        public override string ToString() {
            return GetType().ToString().Substring(GetType().ToString().LastIndexOf('.')) + " " + complete + " " + dest;
        }
    }
}