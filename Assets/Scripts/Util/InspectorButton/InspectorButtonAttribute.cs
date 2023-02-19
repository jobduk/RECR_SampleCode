using System;

/// <summary>
/// Inspector에서 메소드를 버튼으로 실행시킬 수 있게 만들어주는 class
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