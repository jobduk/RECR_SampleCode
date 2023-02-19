using UnityEngine;

/// <summary>
/// condition�̸� �޼ҵ��� return���� true�϶��� property�� �����ִ� class
/// </summary>
public class ShowIfAttribute : PropertyAttribute
{
    public string condition;

    public ShowIfAttribute(string condition)
    {
        this.condition = condition;
    }
}