namespace Floraison;

/// <summary>
/// In Game Time
/// </summary>
public struct GTime
{
    public int Frames;
    public int MsInt => Frames * 1000 / TheGame.FrameRate;

    public float Seconds { get => Frames / TheGame.FrameRate; set => Frames = (int)(value * TheGame.FrameRate); }
    public float Ms { get => Seconds * 1000; set => Seconds = value / 1000; }

    public static GTime OneSecond => new(60);

    public static GTime Hz60(int duration) => new(duration * TheGame.FrameRate / 60);
    public static GTime Second(float s) => new((int)(s * OneSecond.Seconds));
    public static GTime MilliSecond(float s) => new((int)((s/1000) * OneSecond.Seconds));

    private GTime(int frames) { Frames = frames; }
    public GTime(float seconds) { Frames = (int)(seconds * TheGame.FrameRate); }
    public static implicit operator GTime(float s) => new(s);
    public static implicit operator float(GTime t) => t.Seconds;

    public GTime Elapsed => (All.Game.Time - this);

    public static bool operator > (GTime a, GTime b) => a.Frames >  b.Frames;
    public static bool operator >=(GTime a, GTime b) => a.Frames >= b.Frames;
    public static bool operator < (GTime a, GTime b) => a.Frames <  b.Frames;
    public static bool operator <=(GTime a, GTime b) => a.Frames <= b.Frames;

    public static bool operator ==(GTime a, GTime b) => a.Frames == b.Frames;
    public static bool operator !=(GTime a, GTime b) => !(a == b);

    public static GTime operator -(GTime a, GTime b) => a.Frames - b.Frames;
    public static GTime operator +(GTime a, GTime b) => a.Frames + b.Frames;

    public override bool Equals(object obj) => obj != null && obj is GTime t && t == this;
    public override int GetHashCode() => Seconds.GetHashCode();
}