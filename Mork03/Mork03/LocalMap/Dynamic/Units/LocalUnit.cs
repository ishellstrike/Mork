using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Mork.Local_Map.Dynamic.Actions;
using Mork.Local_Map.Dynamic.Local_Items;
using Mork.Local_Map.Dynamic.Units.Actions;

namespace Mork.Local_Map.Dynamic {
    public class LocalUnit {
        //More technical variables

        public TimeSpan Age;
        public LocalItem Carry = new LocalItem();
        public Order CurrentOrder = new NothingOrder();
        public int ErrorID;
        public int ID;

        public TimeSpan IddleTime;

        // More gameplay variables

        public TimeSpan MaxLifeTime;
        public Stack<Vector3> Patch = new Stack<Vector3>();
        public Vector3 Pos;
        public Vector3 PrePos;
        public Order PreviousOrder = new NothingOrder();

        public LocalUnit(int id) {
            if (Main.dbcreatures.Data.ContainsKey(id)) {
                ID = id;
            }
            else {
                ID = 666;
                ErrorID = id;
            }
        }

        public void MoveUnit(GameTime gt) {
            if (Patch.Count <= 0) {
                return;
            }

            Vector3 temp = Patch.Peek();

            if (Pos.X > temp.X) {
                Pos.X -= 10*(float) gt.ElapsedGameTime.TotalSeconds;
            }
            if (Pos.Y > temp.Y) {
                Pos.Y -= 10*(float) gt.ElapsedGameTime.TotalSeconds;
            }
            if (Pos.Z > temp.Z) {
                Pos.Z -= 10*(float) gt.ElapsedGameTime.TotalSeconds;
            }

            if (Pos.X < temp.X) {
                Pos.X += 10*(float) gt.ElapsedGameTime.TotalSeconds;
            }
            if (Pos.Y < temp.Y) {
                Pos.Y += 10*(float) gt.ElapsedGameTime.TotalSeconds;
            }
            if (Pos.Z < temp.Z) {
                Pos.Z += 10*(float) gt.ElapsedGameTime.TotalSeconds;
            }

            if ((int) Pos.X != (int) temp.X || (int) Pos.Y != (int) temp.Y || (int) Pos.Z != (int) temp.Z) {
                return;
            }

            Patch.Pop();
        }
    }
}