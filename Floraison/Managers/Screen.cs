using Floraison;
using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SimulationGraphique.Managers;

public class Screen : TimeRelated
{
    private Point2 _WindowSize;
    public Point2 WindowSize { get => _WindowSize; set { _WindowSize = GetFinalSize(value); } }
    public Vec2 WindowSizeVec2 => WindowSize;

    public int NbTimeScreenChanged { get; private set; } = 0;

    public bool IsFullScreen
    {
        get => All.GraphicsDeviceManager.IsFullScreen;
        set
        {
            if (IsFullScreen != value)
            {
                All.GraphicsDeviceManager.ToggleFullScreen();
            }
        }
    }

    public Point2 ScreenSize { get; private set; }
    public Vec2 ScreenSizeVec2 => ScreenSize; 

    public Screen()
    {
        Load();
    }

    public override void Load()
    {
        ReadScreenSize();
        WindowSize = ScreenSize / 2;
        ApplyGraphicChange();
    }

    private bool ReadScreenSize() 
    {
        bool changed = false;
        var tmp = new Point2(All.Graphics.Adapter.CurrentDisplayMode.Width, All.Graphics.Adapter.CurrentDisplayMode.Height);
        changed |= ScreenSize != tmp;

        ScreenSize = tmp;
        //tmp = new Point2(All.GraphicsDeviceManager.PreferredBackBufferWidth, All.GraphicsDeviceManager.PreferredBackBufferHeight);
        tmp = new Point2(All.Graphics.Viewport.Bounds.Width, All.Graphics.Viewport.Bounds.Height);
        changed |= WindowSize != tmp;
        WindowSize = tmp;

        return changed;
    }

    public override void Update()
    {
        if (ReadScreenSize()) 
        {
            NbTimeScreenChanged++;
        }
    }

    public override void Draw()
    {
#if DEBUG
        All.SpriteBatch.DebugTextLn("windows: " + WindowSize + ", screen: " + ScreenSize + ", " + NbTimeScreenChanged + " changes");
#endif
    }

    public void ApplyGraphicChange()
    {
        NbTimeScreenChanged++;
        All.GraphicsDeviceManager.PreferredBackBufferWidth  = WindowSize.X;
        All.GraphicsDeviceManager.PreferredBackBufferHeight = WindowSize.Y;
        All.GraphicsDeviceManager.ApplyChanges();
    }

    private Point2 GetFinalSize(Point2 size)
    {
        //No zero pixel window size, please
        size.X = Math.Max(1, size.X);
        size.Y = Math.Max(1, size.Y);
        return size;
    }
}