using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.Controls
{
    public class PictureBox : Control
    {
        #region Field Region

        private Texture2D _image;
        private Rectangle _sourceRect;
        private Rectangle _destRect;

        #endregion

        #region Property Region

        public Texture2D Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public Rectangle SourceRectangle
        {
            get { return _sourceRect; }
            set { _sourceRect = value; }
        }

        public Rectangle DestinationRectangle
        {
            get { return _destRect; }
            set { _destRect = value; }
        }

        #endregion

        #region Constructors

        public PictureBox(Texture2D image, Rectangle destination)
        {
            Image = image;
            DestinationRectangle = destination;

            if (image != null)
                SourceRectangle = new Rectangle(0, 0, image.Width, image.Height);
            else
                SourceRectangle = new Rectangle(0, 0, 0, 0);
            Color = Color.White;
        }

        public PictureBox(Texture2D image, Rectangle destination, Rectangle source)
        {
            Image = image;
            DestinationRectangle = destination;
            SourceRectangle = source;
            Color = Color.White;
        }

        #endregion

        #region Abstract Method Region

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_image != null && _destRect != null && _sourceRect != null)
            {
                spriteBatch.Draw(_image, _destRect, _sourceRect, _color);
            }
        }

        public override void HandleInput()
        {
        }

        #endregion

        #region Picture Box Methods

        public void SetPosition(Vector2 newPosition)
        {
            _destRect = new Rectangle(
                (int)newPosition.X,
                (int)newPosition.Y,
                _sourceRect.Width,
                _sourceRect.Height);
        }

        #endregion
    }
}
