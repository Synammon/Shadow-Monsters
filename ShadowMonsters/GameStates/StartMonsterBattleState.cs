using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShadowMonsters.ShadowMonsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.GameStates
{
    public class StartMonsterBattleState : BaseGameState
    {
        private Player player;
        private ShadowMonster monster;
        private TimeSpan timer;
        private Rectangle playerDestination;
        private Rectangle enemyDestination;
        private Texture2D background;
        private Vector3 offset;

        public StartMonsterBattleState(Game game) : base(game)
        {
            ResetRectangles();
            timer = TimeSpan.Zero;
        }

        private void ResetRectangles()
        {
            playerDestination = new Rectangle(-600, 40, 600, 400);
            enemyDestination = new Rectangle(1280, 10, 600, 400);
        }

        protected override void LoadContent()
        {
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
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime;

            if (timer.TotalMilliseconds >= 400)
            {
                timer = TimeSpan.FromMilliseconds(400);
                if (Xin.CheckAnyKeyPressed() || Xin.CheckMouseReleased(MouseButtons.Left))
                {
                    manager.PopState();
                }
            }
            else
            {
                offset = new Vector3((float)(600 * Settings.Scale.X * (timer.TotalMilliseconds / 400)), 0, 0);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GameRef.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            GameRef.SpriteBatch.Draw(
                background,
                new Rectangle(0, 0, 1280, 720).Scale(Settings.Scale),
                Color.White);

            GameRef.SpriteBatch.End();

            GameRef.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                Matrix.CreateTranslation(offset));

            GameRef.SpriteBatch.End();

            GameRef.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                Matrix.CreateTranslation(-offset));

            GameRef.SpriteBatch.Draw(
                monster.Texture,
                enemyDestination.Scale(Settings.Scale),
                Color.White);

            GameRef.SpriteBatch.End();

            if (timer.TotalMilliseconds >= 400)
            {
                GameRef.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
                GameRef.SpriteBatch.DrawString(
                    FontManager.GetFont("testfont"),
                    "An angry wild " + monster.Name + " has appeared!\n\rContinue",
                    new Vector2(10, Settings.Resolution.Y * 0.75f),
                    Color.Black);
                GameRef.SpriteBatch.End();
            }
        }

        public void SetCombatants(Player player, ShadowMonster monster)
        {
            this.player = player;
            this.monster = monster;

            GameRef.BattleState.SetShadowMonsters(Game1.Player.Selected, monster);
            GameRef.ActionSelectionState.SetShadowMonsters(Game1.Player.Selected, monster);
            manager.PushState((BattleState)GameRef.BattleState);
            manager.PushState((ActionSelectionState)GameRef.ActionSelectionState);
            ResetRectangles();
        }

        public override void Show()
        {
            base.Show();

            timer = TimeSpan.Zero;
            ResetRectangles();
        }
    }
}
