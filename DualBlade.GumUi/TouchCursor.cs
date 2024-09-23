using FunctionalMonads.Monads.MaybeMonad;
using Gum.Wireframe;
using Microsoft.Xna.Framework.Input.Touch;
using System.Linq;

namespace DualBlade.GumUi;

internal class TouchCursor : IGumCursor
{
    public int X => (int)touchLocation.X;

    public int Y => (int)touchLocation.Y;

    public int XChange => (int)delta.X;

    public int YChange => (int)delta.Y;

    public int ScrollWheelChange => 0;

    public bool PrimaryPush { get; set; }

    public bool PrimaryDown => firstTouchLocation.IsSome;

    public bool PrimaryClick { get; private set; }

    public bool PrimaryClickNoSlide { get; private set; }

    public bool PrimaryDoubleClick => false;

    public bool SecondaryPush => false;

    public bool SecondaryDown => false;

    public bool SecondaryClick => false;

    public bool SecondaryDoubleClick => false;

    public bool MiddlePush => false;

    public bool MiddleDown => false;

    public bool MiddleClick => false;

    public bool MiddleDoubleClick => false;

    private TouchCollection touchState;
    private IMaybe<TouchLocation> firstTouchLocation = Maybe.None<TouchLocation>();

    private Vector2 touchLocation = Vector2.Zero;
    private Vector2 delta = Vector2.Zero;

    public void Activity(double seconds)
    {
        touchState = TouchPanel.GetState();
        PrimaryPush = false;
        PrimaryClick = false;
        PrimaryClickNoSlide = false;

        if (touchState.Count > 0)
        {
            if (firstTouchLocation.IsNone)
            {
                PrimaryPush = true;
            }

            var first = touchState.First();
            firstTouchLocation = Maybe.Some(first);

            delta = first.Position - touchLocation;
            touchLocation = first.Position;
        }
        else
        {
            if (firstTouchLocation.IsSome)
            {
                PrimaryClick = true;
                PrimaryClickNoSlide = true;
            }

            firstTouchLocation = Maybe.None<TouchLocation>();
        }
    }

    public InteractiveGue WindowPushed { get; set; }
    public InteractiveGue WindowOver { get; set; }
}
