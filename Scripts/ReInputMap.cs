using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReInput
{
    [CreateAssetMenu (menuName = "ReInput Map")]
    public class ReInputMap : ScriptableObject
    {
        [SerializeField] List<ReInput> inputMap;

        public List<ReInput> InputMap => inputMap;

        public ReInputMap ( )
        {
            inputMap = new List<ReInput> ( );
        }

        public ReInput FindInput (string name)
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
