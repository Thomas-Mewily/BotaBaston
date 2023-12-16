using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Useful;

namespace Floraison;

public static class SpriteBatchExtension
{
    private static bool Active = false;
    public static bool IsActive(this SpriteBatch spriteBatch) => Active;

    private static Texture2D _Circle;
    private static Texture2D _Pixel;

    private static float _CircleDiameter;

    public static SpriteFont[] Fonts;
    public static SpriteFont FontJosefinSans => Fonts[0];
    public static SpriteFont FontDefault => FontJosefinSans;

    public static void Load()
    {
        _Circle = All.Content.Load<Texture2D>("circle");
        _CircleDiameter = _Circle.Width;

        _Pixel = new Texture2D(All.Graphics, 1, 1);
        _Pixel.SetData(new[] { Color.White });

        Fonts = new SpriteFont[]
        {
            All.Content.Load<SpriteFont>("FontJosefinSans"),
        };
    }

    public static void Unload()
    {
        _Circle.Dispose();
        _Pixel.Dispose();
    }

    public static void DrawCircle(this SpriteBatch spriteBatch, Vec2 pos, float radius, Color color)
    => DrawEllipse(spriteBatch, pos, new Vec2(radius, radius), color);

    public static void DrawEllipse(this SpriteBatch spriteBatch, Vec2 pos, float radius, Color color)
    => DrawEllipse(spriteBatch, pos, new Vec2(radius, radius), color);

    public static void DrawEllipse(this SpriteBatch spriteBatch, Vec2 pos, Vec2 radius, Color color)
    {
        spriteBatch.Draw(_Circle, pos, null, color, 0, new Vec2(_CircleDiameter/2), radius * 2 / _CircleDiameter, SpriteEffects.None, 0);
    }

    public static void DrawRectangle(this SpriteBatch spriteBatch, Vec2 pos, Vec2 size, Color color)
    {
        spriteBatch.Draw(_Pixel, pos, null, color, 0, Vec2.Zero, size, SpriteEffects.None, 0);
    }

    public static void DrawRectangle(this SpriteBatch spriteBatch, Rect2F rect, Color color)
        => spriteBatch.DrawRectangle(rect.Min, rect.SizeY, color);


    public enum TextSize
    {
        Normal = 36,
    }

    public static void DrawText(this SpriteBatch spriteBatch, Font font, string text, Vec2 pos, Vec2 coefCenter, Color color, TextSize size = TextSize.Normal)
    {
        var m = (Vec2)font.MeasureString(text);
        float scale = (float)size / m.Y * Camera.Peek().Rect.SizeY / All.Screen.WindowSize.Y;

        pos -= m * scale * coefCenter;
        spriteBatch.DrawString(font, text, pos, color, 0, Vec2.Zero, scale, SpriteEffects.None, 0);
    }
    public static void DrawText(this SpriteBatch spriteBatch, Font font, string text, Vec2 pos, Color color, TextSize size = TextSize.Normal)
    => DrawText(spriteBatch, font, text, pos, new Vec2(0.5f, 0), color, size);
    public static void DrawText(this SpriteBatch spriteBatch, string text, Vec2 pos, Color color, TextSize size = TextSize.Normal)
        => DrawText(spriteBatch, FontDefault, text, pos, new Vec2(0.5f, 0), color, size);
    public static void DrawText(this SpriteBatch spriteBatch, string text, Vec2 pos, Vec2 coefCenter, Color color, TextSize size = TextSize.Normal)
        => DrawText(spriteBatch, FontDefault, text, pos, coefCenter, color, size);


    public static void DebugText(this SpriteBatch spriteBatch, string text)
    {
        if (_DebugText.IsEmpty()) 
        {
            _DebugText.Push(text);
        }
        else 
        {
            var t = _DebugText.Pop();
            t += text;
            _DebugText.Push(t);
        }
    }

    public static void DebugTextLn(this SpriteBatch spriteBatch, string text)
    {
        _DebugText.Push(text);
    }



    public static void Debut(this SpriteBatch spriteBatch) => spriteBatch.Debut(new SpriteBatchOption());
    public static void Debut(this SpriteBatch spriteBatch, SpriteBatchOption option)
    {
        Active = true;
        spriteBatch.Begin(option.SortMode, option.Blend, option.SamplerState, option.DepthStencilState, option.RasterizerState, null, Camera.Peek().TransformMatrix);
    }
    public static void Fin(this SpriteBatch spriteBatch) { Active = false; spriteBatch.End(); }


    private static List<string> _DebugText = new();

    public enum LineEdgeMode 
    {
        None,
        Circle,
    }

    public static void DrawLine(this SpriteBatch spriteBatch, Vec2 begin, Vec2 end, Color c, float tickness = 1, LineEdgeMode mode = LineEdgeMode.Circle)
    {
        spriteBatch.DrawRawLine(begin, end, c, tickness);
        if (mode == LineEdgeMode.Circle) 
        {
            spriteBatch.DrawCircle(begin, tickness / 2, c);
            spriteBatch.DrawCircle(end, tickness / 2, c);
        }
    }

    public static void DrawRawLine(this SpriteBatch spriteBatch, Vec2 begin, Vec2 end, Color c, float tickness = 1) 
    {
        var d = end - begin;
        spriteBatch.Draw(_Pixel, begin, null, c, d.Angle - Angle.FromDegree(90f), new Vec2(0.5f, 0), new Vec2(tickness, d.Length), SpriteEffects.None, 0);
    }

    public static void Draw()
    {
        if (_DebugText.IsEmpty())  { return; }
        Camera.Push(Camera.Hud);
        int line = 0;
        foreach (var v in _DebugText)
        {
            All.SpriteBatch.DrawText(v, new Vec2(0, -line * (int)TextSize.Normal), new Vec2(0, 0), Color.Gray, TextSize.Normal);
            line--;
        }
        _DebugText.Clear();
        Camera.Pop();
    }

    public static Vec2 Size(this Texture2D t) => new Vec2(t.Width, t.Height);
}
