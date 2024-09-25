﻿using AutomaticInterface;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace DualBlade.Core.Services;

[GenerateAutomaticInterface]
public sealed class InputManager : IInputManager
{
    private HashSet<Keys> _pressedKeysSet = [];
    private HashSet<Keys> _justPressedKeys = [];

    private int _oldScrollWheelValue = 0;

    public void Update()
    {
        TouchState = TouchPanel.GetState();
        var kstate = Keyboard.GetState();
        var newKey = kstate.GetPressedKeys();
        _justPressedKeys = new HashSet<Keys>(newKey);
        _justPressedKeys.ExceptWith(_pressedKeysSet);
        _pressedKeysSet = new HashSet<Keys>(newKey);

        var mstate = Mouse.GetState();
        var lPressed = mstate.LeftButton == ButtonState.Pressed;
        var rPressed = mstate.RightButton == ButtonState.Pressed;
        IsLeftMouseJustPressed = !IsLeftMousePressed && lPressed;
        IsRightMouseJustPressed = !IsRightMousePressed && rPressed;
        IsLeftMousePressed = mstate.LeftButton == ButtonState.Pressed;
        IsRightMousePressed = mstate.RightButton == ButtonState.Pressed;
        IsMiddleMousePressed = mstate.MiddleButton == ButtonState.Pressed;

        MousePos = mstate.Position.ToVector2();
        ScrollWheelValue = mstate.ScrollWheelValue;
        ScrollWheelDelta = mstate.ScrollWheelValue - _oldScrollWheelValue;
        _oldScrollWheelValue = ScrollWheelValue;
    }

    public bool IsLeftMousePressed { get; private set; }
    public bool IsLeftMouseJustPressed { get; private set; }
    public bool IsRightMousePressed { get; private set; }
    public bool IsRightMouseJustPressed { get; private set; }
    public bool IsMiddleMousePressed { get; private set; }
    public int ScrollWheelValue { get; private set; }
    public int ScrollWheelDelta { get; private set; }

    public Vector2 MousePos { get; private set; }

    public TouchCollection TouchState { get; private set; }

    public bool IsKeyPressed(Keys key) =>
        _pressedKeysSet.Contains(key);

    public bool IsKeyJustPressed(Keys key) =>
        _justPressedKeys.Contains(key);
}
