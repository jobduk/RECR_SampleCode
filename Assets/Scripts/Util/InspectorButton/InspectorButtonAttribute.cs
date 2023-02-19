using System;

/// <summary>
/// Inspector���� �޼ҵ带 ��ư���� �����ų �� �ְ� ������ִ� class
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class InspectorButtonAttribute : Attribute
{
    public string buttonName;

    public InspectorButtonAttribute()
    {
        buttonName = "";
    }

    public InspectorButtonAttribute(string targetName)
    {
        buttonName = targetName;
    }
}