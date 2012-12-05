using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Mork.Local_Map.Dynamic.Actions;
using Mork.Local_Map.Dynamic.Local_Items;
using Mork.Local_Map.Dynamic.Units.Actions;

namespace Mork.Local_Map.Dynamic
{
    public abstract class LocalUnit
    {
        public Vector3 pos;
        public int unit_id;
        public Order current_order = new NothingOrder();
        public Order previous_order = new NothingOrder();

        public LocalItem carry = new LocalItem();

        public Vector3 pre_pos;
        public TimeSpan iddle_time;

        public Stack<Vector3> patch = new Stack<Vector3>();

        public void MoveUnit(GameTime gt)
        {
            if (patch.Count > 0)
            {
                Vector3 temp = new Vector3();
                temp = patch.Peek();/////////

                if (pos.X > temp.X) pos.X -= 10 * (float)gt.ElapsedGameTime.TotalSeconds;
                if (pos.Y > temp.Y) pos.Y -= 10 * (float)gt.ElapsedGameTime.TotalSeconds;
                if (pos.Z > temp.Z) pos.Z -= 10 * (float)gt.ElapsedGameTime.TotalSeconds;

                if (pos.X < temp.X) pos.X += 10 * (float)gt.ElapsedGameTime.TotalSeconds;
                if (pos.Y < temp.Y) pos.Y += 10 * (float)gt.ElapsedGameTime.TotalSeconds;
                if (pos.Z < temp.Z) pos.Z += 10 * (float)gt.ElapsedGameTime.TotalSeconds;

                //затаптывание травы
                if ((int)pos.X == (int)temp.X && (int)pos.Y == (int)temp.Y && (int)pos.Z == (int)temp.Z)
                {
                    //if (Main.mmap.n[pos.X, pos.Y, pos.Z+1].Obj == ObjectID.DirtWall_Grass3 && rnd.Next(0,20) == 0) Main.mmap.n[pos.X, pos.Y, pos.Z+1].Obj = ObjectID.DirtWall_Grass2;
                    //else
                    //    if (Main.mmap.n[pos.X, pos.Y, pos.Z + 1].Obj == ObjectID.DirtWall_Grass2 && rnd.Next(0, 20) == 0) Main.mmap.n[pos.X, pos.Y, pos.Z + 1].Obj = ObjectID.DirtWall_Grass1;
                    //else
                    //        if (Main.mmap.n[pos.X, pos.Y, pos.Z + 1].Obj == ObjectID.DirtWall_Grass1 && rnd.Next(0, 20) == 0) Main.mmap.n[pos.X, pos.Y, pos.Z + 1].Obj = ObjectID.DirtWall;
                    patch.Pop();
                }
            }
        }
    }
}
