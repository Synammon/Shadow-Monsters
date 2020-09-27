using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.TileEngine
{
    public class WildArea
    {
        public Point TopLeft { get; set; }
        public Point BottomRight { get; set; }
        public List<string> Monsters { get; set; }

        public WildArea()
        {
            Monsters = new List<string>();
        }
    }

    public class WildLayer
    {
        #region Field Region

        private List<WildArea> areas = new List<WildArea>();
        private static Random random = new Random();

        #endregion

        #region Property Region
        
        public List<WildArea> Areas
        {
            get { return areas; }
        }

        #endregion

        #region Constructor Region
        #endregion

        #region Method Region

        public WildArea Enter(Point tile)
        {
            foreach (var w in areas)
            {
                if (w.TopLeft.X <= tile.X && w.BottomRight.X >= tile.X &&
                    w.TopLeft.Y <= tile.Y && w.BottomRight.Y >= tile.Y)
                {
                    return w;
                }
            }

            return null;
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(areas.Count);

            foreach (var w in areas)
            {
                writer.Write(w.TopLeft);
                writer.Write(w.BottomRight);

                writer.Write(w.Monsters.Count);
                foreach (var m in w.Monsters)
                {
                    writer.Write(m);
                    writer.Write(-1);
                }
            }
        }

        public static WildLayer Load(BinaryReader reader)
        {
            WildLayer layer = new WildLayer();

            int layers = reader.ReadInt32();

            for (int i = 0; i < layers; i++)
            {
                WildArea area = new WildArea
                {
                    TopLeft = reader.ReadPoint(),
                    BottomRight = reader.ReadPoint()
                };

                int monsters = reader.ReadInt32();

                for (int j = 0; j < monsters; j++)
                {
                    string s = reader.ReadString();
                    reader.ReadInt32();

                    area.Monsters.Add(s);
                }

                layer.Areas.Add(area);
            }
            return layer;
        }
        #endregion
    }
}
