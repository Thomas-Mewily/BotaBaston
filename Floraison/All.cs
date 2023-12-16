using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimulationGraphique.Managers;
using System;
using Useful;

namespace Floraison;

public class All 
{
    public static SpriteBatch SpriteBatch => MainGame._SpriteBatch;
    public static GraphicsDeviceManager GraphicsDeviceManager => MainGame._GraphicsDeviceManager;
    public static GraphicsDevice Graphics => GraphicsDeviceManager.GraphicsDevice;
    public static ContentManager Content => MainGame._Content;

    public static Color BackgroundColor = Color.Black;

    public static Performance Performance;
    public static TheGame Game;
    public static Screen Screen;
    /// <summary>
    /// C'est pas mersenne twister mais ça devrait le faire
    /// </summary>
    public static Random Rng = new Random();

    public static bool IsDrawTime => MainGame.IsDrawTime;
}

public class MainGame : Game
{
    public static MainGame Instance;

    public static SpriteBatch _SpriteBatch => Instance._spriteBatch;
    public static GraphicsDeviceManager _GraphicsDeviceManager => Instance._graphics;
    public static ContentManager _Content => Instance.Content;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static bool IsDrawTime = false;


    public MainGame()
    {
        TargetElapsedTime = TimeSpan.FromSeconds(1.0f / TheGame.FrameRate);

        Instance = this;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        All.Performance = new();
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        //Window.AllowAltF4 = false;
        _graphics.PreferMultiSampling = true;
        _graphics.GraphicsProfile = GraphicsProfile.Reach;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        SpriteBatchExtension.Load();
        Assets.Load();

        All.Screen = new Screen();

        All.Game = new TheGame();
        new GameLoader().Load();
    }

    protected override void Update(GameTime gameTime)
    {
        IsDrawTime = false;

        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) { Exit(); }

        All.Screen.Update();
        All.Performance.Update(gameTime);

        All.Game.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        IsDrawTime = true;

        GraphicsDevice.Clear(All.BackgroundColor);
        All.Game.Draw();

        All.Screen.Draw();
        All.Performance.Draw(gameTime);

        SpriteBatchExtension.Draw();

        Crash.Check(Camera.Count == 0, "Forget to Pop some camera");
        base.Draw(gameTime);
    }
}
