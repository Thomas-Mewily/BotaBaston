#if USE_MONOGAME
using Microsoft.Xna.Framework;
#endif

using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry;

public struct Point2
{
    #region Constructor
    #region Properties
    public int X;
    public int Y;

    public float LengthSquared => X * X + Y * Y;
    public float Length => MathF.Sqrt(LengthSquared);

    public bool HaveLength => HaveXLength || HaveYLength;
    public bool HaveXLength => X != 0;
    public bool HaveYLength => Y != 0;
    #endregion

    public Point2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point2(int xy)
    {
        X = Y = xy;
    }

    public Point2(Point2 start, Point2 end)
    {
        X = end.X - start.X;
        Y = end.Y - start.Y;
    }
    #endregion

    #region Operator
    #region Equality
    public static bool operator ==(Point2 a, Point2 b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Point2 a, Point2 b) => a.X != b.X || a.Y != b.Y;
    #endregion

    #region Additive
    public static Point2 operator +(Point2 a, Point2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Point2 operator +(Point2 a) => a;

    public static Point2 operator -(Point2 a, Point2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Point2 operator -(Point2 a) => a;
    #endregion

    #region Multiplicative
    public static Point2 operator *(Point2 a, Point2 b) => new(a.X * b.X, a.Y * b.Y);
    public static Point2 operator *(int a, Point2 b) => new(a * b.X, a * b.Y);
    public static Point2 operator *(Point2 a, int b) => new(b * a.X, b * a.Y);

    public static Point2 operator /(Point2 a, Point2 b) => new(a.X / b.X, a.Y / b.Y);
    public static Point2 operator /(int a, Point2 b) => new(a / b.X, a / b.Y);
    public static Point2 operator /(Point2 a, int b) => new(a.X / b, a.Y / b);
    #endregion

    #region Implicit
    public static implicit operator Vec2(Point2 a) => new(a.X, a.Y);
    public static implicit operator Point2(int a) => new(a);

#if USE_MONOGAME
    public static implicit operator Vector2(Point2 a) => new(a.X, a.Y);
    public static implicit operator Point2(Point a) => new(a.X, a.Y);
#endif
    public static implicit operator Point3(Point2 a) => new(a.X, a.Y, 0);
    #endregion

    #region Explicit
#if USE_MONOGAME
    public static explicit operator Point(Point2 a) => new(a.X, a.Y);
#endif

    #endregion
    #endregion

    #region Basic Method
    public override string ToString() => X.ToString() + " , " + Y.ToString();
    public override bool Equals(object obj) => (obj != null && obj is Point2 p && p == this);
    public override int GetHashCode() => X * 1013 + Y; 
#endregion

#region Static Method
    public static Point2 Zero => new(0);
    public static Point2 One => new(1);

    public static Point2 UnitX => new(1, 0);
    public static Point2 UnitY => new(0, 1);

    public static Point2 MinusOne => new(-1);
    public static Point2 InverseX => new(-1,  1); 
    public static Point2 InverseY => new( 1, -1);

    public static Point2 InverseAxis(Point2 point) => new(point.Y, point.X);
    public static Vec2 GetVector(Point2 start, Point2 end) => new(start, end);

    public static Point2 LocatePointBetween(Point2 point, Point2 minSize, Point2 maxSize)
    {
        if (point.X < minSize.X)
        {
            point.X = minSize.X;
        }
        else if (point.X > maxSize.X)
        {
            point.X = maxSize.X;
        }

        if (point.Y < minSize.Y)
        {
            point.Y = minSize.Y;
        }
        else if (point.Y > maxSize.Y)
        {
            point.Y = maxSize.Y;
        }
        return point;
    }
#endregion
}
