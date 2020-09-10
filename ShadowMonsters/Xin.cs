using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ShadowMonsters
{
    public enum MouseButtons
    {
        Left,
        Right,
        Center
    }

    public class Xin : GameComponent
    {
        private static KeyboardState currentKeyboardState = Keyboard.GetState();
        private static KeyboardState previousKeyboardState = Keyboard.GetState();

        private static MouseState currentMouseState = Mouse.GetState();
        private static MouseState previousMouseState = Mouse.GetState();

        public static MouseState MouseState
        {
            get { return currentMouseState; }
        }

        public static KeyboardState KeyboardState
        {
            get { return currentKeyboardState; }
        }

        public static KeyboardState PreviousKeyboardState
        {
            get { return previousKeyboardState; }
        }

        public static MouseState PreviousMouseState
        {
            get { return previousMouseState; }
        }

        public static Point MouseAsPoint { get; internal set; }

        public Xin(Game game)
            : base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            Xin.previousKeyboardState = Xin.currentKeyboardState;
            Xin.currentKeyboardState = Keyboard.GetState();

            Xin.previousMouseState = Xin.currentMouseState;
            Xin.currentMouseState = Mouse.GetState();

            MouseAsPoint = new Point(currentMouseState.X, currentMouseState.Y);

            base.Update(gameTime);
        }

        public static void FlushInput()
        {
            currentMouseState = previousMouseState;
            currentKeyboardState = previousKeyboardState;
        }

        public static bool CheckKeyReleased(Keys key)
        {
            return currentKeyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key);
        }

        public static bool CheckKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public static bool CheckMouseReleased(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return (currentMouseState.LeftButton == ButtonState.Released) && (previousMouseState.LeftButton == ButtonState.Pressed);
                case MouseButtons.Right:
                    return (currentMouseState.RightButton == ButtonState.Released) && (previousMouseState.RightButton == ButtonState.Pressed);
                case MouseButtons.Center:
                    return (currentMouseState.MiddleButton == ButtonState.Released) && (previousMouseState.MiddleButton == ButtonState.Pressed);
            }

            return false;
        }

        internal static bool CheckMouseReleased(object left)
        {
            throw new NotImplementedException();
        }
    }
}
