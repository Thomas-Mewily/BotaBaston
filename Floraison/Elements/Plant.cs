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
}

public class Plant : Entite
{
    public Pot PlantedIn => OwnedBy == null ? null : (Pot)OwnedBy;
    public PlantBehavior Behavior;

    private Vec2 offsetFlicker = Vec2.Zero;

    public override void Load()
    {
        Behavior = new PlantBehaviorMartin(this);
        Behavior.Load();
    }

    public override void Update()
    {
        Behavior.Update();
        //PositionRelative += Input.RightJoystick.UnitPerSecond * 4;
    }

    public override void Unload()
    {
        Behavior.Unload();
    }

    public override void Draw()
    {
        Color c = Input.IsConnected ? Teams.GetColor() : Color.White;
        /*
        if (Game.Time.MsInt / 250  % 2  == 0 && AllOtherEntitiesInsideMe().Any())
        {
            c = Color.White;
        }*/


        /*
        if((int)SpawnTime.Elapsed.Seconds % 2 == 0) 
        {
            c = Color.White;
        }
        */
        Behavior.Draw();


        SpriteBatch.DrawCircle(Position, ScaledRadius, c);

        Vec2 drawPos = Position + offsetFlicker;

        SpriteBatch.Draw(Assets.Plant, drawPos, null, c, Angle.Zero, Assets.Plant.Size() * 0.5f, 2*ScaledRadius / Assets.Plant.Size(), SpriteEffects.None, 0);
        
        
        if(OwnedBy != null && Input.IsConnected) 
        {
            SpriteBatch.DrawLine(drawPos, OwnedBy.Position, Color.Green, 0.25f);
        }
    }

    public void Flicker(float v)
    {
        offsetFlicker.X = All.Rng.FloatUniform(-v, v);
        offsetFlicker.Y = All.Rng.FloatUniform(-v, v);
    }
}
