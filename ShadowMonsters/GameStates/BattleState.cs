using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShadowMonsters.ConversationComponents;
using ShadowMonsters.ShadowMonsters;
using System.Collections.Generic;

namespace ShadowMonsters.GameStates
{
    public interface IBattleState
    {
        void SetShadowMonsters(ShadowMonster player, ShadowMonster enemy);
        void StartBattle();
        void ChangePlayerShadowMonster(ShadowMonster selected);
    }

    public class BattleState : BaseGameState, IBattleState
    {
        #region Field Region

        private ShadowMonster player;
        private ShadowMonster enemy;
        private GameScene combatScene;
        private Texture2D combatBackground;
        private Rectangle playerRect;
        private Rectangle enemyRect;
        private Rectangle playerBorderRect;
        private Rectangle enemyBorderRect;
        private Rectangle playerMiniRect;
        private Rectangle enemyMiniRect;
        private Rectangle playerHealthRect;
        private Rectangle enemyHealthRect;
        private Rectangle healthSourceRect;
        private Vector2 playerName;
        private Vector2 enemyName;
        private float playerHealth;
        private float enemyHealth;
        private Texture2D ShadowMonsterBorder;
        private Texture2D ShadowMonsterHealth;

        public ShadowMonster EnemyShadowMonster { get { return enemy; } }

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public BattleState(Game game)
            : base(game)
        {
            playerRect = new Rectangle(10, 90, 400, 400);
            enemyRect = new Rectangle(1280 - 425, 10, 400, 400);

            playerBorderRect = new Rectangle(10, 10, 400, 75);
            enemyBorderRect = new Rectangle(1280 - 425, 420, 400, 75);

            healthSourceRect = new Rectangle(10, 50, 390, 20);
            playerHealthRect = new Rectangle(playerBorderRect.X + 12, playerBorderRect.Y + 52, 386, 16);
            enemyHealthRect = new Rectangle(enemyBorderRect.X + 12, enemyBorderRect.Y + 52, 386, 16);

            playerMiniRect = new Rectangle(playerBorderRect.X + 11, playerBorderRect.Y + 11, 28, 28);
            enemyMiniRect = new Rectangle(enemyBorderRect.X + 11, enemyBorderRect.Y + 11, 28, 28);

            playerName = new Vector2(playerBorderRect.X + 55, playerBorderRect.Y + 5);
            enemyName = new Vector2(enemyBorderRect.X + 55, enemyBorderRect.Y + 5);
        }

        #endregion

        #region Method Region

        protected override void LoadContent()
        {
            if (combatScene == null)
            {
                combatBackground = new Texture2D(GraphicsDevice, Settings.Resolution.X, Settings.Resolution.Y);
                Color[] buffer = new Color[Settings.Resolution.X * Settings.Resolution.Y];

                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = Color.White;
                }

                combatBackground.SetData(buffer);

                ShadowMonsterBorder = new Texture2D(GraphicsDevice, 300, 75);
                ShadowMonsterHealth = new Texture2D(GraphicsDevice, 300, 25);
                
                buffer = new Color[300 * 75];

                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = Color.Green;
                }

                ShadowMonsterBorder.SetData(buffer);

                buffer = new Color[300 * 25];

                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = Color.Red;
                }

                ShadowMonsterHealth.SetData(buffer);

                combatScene = new GameScene(GameRef, "", new List<SceneOption>());
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Xin.CheckKeyReleased(Keys.P))
            {
                manager.PopState();
            }

            combatScene.Update();

            if (Xin.CheckKeyReleased(Keys.Space) ||
                Xin.CheckKeyReleased(Keys.Enter) ||
                (Xin.CheckMouseReleased(MouseButtons.Left) &&
                combatScene.IsMouseOver))
            {
                manager.PushState((DamageState)GameRef.DamageState);
                GameRef.DamageState.SetShadowMonsters(player, enemy);

                IMove enemyMove = null;

                do
                {
                    int move = random.Next(0, enemy.KnownMoves.Count);
                    int i = 0;

                    foreach (string s in enemy.KnownMoves.Keys)
                    {
                        if (move == i)
                        {
                            enemyMove = (IMove)enemy.KnownMoves[s].Clone();
                        }

                        i++;
                    }

                } while (!enemyMove.Unlocked);

                GameRef.DamageState.SetMoves(
                    (IMove)player.KnownMoves[combatScene.OptionText].Clone(),
                    enemyMove);
                GameRef.DamageState.Start();

                player.Update();
                enemy.Update();
            }

            Visible = true;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GameRef.SpriteBatch.Begin();

            combatScene.Draw(GameRef.SpriteBatch, combatBackground);

            Vector2 scale = new Vector2(
                Settings.Resolution.X / 1280,
                Settings.Resolution.Y / 720);

            GameRef.SpriteBatch.Draw(player.Texture, playerRect.Scale(scale), Color.White);

            GameRef.SpriteBatch.Draw(enemy.Texture, enemyRect.Scale(scale), Color.White);

            GameRef.SpriteBatch.Draw(ShadowMonsterBorder, playerBorderRect.Scale(scale), Color.White);

            playerHealth = (float)player.CurrentHealth / (float)player.GetHealth();
            MathHelper.Clamp(playerHealth, 0f, 1f);
            playerHealthRect.Width = (int)(playerHealth * 366 );

            GameRef.SpriteBatch.Draw(ShadowMonsterHealth, playerHealthRect.Scale(scale), healthSourceRect, Color.White);

            GameRef.SpriteBatch.Draw(ShadowMonsterBorder, enemyBorderRect.Scale(scale), Color.White);

            enemyHealth = (float)enemy.CurrentHealth / (float)enemy.GetHealth();
            MathHelper.Clamp(enemyHealth, 0f, 1f);
            enemyHealthRect.Width = (int)(enemyHealth * 366);

            GameRef.SpriteBatch.Draw(
                ShadowMonsterHealth,
                enemyHealthRect.Scale(scale),
                healthSourceRect,
                Color.White);
            GameRef.SpriteBatch.DrawString(
                FontManager.GetFont("testfont"),
                player.DisplayName,
                playerName.Scale(scale),
                Color.White);
            GameRef.SpriteBatch.DrawString(
                FontManager.GetFont("testfont"),
                enemy.DisplayName,
                enemyName.Scale(scale),
                Color.White);

            GameRef.SpriteBatch.Draw(
                player.Texture,
                playerMiniRect.Scale(scale),
                Color.White);

            GameRef.SpriteBatch.Draw(
                enemy.Texture,
                enemyMiniRect.Scale(scale),
                Color.White);

            GameRef.SpriteBatch.End();
        }

        public void SetShadowMonsters(ShadowMonster player, ShadowMonster enemy)
        {
            this.player = player;
            this.enemy = enemy;

            player.StartCombat();
            enemy.StartCombat();

            List<SceneOption> moves = new List<SceneOption>();

            if (combatScene == null)
            {
                LoadContent();
            }

            foreach (string s in player.KnownMoves.Keys)
            {
                if (player.KnownMoves[s].Unlocked)
                {
                    SceneOption option = new SceneOption(s, s, new SceneAction());
                    moves.Add(option);
                }
            }

            combatScene.SelectedIndex = 0;
            combatScene.Options = moves;
        }

        public void StartBattle()
        {
            player.StartCombat();
            enemy.StartCombat();
            playerHealth = 100f;
            enemyHealth = 100f;
        }

        public void ChangePlayerShadowMonster(ShadowMonster selected)
        {
            this.player = selected;

            List<SceneOption> moves = new List<SceneOption>();

            foreach (string s in player.KnownMoves.Keys)
            {
                if (player.KnownMoves[s].Unlocked)
                {
                    SceneOption option = new SceneOption(s, s, new SceneAction());
                    moves.Add(option);
                }
            }

            combatScene.SelectedIndex = 0;
            combatScene.Options = moves;
        }

        #endregion
    }
}
