using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters
{
    public static class ExtensionMethods
    {
        public static void Write(this BinaryWriter writer, Point p)
        {
            writer.Write(p.X);
            writer.Write(p.Y);
        }

        public static Point ReadPoint(this BinaryReader reader)
        {
            return new Point(reader.ReadInt32(), reader.ReadInt32());
        }

        public static Rectangle Scale(this Rectangle r, Vector2 scale)
        {
            Rectangle scaled = new Rectangle(
                (int)(r.X * scale.X),
                (int)(r.Y * scale.Y),
                (int)(r.Width * scale.X),
                (int)(r.Height * scale.Y));

            return scaled;
        }

        public static Vector2 Scale(this Vector2 v, Vector2 scale)
        {
            Vector2 scaled = new Vector2(v.X * scale.X, v.Y * scale.Y);

            return scaled;
        }

        public static Vector3 Scale(this Vector3 c, Vector2 scale)
        {
            Vector3 scaled = new Vector3(c.X * scale.X, c.Y * scale.Y, 0);

            return scaled;
        }
    }
}
