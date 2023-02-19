using UnityEngine;
using DG.Tweening;

/// <summary>
/// Customization UI 패널의 아이템 아이콘들을 보여주고 연출의 기능을 담당하는 class
/// </summary>
public class CustomItemPanelUI : MonoBehaviour
{
    public enum CustomItemPanelType 
    {
        Room,
        Character
    }
    public CustomItemPanelType panelType;

    public TitleController titleController;

    public Canvas panelCanvas;
    public RectTransform panelRectTransform;

    float baseAniTime = 0.2f;

    public void Init()
    {
        RenewalCategoryButtons();
        SetTargetCategoryButton(categoryButtons[0]);
    }

    public void PlayOnAnimation()
    {
        var charMain = titleController.characterMain;
        var camController = titleController.cameraController;

        charMain.moveController.isMoveLock = true;

        panelCanvas.sortingOrder = 1;
        panelCanvas.gameObject.SetActive(true);

        titleController.customRoomButtonRectTransform.DOAnchorPosY(-183, baseAniTime).SetRelative();
        titleController.customCharButtonRectTransform.DOAnchorPosY(-183, baseAniTime).SetRelative();

        float uiMoveTime = baseAniTime + 0.2f;

        titleController.BackButtonOn(uiMoveTime, 0, Ease.OutBack);
        panelRectTransform.anchoredPosition = Vector3.down * 136;
        panelRectTransform.DOAnchorPosY(0, uiMoveTime).SetEase(Ease.OutBack);
        
        charMain.moveController.SetMoveToEnd();
        charMain.animatorController.AnimationDirSet(Vector3.down);

        if (panelType == CustomItemPanelType.Character)
        {
            float camMoveTime = (baseAniTime + 0.4f);
            camController.camTransform.DOLocalMove(Vector3.back * 10, camMoveTime);
            camController.thisTransform.DOMove(charMain.transform.position, camMoveTime);
            camController.DoSetZoom(2, camMoveTime);
        }
        else
        {
            float camMoveTime = baseAniTime + 0.25f;
            camController.thisTransform.DOMove(Vector3.down * 18, camMoveTime);
        }

        RenewalItemIcons();
    }

    public void PlayOffAnimation()
    {
        var camController = titleController.cameraController;

        panelCanvas.sortingOrder = -1;

        float uiMoveTime = baseAniTime + 0.2f;

        panelRectTransform.DOAnchorPosY(-136, uiMoveTime).SetEase(Ease.InBack).OnComplete(() => { panelCanvas.gameObject.SetActive(false); });
        titleController.BackButtonOff(uiMoveTime, 0, Ease.InBack);

        if (panelType == CustomItemPanelType.Character)
        {
            float camMoveTime = baseAniTime + 0.4f;
            camController.camTransform.DOLocalMove(new Vector3(-9, 0, -10), camMoveTime).SetEase(Ease.OutQuad);
            camController.thisTransform.DOMove(Vector3.zero, camMoveTime).SetEase(Ease.OutQuad);
            camController.DoSetZoom(1, camMoveTime, Ease.OutQuad);
        }
        else
        {
            float camMoveTime = uiMoveTime * 0.5f;
            camController.thisTransform.DOMove(Vector3.zero, uiMoveTime * 1f).SetDelay(uiMoveTime * 0.5f);
        }

        if (BuildManager.Instance.buildController.isBuildModeOn)
            BuildManager.Instance.buildController.BuildModeOff();
    }

    [System.NonSerialized]
    public CustomItemCategoryButton targetCategoryButton;

    public CustomItemCategoryButton[] categoryButtons;

    public CustomItemCategoryButton GetCategoryButtonByCustomType(CustomizationType customType)
    {
        for(int i=0;i<categoryButtons.Length; i++)
        {
            if (categoryButtons[i].customizationType == customType)
            {
                return categoryButtons[i];
            }
        }
        return null;
    }

    public void RenewalCategoryButtons()
    {
        for (int i =0;i< categoryButtons.Length; i++)
        {
            categoryButtons[i].Renewal();
        }
    }

    public void SetTargetCategoryButton(CustomItemCategoryButton categoryButton)
    {
        if (targetCategoryButton != null)
            targetCategoryButton.SetNotTarget();
        categoryButton.SetTarget();
        targetCategoryButton = categoryButton;
        RenewalItemIcons();
    }

    [System.NonSerialized]
    public Vector2 slotStartPosi = new Vector2(17, -14);
    [System.NonSerialized]
    public Vector2 slotIconGap = new Vector2(23, -23);
    public int xlength = 7;

    public PoolParent customItemIconPoolParent;
    public RectTransform targetIconTransform;

    public void RenewalItemIcons()
    {
        customItemIconPoolParent.ClearAll();
        int addCnt = 0;

        CustomizationType targetCustomType = targetCategoryButton.customizationType;

        for (int i = 0; i < DataDictionary.Instance.customTypeToItems[targetCustomType].Count; i++)
        {
            CustomItemIcon popCustomItemIcon = customItemIconPoolParent.DoPop<CustomItemIcon>();
            popCustomItemIcon.popIndex = addCnt++;
            popCustomItemIcon.SetCustomItem(DataDictionary.Instance.customTypeToItems[targetCustomType][i]);
            popCustomItemIcon.thisObject.SetActive(true);

            if (popCustomItemIcon.CheckThisTarget())
            {
                SetTargetItemIcon(popCustomItemIcon);

                if (CustomizationItem.CheckFurniture(targetCategoryButton.customizationType)) 
                {
                    if(targetCategoryButton.customizationType == CustomizationType.Furniture_Ground)
                        BuildManager.Instance.buildController.BuildModeOn(GameManager.Instance.titleController.roomController.tilemapAreaInformation_Ground,
                            popCustomItemIcon.customizationItem.furniturePrefab);
                    else if (targetCategoryButton.customizationType == CustomizationType.Furinture_Wall)
                        BuildManager.Instance.buildController.BuildModeOn(GameManager.Instance.titleController.roomController.tilemapAreaInformation_Wall,
                            popCustomItemIcon.customizationItem.furniturePrefab);
                }
                else if (BuildManager.Instance.buildController.isBuildModeOn)
                    BuildManager.Instance.buildController.BuildModeOff();
            }
        }

        ScrollContentsSizeSet();
        GetCategoryButtonByCustomType(targetCustomType).Renewal();
    }

    [System.NonSerialized]
    public CustomItemIcon targetItemIcon;

    public void SetTargetItemIcon(CustomItemIcon customItemIcon)
    {
        targetItemIcon = customItemIcon;
        targetIconTransform.anchoredPosition = targetItemIcon.GetMyPosition();
        targetCategoryButton.selectedItemIdNumber = customItemIcon.customizationItem.myIdNumber;
    }

    public RectTransform scrollCotentsTransform;

    void ScrollContentsSizeSet()
    {
        var targetSize = scrollCotentsTransform.sizeDelta;
        float targetHeight;
        float slotGapY = slotIconGap.y * -1;
        if (customItemIconPoolParent.popList.Count == 0)
            targetHeight = 0;
        else
        {
            int lineCount;
            int divide = customItemIconPoolParent.popList.Count / xlength;
            int remain = customItemIconPoolParent.popList.Count % xlength;
            if (remain == 0)
                lineCount = divide;
            else
                lineCount = divide + 1;

            targetHeight = lineCount * slotGapY + 1;
            if (lineCount >= 4)
                targetHeight += 2;
        }
        targetSize.y = targetHeight;
        scrollCotentsTransform.sizeDelta = targetSize;
    }

    public TrashCan trashCan;
}
