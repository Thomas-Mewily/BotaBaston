#if USE_MONOGAME
using Microsoft.Xna.Framework;
#endif
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry;

public struct Vec2
{
    #region Constructor
    public float X;
    public float Y;

    #region Properties
    public float LengthSquared => X * X + Y * Y;
    public float Length { get => MathF.Sqrt(LengthSquared); set { this = Normalized * value; } }
    public bool HaveLength => HaveXLength || HaveYLength;
    public bool HaveXLength => X != 0;
    public bool HaveYLength => Y != 0;

    public Vec2 WithLength(float length) { Length = length; return this; }

    public float XY { set { X = value; Y = value; } }
    public Vec2 Absolute => new(Math.Abs(X), Math.Abs(Y));

    public Angle Angle
    {
        get => Angle.FromRadian(MathF.Atan2(Y, X));
        set
        {
            this = Vec2.FromAngle(value, Length);
        }
    }

    public float RatioXY => X / Y;
    public float RatioYX => Y / X;

    public bool IsNaN => (float.IsNaN(X) || float.IsNaN(Y));
    #endregion

    public Vec2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public Vec2(float xy)
    {
        X = Y = xy;
    }

    public Vec2(Vec2 start, Vec2 end)
    {
        X = end.X - start.X;
        Y = end.Y - start.Y;
    }
    #endregion

    #region Operator
    #region Equality
    public static bool operator ==(Vec2 a, Vec2 b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Vec2 a, Vec2 b) => a.X != b.X || a.Y != b.Y;
    #endregion

    #region Additive
    public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vec2 operator +(Vec2 a) => a;

    public static Vec2 operator -(Vec2 a, Vec2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vec2 operator -(Vec2 a) => new(-a.X, -a.Y);
    #endregion

    #region Multiplicative
    public static Vec2 operator *(Vec2 a, Vec2 b) => new(a.X * b.X, a.Y * b.Y);
    public static Vec2 operator *(float a, Vec2 b) => new(a * b.X, a * b.Y);
    public static Vec2 operator *(Vec2 a, float b) => new(b * a.X, b * a.Y);

    public static Vec2 operator *(Vec2 a, Angle b) => Vec2.MultiplyAngle(a, b);


    public static Vec2 operator /(Vec2 a, Vec2 b) => new(a.X / b.X, a.Y / b.Y);
    public static Vec2 operator /(float a, Vec2 b) => new(a / b.X, a / b.Y);
    public static Vec2 operator /(Vec2 a, float b) => new(a.X / b, a.Y / b);
    #endregion

    #region Implicit
    //public static implicit operator Vec3(Vec2 a) => new Vec3(a);
    public static implicit operator Angle(Vec2 a) => a.Angle;
    public static implicit operator Vec2(float a) => new(a);

#if USE_MONOGAME
    public static implicit operator Microsoft.Xna.Framework.Vector2(Vec2 a) => new(a.X,a.Y);
    public static implicit operator Vec2(Microsoft.Xna.Framework.Vector2 a) => new(a.X,a.Y);
#endif
    //public static implicit operator CoefVec2(Vec2 a) => new CoefVec2(a.X, a.Y);
    //public static implicit operator Vector2(Vec2 a) => new Vector2((float)a.X, (float)a.Y);
    //public static implicit operator Vec2(Vector2 a) => new Vec2(a.X, a.Y);
    #endregion

    #region Explicit
    //public static explicit operator Point2(Vec2 a) => new Point2((int)a.X, (int)a.Y);
    //public static explicit operator Point3(Vec2 a) => new Point3((int)a.X, (int)a.Y, 0);
    //public static explicit operator Vec2(Point a) => new Vec2(a.X, a.Y);
    #endregion
    #endregion

    #region Basic Methods
    public override string ToString() => X.ToString() + " / " + Y.ToString();
    public string ToString(string format) => X.ToString(format) + " / " + Y.ToString(format);
    public override bool Equals(object obj) => (obj != null && obj is Vec2 v && v == this);
    public override int GetHashCode() => (X * 10000 + Y).GetHashCode();
#endregion

    #region Static Methods
    public static Vec2 Zero => new(0);
    public static Vec2 One  => new(1);

    public static Vec2 UnitX => new(1, 0);
    public static Vec2 UnitY => new(0, 1);

    public static Vec2 Half  => new(0.5f);

    public static Vec2 InverseX => new(-1, 1);
    public static Vec2 InverseY => new( 1, -1);

    public float DotProduct(Vec2 b) => X * b.X + Y * b.Y;
    //public static float DotProduct(Angle a, Angle b) => DotProduct(FromAngle(a), FromAngle(b));

    public Vec2 SwapAxis() => new(Y, X);
    public void Normalize() => this = Normalized;
    public Vec2 Normalized => HaveLength ? this / Length : Zero;

    public static Vec2 Perpendicular(Vec2 a) => new(-a.Y, a.X);
    public static Vec2 FromAngle(Angle angle, float length = 1) => new(MathF.Cos(angle.Radian) * length, MathF.Sin(angle.Radian) * length);

    public static Vec2 FromAngleMostCollinear(Angle angleToFollow, Angle angle, float length = 1)
    {
        Vec2 follow = FromAngle(angleToFollow, 1);

        Vec2 direction1 = FromAngle(angle, length);
        if (follow.DotProduct(direction1) < 0)
        {
            direction1 = FromAngle(Angle.FromRadian(angle.Radian + Angle.FlatRadian), length);
        }
        return direction1;
    }

    public static bool SegmentsIntersect(Vec2 A, Vec2 B, Vec2 C, Vec2 D)
        => (Determinant(new Vec2(B, A), new Vec2(C, A)) * Determinant(new Vec2(B, A), new Vec2(D, A)) < -float.Epsilon &&
           Determinant(new Vec2(D, C), new Vec2(A, C)) * Determinant(new Vec2(D, C), new Vec2(B, C)) < -float.Epsilon);

    public static double Determinant(Vec2 A, Vec2 B) => A.X * B.Y - A.Y * B.X;

    public static Vec2 MultiplyAngle(Vec2 vec2, Angle angle) => new(vec2.X * MathF.Cos(angle.Radian) - vec2.Y * MathF.Sin(angle.Radian), vec2.X * MathF.Sin(angle.Radian) + vec2.Y * MathF.Cos(angle.Radian));
    #endregion
}


