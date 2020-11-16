using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Refsa.ReInput;

namespace Refsa.ReInput.Editor
{
    [CustomEditor(typeof(ReInputMap))]
    public class ReInputMapEditor : UnityEditor.Editor
    {
        Vector2 inputMapScrollPosition;
        bool foldoutInputMap;
        bool wasAdded = false;

        ReInput inputToRemove = null;
        Dictionary<ReInput, bool> foldoutInput;
        GUIStyle foldoutStyle;
        ReInputMap targetAs;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            targetAs = (ReInputMap)target;

            if (foldoutStyle == null)
            {
                foldoutStyle = new GUIStyle(EditorStyles.foldout);
                foldoutStyle.normal.textColor = Color.blue;
            }

            if (foldoutInput == null) foldoutInput = new Dictionary<ReInput, bool>();
            foreach (var input in targetAs.InputMap)
            {
                if (!foldoutInput.ContainsKey(input))
                {
                    foldoutInput.Add(input, true);
                }
            }

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Add"))
                {
                    targetAs.InputMap.Add(new ReInput());
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
                        foreach (var input in targetAs.InputMap)
                        {
                            DrawInputFoldout(input);

                            if (wasAdded) break;

                            if (foldoutInput[input])
                            {
                                DrawReInput(input);
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
                targetAs.InputMap.Remove(inputToRemove);
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

        void DrawInputFoldout(ReInput input)
        {
            if (!input.Validate()) foldoutStyle.normal.textColor = Color.red;
            else foldoutStyle.normal.textColor = Color.blue;

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
                    ((ReInputMap)target).InputMap.Add(copy);
                    wasAdded = true;
                }
                if (GUILayout.Button("▼", EditorStyles.miniButtonMid))
                {
                    int indexOf = targetAs.InputMap.IndexOf(input);
                    if (indexOf == -1 || indexOf == targetAs.InputMap.Count - 1) return;

                    targetAs.InputMap[indexOf] = targetAs.InputMap[indexOf + 1];
                    targetAs.InputMap[indexOf + 1] = input;
                    wasAdded = true;
                }
                if (GUILayout.Button("▲", EditorStyles.miniButtonRight))
                {
                    int indexOf = targetAs.InputMap.IndexOf(input);
                    if (indexOf == -1 || indexOf == 0 || targetAs.InputMap.Count == 0) return;

                    targetAs.InputMap[indexOf] = targetAs.InputMap[indexOf - 1];
                    targetAs.InputMap[indexOf - 1] = input;
                    wasAdded = true;
                }
            }
        }

        void DrawReInput(ReInput ReInput)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUI.indentLevel++;

                ReInput.Name = EditorGUILayout.TextField("Name", ReInput.Name);
                ReInput.InputType = (InputType)EditorGUILayout.EnumPopup("Input Type", ReInput.InputType);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Keyboard", EditorStyles.boldLabel);
                if (ReInput.InputType == InputType.Button || ReInput.InputType == InputType.Both)
                    ReInput.KeyboardButton = (KeyCode)EditorGUILayout.EnumPopup("Keyboard Button", ReInput.KeyboardButton);
                if (ReInput.InputType == InputType.Axis || ReInput.InputType == InputType.Both)
                {
                    ReInput.KeyboardAxis = (KeyboardAxis)EditorGUILayout.EnumPopup("Keyboard Axis", ReInput.KeyboardAxis);
                    ReInput.MouseAxis = (MouseAxis)EditorGUILayout.EnumPopup("Mouse Axis", ReInput.MouseAxis);
                }

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Switch", EditorStyles.boldLabel);
                if (ReInput.InputType == InputType.Button || ReInput.InputType == InputType.Both)
                    ReInput.SwitchButton = (SwitchButton)EditorGUILayout.EnumPopup("Switch Button", ReInput.SwitchButton);
                if (ReInput.InputType == InputType.Axis || ReInput.InputType == InputType.Both)
                    ReInput.SwitchAxis = (SwitchAxis)EditorGUILayout.EnumPopup("Switch Axis", ReInput.SwitchAxis);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Xbox", EditorStyles.boldLabel);
                if (ReInput.InputType == InputType.Button || ReInput.InputType == InputType.Both)
                    ReInput.XboxButton = (XboxButton)EditorGUILayout.EnumPopup("Xbox Button", ReInput.XboxButton);
                if (ReInput.InputType == InputType.Axis || ReInput.InputType == InputType.Both)
                    ReInput.XboxAxis = (XboxAxis)EditorGUILayout.EnumPopup("Xbox Axis", ReInput.XboxAxis);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("PS4", EditorStyles.boldLabel);
                if (ReInput.InputType == InputType.Button || ReInput.InputType == InputType.Both)
                    ReInput.Ps4Button = (PS4Button)EditorGUILayout.EnumPopup("PS4 Button", ReInput.Ps4Button);
                if (ReInput.InputType == InputType.Axis || ReInput.InputType == InputType.Both)
                    ReInput.Ps4Axis = (PS4Axis)EditorGUILayout.EnumPopup("PS4 Axis", ReInput.Ps4Axis);

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}
