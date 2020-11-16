using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Refsa.ReInput;

namespace Refsa.ReInput.Editor
{
    static class ContentHelpers
    {
        public static GUIContent RemoveButtonLabel;
        public static GUIContent CopyButtonLabel;
        public static GUIContent MoveUpButtonLabel;
        public static GUIContent MoveDownButtonLabel;
        public static GUIContent AddButtonLabel;

        static ContentHelpers()
        {
            RemoveButtonLabel = new GUIContent("X", "Remove Entry");
            CopyButtonLabel = new GUIContent("C", "Duplicate Entry");
            MoveDownButtonLabel = new GUIContent("▼", "Move Entry Down");
            MoveUpButtonLabel = new GUIContent("▲", "Move Contet Up");

            AddButtonLabel = new GUIContent("Add", "Add New Entry");
        }
    }

    [CustomEditor(typeof(ReInputMap))]
    public class ReInputMapEditor : UnityEditor.Editor
    {
        Vector2 inputMapScrollPosition;
        [SerializeField] bool foldoutInputMap = true;
        bool wasAdded = false;

        ReInput inputToRemove = null;
        Dictionary<ReInput, bool> foldoutInput;
        GUIStyle foldoutStyle;
        ReInputMap targetAs;

        EditorSettings.Settings settings;

        void OnDestroy() 
        {
            EditorSettings.Save();    
        }

        public override void OnInspectorGUI()
        {
            targetAs = (ReInputMap)target;
            if (settings == null) settings = EditorSettings.GetSettings(targetAs);

            serializedObject.Update();

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
                if (GUILayout.Button(ContentHelpers.AddButtonLabel))
                {
                    targetAs.InputMap.Add(new ReInput());
                    wasAdded = true;
                }
            }
            EditorGUILayout.EndHorizontal();

            settings.FoldoutInputMap = EditorGUILayout.Foldout(settings.FoldoutInputMap, "Input Map");

            if (settings.FoldoutInputMap && !wasAdded)
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

                if (GUILayout.Button(ContentHelpers.RemoveButtonLabel, EditorStyles.miniButtonLeft))
                {
                    inputToRemove = input;
                }
                if (GUILayout.Button(ContentHelpers.CopyButtonLabel, EditorStyles.miniButtonMid))
                {
                    var copy = input.DeepCopy();
                    ((ReInputMap)target).InputMap.Add(copy);
                    wasAdded = true;
                }
                if (GUILayout.Button(ContentHelpers.MoveDownButtonLabel, EditorStyles.miniButtonMid))
                {
                    int indexOf = targetAs.InputMap.IndexOf(input);
                    if (indexOf == -1 || indexOf == targetAs.InputMap.Count - 1) return;

                    targetAs.InputMap[indexOf] = targetAs.InputMap[indexOf + 1];
                    targetAs.InputMap[indexOf + 1] = input;
                    wasAdded = true;
                }
                if (GUILayout.Button(ContentHelpers.MoveUpButtonLabel, EditorStyles.miniButtonRight))
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


    static class EditorSettings
    {
        const string PrefsStorePath = "reinput_editor_settings";

        [System.Serializable]
        public class Settings
        {
            [SerializeReference] public readonly ReInputMap TargetInputMap;
            public bool FoldoutInputMap;
            public Vector2 InputMapScrollPosition;

            public Settings(ReInputMap inputMap) { FoldoutInputMap = true; InputMapScrollPosition = Vector2.zero; TargetInputMap = inputMap;}
        }

        [System.Serializable]
        class StoredSettings
        {
            public List<Settings> Settings = new List<Settings>();

            public void AddSettings(Settings settings)
            {
                Settings.Add(settings);
            }

            public bool TryGetSettings(ReInputMap inputMap, out Settings settings)
            {
                settings = Settings.Find(e => e.TargetInputMap == inputMap);
                return settings != null;            
            }
        }

        static StoredSettings storedSettings;

        public static Settings GetSettings(ReInputMap inputMap)
        {
            if (storedSettings == null)
            {
                Load();
            }

            if (storedSettings.TryGetSettings(inputMap, out var settings))
            {
                return settings;
            }

            settings = new Settings(inputMap);
            storedSettings.AddSettings(settings);
            return settings;
        }

        public static void Save()
        {
            var asJson = JsonUtility.ToJson(storedSettings, true);
            UnityEngine.Debug.Assert(!string.IsNullOrEmpty(asJson), $"ReInput: StoredSettings was not resolved to JSON format");
            EditorPrefs.SetString(PrefsStorePath, asJson);
        }

        static void Load()
        {
            if (EditorPrefs.HasKey(PrefsStorePath))
            {
                storedSettings = JsonUtility.FromJson<StoredSettings>(EditorPrefs.GetString(PrefsStorePath));
                UnityEngine.Debug.Assert(storedSettings != null, $"ReInput: Stored settings were null");
            } 
            
            if (storedSettings == null) 
            {
                storedSettings = new StoredSettings();    
            }
        }
    }
}
