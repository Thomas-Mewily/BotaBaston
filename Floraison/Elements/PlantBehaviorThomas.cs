using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Useful;

namespace Floraison;

public class PlantBehaviorThomas : PlantBehavior
{
    public List<Vec2> Points;
    public float TotalLengthScale = 5;
    public float MaxLengthWithScale = 1;

    public PlantBehaviorThomas(Plant p) : base(p)
    {
        Points = new List<Vec2>();
        for(int i = 0; i < 128; i++) 
        {
            Points.Push(new Vec2(0, 0));
        }
    }

    public override void Update()
    {
        MaxLengthWithScale = TotalLengthScale*P.Scale / Points.Count;
        Vec2 Target = P.Input.RightJoystick.Axis * 5 * P.Scale;
        P.PositionRelative = (P.PositionRelative * 16 + Target) / 17;

        //Points[Points.Count - 1] = (Points[Points.Count - 1] * 16 + P.PositionRelative) / 17;
        Points[Points.Count - 1] = P.PositionRelative;

        for(int j = 0; j < 1; j++) 
        {
            for (int i = Points.Count - 1; i >= 1; i--)
            {
                Vec2 next = Points[i];
                Vec2 old = Points[i - 1];
                float coef = (i - 1) / (float)Points.Count;
                //old = (old * 2 + next) / 3;
                old = next * coef + old * (1 - coef);
                Points[i - 1] = old;
            }
        }
    }

    public override void Draw()
    {
        if (P.OwnedBy != null && P.Input.IsConnected)
        {
            var p = P.OwnedBy.Position;

            for (int i = Points.Count - 1; i >= 1; i--)
            {
                Vec2 next = Points[i];
                Vec2 old = Points[i - 1];
                SpriteBatch.DrawLine(p + old, p + next, Color.Green, 0.25f);
            }
        }
    }
}