using UnityEngine;
using UnityEngine.InputSystem;

namespace OmegaInput
{
    public class OmegaInputController : MonoBehaviour
    {
        static OmegaInputController instance;

        [SerializeField] OmegaInputMap characterInputMap;
        [SerializeField] OmegaInputMap debugInputMap;

        [SerializeField] ControllerType activeControllerType;

        static OmegaInputMap _characterInputMap;
        public static OmegaInputMap CharacterInputMap => _characterInputMap;

        static OmegaInputMap _debugInputMap;
        public static OmegaInputMap DebugInputMap => _debugInputMap;

        static ControllerType _activeControllerType;
        public static ControllerType ActiveControllerType => _activeControllerType;

        public static bool LockDebugInput;
        public static bool LockCharacterInput;

        void Awake ( )
        {
            if (instance == null) instance = this;
            else Destroy (this);

            _characterInputMap = characterInputMap;
            _debugInputMap = debugInputMap;

            InitInputMap (characterInputMap);
            InitInputMap (debugInputMap);
        }

        void Update ( )
        {
            //string currentGamepad = Gamepad.current != null ? Gamepad.current.name : "KeyboardAndMouse";
            string currentGamepad = "KeyboardAndMouse";

            if (Gamepad.current.IsActuated ( ))
            {
                currentGamepad = Gamepad.current.name;
            }

            //Debug.Log ("Current gamepad: " + currentGamepad);

            if (!LockDebugInput)
                FetchInput (debugInputMap, currentGamepad);
            if (!LockCharacterInput)
                FetchInput (characterInputMap, currentGamepad);

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

        public static OmegaInput FindInputButton (string name)
        {
            return _characterInputMap.InputMap.Find (bi => bi.Name == name);
        }
    }
}
