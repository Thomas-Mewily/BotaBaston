using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;

public class PlantBehaviorMartin : PlantBehavior
{
    public PlantBehaviorMartin(Plant p) : base(p) { }

    public override void Grow()
    {
        StemMaxLength += 0.5f;
    }

    //taille max de la tige (hors étirage)
    public float StemMaxLength = 4f;

    //Taille de la tige max quand étirée (par rapport a stemMaxLength)
    public float StemStreched = 1.15f;

    // Inverse de la force qui attire la plante au pot
    public float RetractSpeed = .85f;
    //Pourcentage de la distance de la plante transformée en vitesse
    public float ElasticStrenth = -0.25f;

    //Quantité de vitesse de la plante transférée au pot
    public float PotStrenth = 0.4f;

    //Distance a laquelle la plante doit etre du pot pour pouvoir controller de nouveau la plante après un lancé
    public float ControlDist = 0.4f;

    //Indique si la plante peut etre controlée, utilisé dans la physique
    public bool Available = true;


    public void SpawBrambleOnClick()
    {
        if (All.KbMInput.MouseLeftReleased && P.Input.PlayerControl == Controller.PlayerControlEnum.One)
        {
            Vec2 temp = Camera.Peek().ToWorldPosition((Vec2)All.KbMInput.Mouse.Position.ToVector2());
            if (Game.WorldHitbox.IsCollidingWith(temp))
            {
                BrambleSeed b = new BrambleSeed(temp);
                b.Spawn();
            }
        }
    }

    public void RightClickAction(Vec2 clickLocation)
    {
        GameLoader.logicRef.SpawnPowerUp();
    }

    public override void Update()
    {
        
        SpawBrambleOnClick();
        Vec2 newPosR = P.PositionRelative;

        if (All.KbMInput.MouseRightReleased && P.Input.PlayerControl == Controller.PlayerControlEnum.One)
        {
            Vec2 temp = Camera.Peek().ToWorldPosition((Vec2)All.KbMInput.Mouse.Position.ToVector2());
            if (Game.WorldHitbox.IsCollidingWith(temp))
            {

                RightClickAction(temp);
            }
        }


        if (P.Input.RightTrigger.JustReleased)
        {
            // newPosR.Normalize();
            P.Flicker(0f);
            P.Speed = newPosR * ElasticStrenth;
            P.PlantedIn.Speed = newPosR * ElasticStrenth * PotStrenth;
            Available = false;
        }
        else
        {
            if (P.Input.RightJoystick.IsNeutral || !Available) //(!Available && LastTimeExtended.Elapsed.Seconds >= 3f))
            {
                newPosR *= RetractSpeed;
            }
            else
            {
                newPosR += P.Input.RightJoystick.UnitPerSecond * 30;
                if (P.Input.RightTrigger.IsPressed) //Stretch
                {
                    float stretchedSize = StemMaxLength * StemStreched;
                    if (newPosR.Length > stretchedSize)
                    {
                        newPosR.Length = stretchedSize;
                        P.Flicker(0.2f);
                    }
                }
                else // Not pressed
                {
                    if (newPosR.Length > StemMaxLength)
                    {
                        newPosR.Length = StemMaxLength;
                    }
                }
            }
        }

        if (P.Input.RightJoystick.IsNeutral || newPosR.Length <= ControlDist * StemMaxLength) {Available = true;}

        P.PositionRelative = newPosR;

        if (P.Input.LeftTrigger.JustPressed && P.NbSeed > 0)
        {
            P.NbSeed--;
            BrambleSeed b = new BrambleSeed(P.Position);
            b.Spawn();
        }

    }
}

//Idées musiques 
/*
https://www.youtube.com/watch?v=-opUgucsoHs&list=PL0A12ED38862DB0F5&index=15

*/