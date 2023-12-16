using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Floraison;

public class PlantBehaviorMartin : PlantBehavior
{
    public PlantBehaviorMartin(Plant p) : base(p) { }
    
    //taille max de la tige (hors étirage)
    private float stemMaxLength = 5f;

    //Taille de la tige max quand étirée (par rapport a stemMaxLength)
    private float stemStreched = 1.15f;

    // Inverse de la force qui attire la plante au pot
    private float retractSpeed = .85f;
    //Pourcentage de la distance de la plante transformée en vitesse
    private float elasticStrenth = -0.25f;

    //Quantité de vitesse de la plante transférée au pot
    private float potStrenth = 0.4f;

    //Distance a laquelle la plante doit etre du pot pour pouvoir controller de nouveau la plante après un lancé
    private float controlDist = 0.4f;

    //Indique si la plante peut etre controlée, utilisé dans la physique
    private bool available = true;

    public override void Update()
    {
        if (All.KbMInput.MouseLeftReleased)
        {
            Bramble b = new Bramble(){

                Position = Camera.Peek().ToWorldPosition((Vec2)All.KbMInput.Mouse.Position.ToVector2()),
                CollisionLayer = Entite.CollisionLayerPot
            };
            b.Teams = Entite.TeamsEnum.Alone;
            b.collisionType = Entite.CollisionTypeEnum.Fixed;
            b.Spawn();
        }

        Vec2 newPosR = P.PositionRelative;
        if (P.Input.RightTrigger.JustReleased)
        {
            // newPosR.Normalize();
            P.Flicker(0f);
            P.Speed = newPosR * elasticStrenth;
            P.PlantedIn.Speed = newPosR * elasticStrenth * potStrenth;
            available = false;
        }
        else
        {
            if (P.Input.RightJoystick.IsNeutral || !available )
            {
                newPosR *= retractSpeed;
            }
            else
            {
                newPosR += P.Input.RightJoystick.UnitPerSecond * 30;
                if (P.Input.RightTrigger.IsPressed) //Stretch
                {
                    float stretchedSize = stemMaxLength * stemStreched;
                    if (newPosR.Length > stretchedSize)
                    {
                        newPosR.Normalize();
                        newPosR *= stretchedSize;
                        P.Flicker(0.2f);
                    }
                }
                else // Not pressed
                {
                    if (newPosR.Length > stemMaxLength)
                    {
                        newPosR.Normalize();
                        newPosR *= stemMaxLength;
                    }
                }
            }
        }

        if (P.Input.RightJoystick.IsNeutral || newPosR.Length <= controlDist * stemMaxLength) {available = true;}

        P.PositionRelative = newPosR;

    }
}