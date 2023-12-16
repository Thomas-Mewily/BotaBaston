using Floraison;
using Geometry;
using Microsoft.Xna.Framework;
using System;

namespace SimulationGraphique.Managers;

public class Performance
{
    #region Constructor
    public bool IsLaggy { get; private set; } = false;

    /// <summary> Update per Second </summary>
    public  float Ups { get; private set; } = -1;
    private float UpsFrameRate = 0;
    private int UpsSecond = 0;

    /// <summary>Frame per second </summary>
    public  float Fps { get; private set; } = -1;
    private float FpsFrameRate = 0;
    private int FpsSecond = 0;

    public Performance() { }
    #endregion

    public void Update(GameTime gameTime)
    {
        if (IsLaggy != gameTime.IsRunningSlowly)
        {
            IsLaggy = gameTime.IsRunningSlowly;
        }

        UpsFrameRate++;
        int currentSecond = (int)Math.Floor(gameTime.TotalGameTime.TotalSeconds);
        if (currentSecond != UpsSecond)
        {
            UpsSecond = currentSecond;
            Ups = UpsFrameRate;
            UpsFrameRate = 0;
        }
    }



    public void Draw(GameTime gameTime) 
    {
        FpsFrameRate++;
        int currentSecond = (int)Math.Floor(gameTime.TotalGameTime.TotalSeconds);
        if (currentSecond != FpsSecond)
        {
            FpsSecond = currentSecond;
            Fps = FpsFrameRate;
            FpsFrameRate = 0;
        }
#if DEBUG
        All.SpriteBatch.DebugTextLn(Ups + " ups, " + Fps + " fps. ");
#endif
    }
}
