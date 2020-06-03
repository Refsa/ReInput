using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Refsa.OmegaInput
{
    public abstract class OmegaInputController : MonoBehaviour
    {
        static OmegaInputController instance;

        [SerializeField] ControllerType activeControllerType;

        List<OmegaInputMap> inputMaps;

        static ControllerType _activeControllerType;
        public static ControllerType ActiveControllerType => _activeControllerType;

        void Awake ( )
        {
            if (instance == null) instance = this;
            else Destroy (this);

            inputMaps = new List<OmegaInputMap>();
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

            if (Gamepad.current.IsActuated ( ))
            {
                currentGamepad = Gamepad.current.name;
            }

            for (int i = 0; i < inputMaps.Count; i++)
                FetchInput (inputMaps[i], currentGamepad);

            activeControllerType = _activeControllerType;
        }

        void InitInputMap (OmegaInputMap inputMap)
        {
            foreach (var input in inputMap.InputMap)
            {
                input.Init ( );
            }
        }

        void FetchInput (OmegaInputMap inputMap, string deviceName)
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
    }
}
