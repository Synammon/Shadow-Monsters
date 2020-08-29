using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.TileEngine
{
    public class PortalLayer
    {
        #region Field Region

        private Dictionary<string, Portal> portals;
        private static Texture2D texture;

        #endregion

        #region Property Region
        public static Texture2D Texture { get => texture; set => texture = value; }

        public Dictionary<string, Portal> Portals
        {
            get { return portals; }
            private set { portals = value; }
        }

        #endregion

        #region Constructor Region

        public PortalLayer()
        {
            portals = new Dictionary<string, Portal>();
        }

        #endregion

        public void AddPortal(string name, Portal portal)
        {
            if (!portals.ContainsKey(name))
            {
                portals.Add(name, portal);
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(portals.Count);

            foreach (var r in portals.Keys)
            {
                writer.Write(r);
                portals[r].Save(writer);
            }
        }
        public static PortalLayer Load(BinaryReader reader)
        {
            PortalLayer layer = new PortalLayer();

            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                string r = reader.ReadString();
                Portal p = Portal.Load(reader);
                layer.portals.Add(r, p);
            }

            return layer;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                camera.Transformation);

            foreach (var s in portals.Keys)
            {
                Rectangle r = new Rectangle(
                    portals[s].SourceTile.X * Engine.TileWidth,
                    portals[s].SourceTile.Y * Engine.TileHeight,
                    Engine.TileWidth,
                    Engine.TileHeight);
                spriteBatch.Draw(texture, r, Color.Red);
            }

            spriteBatch.End();

        }
    }
}
