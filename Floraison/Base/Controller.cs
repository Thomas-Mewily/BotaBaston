using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Floraison;

public class Controller : GameRelated
{
    public enum PlayerControlEnum
    {
        NotControlledByAPlayer = 0,
        One   = 1,
        Two   = 2,
        Three = 3,
        Four  = 4,
        /*
        Five  = 5,
        Six   = 6,
        Seven = 7,
        Height= 8,*/
    }

    public Joystick LeftJoystick;
    public Joystick RightJoystick;

    public Button A { get; set; }
    public Button B { get; set; }
    public Button X { get; set; }
    public Button Y { get; set; }

    public Button Start { get; set; }
    public Button Select { get; set; }

    public Button LeftTrigger { get; set; }
    public Button RightTrigger { get; set; }

    private PlayerIndex Index;

    public bool IsConnected { get; private set; }

    // 0 : error
    // 1-4 : player
    public static Controller[] Controllers { get; private set; } = new Controller[]
    {
        new((PlayerIndex)(-1)),
        new(PlayerIndex.One),
        new(PlayerIndex.Two),
        new(PlayerIndex.Three),
        new(PlayerIndex.Four),
    };

    public static implicit operator Controller(PlayerControlEnum playerControl) => From(playerControl);

    public static Controller From(PlayerControlEnum playerControl) 
    {
        int idx = 0;
        switch (playerControl) 
        {
            case PlayerControlEnum.One  : idx = 1;   break;
            case PlayerControlEnum.Two  : idx = 2;   break;
            case PlayerControlEnum.Three: idx = 3; break;
            case PlayerControlEnum.Four : idx = 4;  break;
        }
        return Controllers[idx];
    }

    private Controller(PlayerIndex index) 
    {
        Index = index;
        A = B = X = Y = Start = Select = new Button(false);
        LeftJoystick = RightJoystick = new Joystick();
    }

    public Vec2 JoystickUpdate(Vec2 v)
    {
        v.Y = -v.Y;
        if (v.LengthSquared >= 0.25f * 0.25f) 
        {
            return v.Normalized;
        }
        return v;
    }

    public override void Update()
    {
        if(Index == (PlayerIndex)(-1))
        {
            IsConnected = false;
            return;
        }

        var gs = GamePad.GetCapabilities(Index);
        IsConnected = gs.IsConnected;
        var gp = GamePad.GetState(Index);

        LeftJoystick.Update(gp.ThumbSticks.Left);
        RightJoystick.Update(gp.ThumbSticks.Right);

        LeftTrigger.Update(gp.Triggers.Left >= 0.5f || gp.Buttons.LeftShoulder == ButtonState.Pressed);
        RightTrigger.Update(gp.Triggers.Right >= 0.5f || gp.Buttons.RightShoulder == ButtonState.Pressed);

        X.Update(gp.Buttons.X == ButtonState.Pressed);
        Y.Update(gp.Buttons.Y == ButtonState.Pressed);
        A.Update(gp.Buttons.A == ButtonState.Pressed);
        B.Update(gp.Buttons.B == ButtonState.Pressed);

        Start.Update(gp.Buttons.Start == ButtonState.Pressed);
        Select.Update(gp.Buttons.Back == ButtonState.Pressed);

        base.Update();
    }
}

public struct Joystick 
{
    public Vec2 UnitPerSecond { get; private set; }
    /// <summary>
    /// With Dead Zone
    /// </summary>
    public Vec2 Axis { get; private set; }
    public Vec2 Square { get; private set; }

    public void Update(Vec2 v) 
    {
        Square = v;
        v.Y = -v.Y;
        
        if(v.LengthSquared > 1) 
        {
            v.Normalize();
        }
        if (v.LengthSquared <= 0.25f * 0.25f)
        {
            v = Vec2.Zero;
        }

        Axis = v;
        UnitPerSecond = Axis / TheGame.FrameRate;
    }
}

public struct Button
{
    public bool IsPress, WasPress;

    public bool IsPullUp     => IsPress && !WasPress;
    public bool IsPullDown   => !IsPress && WasPress;
    public bool PullChanged  => IsPress != WasPress;
    public bool PullConstant => IsPress == WasPress;

    public GTime LastChange { get; private set; }

    public void Update(bool state)
    {
        if (state != IsPress)
        {
            LastChange = All.Game.Time;
        }
        WasPress = IsPress;
        IsPress = state;
    }

    public Button(bool pressed) { IsPress = WasPress = pressed; LastChange = All.Game.Time; }
}