using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ShadowMonsters.Items;

namespace ShadowMonsters.GameStates
{
    public interface IItemSelectionState
    {
        int SelectedIndex { get; }
    }

    public class ItemSelectionState : BaseGameState, IItemSelectionState
    {
        private int selected;
        private bool mouseOver;

        public int SelectedIndex
        {
            get { return selected; }
        }

        public ItemSelectionState(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Xin.CheckKeyReleased(Keys.Down) ||
                Xin.CheckKeyReleased(Keys.S))
            {
                selected++;

                if (selected >= Game1.Player.Backpack.Items.Count)
                {
                    selected = 0;
                }
            }

            if (Xin.CheckKeyReleased(Keys.Up) ||
                Xin.CheckKeyReleased(Keys.W))
            {
                selected--;

                if (selected < 0)
                {
                    selected = Game1.Player.Backpack.Items.Count - 1;
                }
            }

            if ((Xin.CheckKeyReleased(Keys.Space) ||
                Xin.CheckKeyReleased(Keys.Enter) ||
                (Xin.CheckMouseReleased(MouseButtons.Left) &&
                mouseOver)) &&
                selected >= 0 &&
                Game1.Player.Backpack.Items.Count > 0 &&
                Game1.Player.Backpack.PeekItem(
                    Game1.Player.Backpack.Items[selected].Name).Usable)
            {
                GameRef.UseItemState.SetItem(
                    Game1.Player.Backpack.GetItem(
                        Game1.Player.Backpack.Items[selected].Name));
                manager.PushState((UseItemState)GameRef.UseItemState);
            }
            else
            {
                // Play sound effect
            }

            if (Xin.CheckMouseReleased(MouseButtons.Right) || Xin.CheckKeyReleased(Keys.Escape))
            {
                manager.PopState();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Color tint;
            int i = 0;

            Vector2 scale = new Vector2(Settings.Resolution.X / 1280, Settings.Resolution.Y / 720);

            base.Draw(gameTime);

            GameRef.SpriteBatch.Begin();

            GameRef.SpriteBatch.DrawString(
                FontManager.GetFont("testfont"),
                "Item",
                new Vector2(120, 5).Scale(scale),
                Color.Red);

            GameRef.SpriteBatch.DrawString(
                FontManager.GetFont("testfont"),
                "Quantity",
                new Vector2(800, 5).Scale(scale),
                Color.Red);

            GameRef.SpriteBatch.DrawString(
                FontManager.GetFont("testfont"),
                "Price",
                new Vector2(1100, 5).Scale(scale),
                Color.Red);

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
                    Rectangle r = new Rectangle(0, 74 * i + 24, 1280, 64).Scale(scale);

                    if (r.Contains(Xin.MouseAsPoint))
                    {
                        selected = i;
                        mouseOver = true;
                    }

                    GameRef.SpriteBatch.DrawString(
                        FontManager.GetFont("testfont"),
                        v.Name,
                        new Vector2(120, 74 * i + 45).Scale(scale),
                        tint);

                    GameRef.SpriteBatch.DrawString(
                        FontManager.GetFont("testfont"),
                        v.Count.ToString(),
                        new Vector2(800, 74 * i + 45).Scale(scale),
                        tint);

                    GameRef.SpriteBatch.DrawString(
                        FontManager.GetFont("testfont"),
                        item.Price.ToString(),
                        new Vector2(1100, 74 * i + 45).Scale(scale),
                        tint);

                    i++;
                }
            }

            GameRef.SpriteBatch.End();
        }
    }
}
