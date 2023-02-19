using UnityEngine;

/// <summary>
/// Button안에 있는 이미지가 버튼이 눌렸을때 같이 눌리는 것 처럼 보일 수 있게 도와주는 class
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