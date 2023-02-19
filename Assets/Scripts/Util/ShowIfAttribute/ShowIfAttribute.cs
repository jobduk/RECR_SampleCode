using UnityEngine;

/// <summary>
/// condition이름 메소드의 return값이 true일때만 property를 보여주는 class
/// </summary>
public class ShowIfAttribute : PropertyAttribute
{
    public string condition;

    public ShowIfAttribute(string condition)
    {
        this.condition = condition;
    }
}