using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;

public class Bramble : Entite
{

    public float Strenth = 0.6f;
    public override void Update()
    {
        foreach(var e in AllOthersEntitiesAgainstMe().Inside(this)) 
        {
            Vec2 normal = e.Position - Position;
            normal.Normalize();
            e.Speed += normal * Strenth;
            System.Console.WriteLine(e.ToString());
        }
    }

    public override void Draw()
    {
        var c = Camera.Peek().Clone();
        c.Options.Blend = BlendState.AlphaBlend;
        Camera.Push(c);
        SpriteBatch.DrawCircle(Position, ScaledRadius, Color.Aqua);
        Camera.Pop();
    }
}
