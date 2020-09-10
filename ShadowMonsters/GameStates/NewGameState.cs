using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShadowMonsters.Components;
using ShadowMonsters.Controls;

namespace ShadowMonsters.GameStates
{
    public class NewGameState : BaseGameState
    {
        private Texture2D _background;
        private Texture2D _foreground;
        private Rectangle _destination = new Rectangle(0, 0, 1280, 720);
        private Rectangle _portraitDestination = new Rectangle(599, 57, 633, 617);
        private LeftRightSelector _portraitSelector;
        private LeftRightSelector _genderSelector;
        private TextBox _nameTextBox;
        private Dictionary<string, Texture2D> _femalePortraits;
        private Dictionary<string, Texture2D> _malePortraits;
        private Button _create;
        private Button _back;

        public NewGameState(Game game) : base(game)
        {
            _femalePortraits = new Dictionary<string, Texture2D>();
            _malePortraits = new Dictionary<string, Texture2D>();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _background = content.Load<Texture2D>(@"chargen-back");
            _foreground = content.Load<Texture2D>(@"chargen-fore");

            _genderSelector = new LeftRightSelector(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"),
                null)
            {
                Position = new Vector2(207 - 70, 298)
            };
            _genderSelector.SelectionChanged += _genderSelector_SelectionChanged;
            _genderSelector.SetItems(new[] { "Female", "Male" }, 270);

            _portraitSelector = new LeftRightSelector(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"),
                null)
            {
                Position = new Vector2(207 - 70, 458)
            };
            _portraitSelector.SelectionChanged += _portraitSelector_SelectionChanged;

            _nameTextBox = new TextBox(
                content.Load<Texture2D>(@"GUI\textbox"))
            {
                Position = new Vector2(207, 138),
                HasFocus = true,
                Enabled = true,
                Color = Color.White
            };

            _femalePortraits.Add(
                "Healer",
                content.Load<Texture2D>(@"CharacterSprites\healer_f"));
            _femalePortraits.Add(
                "Mage",
                content.Load<Texture2D>(@"CharacterSprites\mage_f"));
            _femalePortraits.Add(
                "Ninja",
                content.Load<Texture2D>(@"CharacterSprites\ninja_f"));
            _femalePortraits.Add(
                "Ranger",
                content.Load<Texture2D>(@"CharacterSprites\ranger_f"));
            _malePortraits.Add(
                "Healer",
                content.Load<Texture2D>(@"CharacterSprites\healer_m"));
            _malePortraits.Add(
                "Mage",
                content.Load<Texture2D>(@"CharacterSprites\mage_m"));
            _malePortraits.Add(
                "Ninja" ,
                content.Load<Texture2D>(@"CharacterSprites\ninja_m"));
            _malePortraits.Add(
                "Ranger",
                content.Load<Texture2D>(@"CharacterSprites\ranger_m"));

            _portraitSelector.SetItems(_femalePortraits.Keys.ToArray(), 270);

            _create = new Button(
                content.Load<Texture2D>(@"GUI\g9202"))
            {
                Text = "Create",
                Position = new Vector2(180, 640)
            };

            _create.Click += _create_Click;

            _back = new Button(
                content.Load<Texture2D>(@"GUI\g9202"))
            {
                Text = "Back",
                Position = new Vector2(350, 640)
            };

            _back.Click += _back_Click;

        }

        private void _back_Click(object sender, EventArgs e)
        {
            manager.PopState();
        }

        private void _create_Click(object sender, EventArgs e)
        {
            Muse.StopSong();
            Game1.Player = new Player(
                GameRef, 
                _nameTextBox.Text, 
                _genderSelector.SelectedIndex == 0, 
                _portraitSelector.SelectedItem.Replace(" ", "-"));
            Muse.StopSong();
            GameRef.GamePlayState.SetUpNewGame();
            manager.PopState();
            manager.PushState(GameRef.GamePlayState);
        }

        private void _genderSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (_genderSelector.SelectedIndex == 0)
            {
                _portraitSelector.SetItems(_femalePortraits.Keys.ToArray(), 270);
            }
            else
            {
                _portraitSelector.SetItems(_malePortraits.Keys.ToArray(), 270);
            }
        }

        private void _portraitSelector_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            _portraitSelector.Update(gameTime);
            _genderSelector.Update(gameTime);
            _nameTextBox.Update(gameTime);
            _create.Update(gameTime);
            _back.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();

            GameRef.SpriteBatch.Draw(
                _background, 
                _destination.Scale(Settings.Scale), 
                Color.White);

            if (_genderSelector.SelectedIndex == 0)
            {
                GameRef.SpriteBatch.Draw(
                    _femalePortraits[_portraitSelector.SelectedItem],
                    _portraitDestination.Scale(Settings.Scale),
                    Color.White);
            }
            else
            {
                GameRef.SpriteBatch.Draw(
                    _malePortraits[_portraitSelector.SelectedItem],
                    _portraitDestination.Scale(Settings.Scale),
                    Color.White);
            }

            _genderSelector.Draw(GameRef.SpriteBatch);
            _portraitSelector.Draw(GameRef.SpriteBatch);
            _nameTextBox.Draw(GameRef.SpriteBatch);
            _create.Draw(GameRef.SpriteBatch);
            _back.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.Draw(
                _foreground, 
                _destination.Scale(Settings.Scale), 
                Color.White);

            base.Draw(gameTime);

            GameRef.SpriteBatch.End();
        }
    }
}
