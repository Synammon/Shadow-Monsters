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
    public class LeftRightSelector : Control
    {
        #region Event Region

        public event EventHandler SelectionChanged;

        #endregion

        #region Field Region

        private List<string> _items = new List<string>();

        private Texture2D _leftTexture;
        private Texture2D _rightTexture;
        private Texture2D _stopTexture;

        private Color _selectedColor = Color.Red;
        private int _maxItemWidth;
        private int _selectedItem;
        private Rectangle _leftSide = new Rectangle();
        private Rectangle _rightSide = new Rectangle();
        private int _yOffset;

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

        public int MaxItemWidth
        {
            get { return _maxItemWidth; }
            set { _maxItemWidth = value; }
        }

        #endregion

        #region Constructor Region

        public LeftRightSelector(Texture2D leftArrow, Texture2D rightArrow, Texture2D stop)
        {
            _leftTexture = leftArrow;
            _rightTexture = rightArrow;
            _stopTexture = stop;
            TabStop = true;
            Color = Color.White;
        }

        #endregion

        #region Method Region

        public void SetItems(string[] items, int maxWidth)
        {
            this._items.Clear();

            foreach (string s in items)
                this._items.Add(s);

            _maxItemWidth = maxWidth;
        }

        protected void OnSelectionChanged()
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, null);
            }
        }

        #endregion

        #region Abstract Method Region

        public override void Update(GameTime gameTime)
        {
            HandleMouseInput();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawTo = _position;
            Vector2 scale = new Vector2(
                Settings.Resolution.X / 1280,
                Settings.Resolution.Y / 720);

            _spriteFont = FontManager.GetFont("testfont");

            _yOffset = (int)((_leftTexture.Height - _spriteFont.MeasureString("W").Y) / 2 * scale.Y);
            _leftSide = new Rectangle(
                (int)_position.X, 
                (int)_position.Y, 
                _leftTexture.Width, 
                _leftTexture.Height).Scale(scale);

            //if (selectedItem != 0)
            spriteBatch.Draw(_leftTexture, _leftSide, Color.White);
            //else
            //    spriteBatch.Draw(stopTexture, drawTo, Color.White);

            drawTo.X += _leftTexture.Width + 5f;

            float itemWidth = _spriteFont.MeasureString(_items[_selectedItem]).X;
            float offset = (_maxItemWidth - itemWidth) / 2 * scale.X;

            drawTo.X += offset;
            drawTo.Y += _yOffset;

            if (_hasFocus)
                spriteBatch.DrawString(_spriteFont, _items[_selectedItem], drawTo.Scale(scale), _selectedColor);
            else
                spriteBatch.DrawString(_spriteFont, _items[_selectedItem], drawTo.Scale(scale), Color);

            drawTo.X += -1 * offset + _maxItemWidth + 5f;

            _rightSide = new Rectangle((int)drawTo.X, (int)drawTo.Y - _yOffset, _rightTexture.Width, _rightTexture.Height).Scale(scale);
            //if (selectedItem != items.Count - 1)
            spriteBatch.Draw(_rightTexture, _rightSide, Color.White);
            //else
            //    spriteBatch.Draw(stopTexture, drawTo, Color.White);
        }

        public override void HandleInput()
        {
            if (_items.Count == 0)
                return;

            if (Xin.CheckKeyReleased(Keys.Left))
            {
                _selectedItem--;
                if (_selectedItem < 0)
                    _selectedItem = this.Items.Count - 1;
                OnSelectionChanged();
            }

            if (Xin.CheckKeyReleased(Keys.Right))
            {
                _selectedItem++;
                if (_selectedItem >= _items.Count)
                    _selectedItem = 0;
                OnSelectionChanged();
            }
        }

        private void HandleMouseInput()
        {
            if (Xin.CheckMouseReleased(MouseButtons.Left))
            {
                Point mouse = Xin.MouseAsPoint;

                if (_leftSide.Contains(mouse))
                {
                    _selectedItem--;
                    if (_selectedItem < 0)
                        _selectedItem = this.Items.Count - 1;
                    OnSelectionChanged();
                }

                if (_rightSide.Contains(mouse))
                {
                    _selectedItem++;
                    if (_selectedItem >= _items.Count)
                        _selectedItem = 0;
                    OnSelectionChanged();
                }
            }
        }

        #endregion
    }
}
