using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Floraison;

public static class KeysInput
{
    public static bool IsDown(this Keys k) => All.KbMInput.IsDown(k);
    public static bool IsUp(this Keys k) => All.KbMInput.IsUp(k);
    public static bool JustPressed(this Keys k) => All.KbMInput.JustPressed(k);
    public static bool JustReleased(this Keys k) => All.KbMInput.JustReleased(k);
}

public class KeyboardMouseInput : TimeRelated
{
    public MouseState Mouse { get; private set; }
    public MouseState MouseOld { get; private set; }

    public KeyboardState Keyboard { get; private set; }
    public KeyboardState KeyboardOld { get; private set; }

    public bool JustPressed(Keys k) => Keyboard.IsKeyUp(k) && !KeyboardOld.IsKeyUp(k);
    public bool JustReleased(Keys k) => !Keyboard.IsKeyUp(k) && KeyboardOld.IsKeyUp(k);

    public bool IsDown(Keys k) => Keyboard.IsKeyDown(k);
    public bool IsUp(Keys k) => Keyboard.IsKeyUp(k);

    public bool MouseLeftPressed => Mouse.LeftButton == ButtonState.Pressed && MouseOld.LeftButton == ButtonState.Released;
    public bool MouseLeftReleased => Mouse.LeftButton == ButtonState.Released && MouseOld.LeftButton == ButtonState.Pressed;

    public bool MouseRightPressed => Mouse.RightButton == ButtonState.Pressed && MouseOld.RightButton == ButtonState.Released;
    public bool MouseRightReleased => Mouse.RightButton == ButtonState.Released && MouseOld.RightButton == ButtonState.Pressed;

    public override void Update()
    {
        MouseOld = Mouse;
        Mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();

        KeyboardOld = Keyboard;
        Keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
    }
}