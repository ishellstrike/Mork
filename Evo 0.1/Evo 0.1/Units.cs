using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Mork
{
    public enum AItype { PlayerControl,Passive, Agressive };

    #region Units clases
    [Serializable]
    public class Hero
    {
        Random rnd = new Random();

        //public Vector3 pos = new Vector3(50, 20, 0), off = new Vector3();
        public Vector3 pos = new Vector3(50, 20, 0);
        public int tex = 1;
        public Color lightness = Color.White;

        public Att att = new Att();

        public Stack<Vector3> patch = new Stack<Vector3>();
        public Vector3 order = new Vector3();
        public Vector3 order_add = new Vector3();
        public OrderID orderid = OrderID.none_order;
        public byte orderphase = 0;

        public OnStoreID item_ininv = OnStoreID.Nothing;
        public MaterialID ininv_material = MaterialID.Basic;

        public Attack attack = new Attack(Attack.BASIC_ATTACK);

        public void MoveHuman(GameTime gt)
        {
            Vector3 temp = new Vector3();
            if (patch.Count > 0)
            {
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

            if (orderid != OrderID.transfer_order && item_ininv != OnStoreID.Nothing && Main.mmap.n[(int)pos.X, (int)pos.Y, (int)pos.Z + 1].Storing == OnStoreID.Nothing)
            {
                Main.mmap.n[(int)pos.X, (int)pos.Y, (int)pos.Z + 1].Storing = item_ininv;
                Main.mmap.n[(int)pos.X, (int)pos.Y, (int)pos.Z + 1].Storing_num++;
                Main.mmap.n[(int)pos.X, (int)pos.Y, (int)pos.Z + 1].storing_material = ininv_material;
                item_ininv = OnStoreID.Nothing;
                ininv_material = MaterialID.Basic;
            }
            if (orderid == OrderID.transfer_order && orderphase == 0 && item_ininv != OnStoreID.Nothing && Main.mmap.n[(int)pos.X, (int)pos.Y, (int)pos.Z + 1].Storing == OnStoreID.Nothing)
            {
                Main.mmap.n[(int)pos.X, (int)pos.Y, (int)pos.Z + 1].Storing = item_ininv;
                Main.mmap.n[(int)pos.X, (int)pos.Y, (int)pos.Z + 1].Storing_num++;
                Main.mmap.n[(int)pos.X, (int)pos.Y, (int)pos.Z + 1].storing_material = ininv_material;
                item_ininv = OnStoreID.Nothing;
                ininv_material = MaterialID.Basic;
            }
            //if (orderid == OrderID.transfer_order && patch.Count == 0 && orderphase == 0)
            //{
            //    orderid = OrderID.none_order;
            //    order = new Vector3();
            //    order_add = new Vector3();
            //    orderphase = 0;
            //}

        }
    }

    public class Heroes
    {
        public int hero_move_step = 0, hero_move_freq = 2;

        public int hero_action_step = 0, hero_attack_freq = 30;

        public List<Hero> n = new List<Hero>();
        public Heroes() { }
    }

    [Serializable]
    public class MCre
    {
        

        public int move_step = 0, move_freq = 5;
        public List<CNode> n = new List<CNode>();

        Random rnd = new Random();

        public void MoveCrea()
        {
            if (n.Count > 0)
            for (int i = 0; i <= n.Count - 1; i++)
            {
                if (n[i].patch.Count == 0)
                {
                    n[i].order = OrderID.none_order;
                    n[i].order_dest = new Vector3();
                }
                Vector3 temp = new Vector3();
                if (n[i].patch.Count > 0)
                {
                    temp = n[i].patch.Peek();

                    if (temp.X > n[i].pos.X) n[i].off.X += 2;
                    if (temp.Y > n[i].pos.Y) n[i].off.Y += 2;
                    if (temp.X < n[i].pos.X) n[i].off.X -= 2;
                    if (temp.Y < n[i].pos.Y) n[i].off.Y -= 2;
                    if (temp.Z < n[i].pos.Z) n[i].off.Z -= 2;
                    if (temp.Z > n[i].pos.Z) n[i].off.Z += 2;

                    if (n[i].off.X >= 5) { n[i].off.X -= 10; n[i].pos.X++; }
                    if (n[i].off.Y >= 5) { n[i].off.Y -= 10; n[i].pos.Y++; }
                    if (n[i].off.X <= -5) { n[i].off.X += 10; n[i].pos.X--; }
                    if (n[i].off.Y <= -5) { n[i].off.Y += 10; n[i].pos.Y--; }
                    if (n[i].off.Z <= -5) { n[i].off.Z += 10; n[i].pos.Z--; }
                    if (n[i].off.Z >= 5) { n[i].off.Z -= 10; n[i].pos.Z++; }

                    if (n[i].pos.X == temp.X && n[i].pos.Y == temp.Y && n[i].pos.Z == temp.Z) n[i].patch.Pop();
                }
                
                if (n[i].att.HP.curent <= 0) n.Remove(n[i]);

            }
        }
        public void AIstep()
        {
            //foreach (CNode temp in n)
            //{
            //    if(temp.order == Order.Fight) temp.attack_step++;
            //    if (temp.attack_step >= temp.attack_freq)
            //    {
            //        temp.attack_step = 0;
            //    }

            //    if (temp.aitype == AItype.Agressive)
            //    {
            //        switch (temp.order)
            //        {
            //            case Order.No_order:
            //                temp.patch = Main.mmap.FindPatch(temp.pos, new XY(temp.pos.X + rnd.Next(-5, 5), temp.pos.Y + rnd.Next(-5, 5)));
            //                temp.order = Order.Move_random;
            //                break;
            //            case Order.Move_random:
            //                if (temp.patch.Count == 0) temp.order = Order.No_order;
            //                if (Main.mmap.n[temp.pos.X, temp.pos.Y].vision == 0) temp.order = Order.Go_fight;
            //                break;
            //            case Order.Go_fight:
            //                temp.patch = Main.mmap.FindPatch(temp.pos,RandomNear(Main.mh.pos));
            //                temp.order = Order.Move_random;
            //                if (IsNear(Main.mh.pos, temp.pos)) temp.order = Order.Fight;
            //                temp.attack_step = 0;
            //                break;
            //            case Order.Fight:
            //                if (!IsNear(Main.mh.pos, temp.pos)) temp.order = Order.Go_fight;
                            
            //                if (temp.attack_step == 1)
            //                {
            //                    int a = ThrowDice(temp.attack);
            //                    Main.AddToLog("You are get " + a.ToString() + " damage from " + temp.name + ".");
            //                    Main.mh.att.HP.curent -= a;
            //                }
            //                break;
            //        }
            //    }
            //    else
            //    if (temp.aitype == AItype.Passive)
            //    {
            //        switch (temp.order)
            //        {
            //            case Order.No_order:
            //                temp.patch = Main.mmap.FindPatch(temp.pos, new XY(temp.pos.X + rnd.Next(-5, 5), temp.pos.Y + rnd.Next(-5, 5)));
            //                temp.order = Order.Move_random;
            //                break;
            //            case Order.Move_random:
            //                if (temp.patch.Count == 0) temp.order = Order.No_order;
            //                break;
            //        }
            //    }
            //}
        }

        public int ThrowDice(int count, int sides)
        {
            int sum = 0;
            for (int i = 1; i <= count; i++, sum += rnd.Next(1, sides)) ;
            return sum;
        }
        public int ThrowDice(Attack attack)
        {
            int sum = 0;
            for (int i = 1; i <= attack.d_count; i++, sum += rnd.Next(1, attack.d_sides)) ;
            return sum;
        }

        //public CNode GetNearCreature()
        //{
        //    foreach (CNode temp in n)
        //    {
        //        if (IsNear(temp.pos, Main.mh.pos)) return temp;
        //    }
        //    return null;
        //}

        //public void AttackCreature(CNode target)
        //{
        //    int a = ThrowDice(Main.mh.attack);
        //    target.att.HP.curent -= a;
        //    Main.mfloat.Add(a.ToString(), target.pos, Color.Red);
        //    Main.mh.hero_attack_step = 2;
        //}

        public Vector3 RandomNear(Vector3 loc)
        {
            int i = rnd.Next(1, 4);
            Vector3 dest = loc;

            switch (i)
            {
                case 1:
                    dest = new Vector3(loc.X + 1, loc.Y, loc.Z);
                    break;
                case 2:
                    dest = new Vector3(loc.X, loc.Y + 1, loc.Z);
                    break;
                case 3:
                    dest = new Vector3(loc.X - 1, loc.Y, loc.Z);
                    break;
                case 4:
                    dest = new Vector3(loc.X, loc.Y - 1, loc.Z);
                    break;
            }
            return dest;
        }
        public bool IsNear(Vector3 tar1, Vector3 tar2)
        {
            if (Math.Abs(tar1.X - tar2.X) <= 1 && Math.Abs(tar1.Y - tar2.Y) <= 1 && Math.Abs(tar1.Z - tar2.Z) <= 1) return true;
            return false;
        }
    }

    [Serializable]
    public class CNode
    {
        public Vector3 pos = new Vector3(50, 20, 0), off = new Vector3();
        public int tex = 1;

        public string name = "тест";

        public int action_freq = 10, action_step = 0; // 1 - ready to attack

        public Att att = new Att();

        public AItype aitype = AItype.Agressive;

        public Stack<Vector3> patch = new Stack<Vector3>();
        public Vector3 order_dest = new Vector3();
        public OrderID order = OrderID.none_order;

        public Attack attack = new Attack(Attack.BASIC_ATTACK);
    }

    [Serializable]
    public class Att 
    {
        //public StatI ST = new StatI(), DX = new StatI(), IQ = new StatI(), HT = new StatI(),
        public StatI HP = new StatI(); 
        //Will = new StatI(), Per = new StatI(), FP = new StatI(); // Сила, ловкость, интеллект, здоровье, хитпойнты, воля, восприятие, усталость
        //public StatF _AC = new StatF(), _Awo = new StatF(), _Acc = new StatF(); // класс брони, уворот, меткость
        //public StatF Exp = new StatF();
        //public StatF Hp_reg = new StatF();
        public StatI Vision = new StatI();
    }

    [Serializable]
    public class StatF
    {
        public float curent = 1, basical = 1;
    }

    [Serializable]
    public class StatI
    {
        public int curent = 1, basical = 1;
    }

    [Serializable]
    public class Attack
    {
        public const int BASIC_ATTACK = 1, BITE = 2;

        public Int16 d_count = 0;
        public Int16 d_sides = 0;

        public Attack(int a)
        {
            switch (a)
            {
                case BASIC_ATTACK: //1d6
                    d_count = 1;
                    d_sides = 6;
                    break;
                case BITE: //2d4
                    d_count = 2;
                    d_sides = 4;
                    break;
            }
        }
    } //класс управления атакой существ
    #endregion
}
