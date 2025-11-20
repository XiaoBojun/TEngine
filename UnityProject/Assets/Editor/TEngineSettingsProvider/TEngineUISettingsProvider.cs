using System;
using TEngine.Editor.UI;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public static class TEngineUISettingsProvider  
{
    private static bool _show = true;
    private static ReorderableList _reorderableList;

    [MenuItem("TEngine/Settings/TEngineUISettings", priority = -1)]
    public static void OpenSettings() => SettingsService.OpenProjectSettings("Project/TEngine/UISettings");
    
    private const string SettingsPath = "Project/TEngine/UISettings";  

    [SettingsProvider]  
    public static SettingsProvider CreateMySettingsProvider()  
    {  
        return new SettingsProvider(SettingsPath, SettingsScope.Project)  
        {  
            label = "TEngine/UISettings",  
            guiHandler = (searchContext) =>  
            {  
                var scriptGeneratorSetting = ScriptGeneratorSetting.Instance;  
                var scriptGenerator = new SerializedObject(scriptGeneratorSetting);
                var useBindComponent = scriptGenerator.FindProperty("useBindComponent");
                EditorGUILayout.PropertyField(useBindComponent);

                if (useBindComponent.boolValue)
                {
                    var codePath = scriptGenerator.FindProperty("_codePath");
                    codePath.stringValue =
                        DrawFolderField("代码文件生成路径", String.Empty, codePath.stringValue); // "FolderOpened Icon"
                    var windowComponentSuffixName = scriptGenerator.FindProperty("windowComponentSuffixName");
                    windowComponentSuffixName.stringValue =
                        EditorGUILayout.TextField("窗体组件脚本后缀名", windowComponentSuffixName.stringValue);
                    var widgetComponentSuffixName = scriptGenerator.FindProperty("widgetComponentSuffixName");
                    widgetComponentSuffixName.stringValue =
                        EditorGUILayout.TextField("widget组件脚本后缀名", widgetComponentSuffixName.stringValue);
                }
                // EditorGUILayout.PropertyField(scriptGenerator.FindProperty("_codePath"));
                EditorGUILayout.PropertyField(scriptGenerator.FindProperty("_namespace"));  
                EditorGUILayout.PropertyField(scriptGenerator.FindProperty("_widgetName"));  
                EditorGUILayout.PropertyField(scriptGenerator.FindProperty("CodeStyle"));
                DrawReorderableList(scriptGenerator);
                scriptGenerator.ApplyModifiedProperties();
            },  
            keywords = new[] { "TEngine", "Settings", "Custom" }  
        };  
    }

    private static string DrawFolderField(string label, string labelIcon, string path)
    {
        using var horizontalScope = new EditorGUILayout.HorizontalScope();

        var buttonGUIContent = new GUIContent("选择", EditorGUIUtility.IconContent("Folder Icon").image);

        if (!string.IsNullOrEmpty(labelIcon))
        {
            var labelGUIContent = new GUIContent(" " + label, EditorGUIUtility.IconContent(labelIcon).image);
            path = EditorGUILayout.TextField(labelGUIContent, path);
        }
        else
        {
            path = EditorGUILayout.TextField(label, path);
        }

        if (GUILayout.Button(buttonGUIContent, GUILayout.Width(60), GUILayout.Height(20)))
        {
            var newPath = EditorUtility.OpenFolderPanel(label, Application.dataPath, string.Empty);

            if (!string.IsNullOrEmpty(newPath) && newPath.StartsWith(Application.dataPath))
            {
                path = "Assets" + newPath.Substring(Application.dataPath.Length);
            }
            else
            {
                Debug.LogError("路径不在Unity项目内: " + newPath);
            }
        }

        return path;
    }

    private static void DrawReorderableList(SerializedObject serializedObject)
    {
        SerializedProperty ruleListProperty = serializedObject.FindProperty("scriptGenerateRule");
        if (ruleListProperty == null) return;

        _show = EditorGUILayout.BeginFoldoutHeaderGroup(_show, "scriptGenerateRule");

        if (_show)
        {
            if (_reorderableList == null)
            {
                _reorderableList = new ReorderableList(serializedObject, ruleListProperty, true, true, true, true);

                _reorderableList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Script Generate Rules"); };

                _reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    // 开始检查字段修改
                    EditorGUI.BeginChangeCheck();
                    SerializedProperty element = ruleListProperty.GetArrayElementAtIndex(index);

                    rect.y += 2;
                    float fieldHeight = EditorGUIUtility.singleLineHeight;

                    float fieldWidth = (rect.width - 10) / 4f;
                    SerializedProperty regexProperty = element.FindPropertyRelative("uiElementRegex");

                    Rect titleRect = new Rect(rect.x, rect.y, fieldWidth, fieldHeight);
                    EditorGUI.LabelField(titleRect, regexProperty.stringValue);
                    Rect regexRect = new Rect(rect.x + fieldWidth, rect.y, fieldWidth, fieldHeight);
                    EditorGUI.PropertyField(regexRect, regexProperty, GUIContent.none);
                    Rect componentRect = new Rect(rect.x + fieldWidth * 2 + 5, rect.y, fieldWidth, fieldHeight);
                    SerializedProperty componentProperty = element.FindPropertyRelative("componentName");
                    EditorGUI.PropertyField(componentRect, componentProperty, GUIContent.none);
                    Rect widgetRect = new Rect(rect.x + fieldWidth * 3 + 20, rect.y, fieldWidth, fieldHeight);
                    SerializedProperty widgetProperty = element.FindPropertyRelative("isUIWidget");
                    EditorGUI.PropertyField(widgetRect, widgetProperty, GUIContent.none);
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                        EditorUtility.SetDirty(serializedObject.targetObject);
                        AssetDatabase.SaveAssets();
                    }
                };

                _reorderableList.elementHeight = EditorGUIUtility.singleLineHeight + 6;
                _reorderableList.onChangedCallback = (ReorderableList list) =>
                {
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(serializedObject.targetObject);
                };
                _reorderableList.onAddCallback = (ReorderableList list) =>
                {
                    list.serializedProperty.arraySize++;
                    list.index = list.serializedProperty.arraySize - 1;
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(serializedObject.targetObject);
                };
                _reorderableList.onRemoveCallback = (ReorderableList list) =>
                {
                    list.serializedProperty.DeleteArrayElementAtIndex(list.index);
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(serializedObject.targetObject);
                };
            }
            _reorderableList.DoLayoutList();
            if (serializedObject.ApplyModifiedProperties())
            {
                EditorUtility.SetDirty(serializedObject.targetObject);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space();
    }
}  