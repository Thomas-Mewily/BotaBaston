using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using Useful;

namespace Floraison;

public class PowerUpPetales : PlantPowerUp
{
    public PowerUpPetales(Plant p) : base(p) { }
    public Angle Offset;
    public GTime LastTimeUsed;

    public bool  IsUsed => LastTimeUsed.Elapsed < Duration;
    public float UsedCoef => IsUsed ? LastTimeUsed.Elapsed / Duration : 0;

    public bool IsCoolDown => !IsUsed && LastTimeUsed.Elapsed < Duration+ CoolDown;
    public float CoolDownCoef => IsCoolDown ? (LastTimeUsed.Elapsed- Duration) / CoolDown : 0;

    public bool Available => !IsCoolDown && !IsUsed;

    public GTime Duration;
    public GTime CoolDown;

    public override void Load()
    {
        CollisionEnable = CollisionEnableEnum.Disable;
        CollisionLayer = 0;
        CollisionLayerAdd(CollisionLayerPlant);
        CollisionLayerAdd(CollisionLayerPot);
    }

    public override void Update()
    {
        Duration = GTime.Second(1.5f);
        CoolDown = GTime.Second(10f);

        Position = P.Position;

        Scale = HRadius;

        if (Available) 
        {
            if (P.Input.LeftTrigger.JustPressed || P.Input.LeftTrigger.IsPressed) 
            {
                LastTimeUsed = P.Game.Time;
                CollisionEnable = CollisionEnableEnum.Enable;
            }
        }
        if (IsCoolDown) 
        {
            CollisionEnable = CollisionEnableEnum.Disable;
        }

        base.Update();
    }

    public override bool AcceptCollision(Entite e)
    {
        return e.Teams != this.Teams;
    }

    public override void EntityIsCollidingWithMe(Entite e)
    {
        if (!AcceptCollision(e)) { return;  }
        e.Speed += new Vec2(Position, e.Position).WithLength(1/64f);
    }


    public float HRadius => 2 * P.ScaledRadius  + (IsUsed ? P.ScaledRadius : 0);

    public override void Draw()
    {
        int nbPetal = 7;

        if(P.PositionRelative.Length > 0.1f) 
        {
            Offset = -P.PositionRelative.Angle;
        }
        Offset += Rng.FloatUniform(0, P.OffsetFlicker.Length);

        var c = P.DrawColor;
        if (IsCoolDown) 
        {
            c = Color.Gray;
        }

        for (int  i =0; i < nbPetal; i++) 
        {
            Angle a = Angle.FromDegree(i * 360f / nbPetal) + Offset;
            if (IsUsed)
            {
                //a += Angle.AngleFromOne(UsedCoef).Cos * 90;
                a += Angle.FromDegree(P.SpawnTime.Elapsed.Seconds * 360*2.5f);
            }
            SpriteBatch.Draw(Assets.Petal, P.DrawPos, null, c, a, Assets.Petal.Size() * 0.5f, HRadius*2 / Assets.Petal.Size(), SpriteEffects.None, 0);
        }
        base.Draw();

        //SpriteBatch.DrawCircle(Position, ScaledRadius, Teams.GetColor());
        //SpriteBatch.DrawCircle(Position, 20, Teams.GetColor());
    }
}

