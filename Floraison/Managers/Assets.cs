using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Useful;

namespace Floraison;

public class Assets
{
    private Assets() { }

    public static Tex2 Plant;
    public static Tex2 Pot;
    public static Tex2 Bramble;
    public static Tex2 BrambleSeed;

    public static List<Tex2> Suns;

    public static Tex2 LoadTexture(string name) 
    {
        return All.Content.Load<Texture2D>(name);
    }

    public static List<Tex2> LoadListTexture(string name)
    {
        var l = new List<Tex2>();
        int i = 0;
        while (true) 
        {
            try 
            {
                var t = LoadTexture(name + "_" + i);
                i++;
                l.Push(t);
            }
            catch 
            {
                l.Reverse();
                return l;
            }
        }
    }

    public static void Load() 
    {
        Plant = LoadTexture("plant");
        Pot   = LoadTexture("pot");
        Suns = LoadListTexture("sun");
        Bramble = LoadTexture("bramble");
        BrambleSeed = LoadTexture("bramble_seed");
    }
}
