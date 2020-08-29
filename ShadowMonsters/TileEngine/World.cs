using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.TileEngine
{
    public class World
    {
        #region Field Region

        private readonly Dictionary<string, TileMap> maps = new Dictionary<string, TileMap>();
        private string currentMapName;
        private Portal startingMap;

        #endregion

        #region Property Region

        public Dictionary<string, TileMap> Maps
        {
            get { return maps; }
        }

        public TileMap Map
        {
            get { return maps[currentMapName]; }
        }

        public string CurrentMapName
        {
            get { return currentMapName; }
        }

        public Portal StartingMap
        {
            get { return startingMap; }
        }

        #endregion

        #region Constructor Region

        private World()
        {
        }

        public World(Portal portal)
        {
            startingMap = portal;
        }

        #endregion

        #region Method Region

        public void AddMap(string mapName, TileMap map)
        {
            maps.Add(mapName, map);
        }

        public void ChangeMap(Portal portal)
        {
            if (maps.ContainsKey(portal.DestinationLevel))
            {
                currentMapName = portal.DestinationLevel;
            }
        }

        public void ChangeMap(string mapName)
        {
            if (maps.ContainsKey(mapName))
            {
                currentMapName = mapName;
            }
        }

        public void Update(GameTime gameTime)
        {
            maps[currentMapName].Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera, bool debug = false)
        {
            maps[currentMapName].Draw(gameTime, spriteBatch, camera, debug);
        }

        public void Save(BinaryWriter writer)
        {
            startingMap.Save(writer);
            writer.Write(maps.Count);

            foreach (TileMap map in maps.Values)
            {
                map.Save(writer);
            }
        }

        public static World Load(ContentManager content, BinaryReader reader)
        {
            World world = new World();

            Portal p = Portal.Load(reader);
            world.startingMap = p;

            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                TileMap map = TileMap.Load(content, reader);
                world.Maps.Add(map.MapName, map);
            }

            world.ChangeMap(p);

            return world;
        }
        #endregion
    }
}
