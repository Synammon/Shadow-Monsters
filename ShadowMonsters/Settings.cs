using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters
{
    public class Settings
    {
        private static float musicVolume = 0.5f;
        private static float soundVolume = 0.5f;
        private static Point resolution = new Point(1280, 720);
        
        public static Vector2 Scale
        {
            get
            {
                return new Vector2(
                    (float)resolution.X / 1280, 
                    (float)resolution.Y / 720);
            }
        }

        public static float MusicVolume 
        {
            get
            {
                return musicVolume;
            }

            set
            {
                musicVolume = MathHelper.Clamp(value, 0, 1f);
            }
        }

        public static float SoundVolume
        {
            get
            {
                return soundVolume;
            }

            set
            {
                soundVolume = MathHelper.Clamp(value, 0, 1f);
            }
        }

        public static Point Resolution
        {
            get { return resolution; }
            set { resolution = value; }
        }

        public static Point TileSize
        {
            get 
            {
                Point p = new Point(64, 64);

                if (resolution.X >= 4000)
                {
                    p = new Point(160, 160);
                }
                else if (resolution.X >= 3000)
                {
                    p = new Point(128, 128);
                }
                else if (resolution.X >= 1920)
                {
                    p = new Point(96, 96);
                }

                return p;
            }
        }

        public static float Multiplier
        {
            get
            {
                float multiplier = 1f;

                if (resolution.X >= 4000)
                {
                    multiplier = 4f;
                }
                else if (resolution.X >= 3000)
                {
                    multiplier = 3f;
                }
                else if (resolution.X >= 1920)
                {
                    multiplier = 2f;
                }

                return multiplier;
            }
        }

        public static void Save()
        {
            string path = Environment.SpecialFolder.LocalApplicationData.ToString();
            path += @"\Company Name\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += "settings.bin";

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                BinaryWriter writer = new BinaryWriter(stream);

                writer.Write(soundVolume);
                writer.Write(musicVolume);
                writer.Write(resolution.X);
                writer.Write(resolution.Y);
            }
        }

        public static void Load()
        {
            string path = Environment.SpecialFolder.LocalApplicationData.ToString();
            path += @"\Company Name\settings.bin";
            
            if (!File.Exists(path))
            {
                Save();
            }

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                BinaryReader reader = new BinaryReader(stream);

                soundVolume = reader.ReadSingle();
                musicVolume = reader.ReadSingle();
                resolution = new Point(reader.ReadInt32(), reader.ReadInt32());
            }
        }
    }
}
