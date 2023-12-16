using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;

public class Bramble : Entite
{

    public float Strenth;
    public override void Update()
    {
        foreach(var e in AllOtherEntitiesColliding()) 
        {
            // e.Speed += e.
        }
    }

    public override void Draw()
    {
        var c = Camera.Peek().Clone();
        c.Options.Blend = BlendState.AlphaBlend;
        Camera.Push(c);
        SpriteBatch.DrawCircle(Position, ScaledRadius, Color.Yellow);
        Camera.Pop();
    }
}
