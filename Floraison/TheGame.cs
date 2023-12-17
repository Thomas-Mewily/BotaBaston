using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Useful;
using static Floraison.SpriteBatchExtension;

namespace Floraison;

public struct ParticleLine 
{
    public Vec2 PosBegin, PosEnd;
    public Color C;
    public float Tickness;
    public GTime TimeBegin, Duration;
    /// <summary>
    /// [0, 1]. 
    /// </summary>
    public float Delta => TimeBegin.Elapsed / Duration;

    public ParticleLine(Vec2 posBegin, Vec2 posEnd, Color c, float tickness, GTime timeBegin, GTime duration)
    {
        PosBegin = posBegin;
        PosEnd = posEnd;
        C = c;
        Tickness = tickness;
        TimeBegin = timeBegin;
        Duration = duration;
    }
}

public class TheGame : TimeRelated
{
    public enum GameStateEnum
    {
        Reset,
        Begin,
        Playing,
        Over,
    }
    public Entite Winner = null;

    public GameStateEnum GameState = GameStateEnum.Reset;

    /// <summary>
    /// They will be in Entites at the end of the frame
    /// </summary>
    public List<Entite> _CreatedThisFrame = new();
    public List<Entite> _Entites = new();
    public List<ParticleLine> ParticlesLines = new();

    public new const int FrameRate = 60;
    private GTime _Time;
    public GTime Time => _Time;

    private GTime TimeWinning;

    public Rect2F WorldHitbox = Rect2F.Centered(new Vec2(16, 9)*4, Vec2.Zero);
    public Camera Cam;

    private GameLoader Loader = new();

    public TheGame() { }

    public void Spawn(Entite gameObj) 
    {
        Crash.Check(gameObj.SpawnState == Entite.SpawnStateEnum.Unknow);
        gameObj.SpawnTime = _Time;
        _CreatedThisFrame.Add(gameObj);
    }

    private void HandleSpawn() 
    {
        while (!_CreatedThisFrame.IsEmpty())
        {
            var obj = _CreatedThisFrame.Pop();
            obj.SpawnState = Entite.SpawnStateEnum.Active;
            obj.SpawnTime = Time;
            _Entites.Push(obj);
            obj.Load();
        }
        _CreatedThisFrame.Clear();
    }


    private void HandleDispawn()
    {
        for (int i = _Entites.Count-1; i >= 0; i--)
        {
            if (_Entites[i].SpawnState == Entite.SpawnStateEnum.DeleteMePlz)
            {
                _Entites[i].SpawnState = Entite.SpawnStateEnum.NotInTheGame;
                _Entites.RemoveAt(i);
            }
        }
    }

    public override void Load()
    {
        GameState = GameStateEnum.Begin;
        ParticlesLines.Clear();
        _Time = GTime.Second(0);
        Cam = Camera.Centered(WorldHitbox);
        Winner = null;

        Loader.Load();
        HandleSpawn();
        HandleDispawn();
    }

    public override void Unload()
    {
        foreach (Entite obj in _CreatedThisFrame)
        {
            obj.Unload();
        }
        foreach (Entite obj in _Entites)
        {
            obj.Unload();
        }
        _CreatedThisFrame.Clear();
        _Entites.Clear();
    }

    private void Reset() 
    {
        Unload();
        Load();
        GameState = GameStateEnum.Begin;
    }

    public override void Update()
    {
        if(GameState == GameStateEnum.Reset) 
        {
            Reset();
        }

        Camera.Push(Cam);

        foreach (Controller c in AllControllers()) 
        {
            c.Update();
        }

        switch (GameState)
        {
            case GameStateEnum.Begin:
                { 
                    if(Time.Seconds > 3) 
                    {
                        GameState = GameStateEnum.Playing;
                    }
                }
                break;
            case GameStateEnum.Playing:
            {
                HandleSpawn();

                foreach (Entite obj in _Entites)
                {
                    obj.Update();
                    if (obj.CoefAboutToWin >= 1)
                    {
                        Winner = obj;
                        TimeWinning = Time;
                            SoundMixer.victory.Play();
                        GameState = GameStateEnum.Over;
                    }
                }

                foreach (Entite obj in _Entites)
                {
                    obj.ApplySpeed();

                    if (obj.TouchSomeLight == false)
                    {
                        obj.Score *= 0.975f;
                    }
                    else
                    {
                        obj.TouchSomeLight = false;
                    }
                }

                HandleDispawn();
            }
            break;
            case GameStateEnum.Over:
            {
                Cam.Center = (Cam.Center * 16 + Winner.Position) / 17;
                var sc = Winner.ScaledRadius * 6;
                float YZoom = (Cam.Zoom.Y * 16 + sc) / 17;
                Cam.Zoom = new Vec2(YZoom * WorldHitbox.SizeX / WorldHitbox.SizeY, YZoom);

                foreach(var c in AllControllers()) 
                {
                    if(c.Start.JustPressed || c.Select.JustPressed) 
                    {
                        GameState = GameStateEnum.Reset;
                    }
                }
            }
            break;
        }
        _Time.Frames++;

        Camera.Pop();
    }

    public override void Draw()
    {
        Camera.Push(Cam);

        foreach (Entite obj in _Entites)
        {
            obj.Draw();
        }

        foreach(var v in ParticlesLines) 
        {
            var d = v.Delta;
            if (d < 1) 
            {
                var c = v.C;
                SpriteBatch.DrawLine(v.PosBegin, v.PosEnd, v.C, v.Tickness * (1 - d), SpriteBatchExtension.LineEdgeMode.Circle);
            }
        }

        ParticlesLines = ParticlesLines.Where(c => c.Delta <= 1).ToList();
        Camera.Pop();


        Camera.Push(Camera.Hud);
        for (int i = (int)Controller.PlayerControlEnum.One; i <= (int)Controller.PlayerControlEnum.Four; i++)
        {
            int signX = (i - 1) % 2 == 0 ? -1 : 1;
            int signY = (i - 1) / 2 % 2 == 0 ? 1 : -1;

            float coefX = (signX + 1) / 2.0f;
            float coefY = (signY + 1) / 2.0f;

            if (Controller.From((Controller.PlayerControlEnum)i).IsConnected == false) { continue; }

            var e = _Entites.First(t => t.PlayerControl == (Controller.PlayerControlEnum)i && t.OwnedBy == null);
            if (e == null) { continue; }

            var c = e.Teams.GetColor();

            Vec2 shake_vec = Vec2.Zero;
            string txt = "P" + i;

            switch (GameState) 
            {
                case GameStateEnum.Begin: 
                {
                    if (e.Input.IsConnected) 
                    {
                        txt += " prêt";
                    }
                }
                break;
                case GameStateEnum.Playing:
                {
                    txt += " " + (e.CoefAboutToWin * 100).ToString("0") + " %";
                    float Shaking = Math.Max(0, e.CoefAboutToWin - 0.62f);
                    shake_vec = new Vec2(Rng.FloatUniform(-1, 1), Rng.FloatUniform(-1, 1)) * Shaking * 18;
                }break;
                case GameStateEnum.Over:
                {
                    txt += (e == Winner ? " winner" : " loser");
                }
                break;
                default:break;
            }

            float color_effect_begin_at = 0.3f;
            if(e.CoefAboutToWin > color_effect_begin_at) 
            {
                float c1 = (e.CoefAboutToWin - color_effect_begin_at) / (1 - color_effect_begin_at);
                float coef = (Angle.FromDegree(Time.Seconds * 360/(3f*(1-c1)+c1*1.5f)).Cos + 1) / 2;
                var tmp = (new Vec3(255, 255, 255) * (1 - coef) + new Vec3(c.R, c.G, c.B) * coef) / 255;
                c = new Color(tmp.X, tmp.Y, tmp.Z);
            }

            SpriteBatch.DrawText(txt, Camera.Peek().Rect.GetCoef(coefX, coefY) + shake_vec, new Vec2(coefX, coefY), c, (1 + e.CoefAboutToWin) * (float)SpriteBatchExtension.TextSize.Normal);
        }

        string[] debut_txt = new string[]
        {
            "À vos marques", "prêts ?", "Plantez!"
        };

        if (GameState == GameStateEnum.Begin) 
        {
            int idx = (int)Math.Floor(Time.Seconds);
            if(idx >= 0 && idx < debut_txt.Length) 
            {
                float m = Time.Seconds % 1;
                float y = 0.5f + (m - 0.5f) * 0.1f;

                SpriteBatch.DrawText(debut_txt[idx], Camera.Peek().Rect.GetCoef(0.5f, y), new Vec2(0.5f, 0.5f), Color.White, (2+ m) * (float)TextSize.Normal);
            }

        }
        Camera.Pop();

    }
}
