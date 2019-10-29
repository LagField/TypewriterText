using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(TypewriterText))]
public class TypewriterTextEditor : GraphicEditor
{
    SerializedProperty m_Text;
    SerializedProperty m_FontData;
    private SerializedProperty m_VisibleCharacterCount;
    private SerializedProperty m_DisplayType;
    private SerializedProperty m_HeadCharSmoothIndex;
    private SerializedProperty m_FadeCharLength;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_Text = serializedObject.FindProperty("m_Text");
        m_FontData = serializedObject.FindProperty("m_FontData");
        m_VisibleCharacterCount = serializedObject.FindProperty("m_VisibleCharacterCount");
        m_DisplayType = serializedObject.FindProperty("m_DisplayType");
        m_HeadCharSmoothIndex = serializedObject.FindProperty("m_HeadCharSmoothIndex");
        m_FadeCharLength = serializedObject.FindProperty("m_FadeCharLength");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_DisplayType);
        EditorGUILayout.PropertyField(m_Text);
        
        if (m_DisplayType.enumValueIndex == 0)
        {
            EditorGUILayout.PropertyField(m_VisibleCharacterCount);
        }
        else
        {
            EditorGUILayout.PropertyField(m_HeadCharSmoothIndex);
            EditorGUILayout.PropertyField(m_FadeCharLength);
        }
        EditorGUILayout.PropertyField(m_FontData);

        AppearanceControlsGUI();
        RaycastControlsGUI();
        serializedObject.ApplyModifiedProperties();
    }
}
