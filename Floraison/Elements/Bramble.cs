using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Useful;

namespace Floraison;

public class BrambleSeed : Entite
{
    private GTime spawnTime = 2;
    public Angle Rota;

    public Entite SpawnedBy;

    public BrambleSeed()
    {
        Rota = Angle.FromDegree(All.Rng.FloatUniform(0, 360));
        CollisionLayerAdd(CollisionLayerBramble);
        CollisionLayerAdd(CollisionLayerPlant);
        CollisionLayerAdd(CollisionLayerPot);
        // Scale = 1f;
    }
    public BrambleSeed(Vec2 pos) : this()
    {
        Position = pos;
    }

    
    public override bool AcceptCollision(Entite e)
    {
        if(SpawnedBy == null) { return true; }
        return SpawnedBy.BaseEntite != e.BaseEntite;
    }

    public override void Update()
    {
        if (SpawnTime.Elapsed.Seconds > spawnTime)
        {
            // All.Sound.play
            DeleteMe();
            Bramble b = new Bramble(Position);
            b.Spawn();
        }
    }

    public override void Draw()
    {
        // SpriteBatch.DrawCircle(Position, Hitbox.Radius, Microsoft.Xna.Framework.Color.Aqua);
        SpriteBatch.Draw(Assets.BrambleSeed, Position, null, Color.White, Rota, Assets.BrambleSeed.Size() * .5f, 3* Hitbox.Radius / Assets.Bramble.Size(), SpriteEffects.None, 0);
    }


}

public class Bramble : Entite
{

    public Color Tint;
    public Angle Rota;
    public SpriteEffects Direction;
    public float Strenth = 0.6f;
    public const int HP_MAX = 4;
    public int hp = HP_MAX;
    public float BumpSizeCoef = 0;

    public Bramble(Vec2 pos)
    {
        float RB = All.Rng.FloatUniform(0.5f, 1);
        Tint = new Color(RB, All.Rng.FloatUniform(RB, 1), RB);
        Rota = Angle.FromDegree(All.Rng.FloatUniform(0, 360));
        Direction = (SpriteEffects)All.Rng.IntUniform(0, 2);
        Position = pos;
        Teams = TeamsEnum.Alone;
        Scale = 1.35f;
        CollisionType = CollisionTypeEnum.Fixed;
        CollisionLayerAdd(CollisionLayerPlant);
        CollisionLayerAdd(CollisionLayerPot);
    }

    
    public override void EntityIsCollidingWithMe(Entite e)
    {
        BumpSizeCoef = Math.Max(2, BumpSizeCoef + 0.5f);

        Vec2 normal = e.Position - Position;
        normal.Normalize();
        e.Speed = normal * (Strenth + e.Speed.Length);
        if (e.IsOnCollisionLayer(CollisionLayerPot))
        {
            hp--;
            if (hp <= 0)
            {
                DeleteMe();
            }
        }
        SoundMixer.bramble_hit.Play();
    }

    public override void Update()
    {
        BumpSizeCoef *= 0.8f;
        /*
        foreach(var e in AllOthersEntitiesAgainstMe().Inside(this)) 
        {
            Vec2 normal = e.Position - Position;
            normal.Normalize();
            e.Speed = normal * (Strenth + e.Speed.Length);
            if (e.IsOnCollisionLayer(CollisionLayerPot))
            {
                hp--;
                if (hp <= 0)
                {
                    DeleteMe();
                }
            }
        }*/
    }

    public override void Draw()
    {
        var c = Camera.Peek().Clone();
        c.Options.Blend = BlendState.AlphaBlend;
        Camera.Push(c);
        // SpriteBatch.DrawCircle(Position, Hitbox.Radius, Color.Aqua);

        float sizeModifier = ((float)hp / (float)HP_MAX) * 0.3f + 1.2f;

        SpriteBatch.Draw(Assets.Bramble, Position, null, Tint, Rota, Assets.Bramble.Size() * .5f, (1+BumpSizeCoef)* 2 * sizeModifier * Hitbox.Radius / Assets.Bramble.Size(), Direction, 0);


        Camera.Pop();
    }
}
