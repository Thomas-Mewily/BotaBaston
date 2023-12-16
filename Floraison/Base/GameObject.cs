using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Floraison;

public class Entite : GameRelated
{
    public void Spawn() => Game.Spawn(this);

    public enum PlayerControlEnum
    {
        NotControlledByAPlayer = 0,
        One   = 1,
        Two   = 2,
        Three = 3,
        Four  = 4,
        Five  = 5,
        Six   = 6,
        Seven = 7,
        Height= 8,
    }

    public enum SpawnStateEnum 
    {
        Unknow,
        CreatedThisFrame,
        Active,
        DeleteMePlz,
    }

    public SpawnStateEnum SpawnState = SpawnStateEnum.Unknow;
    public PlayerControlEnum PlayerControl = PlayerControlEnum.NotControlledByAPlayer;

    public Vec2 Position = 1;
    public float Radius  = 1;

    public float Scale   = 1;

    public Circle Hitbox => new Circle(Position, Radius * Scale);
}
