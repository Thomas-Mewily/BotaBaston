using System;
using System.Diagnostics;

namespace Geometry;

public struct Circle
{
    public Vec2 Center;
    public float Radius;

    public Circle(Vec2 center, float radius = 1)
    {
        Center = center;
        Radius = radius;
    }

    public bool IsCollidingWith(Circle other) 
    {
        //return (Center - other.Center).LengthSquared <= Radius * Radius + other.Radius * other.Radius;
        return (Center - other.Center).Length < Radius+other.Radius;
    }
}