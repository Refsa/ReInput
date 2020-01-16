using System;
using System.Collections.Generic;
using UnityEngine;

namespace OmegaInput
{
    [CreateAssetMenu (menuName = "Input/Omega Input Map")]
    public class OmegaInputMap : ScriptableObject
    {
        [SerializeField] List<OmegaInput> inputMap;

        public List<OmegaInput> InputMap => inputMap;

        public OmegaInputMap ( )
        {
            inputMap = new List<OmegaInput> ( );
        }

        public OmegaInput FindButton (string name)
        {
            return inputMap.Find (bi => bi.Name == name);
        }

        void OnValidate ( )
        {
            foreach (var input in inputMap)
            {
                input.Validate ( );
            }
        }
    }
}
