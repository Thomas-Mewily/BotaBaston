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

    public override void Update()
    {
        P.PositionRelative += P.Input.RightJoystick.UnitPerSecond * 4;
    }
}