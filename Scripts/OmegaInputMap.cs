using System;
using System.Collections.Generic;
using UnityEngine;

namespace OmegaInput
{
    [CreateAssetMenu (menuName = "Input/Omega Input Map")]
    public class OmegaInputMap : ScriptableObject
    {
        [NaughtyAttributes.ReorderableList]
        [SerializeField] List<OmegaInput> inputMap;

        public List<OmegaInput> InputMap => inputMap;

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
