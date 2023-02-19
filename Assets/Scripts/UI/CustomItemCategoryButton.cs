using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Customization UI에서 카테고리 버튼 기능을 담당하는 class
/// </summary>
public class CustomItemCategoryButton : MonoBehaviour, IPointerDownHandler
{
    public CustomItemPanelUI customItemPanelUI;

    public CustomizationType customizationType;

    public Image frameImage;
    public Sprite upSprite;
    public Sprite downSprite;

    public Image iconImage;
    public RectTransform iconRectTransform;

    Vector2 iconUpPosition = new Vector2(0, 1);
    Vector2 iconDownPosition = new Vector2(0, -1);

    [System.NonSerialized]
    public int selectedItemIdNumber = -1;

    public void Renewal()
    {
        SetIcon();
    }

    public void SetIcon()
    {        
        if (CustomizationItem.CheckCharacterCustomItem(customizationType))
            iconImage.sprite = GameManager.Instance.characterMain.customizationController.customTypeToEquipInfo[customizationType].GetEquipCustomItem().iconSprite;
        else
        {
            if (CustomizationItem.CheckTileItem(customizationType))
            {
                if(customizationType == CustomizationType.Tile)
                    iconImage.sprite = GameManager.Instance.titleController.roomController.tilemapAreaInformation_Ground.GetEquipCustomItem().iconSprite;
                else
                    iconImage.sprite = GameManager.Instance.titleController.roomController.tilemapAreaInformation_Wall.GetEquipCustomItem().iconSprite;
            }
            else
            {
                if (selectedItemIdNumber == -1)
                    selectedItemIdNumber = DataDictionary.Instance.customTypeToItems[customizationType][0].myIdNumber;
                iconImage.sprite = DataDictionary.Instance.customizationItems[selectedItemIdNumber].iconSprite;
            }
        }
    }

    public void SetTarget()
    {
        frameImage.sprite = downSprite;
        iconRectTransform.anchoredPosition = iconDownPosition;
    }

    public void SetNotTarget()
    {
        frameImage.sprite = upSprite;
        iconRectTransform.anchoredPosition = iconUpPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        customItemPanelUI.SetTargetCategoryButton(this);
    }
}
