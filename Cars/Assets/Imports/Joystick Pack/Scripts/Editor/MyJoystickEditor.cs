using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MyJoystick))]
public class MyJoystickEditor : FloatingJoystickEditor
{
    [Space]
    private SerializedProperty ramTimer;

    protected override void OnEnable()
    {
        base.OnEnable();
        ramTimer = serializedObject.FindProperty("_ramTimer");
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
    }
}
