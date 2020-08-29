using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.ConversationComponents
{
    public enum ActionType
    {
        Talk,
        End,
        Change,
        Quest,
        Teach,
        Shop,
        GiveItems,
        GiveKey,
        Battle,
        Rest,
    }

    public class SceneAction
    {
        public ActionType Action;
        public string Parameter;

    }

    public class SceneOption
    {
        private string optionText;
        private string optionScene;
        private SceneAction optionAction;

        public string OptionText
        {
            get { return optionText; }
            set { optionText = value; }
        }

        public string OptionScene
        {
            get { return optionScene; }
            set { optionScene = value; }
        }

        public SceneAction OptionAction
        {
            get { return optionAction; }
            set { optionAction = value; }
        }

        private SceneOption()
        {
        }

        public SceneOption(string text, string scene, SceneAction action)
        {
            optionText = text;
            optionScene = scene;
            optionAction = action;
        }

    }
}
