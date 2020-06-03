using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Refsa.OmegaInput;

namespace Refsa.OmegaInput.Editor
{
    [CustomEditor(typeof(OmegaInputMap))]
    public class OmegaInputMapEditor : UnityEditor.Editor
    {
        Vector2 inputMapScrollPosition;
        bool foldoutInputMap;
        bool wasAdded = false;

        OmegaInput inputToRemove = null;
        Dictionary<OmegaInput, bool> foldoutInput;
        GUIStyle foldoutStyle;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var targetas = (OmegaInputMap)target;

            if (foldoutStyle == null)
            {
                foldoutStyle = new GUIStyle(EditorStyles.foldout);
                foldoutStyle.normal.textColor = Color.blue;
            }

            if (foldoutInput == null) foldoutInput = new Dictionary<OmegaInput, bool>();
            foreach (var input in targetas.InputMap)
            {
                if (!foldoutInput.ContainsKey(input))
                {
                    foldoutInput.Add(input, false);
                }
            }

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Add"))
                {
                    targetas.InputMap.Add(new OmegaInput());
                    wasAdded = true;
                }
            }
            EditorGUILayout.EndHorizontal();

            foldoutInputMap = EditorGUILayout.Foldout(foldoutInputMap, "Input Map");

            if (foldoutInputMap && !wasAdded)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    inputMapScrollPosition = EditorGUILayout.BeginScrollView(inputMapScrollPosition);
                    {
                        int i = 0;
                        foreach (var input in targetas.InputMap)
                        {
                            DrawInputFoldout(input);

                            if (wasAdded) break;

                            if (foldoutInput[input])
                            {
                                DrawOmegaInput(input);
                            }

                            GUILayout.Space(3f);
                            i++;
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }

            if (inputToRemove != null)
            {
                targetas.InputMap.Remove(inputToRemove);
                inputToRemove = null;
            }
            wasAdded = false;

            serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
        }

        void DrawInputFoldout(OmegaInput input)
        {
            string foldoutName = input.Name + (foldoutInput[input] ? " ↓" : " →");
            using (new GUILayout.HorizontalScope())
            {
                foldoutInput[input] = EditorGUILayout.Foldout(foldoutInput[input], foldoutName, foldoutStyle);

                if (GUILayout.Button("X", EditorStyles.miniButtonLeft))
                {
                    inputToRemove = input;
                }
                if (GUILayout.Button("C", EditorStyles.miniButtonMid))
                {
                    var copy = input.DeepCopy();
                    ((OmegaInputMap)target).InputMap.Add(copy);
                    wasAdded = true;
                }
                if (GUILayout.Button("↓", EditorStyles.miniButtonMid))
                {

                }
                if (GUILayout.Button("↑", EditorStyles.miniButtonRight))
                {

                }
            }
        }

        void DrawOmegaInput(OmegaInput omegaInput)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUI.indentLevel++;

                omegaInput.Name = EditorGUILayout.TextField("Name", omegaInput.Name);
                omegaInput.InputType = (InputType)EditorGUILayout.EnumPopup("Input Type", omegaInput.InputType);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Keyboard", EditorStyles.boldLabel);
                if (omegaInput.InputType == InputType.Button || omegaInput.InputType == InputType.Both)
                    omegaInput.KeyboardButton = (KeyCode)EditorGUILayout.EnumPopup("Keyboard Button", omegaInput.KeyboardButton);
                if (omegaInput.InputType == InputType.Axis || omegaInput.InputType == InputType.Both)
                {
                    omegaInput.KeyboardAxis = (KeyboardAxis)EditorGUILayout.EnumPopup("Keyboard Axis", omegaInput.KeyboardAxis);
                    omegaInput.MouseAxis = (MouseAxis)EditorGUILayout.EnumPopup("Mouse Axis", omegaInput.MouseAxis);
                }

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Switch", EditorStyles.boldLabel);
                if (omegaInput.InputType == InputType.Button || omegaInput.InputType == InputType.Both)
                    omegaInput.SwitchButton = (SwitchButton)EditorGUILayout.EnumPopup("Switch Button", omegaInput.SwitchButton);
                if (omegaInput.InputType == InputType.Axis || omegaInput.InputType == InputType.Both)
                    omegaInput.SwitchAxis = (SwitchAxis)EditorGUILayout.EnumPopup("Switch Axis", omegaInput.SwitchAxis);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Xbox", EditorStyles.boldLabel);
                if (omegaInput.InputType == InputType.Button || omegaInput.InputType == InputType.Both)
                    omegaInput.XboxButton = (XboxButton)EditorGUILayout.EnumPopup("Xbox Button", omegaInput.XboxButton);
                if (omegaInput.InputType == InputType.Axis || omegaInput.InputType == InputType.Both)
                    omegaInput.XboxAxis = (XboxAxis)EditorGUILayout.EnumPopup("Xbox Axis", omegaInput.XboxAxis);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("PS4", EditorStyles.boldLabel);
                if (omegaInput.InputType == InputType.Button || omegaInput.InputType == InputType.Both)
                    omegaInput.Ps4Button = (PS4Button)EditorGUILayout.EnumPopup("PS4 Button", omegaInput.Ps4Button);
                if (omegaInput.InputType == InputType.Axis || omegaInput.InputType == InputType.Both)
                    omegaInput.Ps4Axis = (PS4Axis)EditorGUILayout.EnumPopup("PS4 Axis", omegaInput.Ps4Axis);

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}
