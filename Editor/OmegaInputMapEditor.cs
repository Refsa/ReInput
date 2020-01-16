using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using OmegaInput;

namespace OmegaInput
{
    [CustomEditor (typeof (OmegaInputMap))]
    public class OmegaInputMapEditor : Editor
    {
        Vector2 inputMapScrollPosition;
        bool foldoutInputMap;
        bool wasAdded = false;

        OmegaInput inputToRemove = null;

        Dictionary<OmegaInput, bool> foldoutInput;

        public override void OnInspectorGUI ( )
        {
            serializedObject.Update ( );

            var targetas = (OmegaInputMap) target;

            EditorGUI.BeginChangeCheck ( );

            if (foldoutInput == null) foldoutInput = new Dictionary<OmegaInput, bool> ( );
            foreach (var input in targetas.InputMap)
            {
                if (!foldoutInput.ContainsKey (input))
                {
                    foldoutInput.Add (input, true);
                }
            }

            EditorGUILayout.BeginHorizontal ( );
            {
                if (GUILayout.Button ("Add"))
                {
                    targetas.InputMap.Add (new OmegaInput ( ));
                    wasAdded = true;
                }
            }
            EditorGUILayout.EndHorizontal ( );

            foldoutInputMap = EditorGUILayout.Foldout (foldoutInputMap, "Input Map");

            if (foldoutInputMap && !wasAdded)
            {
                EditorGUILayout.BeginVertical (EditorStyles.helpBox);
                {
                    inputMapScrollPosition = EditorGUILayout.BeginScrollView (inputMapScrollPosition);
                    {
                        foreach (var input in targetas.InputMap)
                        {
                            foldoutInput[input] = EditorGUILayout.Foldout (foldoutInput[input], input.Name + " ->");
                            if (foldoutInput[input])
                            {
                                DrawOmegaInput (input);
                                if (wasAdded) break;
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView ( );
                }
                EditorGUILayout.EndVertical ( );
            }

            if (inputToRemove != null)
            {
                targetas.InputMap.Remove (inputToRemove);
                inputToRemove = null;
            }
            wasAdded = false;

            serializedObject.ApplyModifiedProperties ( );

            if (EditorGUI.EndChangeCheck ( ))
            {
                EditorUtility.SetDirty (target);
                AssetDatabase.SaveAssets ( );
            }
        }

        void DrawOmegaInput (OmegaInput omegaInput)
        {
            EditorGUILayout.BeginVertical (EditorStyles.helpBox);
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal ( );
                {
                    if (GUILayout.Button ("X"))
                    {
                        inputToRemove = omegaInput;
                    }
                    if (GUILayout.Button ("Duplicate"))
                    {
                        var copy = omegaInput.DeepCopy ( );
                        ((OmegaInputMap) target).InputMap.Add (copy);
                        wasAdded = true;
                    }
                }
                EditorGUILayout.EndHorizontal ( );

                omegaInput.Name = EditorGUILayout.TextField ("Name", omegaInput.Name);
                omegaInput.InputType = (InputType) EditorGUILayout.EnumPopup ("Input Type", omegaInput.InputType);

                EditorGUILayout.Space ( );

                EditorGUILayout.LabelField ("Keyboard", EditorStyles.boldLabel);
                if (omegaInput.InputType == InputType.Button || omegaInput.InputType == InputType.Both)
                    omegaInput.KeyboardButton = (KeyCode) EditorGUILayout.EnumPopup ("Keyboard Button", omegaInput.KeyboardButton);
                if (omegaInput.InputType == InputType.Axis || omegaInput.InputType == InputType.Both)
                {
                    omegaInput.KeyboardAxis = (KeyboardAxis) EditorGUILayout.EnumPopup ("Keyboard Axis", omegaInput.KeyboardAxis);
                    omegaInput.MouseAxis = (MouseAxis) EditorGUILayout.EnumPopup ("Mouse Axis", omegaInput.MouseAxis);
                }

                EditorGUILayout.Space ( );

                EditorGUILayout.LabelField ("Switch", EditorStyles.boldLabel);
                if (omegaInput.InputType == InputType.Button || omegaInput.InputType == InputType.Both)
                    omegaInput.SwitchButton = (SwitchButton) EditorGUILayout.EnumPopup ("Switch Button", omegaInput.SwitchButton);
                if (omegaInput.InputType == InputType.Axis || omegaInput.InputType == InputType.Both)
                    omegaInput.SwitchAxis = (SwitchAxis) EditorGUILayout.EnumPopup ("Switch Axis", omegaInput.SwitchAxis);

                EditorGUILayout.Space ( );

                EditorGUILayout.LabelField ("Xbox", EditorStyles.boldLabel);
                if (omegaInput.InputType == InputType.Button || omegaInput.InputType == InputType.Both)
                    omegaInput.XboxButton = (XboxButton) EditorGUILayout.EnumPopup ("Xbox Button", omegaInput.XboxButton);
                if (omegaInput.InputType == InputType.Axis || omegaInput.InputType == InputType.Both)
                    omegaInput.XboxAxis = (XboxAxis) EditorGUILayout.EnumPopup ("Xbox Axis", omegaInput.XboxAxis);

                EditorGUILayout.Space ( );

                EditorGUILayout.LabelField ("PS4", EditorStyles.boldLabel);
                if (omegaInput.InputType == InputType.Button || omegaInput.InputType == InputType.Both)
                    omegaInput.Ps4Button = (PS4Button) EditorGUILayout.EnumPopup ("PS4 Button", omegaInput.Ps4Button);
                if (omegaInput.InputType == InputType.Axis || omegaInput.InputType == InputType.Both)
                    omegaInput.Ps4Axis = (PS4Axis) EditorGUILayout.EnumPopup ("PS4 Axis", omegaInput.Ps4Axis);

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical ( );
        }
    }
}
