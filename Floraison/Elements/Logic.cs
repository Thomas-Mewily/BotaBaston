using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using Useful;

namespace Floraison;

public class Logic : Entite
{
    GTime LastPowerUp;

    private static float[] probaPowerUp = {
        0.7f,//Sun
        0.3f //BrambleSeed
    };

    public enum PowerUpTypeEnum
    {
        Sun,
        BrambleSeed,
    }

    public void SpawnPowerUp(PowerUpTypeEnum pu) 
    {
        Entite powerUp;
        switch (pu)
        {
            case PowerUpTypeEnum.Sun:            powerUp = new Sun(); break;
            case PowerUpTypeEnum.BrambleSeed:    powerUp = new Spiral(); break;
            default : powerUp = new Sun(); break;
        }


        for(int i = 0; i < 1000; i++) 
        {
            powerUp.Position = Game.WorldHitbox.GetCoef(Rng.NextSingle(), Rng.NextSingle());
            if (!powerUp.EntitiesControlledByActiveOrInactivePlayer().Inside(powerUp).Any()) { break; }
        }
        powerUp.Spawn();
        Console.WriteLine("new Power up : " + pu.ToString());
    }

    public void SpawnPowerUp()
    {
        float choice = All.Rng.FloatUniform(0, 1);
        float cumul = 0;
        for (int i=0; i<probaPowerUp.Length; i++)
        {
            cumul += probaPowerUp[i];
            if (choice < cumul)
            {
                SpawnPowerUp((PowerUpTypeEnum)i);
                break;
            }
        }
    }

    public override void Update()
    {
        if(SpawnTime.Elapsed.Frames % 10 == 0) 
        {
            if (LastPowerUp.Elapsed.Seconds > 10 && AllEntities().OfType<Sun>().Count() < 3) 
            {
                LastPowerUp = Game.Time;
                SpawnPowerUp();
            }
        }
    }
}
