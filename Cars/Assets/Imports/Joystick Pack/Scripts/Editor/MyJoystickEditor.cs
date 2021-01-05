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

    protected override void OnEnable()
    {
        base.OnEnable();
        ramTimer = serializedObject.FindProperty("_ramTimer");
        firstTouchIndicator = serializedObject.FindProperty("_firstTouchIndicator");
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
    }
}
