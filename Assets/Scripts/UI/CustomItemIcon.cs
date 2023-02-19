using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Customzation UI에서 아이템 아이콘의 기능을 담당하는 class
/// </summary>
public class CustomItemIcon : PoolObjectBase, IPointerDownHandler
{
    [System.NonSerialized]
    public int popIndex;
    [System.NonSerialized]
    public CustomizationItem customizationItem;

    public RectTransform myRectTransform;
    public CustomItemPanelUI customItemPanelUI;
    public Image iconImage;

    public void Renewal()
    {
        myRectTransform.anchoredPosition = GetMyPosition();
        iconImage.sprite = customizationItem.iconSprite;
    }

    public void SetCustomItem(CustomizationItem item)
    {
        customizationItem = item;
        Renewal();
    }

    public Vector2 GetMyPosition()
    {
        int targetIndex = popIndex;
        int y = targetIndex / customItemPanelUI.xlength;
        int x = targetIndex % customItemPanelUI.xlength;
        return customItemPanelUI.slotStartPosi + new Vector2(customItemPanelUI.slotIconGap.x * x, customItemPanelUI.slotIconGap.y * y);
    }

    public bool CheckThisTarget()
    {
        if (customizationItem.IsCharacterCustomItem())
        {
            if (GameManager.Instance.characterMain.customizationController.customTypeToEquipInfo[customizationItem.customizationType].GetEquipCustomItem() == customizationItem)
            {
                return true;
            }
        }
        else
        {
            if (customizationItem.IsTileItem())
            {
                if (GetTargetTilemapAreaInformation().GetEquipCustomItem() == customizationItem)
                    return true;
                else
                    return false;
            }
            else
            {
                if (customItemPanelUI.targetCategoryButton.selectedItemIdNumber == -1)
                    return true;
                else
                {
                    if (customItemPanelUI.targetCategoryButton.selectedItemIdNumber == customizationItem.myIdNumber)
                        return true;
                    else
                        return false;
                }
            }
        }
        return false;
    }

    public TilemapAreaInformation GetTargetTilemapAreaInformation()
    {
        if (customizationItem.customizationType == CustomizationType.Tile)
            return GameManager.Instance.titleController.roomController.tilemapAreaInformation_Ground;
        else
            return GameManager.Instance.titleController.roomController.tilemapAreaInformation_Wall;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        customItemPanelUI.SetTargetItemIcon(this);
        if (customizationItem.IsCharacterCustomItem())
        {
            GameManager.Instance.characterMain.customizationController.customTypeToEquipInfo[customizationItem.customizationType].SetEquipCustomItem(customizationItem);
            GameManager.Instance.titleController.customPanelButton_Character.Renewal();
        }
        else
        {
            if(customizationItem.IsTileItem())
                GetTargetTilemapAreaInformation().SetTile(customizationItem);
            else
            {
                GameManager.Instance.buildManager.buildController.ChangeTargetFurniture(customizationItem.furniturePrefab);
            }
            GameManager.Instance.titleController.customPanelButton_Room.Renewal();
        }
        customItemPanelUI.targetCategoryButton.Renewal();
    }
}