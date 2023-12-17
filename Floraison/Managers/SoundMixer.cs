using Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Useful;
using SFX = Microsoft.Xna.Framework.Audio.SoundEffect;
using SFXinst = Microsoft.Xna.Framework.Audio.SoundEffectInstance;

namespace Floraison;

public class SoundPack{
    public List<SFX> Sounds;
    public List<SFXinst> SoundsInst;
    private int lastPlayed = 0;
    public void Play()
    {
        SoundsInst[lastPlayed].Stop();
        int r = All.Rng.IntUniform(0, SoundsInst.Count-1);
        SoundsInst[r].Play();
        lastPlayed = r;
    }

    public SoundPack(string name)
    {
        Sounds = new List<SFX>();
        SoundsInst = new List<SFXinst>();


        int i = 0;
        Console.WriteLine("Chargement son");
        while (true) 
        {
            try 
            {

                SFX s = All.Content.Load<SFX>(name + "_" + i);
                Console.WriteLine("Son charge " + name + "_" + i);
                i++;
                Sounds.Push(s);
                SoundsInst.Push(s.CreateInstance());
            }
            catch 
            {
                break;
            }
        }
    }
}

public class SoundMixer
{
    private SoundMixer() {}

    
    public static SoundPack pot_hit;
    public static SoundPack bramble_hit;
    public static SoundPack victory;

    public static void Load()
    {
        pot_hit = new SoundPack("hit_pot");
        bramble_hit = new SoundPack("hit_bramble");
        victory = new SoundPack("victory");
    }

}