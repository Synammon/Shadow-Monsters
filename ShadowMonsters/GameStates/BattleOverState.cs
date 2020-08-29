using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShadowMonsters.ShadowMonsters;

namespace ShadowMonsters.GameStates
{
    public interface IBattleOverState
    {
        void SetShadowMonsters(ShadowMonster player, ShadowMonster enemy);
    }

    public class BattleOverState : BaseGameState, IBattleOverState
    {
        #region Field Region

        private ShadowMonster player;
        private ShadowMonster enemy;
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
        private readonly string[] battleState;
        private Vector2 battlePosition;
        private bool levelUp;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public BattleOverState(Game game)
            : base(game)
        {
            battleState = new string[3];

            battleState[0] = "The battle was won!";
            battleState[1] = " gained ";
            battleState[2] = "Continue";

            battlePosition = new Vector2(25, 475);

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
            combatBackground = new Texture2D(GameRef.GraphicsDevice, Settings.Resolution.X, Settings.Resolution.Y);
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

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Xin.CheckKeyReleased(Keys.Space) || Xin.CheckKeyReleased(Keys.Enter))
            {
                if (levelUp)
                {
                    manager.PushState((LevelUpState)GameRef.LevelUpState);
                    GameRef.LevelUpState.SetShadowMonster(player);

                    this.Visible = true;
                }
                else if (Game1.Player.Alive())
                {
                    manager.PopState();
                    manager.PopState();
                }
                else
                {
                    manager.PopState();
                    //manager.PopState();
                    // should warp to a location since the player has no ShadowMonsters
                    // with no access to the world it is hard to say where to warp
                    // to
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 position = new Vector2(50f, Settings.Resolution.Y * .75f);

            base.Draw(gameTime);

            GameRef.SpriteBatch.Begin();

            GameRef.SpriteBatch.Draw(combatBackground, Vector2.Zero, Color.White);
            Vector2 scale = new Vector2(
                Settings.Resolution.X / 1280,
                Settings.Resolution.Y / 720);

            for (int i = 0; i < 2; i++)
            {
                GameRef.SpriteBatch.DrawString(
                    FontManager.GetFont("testfont"),
                    battleState[i],
                    position.Scale(scale),
                    Color.Black);
                position.Y += FontManager.GetFont("testfont").LineSpacing;
            }

            GameRef.SpriteBatch.DrawString(
                FontManager.GetFont("testfont"),
                battleState[2],
                position.Scale(scale),
                Color.Red);


            GameRef.SpriteBatch.Draw(player.Texture, playerRect.Scale(scale), Color.White);

            GameRef.SpriteBatch.Draw(enemy.Texture, enemyRect.Scale(scale), Color.White);

            GameRef.SpriteBatch.Draw(ShadowMonsterBorder, playerBorderRect.Scale(scale), Color.White);

            playerHealth = (float)player.CurrentHealth / (float)player.GetHealth();
            MathHelper.Clamp(playerHealth, 0f, 1f);
            playerHealthRect.Width = (int)(playerHealth * 366);

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
            levelUp = false;
            this.player = player;
            this.enemy = enemy;


            long expGained;
            if (player.Alive)
            {
                expGained = player.WinBattle(enemy);
                battleState[0] = player.DisplayName + " has won the battle!";
                battleState[1] = player.DisplayName + " has gained " + expGained + " experience";

                if (player.CheckLevelUp())
                {
                    battleState[1] += " and gained a level!";

                    foreach (string s in player.KnownMoves.Keys)
                    {
                        if (player.KnownMoves[s].Unlocked == false &&
                            player.Level >= player.KnownMoves[s].UnlockedAt)
                        {
                            player.KnownMoves[s].Unlock();
                            battleState[1] += " " + s + " was unlocked!";
                        }
                    }

                    levelUp = true;
                }
                else
                {
                    battleState[1] += ".";
                }
            }
            else
            {
                expGained = player.LoseBattle(enemy);

                battleState[0] = player.DisplayName + " has lost the battle.";
                battleState[1] = player.DisplayName + " has gained " + expGained + " experience";

                if (player.CheckLevelUp())
                {
                    battleState[1] += " and gained a level!";

                    foreach (string s in player.KnownMoves.Keys)
                    {
                        if (player.KnownMoves[s].Unlocked == false && player.Level >= player.KnownMoves[s].UnlockedAt)
                        {
                            player.KnownMoves[s].Unlock();
                            battleState[1] += " " + s + " was unlocked!";
                        }
                    }

                    levelUp = true;
                }
                else
                {
                    battleState[1] += ".";
                }
            }
        }

        #endregion
    }
}
