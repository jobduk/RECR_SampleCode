using UnityEngine;
using System.Reflection;
using UnityEditor;

/// <summary>
/// InspectorButtonAttribute의 기능을 구현한 class
/// </summary>
[CustomEditor(typeof(Object), true, isFallback = false)]
[CanEditMultipleObjects]
public class InspectorButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach (object target in targets)
        {
            var methodInfos = target.GetType().GetMethods();

            foreach (var methosInfo in methodInfos)
            {
                var attribute = methosInfo.GetCustomAttribute<InspectorButtonAttribute>();
                if (attribute != null)
                {
                    string buttonName;
                    if (string.IsNullOrEmpty(attribute.buttonName))
                        buttonName = methosInfo.Name;
                    else
                        buttonName = attribute.buttonName;

                    if (GUILayout.Button(buttonName))
                        methosInfo.Invoke(target, null);
                }
            }
        }
    }
}