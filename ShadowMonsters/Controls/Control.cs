using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.Controls
{
    public abstract class Control
    {
        #region Field Region

        protected string _name;
        protected string _text;
        protected Vector2 _size;
        protected Vector2 _position;
        protected object _value;
        protected bool _hasFocus;
        protected bool _enabled;
        protected bool _visible;
        protected bool _tabStop;
        protected SpriteFont _spriteFont;
        protected Color _color;
        protected string _type;
        protected bool _mouseOver;

        #endregion

        #region Event Region

        public event EventHandler Selected;

        #endregion

        #region Property Region

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _position.Y = (int)_position.Y;
            }
        }

        public object Value
        {
            get { return _value; }
            set { this._value = value; }
        }

        public virtual bool HasFocus
        {
            get { return _hasFocus; }
            set { _hasFocus = value; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public bool TabStop
        {
            get { return _tabStop; }
            set { _tabStop = value; }
        }

        public SpriteFont SpriteFont
        {
            get { return _spriteFont; }
            set { _spriteFont = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public virtual int Width
        {
            get { return (int)_size.X; }
        }

        public virtual int Height
        {
            get { return (int)_size.Y; }
        }
        #endregion

        #region Constructor Region

        public Control()
        {
            Color = Color.White;
            Enabled = true;
            Visible = true;
            SpriteFont = ControlManager.SpriteFont;
            _mouseOver = false;
        }

        #endregion

        #region Abstract Methods

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void HandleInput();

        #endregion

        #region Virtual Methods

        protected virtual void OnSelected(EventArgs e)
        {
            Selected?.Invoke(this, e);
        }

        #endregion
    }
}
