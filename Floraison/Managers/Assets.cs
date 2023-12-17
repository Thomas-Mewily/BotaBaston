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
    public static Tex2 Petal;
    public static Tex2 PowerUpPetal;

    public static List<Tex2> Suns;
    public static List<Tex2> Spirals;

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
        Bramble = LoadTexture("bramble");
        BrambleSeed = LoadTexture("bramble_seed");

        Suns = LoadListTexture("sun");
        Spirals = LoadListTexture("spiral");
        Petal = LoadTexture("petal");
        PowerUpPetal = LoadTexture("power_up_petal");
    }
}
