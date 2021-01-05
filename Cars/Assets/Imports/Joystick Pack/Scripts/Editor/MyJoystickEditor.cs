using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MyJoystick))]
public class MyJoystickEditor : FloatingJoystickEditor
{
    [Space]
    private SerializedProperty ramTimer;
    private SerializedProperty firstTouchIndicator;
    private SerializedProperty firstRaiseIndicator;

    protected override void OnEnable()
    {
        base.OnEnable();
        ramTimer = serializedObject.FindProperty("_ramTimer");
        firstTouchIndicator =
            serializedObject.FindProperty("_firstTouchIndicator");
        firstRaiseIndicator =
            serializedObject.FindProperty("_firstRaiseIndicator");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    protected override void DrawValues()
    {
        base.DrawValues();
        EditorGUILayout.PropertyField(ramTimer,
            new GUIContent("Ram Timer", "Ram timer Rec Transform."));
        EditorGUILayout.PropertyField(firstTouchIndicator,
            new GUIContent("First Touch Indicator",
            "First touch indicator Transform."));
        EditorGUILayout.PropertyField(firstRaiseIndicator,
            new GUIContent("First Raise Indicator",
            "First raise indicator Transform."));
    }
}
