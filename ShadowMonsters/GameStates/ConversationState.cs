using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShadowMonsters.Characters;
using ShadowMonsters.ConversationComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.GameStates
{
    public interface IConversationState
    {
        void SetConversation(Character character);
        void StartConversation();
    }

    public class ConversationState : BaseGameState, IConversationState
    {
        #region Field Region

        private Conversation conversation;
        private Character speaker;
        private int frameCount = 0;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public ConversationState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IConversationState), this);
        }

        #endregion

        #region Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (conversation.CurrentScene == null)
            {
                return;
            }
            frameCount++;
            if ((Xin.CheckKeyReleased(Keys.Space) ||
                Xin.CheckKeyReleased(Keys.Enter) ||
                (Xin.CheckMouseReleased(MouseButtons.Left) &&
                conversation.CurrentScene.IsMouseOver)) && frameCount > 5)
            {
                frameCount = 0;
                switch (conversation.CurrentScene.OptionAction.Action)
                {
                    case ActionType.Teach:
                        BuyShadowMonster();
                        break;
                    case ActionType.Change:
                        speaker.SetConversation(conversation.CurrentScene.OptionScene);
                        manager.PopState();
                        break;
                    case ActionType.End:
                        manager.PopState();
                        break;
                    case ActionType.GiveItems:
                        break;
                    case ActionType.GiveKey:
                        if (conversation.CurrentScene.OptionAction.Parameter != null)
                        {
                            bool success = int.TryParse(
                                conversation.CurrentScene.OptionAction.Parameter,
                                out int key);

                            if (success)
                            {
                            }

                            conversation.ChangeScene(conversation.CurrentScene.OptionScene);
                        }
                        break;
                    case ActionType.Quest:
                        CheckQuest();
                        break;
                    case ActionType.Rest:
                        Game1.Player.HealBattleShadowMonsters();
                        conversation.ChangeScene(conversation.CurrentScene.OptionScene);
                        break;
                    case ActionType.Shop:
                        GameRef.ShopState.SetMerchant((Merchant)speaker);
                        manager.PopState();
                        manager.PushState(GameRef.ShopState);
                        break;
                    case ActionType.Talk:
                        conversation.ChangeScene(conversation.CurrentScene.OptionScene);
                        break;
                }
            }

            conversation.Update();
            base.Update(gameTime);
        }

        private void BuyShadowMonster()
        {
            Game1.Player.AddShadowMonster(speaker.GiveMonster);

            for (int i = 0; i < Player.MaxShadowMonsters; i++)
            {
                if (Game1.Player.BattleShadowMonsters[i] == null)
                {
                    Game1.Player.BattleShadowMonsters[i] = speaker.GiveMonster;
                    break;
                }
            }

            string scene = conversation.CurrentScene.OptionScene;

            conversation.CurrentScene.Options.RemoveAt(conversation.CurrentScene.SelectedIndex);
            conversation.CurrentScene.SelectedIndex = 0;
            conversation.ChangeScene(scene);
        }

        private void CheckQuest()
        {
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GameRef.SpriteBatch.Begin();
            conversation.Draw(GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();
        }

        public void SetConversation(Character character)
        {
            speaker = character;

            if (ConversationManager.Instance.ConversationList.ContainsKey(character.Conversation))
            {
                conversation = ConversationManager.Instance.ConversationList[character.Conversation];
            }
            else
            {
                manager.PopState();
            }
        }

        public void StartConversation()
        {
            if (conversation != null)
            {
                conversation.StartConversation();
            }
        }

        #endregion
    }
}
