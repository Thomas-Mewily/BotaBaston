using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;

public class Pot : Entite
{
    public Plant PlantInside { get; private  set; }

    public void Plant(Plant p) 
    {
        PlantInside = p;
        p.OwnedBy = this;
    }

    public override void Update()
    {
        Position += Input.LeftJoystick.UnitPerSecond * 3;
        if (PlantInside != null) 
        {
            float targetScale = PlantInside.IsFlickering ? 2.25f : 1f;
            Scale = (Scale * 63 + targetScale * 1) / 64f;
        }
    }

    public override void Draw()
    {
        SpriteBatch.DrawCircle(Position, ScaledRadius, Teams.GetColor());

        SpriteBatch.Draw(Assets.Pot, Position, null, Color.White, Angle.Zero, Assets.Pot.Size() * 0.5f, 2 * ScaledRadius / Assets.Pot.Size(), SpriteEffects.None, 0);
    }
}
