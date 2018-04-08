using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameArchitectureEngine
{
    public class InputListener
    {
        private KeyboardState PrevKeyboardState { get; set; }
        private KeyboardState CurrentKeyboardState { get; set; }

        private MouseState PrevMouseState { get; set; }
        private MouseState CurrentMouseState { get; set; }

        public HashSet<Keys> KeyList;
        public HashSet<MouseButton> MouseButtonList;

        public event EventHandler<KeyboardEventArgs> OnKeyDown = delegate { };
        public event EventHandler<KeyboardEventArgs> OnKeyPressed = delegate { };
        public event EventHandler<KeyboardEventArgs> OnKeyUp = delegate { };

        public event EventHandler<MouseEventArgs> OnMouseButtonDown = delegate { };

        public InputListener()
        {
            CurrentKeyboardState = Keyboard.GetState();
            PrevKeyboardState = CurrentKeyboardState;

            CurrentMouseState = Mouse.GetState();
            PrevMouseState = CurrentMouseState;

            KeyList = new HashSet<Keys>();
            MouseButtonList = new HashSet<MouseButton>();
        }

        public void AddKey(Keys key)
        {
            KeyList.Add(key);
        }

        public void AddButton(MouseButton button)
        {
            MouseButtonList.Add(button);
        }

        public void Update()
        {
            PrevKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            PrevMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            FireKeyboardEvents();
            FireMouseEvents();
        }

        private void FireKeyboardEvents()
        {
            foreach (Keys key in KeyList)
            {
                if (CurrentKeyboardState.IsKeyDown(key))
                {
                    if (OnKeyDown != null)
                        OnKeyDown(this, new KeyboardEventArgs(key, CurrentKeyboardState, PrevKeyboardState));
                }

                if (PrevKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key))
                {
                    if (OnKeyUp != null)
                        OnKeyUp(this, new KeyboardEventArgs(key, CurrentKeyboardState, PrevKeyboardState));
                }

                if (PrevKeyboardState.IsKeyUp(key) && CurrentKeyboardState.IsKeyDown(key))
                {
                    if (OnKeyPressed != null)
                        OnKeyPressed(this, new KeyboardEventArgs(key, CurrentKeyboardState, PrevKeyboardState));
                }
            }            
        }

        private void FireMouseEvents()
        {
            foreach (MouseButton button in MouseButtonList)
            {
                if (button == MouseButton.LEFT)
                {
                    if (CurrentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (OnMouseButtonDown != null)
                            OnMouseButtonDown(this, new MouseEventArgs(button, CurrentMouseState, PrevMouseState));
                    }
                }
            }           
        }
    }
}
