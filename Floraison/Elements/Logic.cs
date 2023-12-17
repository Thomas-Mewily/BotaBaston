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

    private void SpawnRandomPowerUp() 
    {
        int rng = Rng.IntUniform(0, 100);
        if(rng <= 25) 
        {
            new Sun().SetRandomPositionForPowerUp().Spawn();
        }
        else 
        {
            new Petal().SetRandomPositionForPowerUp().Spawn();
        }
    }

    public override void Update()
    {
        if(SpawnTime.Elapsed.Frames % 20 == 0) 
        {
            if (LastPowerUp.Elapsed.Seconds > 8 && AllEntities().OfType<Sun>().Count() < 3 && AllEntities().OfType<Petal>().Count() < 3) 
            {
                LastPowerUp = Game.Time;
                SpawnRandomPowerUp();
            }
        }
    }
}
