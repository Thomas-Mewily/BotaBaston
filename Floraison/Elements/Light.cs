using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;

public class Light : Entite
{
    public float ScorePerSecond = 1;

    public override void Update()
    {
        foreach(var t in EntitiesControlledByActivePlayer()) 
        {
            t.Score += ScorePerSecond / FrameRate;
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
