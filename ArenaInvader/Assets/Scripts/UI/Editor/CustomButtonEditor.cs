using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomButton))]
public class CustomButtonEditor : UnityEditor.UI.ButtonEditor {

    SerializedProperty _key;

    protected override void OnEnable()
    {
        _key = serializedObject.FindProperty("_soundKey");
        base.OnEnable();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_key);
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();

    }
}