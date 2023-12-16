namespace Useful;

public static class Crash
{
    public static void Check(this bool mustBeTrue, string? message = null)
    {
        if (!mustBeTrue)
        {
            Now(message == null ? message : "was false");
        }
    }

    public static void Now(string? message = null) 
    {
        throw new Exception(message == null ? message : "");
    }
}