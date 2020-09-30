using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShadowMonsters.ConversationComponents;
using ShadowMonsters.ShadowMonsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShadowMonsters.GameStates
{
    public interface IActionSelectionState
    {
        void Selection();
        void SetShadowMonsters(ShadowMonster player, ShadowMonster enemy);
    }

    public class ActionSelectionState : BaseGameState, IActionSelectionState
    {
        private ShadowMonster player;
        private ShadowMonster enemy;
        private GameScene scene;
        private Texture2D background;
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
        private int frameCount = 0;

        public static bool IsTrainerBattle { get; set; }

        public ActionSelectionState(Game game) : base(game)
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

        protected override void LoadContent()
        {
            base.LoadContent();

            background = new Texture2D(
                GraphicsDevice, 
                Settings.Resolution.X, 
                Settings.Resolution.Y);
            Color[] buffer = new Color[Settings.Resolution.X * Settings.Resolution.Y];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.White;
            }

            background.SetData(buffer);

            ShadowMonsterBorder = new Texture2D(GraphicsDevice, 400, 75);
            ShadowMonsterHealth = new Texture2D(GraphicsDevice, 400, 25);

            buffer = new Color[400 * 75];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.Green;
            }

            ShadowMonsterBorder.SetData(buffer);

            buffer = new Color[400 * 25];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.Red;
            }

            ShadowMonsterHealth.SetData(buffer);

            scene = new GameScene(GameRef, "", new List<SceneOption>());
            SceneOption option = new SceneOption("Fight", "Fight", new SceneAction());
            scene.Options.Add(option);

            option = new SceneOption("Switch", "Switch", new SceneAction());
            scene.Options.Add(option);

            option = new SceneOption("Item", "Item", new SceneAction());
            scene.Options.Add(option);

            option = new SceneOption("Flee", "Flee", new SceneAction());
            scene.Options.Add(option);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            frameCount++;
            scene.Update();
            if (Xin.CheckKeyReleased(Keys.Space) || Xin.CheckKeyReleased(Keys.Enter) &&
                frameCount >= 5)
            {
                frameCount = 0;
                manager.PopState();
                if (scene.SelectedIndex == 0)
                {
                    // do nothing
                }
                if (scene.SelectedIndex == 1)
                {
                    manager.PushState((ShadowMonsterSelectionState)GameRef.ShadowMonsterSelectionState);
                }
                if (scene.SelectedIndex == 2)
                {
                }
                if (scene.SelectedIndex == 3)
                {
                    manager.ChangeState(GameRef.GamePlayState);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GameRef.SpriteBatch.Begin();

            scene.Draw(GameRef.SpriteBatch, background);

            Vector2 scale = new Vector2(
                Settings.Resolution.X / 1280,
                Settings.Resolution.Y / 720);

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

        public void Selection()
        {
        }

        public void SetShadowMonsters(ShadowMonster player, ShadowMonster enemy)
        {
            this.player = player;
            this.enemy = enemy;

            player.StartCombat();
            enemy.StartCombat();
        }
    }
}
