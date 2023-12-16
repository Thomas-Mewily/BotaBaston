using Geometry;
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
    public Entite Spawn() 
    {
        Game.Spawn(this);
        return this;
    }


    public enum TeamsEnum
    {
        // Tout seul
        Alone = 0,

        One   = 1,
        Two   = 2,
        Three = 3,
        Four  = 4,

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

    public void BasedOn(Entite e) 
    {
        Teams    = e.Teams;
        Position = e.Position;
        Radius   = e.Radius;
        Scale    = e.Scale;
    }

    public TeamsEnum Teams = TeamsEnum.Neutral;
    public SpawnStateEnum SpawnState = SpawnStateEnum.Unknow;
    public PlayerControlEnum PlayerControl = PlayerControlEnum.NotControlledByAPlayer;
    public Controller Input => From(PlayerControl);

    public GTime SpawnTime;

    public Vec2 Position = 1;
    public float ScaledRadius => Radius * Scale;
    public float Radius  = 1;

    public float Scale   = 1;

    public Circle Hitbox => new(Position, Radius * Scale);

    /// <summary>
    /// This is not included inside
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Entite> AllOtherEntitiesInsideMe() => AllOthersEntities().Inside(Hitbox);

    public IEnumerable<Entite> AllEntitiesWithMe() => AllEntities().Where(t => SameTeams(t));
    public IEnumerable<Entite> AllOthersEntitiesWithMe() => AllOthersEntities().Where(t => SameTeams(t));

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