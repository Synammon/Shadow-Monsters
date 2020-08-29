using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.TileEngine
{
    public class Portal
    {
        #region Field Region

        Point sourceTile;
        Point destinationTile;
        string destinationLevel;

        #endregion

        #region Property Region

        public Point SourceTile
        {
            get { return sourceTile; }
            private set { sourceTile = value; }
        }

        public Point DestinationTile
        {
            get { return destinationTile; }
            private set { destinationTile = value; }
        }

        public string DestinationLevel
        {
            get { return destinationLevel; }
            private set { destinationLevel = value; }
        }

        #endregion

        #region Constructor Region

        private Portal()
        {
        }

        public Portal(Point sourceTile, Point destinationTile, string destinationLevel)
        {
            SourceTile = sourceTile;
            DestinationTile = destinationTile;
            DestinationLevel = destinationLevel;
        }

        #endregion

        #region Method Region

        public void Save(BinaryWriter writer)
        {
            writer.Write(destinationLevel);
            writer.Write(sourceTile.X);
            writer.Write(sourceTile.Y);
            writer.Write(destinationTile.X);
            writer.Write(destinationTile.Y);
        }
        public static Portal Load(BinaryReader reader)
        {
            Portal p = new Portal
            {
                DestinationLevel = reader.ReadString(),
                SourceTile = new Point(reader.ReadInt32(), reader.ReadInt32()),
                DestinationTile = new Point(reader.ReadInt32(), reader.ReadInt32())
            };

            return p;
        }

        #endregion
    }
}
