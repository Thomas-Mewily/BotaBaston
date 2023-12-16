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
            var p = new Pot();
            p.PlayerControl = (Controller.PlayerControlEnum)i;
            p.Teams = (Entite.TeamsEnum)i;
            p.Spawn();
        }
    }
}
