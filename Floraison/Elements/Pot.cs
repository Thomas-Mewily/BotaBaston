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

    }
}
