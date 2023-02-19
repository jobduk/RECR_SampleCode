using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

/// <summary>
/// Customization에서 설치형 아이템을 제거할 때 사용되는 아이템의 기능을 담당하는 class
/// </summary>
public class TrashCan : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform rectTransform;

    public void DoOn(bool ins = false)
    {
        if (ins)
        {
            rectTransform.DOKill();
            rectTransform.anchoredPosition = Vector2.up * -24f;
        }
        else
            rectTransform.DOAnchorPosY(-24, 0.3f);
    }

    public void DoOff(bool ins = false)
    {
        if (ins)
        {
            rectTransform.DOKill();
            rectTransform.anchoredPosition = Vector2.up * 24f;
        }
        else
            rectTransform.DOAnchorPosY(24, 0.3f);
    }

    public RectTransform coverTransform;
    [System.NonSerialized]
    public bool coverOpen;
    float rotTime = 0.5f;
    public void OnPointerEnter(PointerEventData eventData)
    {
        CoverOpen();
    }

    public void CoverOpen()
    {
        coverOpen = true;
        coverTransform.DORotate(Vector3.forward * -105, rotTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CoverClose();
    }

    public void CoverClose()
    {
        coverOpen = false;
        coverTransform.DORotate(Vector3.zero, rotTime);
    }
}
