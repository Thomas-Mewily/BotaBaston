using System;
using System.Collections.Immutable;
using System.Text;

namespace Useful;

public static class Extension 
{
    public static void Push<T>(this IList<T> l, T val) => l.Add(val);
    public static bool IsEmpty<T>(this IList<T> l) => l.Count == 0;
    public static T Peek<T>(this IList<T> l) =>  l[l.Count - 1];
    public static T Pop<T>(this IList<T> l) 
    {
        T t = l.Peek();
        l.RemoveAt(l.Count - 1);
        return t;
    }

    public static bool IsEmpty(this string s) => s.Length == 0;

    // Thank to https://stackoverflow.com/questions/17590528/pad-left-pad-right-pad-center-string
    public static string PadMiddle(this string str, int length)
    {
        int spaces = length - str.Length;
        int padLeft = spaces / 2 + str.Length;
        return str.PadLeft(padLeft).PadRight(length);
    }
}