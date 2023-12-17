using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using Useful;

namespace Floraison;

public abstract class PlantBehavior : TimeRelated
{
    public PlantBehavior(Plant p) { P = p; }
    public Plant P;

    public virtual void Grow() { }
}

public class PlantPowerUp : TimeRelated
{
    public PlantPowerUp(Plant p) { P = p; }
    public Plant P;

    public override void Draw()
    {
        SpriteBatch.Draw(Assets.Plant, P.DrawPos, null, P.DrawColor, Angle.Zero, Assets.Plant.Size() * 0.5f, 2 * P.ScaledRadius / Assets.Plant.Size(), SpriteEffects.None, 0);
    }
}

public class Plant : Entite
{
    public Pot PlantedIn => OwnedBy == null ? null : (Pot)OwnedBy;
    public PlantBehavior Behavior;
    public PlantPowerUp  PowerUp;

    public Vec2 OffsetFlicker = Vec2.Zero;
    public Vec2 DrawPos => Position + OffsetFlicker;
    public Color DrawColor => Input.IsConnected ? Teams.GetColor() : Color.White;

    public int NbSeed = 3;

    public override void Load()
    {
        PowerUp = new PlantPowerUp(this);
        Behavior = new PlantBehaviorMartin(this);
        Behavior.Load();
    }

    public override void Update()
    {
        Behavior.Update();
        PowerUp.Update();
    }

    public override void Unload()
    {
        Behavior.Unload();
    }

    public override void Draw()
    {

        Behavior.Draw();


        // SpriteBatch.DrawCircle(Position, ScaledRadius, c);

        Vec2 drawPos = Position + OffsetFlicker;

        if (OwnedBy != null)
        {
            SpriteBatch.DrawLine(drawPos, OwnedBy.Position, Color.Green, 0.4f);
        }
        PowerUp.Draw();
    }

    public void Flicker(float v)
    {
        OffsetFlicker.X = All.Rng.FloatUniform(-v, v);
        OffsetFlicker.Y = All.Rng.FloatUniform(-v, v);
    }
}
