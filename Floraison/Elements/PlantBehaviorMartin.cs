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
    private float stemMaxLength = 5f;
    private float stemStreched = 1.15f;

    private float retractSpeed = .85f;
    private float elasticStrenth = -0.5f;

    /// <summary>
    /// Indique si la plante peut etre controlée, utilisé dans la physique
    /// </summary>
    private bool available = true;

    public override void Update()
    {
        Vec2 newPosR = P.PositionRelative;
        if (P.Input.RightTrigger.JustReleased)
        {
            // newPosR.Normalize();
            P.Flicker(0f);
            P.Speed = newPosR * elasticStrenth;
            P.PlantedIn.Speed = newPosR * elasticStrenth * 0.2f;
            available = false;
        }
        else
        {
            if (P.Input.RightJoystick.IsNeutral || !available )
            {
                newPosR *= retractSpeed;
            }
            else
            {
                newPosR += P.Input.RightJoystick.UnitPerSecond * 30;
                if (P.Input.RightTrigger.IsPressed) //Stretch
                {
                    float stretchedSize = stemMaxLength * stemStreched;
                    if (newPosR.Length > stretchedSize)
                    {
                        newPosR.Normalize();
                        newPosR *= stretchedSize;
                        P.Flicker(0.2f);
                    }
                }
                else // Not pressed
                {
                    if (newPosR.Length > stemMaxLength)
                    {
                        newPosR.Normalize();
                        newPosR *= stemMaxLength;
                    }
                }
            }
        }

        if (P.Input.RightJoystick.IsNeutral) {available = true;}

        P.PositionRelative = newPosR;

    }
}