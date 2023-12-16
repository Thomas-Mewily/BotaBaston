using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;

public class PlantBehaviorTest : PlantBehavior
{
    public PlantBehaviorTest(Plant p) : base(p) { }

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
    public GTime DashUsed;

    public override void Update()
    {
        Vec2 newPosR = P.PositionRelative;


        if (P.Input.RightTrigger.JustReleased)
        {
            // newPosR.Normalize();
            P.Flicker(0f);
            P.Speed = newPosR * ElasticStrenth;
            P.PlantedIn.Speed = newPosR * ElasticStrenth * PotStrenth;
            Available = false;
            DashUsed = P.Game.Time;
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

        if (P.Input.RightJoystick.IsNeutral || newPosR.Length <= ControlDist * StemMaxLength) { Available = true; }

        P.PositionRelative = newPosR;

    }
}