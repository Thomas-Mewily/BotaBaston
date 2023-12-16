#if USE_MONOGAME
using Microsoft.Xna.Framework;
#endif
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry;

public struct Point3
{
    #region Constructor
    #region Properties
    public int X;
    public int Y;
    public int Z;

    public float LengthSquared => X * X + Y * Y + Z * Z;
    public float Length => MathF.Sqrt(LengthSquared);
    
    public bool HaveLength  => HaveXLength || HaveYLength;
    public bool HaveXLength => X != 0;
    public bool HaveYLength => Y != 0;
    #endregion

    public Point3(int x, int y, int z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Point3(int xyz)
    {
        X = Y = Z = xyz;
    }

    public Point3(Point3 start, Point3 end)
    {
        X = end.X - start.X;
        Y = end.Y - start.Y;
        Z = end.Z - start.Z;
    }
    #endregion

    #region Operator
    #region Equality
    public static bool operator ==(Point3 a, Point3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    public static bool operator !=(Point3 a, Point3 b) => a.X != b.X || a.Y != b.Y || a.Z != b.Z;
    #endregion

    #region Additive
    public static Point3 operator +(Point3 a, Point3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Point3 operator +(Point3 a) => a;

    public static Point3 operator -(Point3 a, Point3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Point3 operator -(Point3 a) => a;
    #endregion

    #region Multiplicative
    public static Point3 operator *(Point3 a, Point3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    public static Point3 operator *(int a, Point3 b) => new(a * b.X, a * b.Y, a * b.Z);
    public static Point3 operator *(Point3 a, int b) => new(b * a.X, b * a.Y, a.Z * b);

    public static Point3 operator /(Point3 a, Point3 b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    public static Point3 operator /(int a, Point3 b) => new(a / b.X, a / b.Y, a / b.Z);
    public static Point3 operator /(Point3 a, int b) => new(a.X / b, a.Y / b, a.Z / b);
    #endregion

    #region Implicit
    public static implicit operator Vec3(Point3 a) => new(a.X, a.Y, a.Z);
    public static implicit operator Point3(int a) => new(a);

    #endregion
    #endregion

    #region Basic Method
    public override string ToString() => X.ToString() + " , " + Y.ToString() + " , " + Z.ToString();
    public string ToString(string format) => X.ToString(format) + " , " + Y.ToString(format) + " , " + Z.ToString(format);
    public override bool Equals(object obj) => (obj != null && obj is Point3 p && p == this);
    public override int GetHashCode() => X * 1031 +  Y + Z * 443;
    #endregion

    #region Static Method
    public static Point3 Zero => new(0);
    public static Point3 One  => new(1);

    public static Point3 UnitX => new(1, 0, 0);
    public static Point3 UnitY => new(0, 1, 0);
    public static Point3 UnitZ => new(0, 0, 1);

    public static Point3 MinusOne => new(-1);
    public static Point3 InverseX => new(-1, 1, 1);
    public static Point3 InverseY => new( 1,-1, 1);
    public static Point3 InverseZ => new( 1, 1,-1);
    #endregion
}
