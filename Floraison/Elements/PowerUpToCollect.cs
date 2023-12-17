using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;

public class Sun : Entite
{
    public override void Update()
    {
        foreach(var v in EntitiesControlledByActivePlayer().Inside(this)) 
        {
            if(v is Plant p) 
            {
                p.Behavior.Grow();
                DeleteMe();
                break;
            }
        }
    }

    public override void Draw()
    {
        int i = 0;
        foreach(var s in Assets.Suns) 
        {
            i++;
            SpriteBatch.Draw(s, Position, null, Color.White, Angle.FromDegree(SpawnTime.Elapsed.Hz_60*i), s.Size() * 0.5f, 2 * ScaledRadius / s.Size(), SpriteEffects.None, 0);
        }
    }
}


public class Petal : Entite
{
    public override void Update()
    {
        foreach (var v in EntitiesControlledByActivePlayer().Inside(this))
        {
            if (v is Plant p)
            {
                p.PowerUp = new PowerUpPetales(p);
                DeleteMe();
                break;
            }
        }
    }

    public override void Draw()
    {
        SpriteBatch.Draw(Assets.PowerUpPetal, Position, null, Color.White, 0, Assets.PowerUpPetal.Size() * 0.5f, 2 * ScaledRadius / Assets.PowerUpPetal.Size() * (1+Angle.FromDegree(SpawnTime.Elapsed).Sin*0.125f+0.125f), SpriteEffects.None, 0);
    }
}

public class Spiral : Entite
{
    public override void Update()
    {
        foreach(var v in EntitiesControlledByActivePlayer().Inside(this)) 
        {
            if(v is Plant p) 
            {
                p.NbSeed += 5;
                DeleteMe();
                break;
            }
        }
    }

    public override void Draw()
    {
        int i = 0;
        foreach(var s in Assets.Spirals) 
        {
            i++;
            SpriteBatch.Draw(s, Position, null, Color.White, Angle.FromDegree(SpawnTime.Elapsed.Hz_60*i), s.Size() * 0.5f, 2 * ScaledRadius / s.Size(), SpriteEffects.None, 0);
        }
    }
}