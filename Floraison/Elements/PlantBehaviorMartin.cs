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
        if (P.Input.RightTrigger.JustReleased)
        {
            // P.PositionRelative.Normalize();
            P.Flicker(0f);
            P.Speed = P.PositionRelative * elasticStrenth;
            available = false;
        }
        else
        {
            if (P.Input.RightJoystick.IsNeutral || !available )
            {
                P.PositionRelative *= retractSpeed;
            }
            else
            {
                P.PositionRelative += P.Input.RightJoystick.UnitPerSecond * 30;
                if (P.Input.RightTrigger.IsPressed) //Stretch
                {
                    float stretchedSize = stemMaxLength * stemStreched;
                    if (P.PositionRelative.Length > stretchedSize)
                    {
                        P.PositionRelative.Normalize();
                        P.PositionRelative *= stretchedSize;
                        P.Flicker(0.2f);
                    }
                }
                else // Not pressed
                {
                    if (P.PositionRelative.Length > stemMaxLength)
                    {
                        P.PositionRelative.Normalize();
                        P.PositionRelative *= stemMaxLength;
                    }
                }

            }
        }

        if (P.Input.RightJoystick.IsNeutral) {available = true;}

    }
}