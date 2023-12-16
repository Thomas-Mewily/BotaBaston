using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;

public class PlantBehaviorTest : PlantBehavior
{
    public PlantBehaviorTest(Plant p) :base(p) {}

    public override void Update()
    {
        Vec2 Target = P.Input.RightJoystick.Axis * 5 * P.Scale;
        P.PositionRelative = (P.PositionRelative * 16 + Target) / 17;
    }
}