using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using static Floraison.Controller;

namespace Floraison;

public class GameRelated : TimeRelated
{
    public IEnumerable<Entite> AllEntities() => Game._Entites;
    public IEnumerable<T> EntitiesOfType<T>() where T : Entite => AllEntities().Where(t => typeof(T).IsAssignableFrom(t.GetType())).Select(t => (T)t);

    /// <summary>
    /// Return all entities controllerdby any connected controller
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEnumerable<Entite> ControlledByActivePlayer<T>() => AllEntities().Where(t => t.PlayerControl == PlayerControlEnum.NotControlledByAPlayer && t.Input.IsConnected);
    public IEnumerable<Entite> ControlledByActiveOrInactivePlayer<T>() => AllEntities().Where(t => t.PlayerControl == PlayerControlEnum.NotControlledByAPlayer);
    
    public IEnumerable<Entite> NotControlledByPlayer<T>() => AllEntities().Where(t => t.PlayerControl != PlayerControlEnum.NotControlledByAPlayer);
}

public static class Extension 
{
    public static IEnumerable<T> OfType<T>(this IEnumerable<Entite> i) where T : Entite => i.Where(t => typeof(T).IsAssignableFrom(t.GetType())).Select(t=>(T)t);
    public static IEnumerable<T> In<T>(this IEnumerable<T> i, Circle hitbox) where T : Entite => i.Where(t => hitbox.IsCollidingWith(hitbox));
    public static IEnumerable<T> Inside<T>(this IEnumerable<T> i, Circle hitbox) where T : Entite => i.Where(t => hitbox.IsCollidingWith(t.Hitbox));
}