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
    public enum CurrentTurn
    {
        Players, Enemies
    }

    public interface IDamageState
    {
        void SetShadowMonsters(ShadowMonster player, ShadowMonster enemy);
        void SetMoves(IMove playerMove, IMove enemyMove);
        void Start();
    }

    public class DamageState : BaseGameState, IDamageState
    {
        #region Field Region

        private CurrentTurn turn;
        private Texture2D combatBackground;
        private Rectangle playerRect;
        private Rectangle enemyRect;
        private TimeSpan cTimer;
        private TimeSpan dTimer;
        private ShadowMonster player;
        private ShadowMonster enemy;
        private IMove playerMove;
        private IMove enemyMove;
        private bool first;
        private Rectangle playerBorderRect;
        private Rectangle enemyBorderRect;
        private Rectangle playerMiniRect;
        private Rectangle enemyMiniRect;
        private Rectangle playerHealthRect;
        private Rectangle enemyHealthRect;
        private Rectangle healthSourceRect;
        private float playerHealth;
        private float enemyHealth;
        private Texture2D ShadowMonsterBorder;
        private Texture2D ShadowMonsterHealth;
        private Vector2 playerName;
        private Vector2 enemyName;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public DamageState(Game game)
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

        public override void Initialize()
        {
            base.Initialize();
        }

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
            if ((cTimer > TimeSpan.FromSeconds(3) ||
                !enemy.Alive ||
                !player.Alive) &&
                dTimer > TimeSpan.FromSeconds(2))
            {
                if (!enemy.Alive || !player.Alive)
                {
                    manager.PopState();
                    manager.PushState((BattleOverState)GameRef.BattleOverState);
                    GameRef.BattleOverState.SetShadowMonsters(player, enemy);
                }
                else
                {
                    manager.PopState();
                    manager.PushState((ActionSelectionState)GameRef.ActionSelectionState);
                }
            }
            else if (cTimer > TimeSpan.FromSeconds(2) && first && enemy.Alive && player.Alive)
            {
                first = false;
                dTimer = TimeSpan.Zero;

                if (turn == CurrentTurn.Players)
                {
                    turn = CurrentTurn.Enemies;
                    enemy.ResoleveMove(enemyMove, player);
                }
                else
                {
                    turn = CurrentTurn.Players;
                    player.ResoleveMove(playerMove, enemy);
                }
            }
            else if (cTimer == TimeSpan.Zero)
            {
                dTimer = TimeSpan.Zero;

                if (turn == CurrentTurn.Players)
                {
                    player.ResoleveMove(playerMove, enemy);
                }
                else
                {
                    enemy.ResoleveMove(enemyMove, player);
                }
            }

            cTimer += gameTime.ElapsedGameTime;
            dTimer += gameTime.ElapsedGameTime;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GameRef.SpriteBatch.Begin();
            GameRef.SpriteBatch.Draw(combatBackground, Vector2.Zero, Color.White);
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

            Vector2 location = new Vector2(50f, Settings.Resolution.Y * .75f);

            if (turn == CurrentTurn.Players)
            {
                GameRef.SpriteBatch.DrawString(
                    FontManager.GetFont("testfont"),
                    player.DisplayName + " uses " + playerMove.Name + ".",
                    location,
                    Color.Black);

                if (playerMove.Target == Target.Enemy && playerMove.MoveType == MoveType.Attack)
                {
                    location.Y += FontManager.GetFont("testfont").LineSpacing;

                    if (ShadowMonster.GetMoveModifier(playerMove.MoveElement, enemy.Element) < 1f)
                    {
                        GameRef.SpriteBatch.DrawString(
                            FontManager.GetFont("testfont"),
                            "It is not very effective.",
                            location,
                            Color.Black);
                    }
                    else if (ShadowMonster.GetMoveModifier(playerMove.MoveElement, enemy.Element) > 1f)
                    {
                        GameRef.SpriteBatch.DrawString(
                            FontManager.GetFont("testfont"),
                            "It is super effective.",
                            location,
                            Color.Black);
                    }
                }
            }
            else
            {
                GameRef.SpriteBatch.DrawString(
                    FontManager.GetFont("testfont"),
                    "Enemy " + enemy.DisplayName + " uses " + enemyMove.Name + ".",
                    location,
                    Color.Black);

                if (enemyMove.Target == Target.Enemy && playerMove.MoveType == MoveType.Attack)
                {
                    location.Y += FontManager.GetFont("testfont").LineSpacing;

                    if (ShadowMonster.GetMoveModifier(enemyMove.MoveElement, player.Element) < 1f)
                    {
                        GameRef.SpriteBatch.DrawString(
                            FontManager.GetFont("testfont"),
                            "It is not very effective.",
                            location,
                            Color.Black);
                    }
                    else if (ShadowMonster.GetMoveModifier(enemyMove.MoveElement, player.Element) > 1f)
                    {
                        GameRef.SpriteBatch.DrawString(
                            FontManager.GetFont("testfont"),
                            "It is super effective.",
                            location,
                            Color.Black);
                    }
                }
            }

            GameRef.SpriteBatch.End();
        }

        public void SetShadowMonsters(ShadowMonster player, ShadowMonster enemy)
        {
            this.player = player;
            this.enemy = enemy;

            if (player.GetSpeed() >= enemy.GetSpeed())
            {
                turn = CurrentTurn.Players;
            }
            else
            {
                turn = CurrentTurn.Enemies;
            }
        }

        public void SetMoves(IMove playerMove, IMove enemyMove)
        {
            this.playerMove = playerMove;
            this.enemyMove = enemyMove;
        }

        public void Start()
        {
            cTimer = TimeSpan.Zero;
            dTimer = TimeSpan.Zero;
            first = true;
        }

        #endregion
    }
}
