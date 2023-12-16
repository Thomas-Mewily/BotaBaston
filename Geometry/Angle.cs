using System;
using System.Diagnostics;

namespace Geometry;

public struct Angle
{
    #region Constructor
    public float Degree
    {
        get => Radian * TurnDegree / TurnRadian;
        set => Radian = value * TurnRadian / TurnDegree;
    }
    public float Radian { get; set; }
    public float One { get => Radian / TurnRadian; set => Radian = value * TurnRadian; }

    /// <summary>
    /// [0, 2PI[
    /// </summary>
    public Angle Normalized => FromRadian(Radian % TurnRadian + TurnRadian) % TurnRadian;

    /// <summary>
    /// ]PI; PI]
    /// </summary>
    public Angle NormalizedCenter { get { var tmp = Normalized; return tmp < FlatRadian ? tmp : tmp - TurnRadian; } }

    public float Cos => MathF.Cos(Radian);
    public float Sin => MathF.Sin(Radian);

    public bool Inside(Angle middle) => Inside(this - middle / 2, this + middle / 2);
    public bool Inside(Angle debut, Angle fin)
    {
        var angleNormalise = Normalized.Radian;
        var debutNormalise = debut.Normalized.Radian;
        var finNormalise   = fin.Normalized.Radian;

        if (debutNormalise <= finNormalise)
        {
            return angleNormalise >= debutNormalise && angleNormalise <= finNormalise;
        }
        else
        {
            return angleNormalise >= debutNormalise || angleNormalise <= finNormalise;
        }
    }

    #region Const
    public const float TurnRadian = MathF.PI * 2;
    public const float TurnDegree = 360;
    public const float TurnOne = 1;

    public const float FlatRadian = TurnRadian / 2;
    public const float FlatDegree = TurnDegree / 2;
    public const float FlatOne = TurnOne / 2;

    public const float RightRadian = TurnRadian / 4;
    public const float RightDegree = TurnDegree / 4;
    public const float RightOne = TurnOne / 4;
    #endregion

    /// <summary>
    /// Based on the rad because it is used by the system for trigo (ex: Math.Sin)
    /// </summary>
    /// <param name="fromRadianUsedBySystem"></param>
    public Angle(float fromRadianUsedBySystem)
    {
        Radian = fromRadianUsedBySystem;
    }

    #region Operator
    public static bool operator ==(Angle a, Angle b) => a.Radian == b.Radian;
    public static bool operator !=(Angle a, Angle b) => a.Radian != b.Radian;

    public static Angle operator +(Angle a, Angle b) => a.Radian + b.Radian;
    public static Angle operator -(Angle a, Angle b) => a.Radian - b.Radian;

    public static Angle operator *(int   a, Angle b) => a * b.Radian;
    public static Angle operator *(float a, Angle b) => a * b.Radian;
    public static Angle operator *(Angle a, float b) => b * a.Radian;
    public static Angle operator *(Angle a, int   b) => b * a.Radian;

    public static float operator /(Angle a, Angle b) => a.Radian / b.Radian;
    public static Angle operator /(Angle a, float b) => a.Radian / b;
    public static Angle operator /(Angle a, int   b) => a.Radian / b;

    public static implicit operator float(Angle a) => a.Radian;
    public static implicit operator Angle(float a) => FromRadian(a);
    #endregion

    #region Basic Method
    public override string ToString() => Degree + "°";
    public override bool Equals(object obj) => obj is Angle a && a == this;
    public override int GetHashCode() => Radian.GetHashCode();

    
    #endregion

    #region Static Methods
    public static Angle FromDegree(float degree) => new() { Degree = degree };
    public static Angle FromRadian(float radian) => new() { Radian = radian };
    public static Angle AngleFromOne(float one)  => new() { One = one };

    public static Angle Zero => FromRadian(0);
    #endregion
    #endregion
}