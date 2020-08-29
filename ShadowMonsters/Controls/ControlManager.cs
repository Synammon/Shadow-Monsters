using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.Controls
{
    public class ControlManager : List<Control>
    {
        #region Fields and Properties

        int _selectedControl = 0;
        bool _acceptInput = true;

        static SpriteFont _spriteFont;

        public static SpriteFont SpriteFont
        {
            get { return _spriteFont; }
        }

        public bool AcceptInput
        {
            get { return _acceptInput; }
            set { _acceptInput = value; }
        }

        #endregion

        #region Event Region

        public event EventHandler FocusChanged;

        #endregion

        #region Constructors

        public ControlManager(SpriteFont spriteFont)
            : base()
        {
            ControlManager._spriteFont = spriteFont;
        }

        public ControlManager(SpriteFont spriteFont, int capacity)
            : base(capacity)
        {
            ControlManager._spriteFont = spriteFont;
        }

        public ControlManager(SpriteFont spriteFont, IEnumerable<Control> collection) :
            base(collection)
        {
            ControlManager._spriteFont = spriteFont;
        }

        #endregion

        #region Methods

        public void Update(GameTime gameTime)
        {
            if (Count == 0)
            {
                return;
            }

            foreach (Control c in this)
            {
                if (c.Enabled)
                {
                    c.Update(gameTime);
                }
            }

            foreach (Control c in this)
            {
                if (c.HasFocus)
                {
                    c.HandleInput();
                    break;
                }
            }

            if (!AcceptInput)
            {
                return;
            }

            if (Xin.CheckKeyReleased(Keys.Up))
            {
                PreviousControl();
            }

            if (Xin.CheckKeyReleased(Keys.Tab))
            {
                NextControl();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Control c in this)
            {
                if (c.Visible)
                {
                    c.Draw(spriteBatch);
                }
            }
        }

        public void NextControl()
        {
            if (Count == 0)
            {
                return;
            }

            int currentControl = _selectedControl;

            this[_selectedControl].HasFocus = false;

            do
            {
                _selectedControl++;

                if (_selectedControl == Count)
                {
                    _selectedControl = 0;
                }

                if (this[_selectedControl].TabStop && this[_selectedControl].Enabled)
                {
                    FocusChanged?.Invoke(this[_selectedControl], null);

                    break;
                }

            } while (currentControl != _selectedControl);

            this[_selectedControl].HasFocus = true;
        }

        public void PreviousControl()
        {
            if (Count == 0)
            {
                return;
            }

            int currentControl = _selectedControl;

            this[_selectedControl].HasFocus = false;

            do
            {
                _selectedControl--;

                if (_selectedControl < 0)
                {
                    _selectedControl = Count - 1;
                }

                if (this[_selectedControl].TabStop && this[_selectedControl].Enabled)
                {
                    FocusChanged?.Invoke(this[_selectedControl], null);

                    break;
                }
            } while (currentControl != _selectedControl);

            this[_selectedControl].HasFocus = true;
        }

        #endregion
    }
}
