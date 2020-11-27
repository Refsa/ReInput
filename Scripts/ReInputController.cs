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

        [SerializeField] List<ReInputMap> inputMaps = new List<ReInputMap>();

        static ControllerType _activeControllerType;
        public static ControllerType ActiveControllerType => _activeControllerType;

        string lastGamepadName;

        static bool isSetup = false;

        public void Setup ( )
        {
            if (instance == null) instance = this;
            else 
            {
                Destroy (this);
                return;
            }

            isSetup = true;
        }

        void Start()
        {
            if (!isSetup) Setup();

            for (int i = 0; i < inputMaps.Count; i++)
            {
                InitInputMap(inputMaps[i]);
            }
        }

        void Update ( )
        {
            if (!isSetup) Setup();
            
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
            if (!isSetup) Setup();

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
            if (!isSetup) Setup();

            foreach (var input in inputMap.InputMap)
            {
                input.Init ( );
            }
        }

        public static void AddInputMap(ReInputMap inputMap)
        {
            if (!isSetup) return;
            
            instance.inputMaps.Add(inputMap);
            instance.InitInputMap(inputMap);
        }
    }
}
