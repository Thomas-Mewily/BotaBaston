﻿using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Floraison;

public class GameLoader : Shortcut
{
    public GameLoader() 
    {

    }

    public void Load() 
    {
        

        for (int i = (int)Controller.PlayerControlEnum.One; i <= (int)Controller.PlayerControlEnum.Four; i++) 
        {
            int coefX = (i-1) / 2 % 2 == 0 ? -1 : 1;
            int coefY = (i-1) % 2 == 0 ? -1 : 1;

            var pot = new Pot
            {
                PlayerControl = (Controller.PlayerControlEnum)i,
                Teams = (Entite.TeamsEnum)i
            };

            pot.Position = Game.WorldHitbox.GetCoef(new Vec2(0.5f + coefX * 0.3f, 0.5f + coefY * 0.3f));

            var plant = new Plant
            {
                PlayerControl = (Controller.PlayerControlEnum)i,
                Teams = (Entite.TeamsEnum)i,
            };

            pot.Plant(plant);

            plant.Spawn();
            pot.Spawn();
        }
    }
}
