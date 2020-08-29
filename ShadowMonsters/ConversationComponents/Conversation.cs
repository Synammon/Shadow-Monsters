using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.ConversationComponents
{
    public class Conversation
    {
        #region Field Region

        private string name;
        private string firstScene;
        private string currentScene;
        private readonly Dictionary<string, GameScene> scenes;
        private string backgroundName;
        private Texture2D background;

        #endregion

        #region Property Region

        public string Name
        {
            get { return name; }
        }

        public string FirstScene
        {
            get { return firstScene; }
        }

        [ContentSerializerIgnore]
        public GameScene CurrentScene
        {
            get
            {
                if (!string.IsNullOrEmpty(currentScene) && scenes.ContainsKey(currentScene))
                    return scenes[currentScene];
                return null;
            }
        }

        public Dictionary<string, GameScene> Scenes
        {
            get { return scenes; }
        }

        public string BackgroundName
        {
            get { return backgroundName; }
            set { backgroundName = value; }
        }

        [ContentSerializerIgnore]
        public Texture2D Background
        {
            get { return background; }
        }

        #endregion

        #region Constructor Region

        private Conversation()
        {
            scenes = new Dictionary<string, GameScene>();
        }

        public Conversation(string name, string firstScene)
        {
            this.scenes = new Dictionary<string, GameScene>();
            this.name = name;
            this.firstScene = firstScene;
        }

        #endregion

        #region Method Region

        public void LoadContent(ContentManager conetnt)
        {
            background = conetnt.Load<Texture2D>(@"Textures\" + BackgroundName);
        }

        public void RemoveScene(string optionText)
        {
            scenes.Remove(optionText);
        }

        public void Update()
        {
            if (CurrentScene != null)
            {
                CurrentScene.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentScene != null)
            {
                CurrentScene.Draw(spriteBatch, background);
            }
        }

        public void AddScene(string sceneName, GameScene scene)
        {
            if (!scenes.ContainsKey(sceneName))
            {
                scenes.Add(sceneName, scene);
            }
        }

        public GameScene GetScene(string sceneName)
        {
            return scenes.ContainsKey(sceneName) ? scenes[sceneName] : null;
        }

        public void StartConversation()
        {
            currentScene = firstScene;
        }

        public void ChangeScene(string sceneName)
        {
            currentScene = sceneName;
        }
        #endregion
    }
}
