using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShadowMonsters.Items;
using ShadowMonsters.ShadowMonsters;

namespace ShadowMonsters.GameStates
{
    public interface IUseItemState
    {
        void SetItem(IItem item);
    }

    public class UseItemState : BaseGameState, IUseItemState
    {
        private IItem item;
        private bool mouseOver;
        private Texture2D shadowMonsterBorder;
        private Texture2D shadowMonsterHealth;
        private int selected;
        public UseItemState(Game game)
            : base(game)
        {
        }
        protected override void LoadContent()
        {
            base.LoadContent();

            shadowMonsterBorder = new Texture2D(GraphicsDevice, 300, 75);
            shadowMonsterHealth = new Texture2D(GraphicsDevice, 300, 25);
            Color[] buffer = new Color[300 * 75];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.Green;
            }

            shadowMonsterBorder.SetData(buffer);

            buffer = new Color[300 * 25];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.Red;
            }

            shadowMonsterHealth.SetData(buffer);
        }

        public override void Update(GameTime gameTime)
        {
            if (Xin.CheckKeyReleased(Keys.Up) || Xin.CheckKeyReleased(Keys.W))
            {
                selected--;

                if (selected < 0)
                {
                    selected = Game1.Player.BattleShadowMonsters.Length - 1;
                }
            }

            if (Xin.CheckKeyReleased(Keys.Down) || Xin.CheckKeyReleased(Keys.S))
            {
                selected++;

                if (selected >= Game1.Player.BattleShadowMonsters.Length)
                {
                    selected = 0;
                }
            }

            if (Xin.CheckKeyReleased(Keys.Space) ||
                Xin.CheckKeyReleased(Keys.Enter) ||
                (mouseOver && Xin.CheckMouseReleased(MouseButtons.Left)))
            {
                item = Game1.Player.Backpack.PeekItem(Game1.Player.Backpack.Items[selected].Name);
                if (Game1.Player.BattleShadowMonsters[selected] != null)
                {
                    if (item.Target == ItemTarget.Self)
                    {
                        item.Apply(Game1.Player.BattleShadowMonsters[selected]);
                        manager.PopState();
                        manager.PopState();
                    }
                    else
                    {
                        if (ActionSelectionState.IsTrainerBattle)
                        {
                            manager.PushState(GameRef.MessageState);
                            GameRef.MessageState.Message = "You can't use that on bound monsters.";
                        }
                        else
                        {
                            bool result = Game1.Player.Backpack.GetItem(
                                Game1.Player.Backpack.Items[selected].Name).Apply(
                                    GameRef.ActionSelectionState.EnemyShadowMonster);
                            if (result)
                            {
                                manager.PopState();
                                manager.PushState(GameRef.BindSuccessState);
                                GameRef.BindSuccessState.Monster = GameRef.ActionSelectionState.EnemyShadowMonster;
                                GameRef.BattleState.Visible = true;

                                Game1.Player.AddShadowMonster(GameRef.ActionSelectionState.EnemyShadowMonster);

                                for (int i = 0; i < Player.MaxShadowMonsters; i++)
                                {
                                    if (Game1.Player.BattleShadowMonsters[i] == null)
                                    {
                                        Game1.Player.BattleShadowMonsters[i] = GameRef.ActionSelectionState.EnemyShadowMonster;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                manager.PopState();
                                manager.PushState(GameRef.BindFailureState);
                                GameRef.BindFailureState.Monster = GameRef.ActionSelectionState.EnemyShadowMonster;
                                GameRef.BattleState.Visible = true;
                            }
                        }
                    }
                }
            }

            if (Xin.CheckMouseReleased(MouseButtons.Right) || Xin.CheckKeyReleased(Keys.Escape))
            {
                manager.PopState();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();

            base.Draw(gameTime);

            Color tint;
            Rectangle destination = new Rectangle(50, 20, 100, 100);
            Rectangle playerBorderRect = new Rectangle(250, 20, 400, 100);
            Rectangle playerHealthRect = new Rectangle(
                playerBorderRect.X + 16,
                playerBorderRect.Y + 73, 350, 19);
            Vector2 playerName = new Vector2(325, 25);
            Rectangle healthSourceRect = new Rectangle(10, 50, 290, 20);
            Point cursor = Xin.MouseAsPoint;

            GameRef.SpriteBatch.DrawString(
                FontManager.GetFont("testfont"),
                "Item",
                new Vector2(120, 5).Scale(Settings.Scale),
                Color.Red);

            GameRef.SpriteBatch.DrawString(
                FontManager.GetFont("testfont"),
                "Quantity",
                new Vector2(800, 5).Scale(Settings.Scale),
                Color.Red);

            int i = 0;

            foreach (var v in Game1.Player.Backpack.Items)
            {
                tint = Color.White;

                if (i == selected)
                {
                    tint = Color.Red;
                }

                IItem item = Game1.Player.Backpack.PeekItem(v.Name);

                if (item != null)
                {
                    Rectangle r = new Rectangle(0, 74 * i + 24, 1280, 64).Scale(Settings.Scale);

                    if (r.Contains(Xin.MouseAsPoint))
                    {
                        selected = i;
                        mouseOver = true;
                    }

                    GameRef.SpriteBatch.DrawString(
                        FontManager.GetFont("testfont"),
                        v.Name,
                        new Vector2(120, 74 * i + 45).Scale(Settings.Scale),
                        tint);

                    GameRef.SpriteBatch.DrawString(
                        FontManager.GetFont("testfont"),
                        v.Count.ToString(),
                        new Vector2(800, 74 * i + 45).Scale(Settings.Scale),
                        tint);

                    i++;
                }
            }
            GameRef.SpriteBatch.End();
        }

        public void SetItem(IItem item)
        {
            this.item = item;
        }
    }
}
