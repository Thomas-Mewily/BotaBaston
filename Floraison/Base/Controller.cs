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
    public PlayerControlEnum PlayerControl { get; private set; }

    public bool IsConnected { get; private set; }

    // 0 : error
    // 1-4 : player
    public static Controller[] Controllers { get; private set; } = new Controller[]
    {
        new(PlayerControlEnum.NotControlledByAPlayer, (PlayerIndex)(-1)),
        new(PlayerControlEnum.One,PlayerIndex.One),
        new(PlayerControlEnum.Two,PlayerIndex.Two),
        new(PlayerControlEnum.Three,PlayerIndex.Three),
        new(PlayerControlEnum.Four,PlayerIndex.Four),
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

    private Controller(PlayerControlEnum playerControl, PlayerIndex index) 
    {
        PlayerControl = playerControl;
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
            LeftJoystick.Update(Vec2.Zero);
            RightJoystick.Update(Vec2.Zero);

            LeftTrigger = LeftTrigger.Update(false);
            RightTrigger = RightTrigger.Update(false);

            X = X.Update(false);
            Y = Y.Update(false);
            A = A.Update(false);
            B = B.Update(false);

            Start = Start.Update(false);
            Select = Select.Update(false);
            return;
        }

        var gs = GamePad.GetCapabilities(Index);
        IsConnected = gs.IsConnected;
        var gp = GamePad.GetState(Index);

        LeftJoystick.Update(gp.ThumbSticks.Left);
        RightJoystick.Update(gp.ThumbSticks.Right);

        LeftTrigger = LeftTrigger.Update(gp.Triggers.Left >= 0.5f || gp.Buttons.LeftShoulder == ButtonState.Pressed);
        RightTrigger = RightTrigger.Update(gp.Triggers.Right >= 0.5f || gp.Buttons.RightShoulder == ButtonState.Pressed);

        X = X.Update(gp.Buttons.X == ButtonState.Pressed);
        Y = Y.Update(gp.Buttons.Y == ButtonState.Pressed);
        A = A.Update(gp.Buttons.A == ButtonState.Pressed);
        B = B.Update(gp.Buttons.B == ButtonState.Pressed);

        Start  = Start.Update(gp.Buttons.Start == ButtonState.Pressed);
        Select = Select.Update(gp.Buttons.Back == ButtonState.Pressed);

        base.Update();
    }

    public override string ToString() => $"Controller {PlayerControl}, Left: {LeftJoystick.Axis}, Right: {RightJoystick.Axis}, Start {Start}, Select {Select}, A {A}, B {B}, X {X}, Y {Y}";
}


public struct Joystick
{
    public Vec2 UnitPerSecond { get; private set; }
    /// <summary>
    /// With Dead Zone
    /// </summary>
    public Vec2 Axis { get; private set; }
    public Vec2 AxisOld { get; private set; }
    public Vec2 Square { get; private set; }

    public bool IsNeutral => Axis == Vec2.Zero;
    public bool WasNeutral => AxisOld == Vec2.Zero;

    public void Update(Vec2 v)
    {
        AxisOld = Axis;
        Square = v;
        v.Y = -v.Y;

        if (v.LengthSquared > 1)
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

    public override string ToString() => Axis.ToString();
}

public struct Button
{
    public bool IsPressed, WasPressed;

    public bool JustPressed    => IsPressed && !WasPressed;
    public bool JustReleased   => !IsPressed && WasPressed;

    public bool IsPullUp       => JustPressed;
    public bool IsPullDown     => JustReleased;
    public bool PullChanged    => IsPressed != WasPressed;
    public bool PullConstant   => IsPressed == WasPressed;

    public GTime LastChange { get; private set; }

    public Button Update(bool state)
    {
        if (state != IsPressed)
        {
            LastChange = All.Game.Time;
        }
        WasPressed = IsPressed;
        IsPressed = state;
        return this;
    }

    public Button(bool pressed) { IsPressed = WasPressed = pressed; LastChange = All.Game.Time; }
    public override string ToString() => IsPressed ? "1" : "0";
}