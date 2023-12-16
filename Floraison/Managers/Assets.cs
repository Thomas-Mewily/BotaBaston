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

    public static Tex2 LoadTexture(string name) 
    {
        return All.Content.Load<Texture2D>(name);
    }

    public static void Load() 
    {
        Plant = LoadTexture("plant");
        Pot   = LoadTexture("pot");
    }
}
