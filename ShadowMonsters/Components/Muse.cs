using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.Components
{
    public class Muse : GameComponent
    {
        public static Dictionary<string, Song> Songs { get; private set; }
        public static Dictionary<string, SoundEffect> SoundEffects { get; private set;}
        private static Song Song { get; set; }

        public Muse(Game game)
            : base(game)
        {
            Songs = new Dictionary<string, Song>();
            SoundEffects = new Dictionary<string, SoundEffect>();            
        }

        public static void PlaySong(string name)
        {
            if (Songs.ContainsKey(name))
            {
                Song = Songs[name];
                MediaPlayer.Play(Song);
            }
        }

        public static void StopSong()
        {
            MediaPlayer.Stop();                                                               
        }

        public static void PlaySoundEffect(string name)
        {
            if (SoundEffects.ContainsKey(name))
            {
                SoundEffects[name].Play();
            }
        }

        public static void SetSongVolume(float volume)
        {
            MediaPlayer.Volume = MathHelper.Clamp(volume, 0f, 1f);
        }

        public static void SetEffectVolume(float volume)
        {
            SoundEffect.MasterVolume = MathHelper.Clamp(volume, 0f, 1f);
        }
    }
}
