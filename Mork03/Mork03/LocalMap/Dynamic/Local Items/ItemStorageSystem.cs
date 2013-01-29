using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Mork.Local_Map.Dynamic.Local_Items {
    public class ItemStorageSystem {
        public Dictionary<int, LocalItem> n = new Dictionary<int, LocalItem>();

        public List<Vector3> storages = new List<Vector3>();

        public bool AddItem(int _id, int _count) {
            n[_id].count += _count;

            return true;
        }

        public bool RemoveItem(int _id, int _count) {
            if (n[_id].count < _count) {
                return false;
            }

            n[_id].count -= _count;
            return true;
        }
    }
}