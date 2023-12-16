using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;


public class Plant : Entite
{
    public Pot PlantedIn => OwnedBy == null ? null : (Pot)OwnedBy;

    

    public override void Update()
    {
        
        PositionRelative += Input.RightJoystick.UnitPerSecond * 30;
        
    }

    public override void Draw()
    {
        Color c = Input.IsConnected ? Color.White : Teams.GetColor();
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

        c.A = 128;
        // SpriteBatch.DrawCircle(Position, ScaledRadius, c);

        SpriteBatch.Draw(Assets.Plant, Position, null, Color.LimeGreen, Angle.Zero, Assets.Plant.Size() * 0.5f, 2*ScaledRadius / Assets.Plant.Size(), SpriteEffects.None, 0);
        
        
        if(OwnedBy != null && Input.IsConnected) 
        {

            SpriteBatch.DrawLine(Position, OwnedBy.Position, Color.Green, 0.25f);
        }
    }
}
