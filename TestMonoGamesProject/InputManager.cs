using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace TestMonoGamesProject
{
    public class InputManager
    {
        private HashSet<Keys> _pressedKeysSet = new();
        private HashSet<Keys> _justPressedKeys = new();

        public void Update()
        {
            var kstate = Keyboard.GetState();
            var newKey = kstate.GetPressedKeys();
            _justPressedKeys = new HashSet<Keys>(newKey);
            _justPressedKeys.ExceptWith(_pressedKeysSet);
            _pressedKeysSet = new HashSet<Keys>(newKey);
        }

        public bool IsKeyPressed(Keys key) =>
            _pressedKeysSet.Contains(key);

        public bool IsKeyJustPressed(Keys key) =>
            _justPressedKeys.Contains(key);
    }
}
