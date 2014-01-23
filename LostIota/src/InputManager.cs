using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace LostIota
{
    public class InputManager
    {
        KeyboardState previousKey, currentKey;

        public KeyboardState PreviousKey
        {
            get { return previousKey; }
            set { previousKey = value; }
        }

        public KeyboardState CurrentKey
        {
            get { return currentKey; }
            set { currentKey = value; }
        }

        public void Update()
        {
            previousKey = currentKey;
            currentKey = Keyboard.GetState();
        }

        public bool KeyPressed(Keys key)
        {
            if (currentKey.IsKeyDown(key) && previousKey.IsKeyUp(key))
                return true;
            return false;
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKey.IsKeyDown(key) && previousKey.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public bool KeyReleased(Keys key)
        {
            if (currentKey.IsKeyUp(key) && previousKey.IsKeyDown(key))
                return true;
            return false;
        }

        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKey.IsKeyUp(key) && previousKey.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public bool KeyDown(Keys key)
        {
            if (currentKey.IsKeyDown(key))
                return true;
            return false;
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKey.IsKeyDown(key))
                    return true;
            }
            return false;
        }
    }
}
