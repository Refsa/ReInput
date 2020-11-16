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

        List<ReInputMap> inputMaps;

        static ControllerType _activeControllerType;
        public static ControllerType ActiveControllerType => _activeControllerType;

        public void Setup ( )
        {
            if (instance == null) instance = this;
            else 
            {
                Destroy (this);
                return;
            }

            inputMaps = new List<ReInputMap>();
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
            string currentGamepad = "KeyboardAndMouse";

            if (Gamepad.current != null && Gamepad.current.IsActuated ( ))
            {
                if (Gamepad.current.name != currentGamepad)
                {
                    currentGamepad = Gamepad.current.name;
                }
            }

            for (int i = 0; i < inputMaps.Count; i++)
                FetchInput (inputMaps[i], currentGamepad);

            activeControllerType = _activeControllerType;
        }

        void InitInputMap (ReInputMap inputMap)
        {
            foreach (var input in inputMap.InputMap)
            {
                input.Init ( );
            }
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

        public static void AddInputMap(ReInputMap inputMap)
        {
            instance.inputMaps.Add(inputMap);
        }
    }
}
