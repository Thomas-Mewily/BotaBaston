using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;

public class Pot : Entite
{
    public Texture2D PotSprite;
    public Plant PlantInside;

    public override void Load()
    {
        PotSprite = Content.Load<Texture2D>("pot");
    }

    public override void Update()
    {
        Position += Input.LeftJoystick.UnitPerSecond * 10;
        if (Input.B.JustPressed || Input.A.JustPressed) 
        {
            Position = Vec2.Zero;
        }
        
    }

    public override void Draw()
    {
        SpriteBatch.Draw(PotSprite, Position, null, Color.White, Angle.Zero, PotSprite.Size() * 0.5f, 2*ScaledRadius / PotSprite.Size(), SpriteEffects.None, 0);


        /*
        if((int)SpawnTime.Elapsed.Seconds % 2 == 0) 
        {
            c = Color.White;
        }
        */

        c.A = 128;
        SpriteBatch.DrawCircle(Position, ScaledRadius, c);

        SpriteBatch.Draw(Assets.Pot, Position, null, Color.White, Angle.Zero, Assets.Pot.Size() * 0.5f, 2 * ScaledRadius / Assets.Pot.Size(), SpriteEffects.None, 0);
        SpriteBatch.DrawLine(Position, Position + Vec2.One * 2, Color.Green, 0.25f);
    }
}
