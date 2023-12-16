using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;

public class Plant : Entite
{
    //public Texture2D PotSprite;

    public override void Load()
    {
        //PotSprite = Content.Load<Texture2D>("pot");
    }

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


    }
}
