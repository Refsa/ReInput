using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Refsa.ReInput
{
    [DefaultExecutionOrder(-1000)]
    public class ReInputController : MonoBehaviour
    {
        static ReInputController instance;

        [SerializeField] ControllerType activeControllerType;

        [SerializeField] List<ReInputMap> inputMaps;

        static ControllerType _activeControllerType;
        public static ControllerType ActiveControllerType => _activeControllerType;

        string lastGamepadName;

        public void Setup ( )
        {
            if (instance == null) instance = this;
            else 
            {
                Destroy (this);
                return;
            }

            // inputMaps = new List<ReInputMap>();
        }

        void Start()
        {
            for (int i = 0; i < inputMaps.Count; i++)
            {
                InitInputMap(inputMaps[i]);
            }
        }

        void Update ( )
        {
            if (Gamepad.current != null && Gamepad.current.IsActuated ( ))
            {
                lastGamepadName = Gamepad.current.name;
            }
            else if (Keyboard.current != null && Keyboard.current.IsActuated())
            {
                lastGamepadName = "KeyboardAndMouse";
            }

            for (int i = 0; i < inputMaps.Count; i++)
                FetchInput (inputMaps[i], lastGamepadName);

            activeControllerType = _activeControllerType;
        }

        void FetchInput (ReInputMap inputMap, string deviceName)
        {
            foreach (var input in inputMap.InputMap)
            {
                input.SetDevice (deviceName);

                if (input.InputType == InputType.Button || input.InputType == InputType.Both)
                {
                    input.GetButton ( );
                }
                if (input.InputType == InputType.Axis || input.InputType == InputType.Both)
                {
                    input.GetAxis ( );
                }

                _activeControllerType = input.ActiveControllerType;
            }
        }

        void InitInputMap (ReInputMap inputMap)
        {
            foreach (var input in inputMap.InputMap)
            {
                input.Init ( );
            }
        }

        public static void AddInputMap(ReInputMap inputMap)
        {
            instance.inputMaps.Add(inputMap);
            instance.InitInputMap(inputMap);
        }
    }
}
