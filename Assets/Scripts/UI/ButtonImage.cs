using UnityEngine;

/// <summary>
/// Button�ȿ� �ִ� �̹����� ��ư�� �������� ���� ������ �� ó�� ���� �� �ְ� �����ִ� class
/// </summary>
public class ButtonImage : MonoBehaviour
{
    public RectTransform rectTransform;

    public Vector3 upPosition;
    public Vector3 downPosition;

    public void SetUp()
    {
        rectTransform.anchoredPosition = upPosition;
    }

    public void SetDown()
    {
        rectTransform.anchoredPosition = downPosition;
    }

    private void Reset()
    {
        rectTransform = GetComponent<RectTransform>();

        upPosition = rectTransform.anchoredPosition;
        downPosition = upPosition + Vector3.down * 3f;
    }
}