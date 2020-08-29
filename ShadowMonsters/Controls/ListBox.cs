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
    public class ListBox : Control
    {
        #region Event Region

        public event EventHandler SelectionChanged;
        public event EventHandler Enter;
        public event EventHandler Leave;

        #endregion

        #region Field Region

        private readonly List<string> _items = new List<string>();

        private int _startItem;
        private readonly int _lineCount;

        private readonly Texture2D _image;
        private readonly Texture2D _cursor;

        private Color _selectedColor = Color.Red;
        private int _selectedItem;

        #endregion

        #region Property Region

        public Color SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; }
        }

        public int SelectedIndex
        {
            get { return _selectedItem; }
            set { _selectedItem = (int)MathHelper.Clamp(value, 0f, _items.Count); }
        }

        public string SelectedItem
        {
            get { return Items[_selectedItem]; }
        }

        public List<string> Items
        {
            get { return _items; }
        }

        public override bool HasFocus
        {
            get { return _hasFocus; }
            set
            {
                _hasFocus = value;

                if (_hasFocus)
                    OnEnter(null);
                else
                    OnLeave(null);
            }
        }

        #endregion

        #region Constructor Region

        public ListBox(Texture2D background, Texture2D cursor)
            : base()
        {
            _hasFocus = false;
            _tabStop = false;

            this._image = background;
            this.Size = new Vector2(_image.Width, _image.Height);
            this._cursor = cursor;

            _lineCount = _image.Height / SpriteFont.LineSpacing;
            _startItem = 0;
            Color = Color.Black;
        }

        #endregion

        #region Abstract Method Region

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_image, Position, Color.White);
            Point position = Xin.MouseAsPoint;
            Rectangle destination = new Rectangle(0, 0, 100, (int)SpriteFont.LineSpacing);
            _mouseOver = false;

            for (int i = 0; i < _lineCount; i++)
            {
                if (_startItem + i >= _items.Count)
                {
                    break;
                }

                destination.X = (int)Position.X;
                destination.Y = (int)(Position.Y + i * SpriteFont.LineSpacing);

                if (destination.Contains(position) && Xin.MouseState.LeftButton == ButtonState.Pressed)
                {
                    _mouseOver = true;
                    _selectedItem = _startItem + i;
                    OnSelectionChanged(null);
                }

                if (_startItem + i == _selectedItem)
                {
                    spriteBatch.DrawString(
                        SpriteFont,
                        _items[_startItem + i],
                        new Vector2(Position.X, Position.Y + i * SpriteFont.LineSpacing),
                        SelectedColor);
                }
                else
                {
                    spriteBatch.DrawString(
                        SpriteFont,
                        _items[_startItem + i],
                        new Vector2(Position.X, Position.Y + i * SpriteFont.LineSpacing),
                        Color);
                }
            }
        }

        public override void HandleInput()
        {
            if (!HasFocus)
            {
                return;
            }

            if (Xin.CheckKeyReleased(Keys.Down))
            {
                if (_selectedItem < _items.Count - 1)
                {
                    _selectedItem++;

                    if (_selectedItem >= _startItem + _lineCount)
                    {
                        _startItem = _selectedItem - _lineCount + 1;
                    }

                    OnSelectionChanged(null);
                }
            }
            else if (Xin.CheckKeyReleased(Keys.Up))
            {
                if (_selectedItem > 0)
                {
                    _selectedItem--;

                    if (_selectedItem < _startItem)
                    {
                        _startItem = _selectedItem;
                    }

                    OnSelectionChanged(null);
                }
            }
            if (Xin.CheckMouseReleased(MouseButtons.Left) && _mouseOver)
            {
                HasFocus = true;
                OnSelectionChanged(null);
            }
            if (Xin.CheckKeyReleased(Keys.Enter))
            {
                HasFocus = false;
                OnSelected(null);
            }

            if (Xin.CheckKeyReleased(Keys.Escape))
            {
                HasFocus = false;
                OnLeave(null);
            }
        }

        #endregion

        #region Method Region

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        protected virtual void OnEnter(EventArgs e)
        {
            Enter?.Invoke(this, e);
        }

        protected virtual void OnLeave(EventArgs e)
        {
            Leave?.Invoke(this, e);
        }

        #endregion
    }
}
