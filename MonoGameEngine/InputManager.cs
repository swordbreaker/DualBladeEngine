using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoGameEngine;

public class InputManager
{
    private HashSet<Keys> _pressedKeysSet = new();
    private HashSet<Keys> _justPressedKeys = new();

    private int _oldScrollWheelValue = 0;

    public void Update()
    {
        var kstate = Keyboard.GetState();
        var newKey = kstate.GetPressedKeys();
        _justPressedKeys = new HashSet<Keys>(newKey);
        _justPressedKeys.ExceptWith(_pressedKeysSet);
        _pressedKeysSet = new HashSet<Keys>(newKey);

        var mstate = Mouse.GetState();
        IsLeftMousePressed = mstate.LeftButton == ButtonState.Pressed;
        IsRightMousePressed = mstate.RightButton == ButtonState.Pressed;
        IsMiddleMousePressed = mstate.MiddleButton == ButtonState.Pressed;

        MousePos = mstate.Position.ToVector2();
        ScrollWheelValue = mstate.ScrollWheelValue;
        ScrollWheelDelta = mstate.ScrollWheelValue - _oldScrollWheelValue;
        _oldScrollWheelValue = ScrollWheelValue;
    }

    public bool IsLeftMousePressed { get; private set; }
    public bool IsRightMousePressed { get; private set; }
    public bool IsMiddleMousePressed { get; private set; }
    public int ScrollWheelValue { get; private set; }
    public int ScrollWheelDelta { get; private set; }

    public Vector2 MousePos { get; private set; }

    public bool IsKeyPressed(Keys key) =>
        _pressedKeysSet.Contains(key);

    public bool IsKeyJustPressed(Keys key) =>
        _justPressedKeys.Contains(key);
}
