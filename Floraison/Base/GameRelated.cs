using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using static Floraison.Controller;
using static Floraison.Entite;

namespace Floraison;

public class GameRelated : TimeRelated
{
    public IEnumerable<Entite> AllEntities() => Game._Entites;
    public IEnumerable<Entite> EntitiesOfType<T>() where T : Entite => AllEntities().Where(t => typeof(T).IsAssignableFrom(t.GetType())).Select(t => (T)t);

    /// <summary>
    /// Return all entities controllerdby any connected controller
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEnumerable<Entite> EntitiesControlledByActivePlayer() => AllEntities().ControlledByActivePlayer();
    public IEnumerable<Entite> EntitiesControlledByActiveOrInactivePlayer() => AllEntities().ControlledByActiveOrInactivePlayer();

    public IEnumerable<Entite> EntitiesNotControlledByPlayer() => AllEntities().NotControlledByPlayer();
}

public static class Extension 
{
    public static IEnumerable<T> OfType<T>(this IEnumerable<Entite> i) where T : Entite => i.Where(t => typeof(T).IsAssignableFrom(t.GetType())).Select(t=>(T)t);
    public static IEnumerable<T> In<T>(this IEnumerable<T> i, Circle hitbox) where T : Entite => i.Where(t => hitbox.IsCollidingWith(hitbox));
    public static IEnumerable<T> Inside<T>(this IEnumerable<T> i, Circle hitbox) where T : Entite => i.Where(t => hitbox.IsCollidingWith(t.Hitbox));
    public static IEnumerable<T> Inside<T>(this IEnumerable<T> i, Entite e) where T : Entite => i.Inside(e.Hitbox);
    public static IEnumerable<T> WithTeams<T>(this IEnumerable<T> i, Entite e) where T : Entite => i.Where(t => e.SameTeams(t));
    public static IEnumerable<T> AgainstTeams<T>(this IEnumerable<T> i, Entite e) where T : Entite => i.Where(t => !e.SameTeams(t));


    public static IEnumerable<T> WithCollidingLayer<T>(this IEnumerable<T> i, Entite e) where T : Entite => i.WithCollidingLayer(e.CollisionLayer);
    public static IEnumerable<T> WithCollidingLayer<T>(this IEnumerable<T> i, int layer) where T : Entite => i.Where(t => t.LayerOverlap(layer));
    public static IEnumerable<T> WithCollisionEnable<T>(this IEnumerable<T> i, CollisionEnableEnum collision = CollisionEnableEnum.Enable) where T : Entite => i.Where(t => t.CollisionEnable == collision);


    public static IEnumerable<T> ControlledByActivePlayer<T>(this IEnumerable<T> i) where T : Entite => i.Where(t => t.PlayerControl == PlayerControlEnum.NotControlledByAPlayer && t.Input.IsConnected);
    public static IEnumerable<T> ControlledByActiveOrInactivePlayer<T>(this IEnumerable<T> i) where T : Entite => i.Where(t => t.PlayerControl == PlayerControlEnum.NotControlledByAPlayer);
    public static IEnumerable<T> NotControlledByPlayer<T>(this IEnumerable<T> i) where T : Entite => i.Where(t => t.PlayerControl != PlayerControlEnum.NotControlledByAPlayer);
}