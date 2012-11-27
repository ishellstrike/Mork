using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Mork.Local_Map.Dynamic.Actions;
using Mork.Local_Map.Dynamic.Units.Actions;

namespace Mork.Local_Map.Dynamic
{
    public abstract class LocalUnit
    {
        public Vector3 position;
        public int unit_id;
        public Order current_order = new NothingOrder();
        public Order previous_order = new NothingOrder();

        public Stack<Vector3> patch = new Stack<Vector3>();

        public void MoveUnit(GameTime gt)
        {
            Vector3 temp = new Vector3();
            if (patch.Count > 0)
            {
                temp = patch.Peek();/////////

                if (position.X > temp.X) position.X -= 10 * (float)gt.ElapsedGameTime.TotalSeconds;
                if (position.Y > temp.Y) position.Y -= 10 * (float)gt.ElapsedGameTime.TotalSeconds;
                if (position.Z > temp.Z) position.Z -= 10 * (float)gt.ElapsedGameTime.TotalSeconds;

                if (position.X < temp.X) position.X += 10 * (float)gt.ElapsedGameTime.TotalSeconds;
                if (position.Y < temp.Y) position.Y += 10 * (float)gt.ElapsedGameTime.TotalSeconds;
                if (position.Z < temp.Z) position.Z += 10 * (float)gt.ElapsedGameTime.TotalSeconds;

                //затаптывание травы
                if ((int)position.X == (int)temp.X && (int)position.Y == (int)temp.Y && (int)position.Z == (int)temp.Z)
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
