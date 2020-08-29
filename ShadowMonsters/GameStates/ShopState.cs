using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ShadowMonsters.Characters;
using ShadowMonsters.ConversationComponents;
using ShadowMonsters.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.GameStates
{
    public enum ShopStateType { Buy, Sell, Talk }

    public interface IShopState
    {
        ShopStateType State { get; set; }
        void SetMerchant(Merchant merchant);
    }

    public class ShopState : BaseGameState, IShopState
    {
        private GameScene scene;
        private Merchant merchant;
        private int selected;
        private bool mouseOver;
        private bool isFirst;

        public ShopStateType State { get; set; }

        public ShopState(Game game)
            : base(game)
        {
            State = ShopStateType.Talk;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            scene = new GameScene(GameRef, "", new List<SceneOption>());
            SceneOption option = new SceneOption("Buy", "Buy", new SceneAction());
            scene.Options.Add(option);

            option = new SceneOption("Sell", "Sell", new SceneAction());
            scene.Options.Add(option);

            option = new SceneOption("Leave", "Leave", new SceneAction());
            scene.Options.Add(option);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            scene.Update();

            switch (State)
            {
                case ShopStateType.Buy:
                    if (isFirst)
                    {
                        isFirst = false;
                        break;
                    }

                    if (Xin.CheckKeyReleased(Keys.Down) ||
                        Xin.CheckKeyReleased(Keys.S))
                    {
                        selected++;
                        if (selected >= merchant.Backpack.Items.Count)
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
                            selected = merchant.Backpack.Items.Count - 1;
                        }
                    }


                    if (Xin.CheckKeyReleased(Keys.Space) ||
                        Xin.CheckKeyReleased(Keys.Enter) ||
                        (Xin.CheckMouseReleased(MouseButtons.Left) && mouseOver))
                    {
                        if (selected >= 0 &&
                            Game1.Player.Gold >=
                            merchant.Backpack.PeekItem(
                                merchant.Backpack.Items[selected].Name).Price)
                        {
                            Game1.Player.Backpack.AddItem(
                                merchant.Backpack.GetItem(
                                    merchant.Backpack.Items[selected].Name),
                                1);
                            Game1.Player.Gold -=
                                merchant.Backpack.PeekItem(
                                    merchant.Backpack.Items[selected].Name).Price;
                        }
                    }
                    break;
                case ShopStateType.Sell:
                    if (isFirst)
                    {
                        isFirst = false;
                        break;
                    }
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
                        (Xin.CheckMouseReleased(MouseButtons.Left) && mouseOver)))
                    {
                        if (selected >= 0)
                        {
                            IItem item = Game1.Player.Backpack.GetItem(
                                Game1.Player.Backpack.Items[selected].Name);
                            Game1.Player.Gold += item.Price * 3 / 4;
                        }
                    }
                    break;
                case ShopStateType.Talk:
                    if (Xin.CheckKeyReleased(Keys.Space) ||
                        Xin.CheckKeyReleased(Keys.Enter))
                    {
                        Xin.FlushInput();

                        if (scene.SelectedIndex == 0)
                        {
                            isFirst = true;
                            State = ShopStateType.Buy;
                            //selected = -1;
                            Xin.FlushInput();
                            return;
                        }

                        if (scene.SelectedIndex == 1)
                        {
                            isFirst = true;
                            State = ShopStateType.Sell;
                            //selected = -1;
                            Xin.FlushInput();
                            return;
                        }

                        if (scene.SelectedIndex == 2 && State == ShopStateType.Talk)
                        {
                            manager.PopState();
                        }
                    }
                    break;
            }

            if (Xin.CheckMouseReleased(MouseButtons.Right) || Xin.CheckKeyReleased(Keys.Escape))
            {
                switch (State)
                {
                    case ShopStateType.Buy:
                    case ShopStateType.Sell:
                        State = ShopStateType.Talk;
                        break;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Vector2 scale = new Vector2(Settings.Resolution.X  / 1280, Settings.Resolution.Y / 720);

            GameRef.SpriteBatch.Begin();

            int i = 0;
            Color tint;

            switch (State)
            {
                case ShopStateType.Buy:
                    mouseOver = false;
                    if (isFirst)
                    {
                        break;
                    }
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

                    foreach (var v in merchant.Backpack.Items)
                    {
                        tint = Color.White;
                        if (i == selected)
                        {
                            tint = Color.Red;
                        }

                        IItem item = merchant.Backpack.PeekItem(v.Name);

                        if (item != null)
                        {
                            Rectangle r = new Rectangle(0, 74 * i + 24, 1200, 64).Scale(scale);

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
                    break;
                case ShopStateType.Sell:
                    mouseOver = false;
                    if (isFirst)
                    {
                        break;
                    }
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
                    break;
                case ShopStateType.Talk:
                    scene.Draw(GameRef.SpriteBatch, null);
                    break;
            }

            GameRef.SpriteBatch.End();
        }

        public void SetMerchant(Merchant merchant)
        {
            this.merchant = merchant;
        }
    }
}
