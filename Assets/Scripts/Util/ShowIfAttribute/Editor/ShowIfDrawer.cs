using UnityEditor;
using UnityEngine;
using System.Reflection;

/// <summary>
/// ShowIfAttribute의 기능을 구현한 class
/// </summary>
[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    bool CanShow(SerializedProperty property)
    {
        var conditionAttribute = (ShowIfAttribute)attribute;

        var targetType = property.serializedObject.targetObject.GetType();
        MethodInfo methodInfo = targetType.GetMethod(conditionAttribute.condition);
        var value = methodInfo.Invoke(property.serializedObject.targetObject, new object[] { });

        return (bool)value;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (CanShow(property))
            EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (CanShow(property))
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}