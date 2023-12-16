using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;

public class Plant : Entite
{
    public Texture2D PlantSprite;

    public Pot PlantedIn;
    public override void Load()
    {
        PlantSprite = Content.Load<Texture2D>("pot");
    }

    public override void Update()
    {
        Position += Input.RightJoystick.UnitPerSecond * 3;
        if (Input.B.JustPressed || Input.A.JustPressed) 
        {
            Position = Vec2.Zero;
        }
        // SpriteBatch.DebugTextLn(Input.ToString());
    }

    public override void Draw()
    {
        Color c = Teams.GetColor();

        if (Game.Time.MsInt / 250  % 2  == 0 && AllOtherEntitiesInsideMe().Any())
        {
            c = Color.White;
        }


        /*
        if((int)SpawnTime.Elapsed.Seconds % 2 == 0) 
        {
            c = Color.White;
        }*/

        c.A = 128;
        // SpriteBatch.DrawCircle(Position, ScaledRadius, c);

        SpriteBatch.DrawLine(PlantedIn.Position, PlantedIn.Position + Position, Color.Green, 0.25f);
        SpriteBatch.Draw(PlantSprite, Position + PlantedIn.Position, null, Color.LimeGreen, Angle.Zero, PlantSprite.Size() * 0.5f, 2*ScaledRadius / PlantSprite.Size(), SpriteEffects.None, 0);
    }
}
