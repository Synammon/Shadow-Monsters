using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ShadowMonsters.Components;
using ShadowMonsters.ShadowMonsters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.GameStates
{
    public class MainMenuState : BaseGameState
    {
        #region Field Region

        MenuComponent menu;
        private int frameCount;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public MainMenuState(Game game) : base(game)
        {
        }

        #endregion

        #region Method Region
        
        protected override void LoadContent()
        {
            base.LoadContent();
            Texture2D texture = new Texture2D(GraphicsDevice, 400, 50);

            Color[] buffer = new Color[400 * 50];

            for (int i = 0; i < 400 * 50; i++)
                buffer[i] = Color.Black;

            texture.SetData(buffer);

            menu = new MenuComponent(GameRef, texture);
            
            Components.Add(menu);
        }

        public override void Update(GameTime gameTime)
        {
            frameCount++;

            if (menu.SelectedIndex == -1)
            {
                menu.SetMenuItems(new[] { "New Game", "Continue", "Options", "Exit" });
            }

            if (Xin.CheckKeyReleased(Keys.Escape))
            {
                GameRef.Exit();
            }

            if (Xin.CheckKeyReleased(Keys.Enter) ||
                Xin.CheckKeyReleased(Keys.Space)
                || (menu.MouseOver && Xin.CheckMouseReleased(MouseButtons.Left)) && frameCount > 5)
            {
                Muse.PlaySoundEffect("menu_click");

                switch (menu.SelectedIndex)
                {
                    case 0:
                        manager.PopState();
                        manager.PushState(GameRef.NewGameState);
                        break;
                    case 1:
                        string path = Environment.GetFolderPath(
                            Environment.SpecialFolder.ApplicationData);
                        path += "\\ShadowMonsters\\ShadowMonsters.sav";

                        if (!File.Exists(path))
                        {
                            return;
                        }

                        Muse.StopSong();
                        MoveManager.FillMoves();
                        ShadowMonsterManager.FromFile(@".\Content\ShadowMonsters.txt", content);
                        manager.PopState();
                        manager.PushState(GameRef.GamePlayState);
                        GameRef.GamePlayState.LoadGame();
                        GameRef.GamePlayState.ResetEngine();
                        break;
                    case 2:
                        manager.PushState(GameRef.OptionState);
                        break;
                    case 3:
                        GameRef.Exit();
                        break;
                }
                frameCount = 0;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();
            base.Draw(gameTime);
            GameRef.SpriteBatch.End();
        }

        #endregion
    }
}
