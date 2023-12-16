using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Floraison;

public class GameLoader
{
    public GameLoader() 
    {

    }

    public void Load() 
    {
        for (int i = (int)Controller.PlayerControlEnum.One; i <= (int)Controller.PlayerControlEnum.Four; i++) 
        {
            var pot = new Pot
            {
                PlayerControl = (Controller.PlayerControlEnum)i,
                Teams = (Entite.TeamsEnum)i
            };



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
