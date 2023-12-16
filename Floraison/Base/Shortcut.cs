using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Floraison;

public class Shortcut
{
    public SpriteBatch SpriteBatch => All.SpriteBatch;
    public GraphicsDeviceManager GraphicsDeviceManager => All.GraphicsDeviceManager;
    public TheGame Game => All.Game;
    public ContentManager Content => All.Content;

    public int   FrameRateInt => TheGame.FrameRate;
    public float FrameRate => TheGame.FrameRate;

    public Controller[] AllControllers() => Controller.Controllers;
}
