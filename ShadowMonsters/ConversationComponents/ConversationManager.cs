using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.ConversationComponents
{
    public class ConversationManager
    {
        #region Field Region
        private static ConversationManager instance = new ConversationManager();
        private Dictionary<string, Conversation> conversationList = new Dictionary<string, Conversation>();

        #endregion

        #region Property Region

        public static ConversationManager Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        public Dictionary<string, Conversation> ConversationList
        {
            get { return conversationList; }
            private set { conversationList = value; }
        }

        #endregion

        #region Constructor Region

        private ConversationManager()
        {
        }

        #endregion

        #region Method Region
        public void AddConversation(string name, Conversation conversation)
        {
            if (!conversationList.ContainsKey(name))
                conversationList.Add(name, conversation);
        }

        public Conversation GetConversation(string name)
        {
            if (conversationList.ContainsKey(name))
                return conversationList[name];

            return null;
        }

        public bool ContainsConversation(string name)
        {
            return conversationList.ContainsKey(name);
        }

        public void ClearConversations()
        {
            conversationList = new Dictionary<string, Conversation>();
        }

        public void CreateConversations(Game gameRef)
        {
            ConversationList.Clear();
            Conversation c = new Conversation("PaulHello", "Hello")
            {
                BackgroundName = "scenebackground",
            };

            List<SceneOption> options = new List<SceneOption>();
            SceneOption teach = new SceneOption(
                "Teach",
                "Teach",
                new SceneAction() { Action = ActionType.Teach, Parameter = "none" });
            options.Add(teach);

            SceneOption rest = new SceneOption(
                "Rest",
                "Rest",
                new SceneAction { Action = ActionType.Rest, Parameter = "none" });
            options.Add(rest);

            SceneOption option = new SceneOption(
                "Good bye.",
                "",
                new SceneAction() { Action = ActionType.End, Parameter = "none" });
            options.Add(option);

            GameScene scene = new GameScene(
                gameRef,
                "Hello, my name is Paul. I'm still learning about training shadow monsters.",
                options);

            c.AddScene("Hello", scene);

            options = new List<SceneOption>();

            scene = new GameScene(
                gameRef,
                "I have given you Brownie!",
                options);

            option = new SceneOption(
                "Goodbye",
                "",
                new SceneAction() { Action = ActionType.End, Parameter = "none" });

            options.Add(option);
            c.AddScene("Teach", scene);

            options = new List<SceneOption>();

            scene = new GameScene(
                gameRef,
                "I have restored your shadow monsters' health.",
                options);
            options.Add(option);

            option = new SceneOption(
                "Goodbye",
                "",
                new SceneAction() { Action = ActionType.End, Parameter = "none" });

            c.AddScene("Rest", scene);
            
            ConversationList.Add("PaulHello", c);

            c = new Conversation("BonnieHello", "Hello");

            options = new List<SceneOption>();

            option = new SceneOption(
                "Shop",
                "",
                new SceneAction() { Action = ActionType.Shop, Parameter = "none" });

            options.Add(option);

            option = new SceneOption(
                "Goodbye",
                "",
                new SceneAction() { Action = ActionType.End, Parameter = "none" });

            options.Add(option);

            scene = new GameScene(
                gameRef,
                "Hi! I'm Bonnie. Feel free to browse my wares.",
                options);

            c.AddScene("Hello", scene);
            ConversationList.Add("BonnieHello", c);
        }

        #endregion
    }
}