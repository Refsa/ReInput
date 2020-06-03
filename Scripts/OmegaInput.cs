using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Refsa.OmegaInput
{
    [System.Serializable]
    public class OmegaInput : IOmegaButtonInput, IOmegaInputAxis
    {
        [SerializeField] string name;
        [SerializeField] InputType inputType;

        // # Keyboard Input
        [Header ("KBM")]
        [SerializeField] KeyCode keyboardButton;
        [SerializeField] KeyboardAxis keyboardAxis;
        [SerializeField] MouseAxis mouseAxis;

        // # Switch Input
        [Header ("Switch")]
        [SerializeField] SwitchButton switchButton;
        [SerializeField] SwitchAxis switchAxis;

        // # Xbox Input
        [Header ("Xbox")]
        [SerializeField] XboxButton xboxButton;
        [SerializeField] XboxAxis xboxAxis;

        // # PS4 Input
        [Header ("PS4")]
        [SerializeField] PS4Button ps4Button;
        [SerializeField] PS4Axis ps4Axis;

        public event Action onButtonDown;
        public event Action onButtonUp;
        public event Action onButtonUpWasDown;
        public event Action<float> onButtonHeld;
        public event Action<float> onGetAxis;

        float startTime;
        float endTime;

        KeyCode switchButtonKeycode;
        KeyCode xboxButtonKeycode;
        KeyCode ps4ButtonKeycode;

        ControllerType activeControllerType;
        public ControllerType ActiveControllerType => activeControllerType;

        public string Name { get => name; set => name = value; }
        public InputType InputType { get => inputType; set => inputType = value; }
        public KeyCode KeyboardButton { get => keyboardButton; set => keyboardButton = value; }
        public KeyboardAxis KeyboardAxis { get => keyboardAxis; set => keyboardAxis = value; }
        public MouseAxis MouseAxis { get => mouseAxis; set => mouseAxis = value; }
        public SwitchButton SwitchButton { get => switchButton; set => switchButton = value; }
        public SwitchAxis SwitchAxis { get => switchAxis; set => switchAxis = value; }
        public XboxButton XboxButton { get => xboxButton; set => xboxButton = value; }
        public XboxAxis XboxAxis { get => xboxAxis; set => xboxAxis = value; }
        public PS4Button Ps4Button { get => ps4Button; set => ps4Button = value; }
        public PS4Axis Ps4Axis { get => ps4Axis; set => ps4Axis = value; }

        bool wasPressed = false;
        float axisButtonThreshold = 0.2f;

        public void Init ( )
        {
            switchButtonKeycode = ControllerInputBind.GetButtonKeyCode<SwitchButton> (switchButton);
            xboxButtonKeycode = ControllerInputBind.GetButtonKeyCode<XboxButton> (xboxButton);
            ps4ButtonKeycode = ControllerInputBind.GetButtonKeyCode<PS4Button> (ps4Button);
        }

        public void SetDevice (string deviceName)
        {
            switch (deviceName)
            {
                case "SwitchProControllerHID":
                    activeControllerType = ControllerType.Switch;
                    break;
                case "XInputControllerWindows":
                    activeControllerType = ControllerType.Xbox;
                    break;
                case "KeyboardAndMouse":
                    activeControllerType = ControllerType.KBM;
                    break;
            }
        }

        float GetKeyboardAxis (KeyCode negative, KeyCode positive)
        {
            float input = 0f;

            if (Input.GetKey (negative) || Input.GetKeyDown (negative))
            {
                input += -1f;
            }
            if (Input.GetKey (positive) || Input.GetKeyDown (positive))
            {
                input += 1f;
            }

            return input;
        }

        public void GetAxis ( )
        {
            if (activeControllerType == ControllerType.KBM)
            {
                switch (keyboardAxis)
                {
                    case KeyboardAxis.None:
                        break;
                    case KeyboardAxis.LeftRightArrow:
                        onGetAxis?.Invoke (GetKeyboardAxis (KeyCode.LeftArrow, KeyCode.RightArrow));
                        break;
                    case KeyboardAxis.UpDownArrow:
                        onGetAxis?.Invoke (GetKeyboardAxis (KeyCode.DownArrow, KeyCode.UpArrow));
                        break;
                    case KeyboardAxis.AD:
                        onGetAxis?.Invoke (GetKeyboardAxis (KeyCode.A, KeyCode.D));
                        break;
                    case KeyboardAxis.WS:
                        onGetAxis?.Invoke (GetKeyboardAxis (KeyCode.S, KeyCode.W));
                        break;
                }
            }
            else if (activeControllerType == ControllerType.Switch)
            {
                switch (switchAxis)
                {
                    case SwitchAxis.None:
                        break;
                    case SwitchAxis.LeftStickHorizontal:
                        //onGetAxis?.Invoke (Gamepad.current.leftStick.ReadValue ( ).x);
                        onGetAxis?.Invoke (Input.GetAxis ("Horizontal"));
                        break;
                    case SwitchAxis.LeftStickVertical:
                        //onGetAxis?.Invoke (Gamepad.current.leftStick.ReadValue ( ).y);
                        onGetAxis?.Invoke (Input.GetAxis ("Vertical"));
                        break;
                    case SwitchAxis.RightStickHorizontal:
                        //onGetAxis?.Invoke (Gamepad.current.rightStick.ReadValue ( ).x);
                        onGetAxis?.Invoke (Input.GetAxis ("SwitchLookHorizontal"));
                        break;
                    case SwitchAxis.RightStickVertical:
                        //onGetAxis?.Invoke (Gamepad.current.rightStick.ReadValue ( ).y);
                        onGetAxis?.Invoke (Input.GetAxis ("SwitchLookVertical"));
                        break;
                    case SwitchAxis.DPadHorizontal:
                        //onGetAxis?.Invoke (Gamepad.current.leftStick.ReadValue ( ).x);
                        onGetAxis?.Invoke (Input.GetAxis ("Horizontal"));
                        break;
                    case SwitchAxis.DPadVertical:
                        //onGetAxis?.Invoke (Gamepad.current.leftStick.ReadValue ( ).y);
                        onGetAxis?.Invoke (Input.GetAxis ("Vertical"));
                        break;
                }
            }
            else if (activeControllerType == ControllerType.Xbox)
            {
                float value = 0f;
                switch (xboxAxis)
                {
                    case XboxAxis.None:
                        break;
                    case XboxAxis.LeftStickHorizontal:
                        //onGetAxis?.Invoke (Gamepad.current.leftStick.ReadValue ( ).x);
                        onGetAxis?.Invoke (Input.GetAxis ("Horizontal"));
                        break;
                    case XboxAxis.LeftStickVertical:
                        //onGetAxis?.Invoke (Gamepad.current.leftStick.ReadValue ( ).y);
                        onGetAxis?.Invoke (Input.GetAxis ("Vertical"));
                        break;
                    case XboxAxis.RightStickHorizontal:
                        //onGetAxis?.Invoke (Gamepad.current.rightStick.ReadValue ( ).x);
                        onGetAxis?.Invoke (Input.GetAxis ("LookHorizontal"));
                        break;
                    case XboxAxis.RightStickVertical:
                        //onGetAxis?.Invoke (Gamepad.current.rightStick.ReadValue ( ).y);
                        onGetAxis?.Invoke (Input.GetAxis ("LookVertical"));
                        break;
                    case XboxAxis.RightTrigger:
                        //onGetAxis?.Invoke (Gamepad.current.rightTrigger.ReadValue ( ));
                        value = Input.GetAxis ("XboxBackTrigger");
                        if (value < 0f) value = 0f;
                        onGetAxis?.Invoke (value);
                        break;
                    case XboxAxis.LeftTrigger:
                        //onGetAxis?.Invoke (Gamepad.current.leftTrigger.ReadValue ( ));
                        value = Input.GetAxis ("XboxBackTrigger");
                        if (value > 0f) value = 0f;
                        onGetAxis?.Invoke (-value);
                        break;
                    case XboxAxis.DPadHorizontal:
                        onGetAxis?.Invoke (Gamepad.current.dpad.ReadValue ( ).x);
                        break;
                    case XboxAxis.DPadVertical:
                        onGetAxis?.Invoke (Gamepad.current.dpad.ReadValue ( ).y);
                        break;
                }
            }
            else if (activeControllerType == ControllerType.PS4)
            {
                switch (ps4Axis)
                {
                    case PS4Axis.None:
                        break;
                    case PS4Axis.LeftStickHorizontal:
                        onGetAxis?.Invoke (Gamepad.current.leftStick.ReadValue ( ).x);
                        break;
                    case PS4Axis.LeftStickVertical:
                        onGetAxis?.Invoke (Gamepad.current.leftStick.ReadValue ( ).y);
                        break;
                    case PS4Axis.RightStickHorizontal:
                        onGetAxis?.Invoke (Gamepad.current.rightStick.ReadValue ( ).x);
                        break;
                    case PS4Axis.RightStickVertical:
                        onGetAxis?.Invoke (Gamepad.current.rightStick.ReadValue ( ).y);
                        break;
                    case PS4Axis.DPadHorizontal:
                        onGetAxis?.Invoke (Gamepad.current.dpad.ReadValue ( ).x);
                        break;
                    case PS4Axis.DPadVertical:
                        onGetAxis?.Invoke (Gamepad.current.dpad.ReadValue ( ).y);
                        break;
                }
            }
        }

        public void GetButton ( )
        {
            if (GetButtonDown ( )) { }
            else if (GetButtonHeld ( )) { }
            else if (GetButtonUp ( )) { }
        }

        bool GetButtonDown ( )
        {
            bool wasDown = false;

            if (Input.GetKeyDown (keyboardButton))
            {
                wasDown = true;
            }
            else
            {
                if (activeControllerType == ControllerType.Switch && Input.GetKeyDown (switchButtonKeycode))
                {
                    wasDown = true;
                }
                else if (activeControllerType == ControllerType.Xbox)
                {
                    if (xboxButton == XboxButton.RT || xboxButton == XboxButton.LT)
                    {
                        float value = Input.GetAxis ("XboxBackTrigger");

                        if (!wasPressed && Mathf.Abs (value) >= axisButtonThreshold)
                        {
                            if ((xboxButton == XboxButton.RT && value < 0f) || (xboxButton == XboxButton.LT && value > 0f))
                            {
                                wasDown = true;
                                wasPressed = true;
                            }
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown (xboxButtonKeycode))
                            wasDown = true;
                    }
                }
                else if (activeControllerType == ControllerType.PS4 && Input.GetKeyDown (ps4ButtonKeycode))
                {
                    wasDown = true;
                }
            }

            if (wasDown)
            {
                startTime = Time.time;
                endTime = 0f;
                onButtonDown?.Invoke ( );
                wasPressed = true;
            }

            return wasDown;
        }

        bool GetButtonUp ( )
        {
            bool wasUp = false;

            if (Input.GetKeyUp (keyboardButton))
            {
                wasUp = true;
            }
            else
            {
                if (activeControllerType == ControllerType.Switch && Input.GetKeyUp (switchButtonKeycode))
                {
                    wasUp = true;
                }
                else if (activeControllerType == ControllerType.Xbox)
                {
                    if (xboxButton == XboxButton.RT || xboxButton == XboxButton.LT)
                    {
                        float value = Input.GetAxis ("XboxBackTrigger");

                        if (wasPressed && Mathf.Abs (value) < axisButtonThreshold)
                        {
                            if ((xboxButton == XboxButton.RT && value <= 0f) || (xboxButton == XboxButton.LT && value >= 0f))
                            {
                                wasUp = true;
                                wasPressed = false;
                            }
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown (xboxButtonKeycode))
                            wasUp = true;
                    }
                }
                else if (activeControllerType == ControllerType.PS4 && Input.GetKeyUp (ps4ButtonKeycode))
                {
                    wasUp = true;
                }
            }

            if (wasUp)
            {
                endTime = Time.time;
                onButtonUp?.Invoke ( );

                if (wasPressed)
                {
                    onButtonUpWasDown?.Invoke ( );
                }
                wasPressed = false;
            }

            return wasUp;
        }

        bool GetButtonHeld ( )
        {
            bool wasHeld = false;

            if (Input.GetKey (keyboardButton))
            {
                wasHeld = true;
            }
            else
            {
                if (activeControllerType == ControllerType.Switch && Input.GetKey (switchButtonKeycode))
                {
                    wasHeld = true;
                }
                else if (activeControllerType == ControllerType.Xbox)
                {
                    if (xboxButton == XboxButton.RT || xboxButton == XboxButton.LT)
                    {
                        float value = Input.GetAxis ("XboxBackTrigger");

                        if (wasPressed && Mathf.Abs (value) >= axisButtonThreshold)
                        {
                            if ((xboxButton == XboxButton.RT && value < 0f) || (xboxButton == XboxButton.LT && value > 0f))
                            {
                                wasHeld = true;
                            }
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown (xboxButtonKeycode))
                            wasHeld = true;
                    }
                }
                else if (activeControllerType == ControllerType.PS4 && Input.GetKey (ps4ButtonKeycode))
                {
                    wasHeld = true;
                }
            }

            if (wasHeld)
            {
                onButtonHeld?.Invoke (Time.time - startTime);
            }

            return wasHeld;
        }

        public bool IsAxis ( )
        {
            if (inputType == InputType.Axis ||
                inputType == InputType.Both)
            {
                return true;
            }

            return false;
        }

        public bool IsButton ( )
        {
            if (inputType == InputType.Button ||
                inputType == InputType.Both)
            {
                return true;
            }

            return false;
        }

        public bool Validate ( )
        {
            switch (inputType)
            {
                case InputType.Button:
                    if (keyboardAxis != KeyboardAxis.None ||
                        mouseAxis != MouseAxis.None ||
                        ps4Axis != PS4Axis.None ||
                        xboxAxis != XboxAxis.None ||
                        switchAxis != SwitchAxis.None)
                    {
                        Debug.LogAssertion ("Axis is set on input of name " + name + " even though it's configured as a button");
                        return false;
                    }
                    break;
                case InputType.Axis:
                    if (keyboardButton != KeyCode.None ||
                        ps4Button != PS4Button.None ||
                        xboxButton != XboxButton.None ||
                        switchButton != SwitchButton.None)
                    {
                        Debug.LogAssertion ("Button is set on input of name " + name + " even though it's configured as an axis");
                        return false;
                    }
                    break;
            }

            return true;
        }

        public object ShallowCopy ( )
        {
            return this.MemberwiseClone ( );
        }

        public OmegaInput DeepCopy ( )
        {
            var copy = new OmegaInput ( );

            copy.name = name + "_COPY";
            copy.inputType = inputType;
            copy.keyboardAxis = keyboardAxis;
            copy.keyboardButton = keyboardButton;
            copy.mouseAxis = mouseAxis;
            copy.switchAxis = switchAxis;
            copy.switchButton = switchButton;
            copy.xboxAxis = xboxAxis;
            copy.xboxButton = xboxButton;
            copy.ps4Axis = ps4Axis;
            copy.ps4Button = ps4Button;

            return copy;
        }
    }
}
