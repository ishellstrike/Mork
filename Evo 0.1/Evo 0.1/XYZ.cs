using System;

namespace Mork
{
    /// <summary>
    /// Представляет класс 2х координатного целочисленного вектора
    /// </summary>
    //[Serializable]
    //public class XY
    //{
    //    private Int32 x, y;

    //    public Int32 X
    //    {
    //        get { return x; }
    //        set { x = value; }
    //    }

    //    public Int32 Y
    //    {
    //        get { return y; }
    //        set { y = value; }
    //    }

    //    /// <summary>
    //    /// Инициализирует новый экземпляр класса XY, задает указанные значения координат.
    //    /// </summary>
    //    /// <param name="a">X координата</param>
    //    /// <param name="b">Y координата</param>
    //    public XY(int a, int b)
    //    {
    //        X = a; Y = b;
    //    }
    //    /// <summary>
    //    /// Инициализирует новый экземпляр класса XY, значения координат устанавливаются равными нулю.
    //    /// </summary>
    //    public XY()
    //    {
    //        X = 0; Y = 0;
    //    }
    //    public XY(XY a)
    //    {
    //        X = a.X; Y = a.Y;
    //    }
    //    /// <summary>
    //    /// Предоставляет удобную для восприятия форму записи вектора XY
    //    /// </summary>
    //    /// <returns>строка координат {X, Y}</returns>
    //    public override String ToString()
    //    {
    //        return "{" + Convert.ToString(X) + ", " + Convert.ToString(Y) + "}";
    //    }

    //    public override int GetHashCode()
    //    {
    //        return ToString().GetHashCode();
    //    }
    //    public override bool Equals(object obj)
    //    {
    //        if (obj is XY) return Equals((XY)obj);
    //        return false;
    //    }

    //    public static XY operator +(XY a, XY b)
    //    {
    //        XY c = new XY();
    //        c.X = a.X + b.X;
    //        c.Y = a.Y + b.Y;
    //        return c;
    //    }
    //    public static XY operator -(XY a, XY b)
    //    {
    //        XY c = new XY();
    //        c.X = a.X - b.X;
    //        c.Y = a.Y - b.Y;
    //        return c;
    //    }
    //    public static XY operator *(XY a, int i)
    //    {
    //        XY c = new XY();
    //        c.X = a.X * i;
    //        c.Y = a.Y * i;
    //        return c;
    //    }
    //    public static XY operator /(XY a, int i)
    //    {
    //        XY c = new XY();
    //        c.X = a.X / i;
    //        c.Y = a.Y / i;
    //        return c;
    //    }
    //    //public static bool operator ==(XY a, XY b)
    //    //{
    //    //    if (a.X == b.X && a.Y == b.Y) return true;
    //    //    return false;
    //    //}
    //    //public static bool operator !=(XY a, XY b)
    //    //{
    //    //    if (a.X != b.X || a.Y != b.Y) return true;
    //    //    return false;
    //    //}
    //}
}
