using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;

public class Pot : Entite
{
    public override void Update()
    {
        Position += Input.LeftJoystick.UnitPerSecond;
        if (Input.B.JustPressed || Input.A.JustPressed) 
        {
            Position = Vec2.Zero;
        }
        SpriteBatch.DebugTextLn(Input.ToString());
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

        SpriteBatch.DrawEllipse(Position, ScaledRadius, c);
    }
}
