#if USE_MONOGAME
using Microsoft.Xna.Framework;
#endif
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry;

public struct Vec3
{
    #region Constructor
    public float X;
    public float Y;
    public float Z;

    #region Properties

    public float LengthSquared => X * X + Y * Y + Z * Z;
    public float Length { get=> MathF.Sqrt(LengthSquared); set => this = Normalized * value; }
    public bool HaveLength => HaveXLength || HaveYLength || HaveZLength;
    public bool HaveXLength => X != 0;
    public bool HaveYLength => Y != 0;
    public bool HaveZLength => Z != 0;

    public float XY { set { X = value; Y = value; } }
    public float XZ { set { X = value; Z = value; } }
    public float YZ { set { Y = value; Z = value; } }
    public float XYZ { set { X = value; Y = value; Z = value; } }
    public Vec3 Absolute => new(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));

    public bool IsNaN => (float.IsNaN(X) || float.IsNaN(Y) || float.IsNaN(Z));
    #endregion

    public Vec3(float x, float y, float z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vec3(Vec2 vec, float z = 0)
    {
        X = vec.X;
        Y = vec.Y;
        Z = z;
    }

    public Vec3(float xyz)
    {
        X = Y = Z = xyz;
    }

    public Vec3(Vec3 start, Vec3 end)
    {
        X = end.X - start.X;
        Y = end.Y - start.Y;
        Z = end.Z - start.Z;
    }
    #endregion

    #region Operator
    #region Equality
    public static bool operator ==(Vec3 a, Vec3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    public static bool operator !=(Vec3 a, Vec3 b) => a.X != b.X || a.Y != b.Y || a.Z != b.Z;
    #endregion

    #region Additive
    public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vec3 operator +(Vec3 a) => a;

    public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vec3 operator -(Vec3 a) => new(-a.X, -a.Y, -a.Z);
    #endregion

    #region Multiplicative
    public static Vec3 operator *(Vec3 a, Vec3 b)   => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    public static Vec3 operator *(float a, Vec3 b) => new(a * b.X, a * b.Y, a * b.Z);
    public static Vec3 operator *(Vec3 a, float b) => new(b * a.X, b * a.Y, b * a.Z);

    public static Vec3 operator /(Vec3 a, Vec3 b)   => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    public static Vec3 operator /(float a, Vec3 b) => new(a / b.X, a / b.Y, a / b.Z);
    public static Vec3 operator /(Vec3 a, float b) => new(a.X / b, a.Y / b, a.Z / b);
    #endregion

    #region Implicit
    public static implicit operator Vec3(float a) => new(a);

#if USE_MONOGAME
    public static implicit operator Vector3(Vec3 a) => new((float)a.X, (float)a.Y, (float)a.Z);
#endif
    public static implicit operator Vec3(Vector3 a) => new(a.X, a.Y, a.Z);
    #endregion

    #region Explicit
    public static explicit operator Point3(Vec3 a) => new((int)a.X, (int)a.Y, (int)a.Z);
    #endregion
    #endregion

    #region Basic Method & Interface
    public override string ToString() => X.ToString() + " / " + Y.ToString() + " / " + Z.ToString();
    public string ToString(string format) => X.ToString(format) + " / " + Y.ToString(format) + " / " + Z.ToString(format);
    public override bool Equals(object obj) => (obj != null && obj is Vec3 v && v == this);
    public override int GetHashCode() => (X * 1031 + Y + Z * 443).GetHashCode();
    #endregion

    #region Other Methods
    public Point2 ToPoint2Round() => new((int)Math.Round(X), (int)Math.Round(Y));
    #endregion

    #region Static Methods
    public static Vec3 Zero => new(0);
    public static Vec3 One  => new(1);
    public static Vec3 Half => new(0.5f);

    public static Vec3 UnitX => new(1, 0, 0);
    public static Vec3 UnitY => new(0, 1, 0);
    public static Vec3 UnitZ => new(0, 0, 1);


    public static Vec3 InverseX => new(-1,  1,  1);
    public static Vec3 InverseY => new( 1, -1,  1);
    public static Vec3 InverseZ => new( 1,  1, -1);

    /// <summary> Just in 2D </summary>
    public Vec3 SwapAxis() => new(Y, X, Z);
    public void Normalize() => this = Normalized;
    public Vec3 Normalized => HaveLength ? this / Length : Zero;
    #endregion
}
