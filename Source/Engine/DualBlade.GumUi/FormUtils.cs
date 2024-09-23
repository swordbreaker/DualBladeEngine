using Gum.Wireframe;
using MonoGameGum.Forms.Controls;
using MonoGameGum.Forms.DefaultVisuals;
using MonoGameGum.Input;
using System;

namespace DualBlade.GumUi;
internal class FormUtils
{
    static IGumCursor cursor;

    public static IGumCursor Cursor => cursor;

    static MonoGameGum.Input.Keyboard keyboard;

    public static Keyboard Keyboard => keyboard;

    public static void InitializeDefaults()
    {
        FrameworkElement.DefaultFormsComponents[typeof(Button)] = typeof(DefaultButtonRuntime);
        FrameworkElement.DefaultFormsComponents[typeof(CheckBox)] = typeof(DefaultCheckboxRuntime);
        FrameworkElement.DefaultFormsComponents[typeof(ListBox)] = typeof(DefaultListBoxRuntime);
        FrameworkElement.DefaultFormsComponents[typeof(ListBoxItem)] = typeof(DefaultListBoxItemRuntime);
        FrameworkElement.DefaultFormsComponents[typeof(ScrollBar)] = typeof(DefaultScrollBarRuntime);
        FrameworkElement.DefaultFormsComponents[typeof(ScrollViewer)] = typeof(DefaultScrollViewerRuntime);
        FrameworkElement.DefaultFormsComponents[typeof(TextBox)] = typeof(DefaultTextBoxRuntime);
        FrameworkElement.DefaultFormsComponents[typeof(PasswordBox)] = typeof(DefaultTextBoxRuntime);
        FrameworkElement.DefaultFormsComponents[typeof(Slider)] = typeof(DefaultSliderRuntime);

        if (OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
        {
            cursor = new TouchCursor();
        }
        else
        {
            cursor = new MouseCursor();
        }

        keyboard = new Keyboard();

        FrameworkElement.MainCursor = cursor;

    }

    public static void Update(GameTime gameTime, GraphicalUiElement rootElement)
    {
        cursor.Activity(gameTime.TotalGameTime.TotalSeconds);
        keyboard.Activity(gameTime.TotalGameTime.TotalSeconds);

        rootElement.DoUiActivityRecursively(cursor, keyboard, gameTime.TotalGameTime.TotalSeconds);

    }
}
