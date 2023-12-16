using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using Useful;

namespace Floraison;

public class Bramble : Entite
{

    public Color Tint;
    public float Rota;
    public SpriteEffects Direction;
    public float Strenth = 0.6f;

    public Bramble()
    {
        float RB = All.Rng.FloatUniform(0.5f, 1);
        Tint = new Color(RB, All.Rng.FloatUniform(RB, 1), RB);
        Rota = Angle.FromDegree(All.Rng.FloatUniform(0, 360));
        Direction = (SpriteEffects)All.Rng.IntUniform(0, 2);
        
    }
    public override void Update()
    {
        foreach(var e in AllOthersEntitiesAgainstMe().Inside(this)) 
        {
            Vec2 normal = e.Position - Position;
            normal.Normalize();
            e.Speed += normal * Strenth;
            // System.Console.WriteLine(e.ToString());
        }
    }

    public override void Draw()
    {
        var c = Camera.Peek().Clone();
        c.Options.Blend = BlendState.AlphaBlend;
        Camera.Push(c);
        // SpriteBatch.DrawCircle(Position, Hitbox.Radius, Color.Aqua);
        SpriteBatch.Draw(Assets.Bramble, (Vector2)Position, null, Tint, Rota, Assets.Bramble.Size() * .5f, 2*1.5f*Hitbox.Radius / Assets.Bramble.Size(), Direction, 0);

        Camera.Pop();
    }
}
