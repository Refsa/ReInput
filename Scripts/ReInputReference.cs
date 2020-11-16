using System;
using UnityEngine;

namespace Refsa.ReInput
{
    public static class ControllerInputBind
    {
        static KeyCode[ ] buttonLUT = new KeyCode[15]
        {
            KeyCode.None,
            KeyCode.JoystickButton0,
            KeyCode.JoystickButton1,
            KeyCode.JoystickButton2,
            KeyCode.JoystickButton3,
            KeyCode.JoystickButton4,
            KeyCode.JoystickButton5,
            KeyCode.JoystickButton6,
            KeyCode.JoystickButton7,
            KeyCode.JoystickButton8,
            KeyCode.JoystickButton9,
            KeyCode.JoystickButton10,
            KeyCode.JoystickButton11,
            KeyCode.JoystickButton12,
            KeyCode.JoystickButton13,
        };

        public static KeyCode GetButtonKeyCode<T> (T buttonName) where T : Enum
        {
            return buttonLUT[Convert.ToInt32 (buttonName)];
        }
    }

    public enum ControllerType
    {
        KBM = 0, Switch, Xbox, PS4
    }

    public enum InputType
    {
        Button = 0, Axis, Both
    }

    public enum SwitchButton : int
    {
        None = 0, B, A, Y, X, L, R, LT, RT, Minus, Plus, LS, RS, Home, Capture
    }

    public enum SwitchAxis : int
    {
        None = 0,
        LeftStickHorizontal,
        LeftStickVertical,
        RightStickHorizontal,
        RightStickVertical,
        DPadVertical,
        DPadHorizontal
    }

    public enum Joystick : int
    {
        None = 0,
        LeftStick,
        RightStick
    }

    public enum XboxButton : int
    {
        None = 0, A, B, X, Y, LB, RB, Back, Start, LS, RS, LT, RT
    }

    public enum XboxAxis : int
    {
        None = 0,
        LeftStickHorizontal,
        LeftStickVertical,
        RightStickHorizontal,
        RightStickVertical,
        RightTrigger,
        LeftTrigger,
        DPadVertical,
        DPadHorizontal
    }

    public enum PS4Button : int
    {
        None = 0, Square, Cross, Circle, Triangle, L1, R1, L2, R2, Share, Options, L3, R3, PS, PadPress
    }

    public enum PS4Axis : int
    {
        None = 0,
        LeftStickHorizontal,
        LeftStickVertical,
        RightStickHorizontal,
        RightStickVertical,
        DPadVertical,
        DPadHorizontal
    }

    public enum MouseAxis : int
    {
        None = 0,
        Vertical,
        Horizontal
    }

    public enum KeyboardAxis : int
    {
        None = 0,
        AD,
        WS,
        LeftRightArrow,
        UpDownArrow,
    }
}
