using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using Useful;

namespace Floraison;

public class BrambleSeed : Entite
{
    private float spawnTime = 2;
    public float Rota;

    public BrambleSeed(Vec2 pos)
    {
        Position = pos;
        // Scale = 1f;
        Rota = Angle.FromDegree(All.Rng.FloatUniform(0, 360));
        CollisionLayerAdd(CollisionLayerBramble);
        CollisionLayerAdd(CollisionLayerPlant);
        CollisionLayerAdd(CollisionLayerPot);


    }

    public override void Update()
    {
        if (SpawnTime.Elapsed.Seconds > spawnTime)
        {
            DeleteMe();
            Bramble b = new Bramble(Position);
            b.Spawn();
        }
    }

    public override void Draw()
    {
        // SpriteBatch.DrawCircle(Position, Hitbox.Radius, Microsoft.Xna.Framework.Color.Aqua);
        SpriteBatch.Draw(Assets.BrambleSeed, (Vector2)Position, null, Microsoft.Xna.Framework.Color.White, Rota, Assets.BrambleSeed.Size() * .5f, 3* Hitbox.Radius / Assets.Bramble.Size(), SpriteEffects.None, 0);

    }


}

public class Bramble : Entite
{

    public Microsoft.Xna.Framework.Color Tint;
    public float Rota;
    public SpriteEffects Direction;
    public float Strenth = 0.6f;
    public const int HP_MAX = 4;
    public int hp = HP_MAX;

    public Bramble(Vec2 pos)
    {
        float RB = All.Rng.FloatUniform(0.5f, 1);
        Tint = new Microsoft.Xna.Framework.Color(RB, All.Rng.FloatUniform(RB, 1), RB);
        Rota = Angle.FromDegree(All.Rng.FloatUniform(0, 360));
        Direction = (SpriteEffects)All.Rng.IntUniform(0, 2);
        Position = pos;
        Teams = TeamsEnum.Alone;
        collisionType = CollisionTypeEnum.Fixed;
    }
    public override void Update()
    {
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
        }

    }

    public override void Draw()
    {
        var c = Camera.Peek().Clone();
        c.Options.Blend = BlendState.AlphaBlend;
        Camera.Push(c);
        // SpriteBatch.DrawCircle(Position, Hitbox.Radius, Color.Aqua);

        float sizeModifier = ((float)hp/(float)HP_MAX) * 0.3f + 1.2f;

        SpriteBatch.Draw(Assets.Bramble, (Vector2)Position, null, Tint, Rota, Assets.Bramble.Size() * .5f, 2* sizeModifier * Hitbox.Radius / Assets.Bramble.Size(), Direction, 0);

        Camera.Pop();
    }
}
