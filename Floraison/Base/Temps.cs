namespace InteractionSauvage;

public struct Time
{
    public int T { get; set; }

    public static Time OneSecond => new Time(60);

    public static Time Second(float s) => new Time((int)(s * OneSecond.T));
    public static Time MilliSecond(float s) => new Time((int)((s/1000) * OneSecond.T));

    public Time(int t) { T = t; }
    public static implicit operator Time(int i) => new(i);
    public static implicit operator int(Time t) => t.T;

    public static bool operator > (Time a, Time b) => a.T >  b.T;
    public static bool operator >=(Time a, Time b) => a.T >= b.T;
    public static bool operator < (Time a, Time b) => a.T <  b.T;
    public static bool operator <=(Time a, Time b) => a.T <= b.T;

    public static bool operator ==(Time a, Time b) => a.T == b.T;
    public static bool operator !=(Time a, Time b) => !(a == b);

    public static Time operator -(Time a, Time b) => a.T - b.T;

    public static Time operator ++(Time a) => a.T + 1;

    public override bool Equals(object? obj) => obj != null && obj is Time t && t == this;
    public override int GetHashCode() => T;
}