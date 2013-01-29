using Microsoft.Xna.Framework.Graphics;
using Mork.Local_Map.Dynamic.PlayerOrders;
using Mork.Local_Map.Dynamic.Units;

namespace Mork.LocalMap.Dynamic {
    public class Player {
        public string Name;
        public PlayerOrders Orders;
        public LocalUnits Units;

        public Player(GraphicsDevice gd, Texture2D unittex, string name = "default") {
            Units = new LocalUnits(gd, unittex);
            Orders = new PlayerOrders();
            Name = name;
        }
    }
}