﻿using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Useful;

namespace Floraison;

public class TheGame : TimeRelated
{
    /// <summary>
    /// They will be in Entites at the end of the frame
    /// </summary>
    public List<Entite> _CreatedThisFrame = new();
    public List<Entite> _Entites = new();

    public new const int FrameRate = 60;
    private GTime _Time;
    public GTime Time => _Time;

    public Rect2F WorldHitbox = Rect2F.Center(new Vec2(16, 9)*4, Vec2.Zero);
    public Camera Cam;

    public TheGame() 
    {
        Cam = Camera.Center(WorldHitbox);
    }

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
        for (int i = _Entites.Count; i <= 0; i--)
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

public override void Update()
    {
        Camera.Push(Cam);

        foreach (Controller c in AllControllers()) 
        {
            c.Update();
        }

        HandleSpawn();
        foreach (Entite obj in _Entites)
        {
            obj.Update();
        }

        foreach (Entite obj in _Entites)
        {
            obj.ApplySpeed();
        }

        HandleDispawn();
        _Time.Frames++;

        Camera.Pop();

        Camera.Push(Camera.Hud);
        for (int i = (int)Controller.PlayerControlEnum.One; i <= (int)Controller.PlayerControlEnum.Four; i++)
        {
            int signX = (i - 1) / 2 % 2 == 0 ? -1 : 1;
            int signY = (i - 1) % 2 == 0 ? -1 : 1;

            float coefX = (signX + 1) / 2.0f;
            float coefY = (signY + 1) / 2.0f;

            if (Controller.From((Controller.PlayerControlEnum)i).IsConnected == false) { continue; }

            var e = _Entites.First(t => t.PlayerControl == (Controller.PlayerControlEnum)i);
            if(e == null) { continue; }

            var c = e.Teams.GetColor();

            //SpriteBatch.DrawText("P1", Camera.Peek().Rect.GetCoef(coefX, coefY), new Vec2(1 - coefX, 1 - coefY), c);
        }
        Camera.Pop();
    }

    public override void Draw()
    {
        Camera.Push(Cam);

        foreach (Entite obj in _Entites)
        {
            obj.Draw();
        }

        Camera.Pop();
    }
}
