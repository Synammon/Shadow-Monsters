using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShadowMonsters.Controls
{
    public class TextBox : Control
    {
        #region Event Region

        public event EventHandler Changed;

        #endregion

        #region Field Region

        private readonly Texture2D _background;

        #endregion

        #region Property Region
        
        #endregion

        #region Constructor Region

        public TextBox(Texture2D background)
        {
            _background = background;
            _text = "";
        }

        #endregion

        #region Method Region

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = new Rectangle(
                (int)(Position.X),
                (int)(Position.Y),
                _background.Width,
                _background.Height);

            spriteBatch.Draw(_background, destination, Color.White);
            spriteBatch.DrawString(FontManager.GetFont("testfont"), _text, (Position + Vector2.One * 3), _color);
        }

        public override void HandleInput()
        {
            if (Xin.CheckKeyPressed(Keys.A))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "a";
                }
                else
                {
                    _text += "A";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.B))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "b";
                }
                else
                {
                    _text += "B";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.C))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "c";
                }
                else
                {
                    _text += "C";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.D))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "d";
                }
                else
                {
                    _text += "D";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.E))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "e";
                }
                else
                {
                    _text += "E";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.F))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "f";
                }
                else
                {
                    _text += "F";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.G))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "g";
                }
                else
                {
                    _text += "G";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.H))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "h";
                }
                else
                {
                    _text += "H";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.I))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "i";
                }
                else
                {
                    _text += "I";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.J))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "j";
                }
                else
                {
                    _text += "J";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.K))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "k";
                }
                else
                {
                    _text += "K";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.L))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "l";
                }
                else
                {
                    _text += "L";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.M))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "m";
                }
                else
                {
                    _text += "M";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.N))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "n";
                }
                else
                {
                    _text += "N";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.O))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "o";
                }
                else
                {
                    _text += "O";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.R))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "p";
                }
                else
                {
                    _text += "P";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.Q))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "q";
                }
                else
                {
                    _text += "Q";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.R))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "r";
                }
                else
                {
                    _text += "R";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.S))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "s";
                }
                else
                {
                    _text += "S";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.T))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "t";
                }
                else
                {
                    _text += "T";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.U))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "u";
                }
                else
                {
                    _text += "U";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.V))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "v";
                }
                else
                {
                    _text += "V";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.W))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "w";
                }
                else
                {
                    _text += "W";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.X))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "x";
                }
                else
                {
                    _text += "X";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.Y))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "y";
                }
                else
                {
                    _text += "Y";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.Z))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "z";
                }
                else
                {
                    _text += "Z";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.D1))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "1";
                }
                else
                {
                    _text += "!";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.D2))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "2";
                }
                else
                {
                    _text += "@";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.D3))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "3";
                }
                else
                {
                    _text += "#";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.D4))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "4";
                }
                else
                {
                    _text += "$";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.D5))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "5";
                }
                else
                {
                    _text += "%";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.D6))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "6";
                }
                else
                {
                    _text += "^";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.D7))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "7";
                }
                else
                {
                    _text += "&";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.D8))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "8";
                }
                else
                {
                    _text += "*";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.D9))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "9";
                }
                else
                {
                    _text += "(";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.D0))
            {
                if (!(Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift)))
                {
                    _text += "0";
                }
                else
                {
                    _text += ")";
                }
                OnChange();
            }
            if (Xin.CheckKeyPressed(Keys.Back) && _text.Length > 0)
            {
                _text = _text.Substring(0, _text.Length - 1);
                OnChange();
            }
        }

        private void OnChange()
        {
            Changed?.Invoke(this, null);
        }
        public override void Update(GameTime gameTime)
        {
            HandleInput();
        }

        #endregion
    }
}
