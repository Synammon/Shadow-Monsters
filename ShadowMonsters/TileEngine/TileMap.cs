using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using ShadowMonsters.Characters;

namespace ShadowMonsters.TileEngine
{
    public class TileMap
    {
        #region Field Region

        string mapName;
        TileLayer groundLayer;
        TileLayer edgeLayer;
        TileLayer buildingLayer;
        TileLayer decorationLayer;
        CharacterLayer characterLayer;
        CollisionLayer collisionLayer;
        PortalLayer portalLayer;
        MonsterLayer monsterLayer;
        WildLayer wildLayer;

        int mapWidth;
        int mapHeight;

        TileSet tileSet;

        #endregion

        #region Property Region

        public string MapName
        {
            get { return mapName; }
            private set { mapName = value; }
        }

        public TileSet TileSet
        {
            get { return tileSet; }
            set { tileSet = value; }
        }

        public TileLayer GroundLayer
        {
            get { return groundLayer; }
            set { groundLayer = value; }
        }

        public TileLayer EdgeLayer
        {
            get { return edgeLayer; }
            set { edgeLayer = value; }
        }

 
        public TileLayer BuildingLayer
        {
            get { return buildingLayer; }
            set { buildingLayer = value; }
        }

        public int MapWidth
        {
            get { return mapWidth; }
        }

        public int MapHeight
        {
            get { return mapHeight; }
        }

        public int WidthInPixels
        {
            get { return mapWidth * Engine.TileWidth; }
        }

        public int HeightInPixels
        {
            get { return mapHeight * Engine.TileHeight; }
        }

        public CharacterLayer CharacterLayer => characterLayer;

        public CollisionLayer CollisionLayer => collisionLayer;

        public PortalLayer PortalLayer => portalLayer;

        public MonsterLayer MonsterLayer => monsterLayer;

        public WildLayer WildLayer => wildLayer;
        #endregion

        #region Constructor Region

        private TileMap()
        {
            characterLayer = new CharacterLayer();
            collisionLayer = new CollisionLayer();
            portalLayer = new PortalLayer();
            monsterLayer = new MonsterLayer();
            wildLayer = new WildLayer();
        }

        private TileMap(TileSet tileSet, string mapName)
            : this()
        {
            this.tileSet = tileSet;
            this.mapName = mapName;
        }

        public TileMap(
            TileSet tileSet,
            TileLayer groundLayer,
            TileLayer edgeLayer,
            TileLayer buildingLayer,
            TileLayer decorationLayer,
            string mapName)
            : this(tileSet, mapName)
        {
            this.groundLayer = groundLayer;
            this.edgeLayer = edgeLayer;
            this.buildingLayer = buildingLayer;
            this.decorationLayer = decorationLayer;

            mapWidth = groundLayer.Width;
            mapHeight = groundLayer.Height;
        }

        #endregion

        #region Method Region

        public void SetGroundTile(int x, int y, int set, int index)
        {
            groundLayer.SetTile(x, y, set, index);
        }

        public Tile GetGroundTile(int x, int y)
        {
            return groundLayer.GetTile(x, y);
        }

        public void SetEdgeTile(int x, int y, int set, int index)
        {
            edgeLayer.SetTile(x, y, set, index);
        }

        public Tile GetEdgeTile(int x, int y)
        {
            return edgeLayer.GetTile(x, y);
        }

        public void SetBuildingTile(int x, int y, int set, int index)
        {
            buildingLayer.SetTile(x, y, set, index);
        }

        public Tile GetBuildingTile(int x, int y)
        {
            return buildingLayer.GetTile(x, y);
        }

        public void SetDecorationTile(int x, int y, int set, int index)
        {
            decorationLayer.SetTile(x, y, set, index);
        }

        public Tile GetDecorationTile(int x, int y)
        {
            return decorationLayer.GetTile(x, y);
        }

        public void FillEdges()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    edgeLayer.SetTile(x, y, -1, -1);
                }
            }
        }

        public void FillBuilding()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    buildingLayer.SetTile(x, y, -1, -1);
                }
            }
        }

        public void FillDecoration()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    decorationLayer.SetTile(x, y, -1, -1);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (groundLayer != null)
                groundLayer.Update(gameTime);

            if (edgeLayer != null)
                edgeLayer.Update(gameTime);

            if (buildingLayer != null)
                buildingLayer.Update(gameTime);

            if (decorationLayer != null)
                decorationLayer.Update(gameTime);

            characterLayer.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera, bool debug = false)
        {
            if (groundLayer != null)
                groundLayer.Draw(gameTime, spriteBatch, tileSet, camera);

            if (edgeLayer != null)
                edgeLayer.Draw(gameTime, spriteBatch, tileSet, camera);

            characterLayer.Draw(gameTime, spriteBatch, camera);

            if (buildingLayer != null)
                buildingLayer.Draw(gameTime, spriteBatch, tileSet, camera);

            if (decorationLayer != null)
                decorationLayer.Draw(gameTime, spriteBatch, tileSet, camera);

            if (debug)
            {
                collisionLayer.Draw(spriteBatch, camera);
                portalLayer.Draw(spriteBatch, camera);
            }

            monsterLayer.Draw(gameTime, spriteBatch, camera);
        }

        public bool Save(BinaryWriter writer)
        {
            writer.Write(mapName);

            characterLayer.Save(writer);
            tileSet.Save(writer);
            edgeLayer.Save(writer);
            groundLayer.Save(writer);
            decorationLayer.Save(writer);
            buildingLayer.Save(writer);
            portalLayer.Save(writer);
            collisionLayer.Save(writer);
            monsterLayer.Save(writer);
            wildLayer.Save(writer);

            return true;
        }

        public static TileMap Load(ContentManager content, BinaryReader reader)
        {
            TileMap map = new TileMap();

            map.mapName = reader.ReadString();
            map.characterLayer = CharacterLayer.Load(content, reader);
            map.tileSet = TileSet.Load(content, reader);
            map.edgeLayer = TileLayer.Load(reader);
            map.groundLayer = TileLayer.Load(reader);
            map.decorationLayer = TileLayer.Load(reader);
            map.buildingLayer = TileLayer.Load(reader);
            map.portalLayer = PortalLayer.Load(reader);
            map.collisionLayer = CollisionLayer.Load(reader);
            map.monsterLayer = MonsterLayer.Load(content, reader);
            map.wildLayer = WildLayer.Load(reader);

            map.mapWidth = map.groundLayer.Width;
            map.mapHeight = map.groundLayer.Height;

            return map;
        }
        #endregion
    }
}
