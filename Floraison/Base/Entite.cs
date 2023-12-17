﻿using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static Floraison.Controller;
using static Floraison.Entite;

namespace Floraison;

public class Entite : GameRelated
{
    /// <summary>
    /// Affect Relative Position And Teams
    /// </summary>
    public Entite OwnedBy = null;

    public Entite Spawn()
    {
        Game.Spawn(this);
        return this;
    }


    public enum TeamsEnum
    {
        // Tout seul
        Alone = 0,

        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,

        // Environnement and stuff
        Neutral,
    }

    public bool SameTeams(Entite e) => ReferenceEquals(this, e) ? true : SameTeams(e.Teams);
    public bool SameTeams(TeamsEnum teams)
    {
        return Teams switch
        {
            TeamsEnum.Alone => teams == TeamsEnum.Neutral,
            TeamsEnum.Neutral => true,
            _ => Teams == teams,
        };
    }

    public enum SpawnStateEnum
    {
        Unknow,
        CreatedThisFrame,
        Active,
        DeleteMePlz,
        /// <summary>
        /// Waiting for the garbage collector...
        /// </summary>
        NotInTheGame,
    }
    public void DeleteMe() 
    {
        SpawnState = SpawnStateEnum.DeleteMePlz;
    }



    public const int CollisionLayerUnknow = 0b1;
    public const int CollisionLayerPot    = 0b10;
    public const int CollisionLayerPlant  = 0b100;
    public const int CollisionLayerBramble  = 0b1000;
    public int CollisionLayer = 0;

    public void CollisionLayerAdd(int layer) { CollisionLayer |= layer; } 
    public void CollisionLayerRemove(int layer) { CollisionLayer &= (~layer); } 
    public void CollisionLayerToggle(int layer) { CollisionLayer ^= layer; }
    public bool IsOnCollisionLayer(int layer) { return (CollisionLayer & layer) != 0; }

    public bool LayerOverlap(int layer) => (CollisionLayer & layer) != 0;
    public bool LayerOverlap(Entite e) => LayerOverlap(e.CollisionLayer);

    public enum CollisionEnableEnum
    {
        Enable,
        Disable,
    }
    public enum CollisionTypeEnum
    {
        // L'entité ne recoit pas l'inertie , mais elle collisionne quand meme avec les autres 
        Fixed,
        // L'entité recoit l'inertie
        Free,
    }

    public void BasedOn(Entite e)
    {
        Teams = e.Teams;
        Position = e.Position;
        Radius = e.Radius;
        Scale = e.Scale;
    }

    public TeamsEnum _Teams = TeamsEnum.Neutral;
    public TeamsEnum Teams
    {
        get => OwnedBy == null ? _Teams : OwnedBy.Teams;
        set
        {
            _Teams = value;
            if (OwnedBy != null)
            {
                OwnedBy.Teams = value;
            }
        }
    }
    public SpawnStateEnum SpawnState = SpawnStateEnum.Unknow;
    private PlayerControlEnum _PlayerControl = PlayerControlEnum.NotControlledByAPlayer;
    public PlayerControlEnum PlayerControl 
    {
        get => OwnedBy == null ? _PlayerControl : OwnedBy.PlayerControl;
        set { _PlayerControl = value; if (OwnedBy != null) { OwnedBy.PlayerControl = value; } }
    }
    public Controller Input => From(PlayerControl);

    public CollisionEnableEnum CollisionEnable = CollisionEnableEnum.Enable;

    public void MoveRelative(Vec2 add) => MoveRelative(add.X, add.Y);
    public void MoveRelative(Vec2 add, CollisionEnableEnum type) => MoveRelative(add.X, add.Y, type);
    public void MoveRelative(float x, float y) => MoveRelative(x, y, CollisionEnable);
    public void MoveRelative(float x, float y, CollisionEnableEnum type) 
    {
        // PositionRelativeNoCollision += new Vec2(x, y);
        // foreach (var v in AllOtherEntitiesColliding())
        // {
        //     var delta = new Vec2(Position, v.Position).WithLength(ScaledRadius + v.ScaledRadius+1/64f);
        //     if (v.collisionType == CollisionTypeEnum.Free)
        //     {
        //         v.Speed += delta.WithLength((Speed).Length);
        //         Console.WriteLine(delta);
        //         v.PositionNoCollision = Position + delta;
        //     }
        //     else if (v.collisionType == CollisionTypeEnum.Fixed)
        //     {
        //         PositionNoCollision = v.Position - delta;
        //     }
        //     if (collisionType == CollisionTypeEnum.Free)
        //     {
        //         Speed -= delta.WithLength(v.Speed.Length)*0.5f;
        //     }
        // }

        PositionRelativeNoCollision += new Vec2(x, y);
        foreach (var v in AllOtherEntitiesColliding())
        {
            var delta = new Vec2(Position, v.Position).WithLength(ScaledRadius + v.ScaledRadius+1/64f);
            v.PositionNoCollision = Position + delta;
            v.Speed += delta.WithLength((Speed).Length);
            //Speed   -= delta.WithLength((v.Speed).Length);
            Speed   -= delta.WithLength(v.Speed.Length)*0.5f;
        }
    }

    public CollisionTypeEnum collisionType = CollisionTypeEnum.Free;

    public GTime SpawnTime;

    public Vec2 PositionRelativeNoCollision = 0;

    /// <summary>
    /// Also check for collision
    /// </summary>
    public Vec2 PositionRelative 
    {
        get => PositionRelativeNoCollision;
        set => MoveRelative(new Vec2(PositionRelativeNoCollision, value));
    }

    /// <summary>
    /// Also check for collision
    /// </summary>
    public Vec2 Position 
    {
        get => OwnedBy == null ? PositionRelative : OwnedBy.Position + PositionRelative;
        set 
        {
            PositionRelative = (OwnedBy == null ? value : value - OwnedBy.Position);
            if(OwnedBy == null) 
            {
                PositionRelative = new Vec2(Math.Min(Math.Max(PositionRelative.X, Game.WorldHitbox.XMin + ScaledRadius), Game.WorldHitbox.XMax - ScaledRadius),
                    Math.Min(Math.Max(PositionRelative.Y, Game.WorldHitbox.YMin + ScaledRadius), Game.WorldHitbox.YMax - ScaledRadius));
            }
        }
    }

    public Vec2 PositionNoCollision
    {
        get => OwnedBy == null ? PositionRelativeNoCollision : OwnedBy.PositionNoCollision + PositionRelativeNoCollision;
        set => PositionRelativeNoCollision = (OwnedBy == null ? value : value - OwnedBy.PositionNoCollision);
    }

    public Vec2 Speed;

    public void ApplySpeed() 
    {
        Position += Speed;
        Speed *= 0.95f;
    }

    public float ScaledRadius => Radius * Scale;
    public float Radius  = 1;

    public float Scale   = 1;
    public float Score = 0;

    public Circle Hitbox => new(Position, ScaledRadius);

    /// <summary>
    /// This is not included inside
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Entite> AllOtherEntitiesInsideMe() => AllOthersEntities().Inside(Hitbox);
    public IEnumerable<Entite> AllOtherEntitiesColliding() 
    {
        if(CollisionEnable == CollisionEnableEnum.Disable) 
        {
            return Enumerable.Empty<Entite>();
        }
        return AllOthersEntities().Inside(Hitbox).Where(t => LayerOverlap(t)).WithCollisionEnable();
    }

    public IEnumerable<Entite> AllEntitiesWithMe() => AllEntities().Where(t => SameTeams(t));
    public IEnumerable<Entite> AllOthersEntitiesWithMe() => AllOthersEntities().Where(t => SameTeams(t));

    public IEnumerable<Entite> AllOthersEntitiesAgainstMe() => AllOthersEntities().AgainstTeams(this);

    /// <summary>
    /// This is not included inside
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Entite> AllOthersEntities()
    {
        foreach (var v in AllEntities())
        {
            if (!ReferenceEquals(v, this)) { yield return v; }
        }
    }


}


public static class TeamsEnumExtension 
{
    public static Color GetColor(this TeamsEnum t) 
    {
        switch (t) 
        {
            case TeamsEnum.Alone   : return Color.White;
            case TeamsEnum.Neutral : return Color.Black;
            case TeamsEnum.One     : return Color.Red;
            case TeamsEnum.Two     : return Color.Cyan;
            case TeamsEnum.Three   : return Color.Yellow;
            case TeamsEnum.Four    : return Color.Green;
            default                : return Color.Magenta;
        }
    }
}