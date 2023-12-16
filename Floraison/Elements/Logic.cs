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

    private void SpawnSun() 
    {
        Sun s = new Sun();
        for(int i = 0; i < 1000; i++) 
        {
            s.Position = Game.WorldHitbox.GetCoef(Rng.NextSingle(), Rng.NextSingle());
            if (!s.EntitiesControlledByActiveOrInactivePlayer().Inside(s).Any()) { break; }
        }
        s.Spawn();
    }

    public override void Update()
    {
        if(SpawnTime.Elapsed.Frames % 10 == 0) 
        {
            if (LastPowerUp.Elapsed.Seconds > 10 && AllEntities().OfType<Sun>().Count() < 3) 
            {
                LastPowerUp = Game.Time;
                SpawnSun();
            }
        }
    }
}
