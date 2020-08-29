using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters
{
    public static class ExtensionMethods
    {
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
    }
}
