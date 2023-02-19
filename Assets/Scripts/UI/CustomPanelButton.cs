using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 클릭하면 Customization UI를 띄워주고 Customization의 상태를 간단하게 보여주는 기능을 담당하는 class
/// </summary>
public class CustomPanelButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TitleController titleController;
    public ButtonImage buttonImage;

    public CustomItemPanelUI.CustomItemPanelType customItemPanelType;

    public Image upImg;
    public Image downImg;
    public Image backImg;

    public void Renewal()
    {
        if(customItemPanelType == CustomItemPanelUI.CustomItemPanelType.Character)
        {
            upImg.sprite = FindCharSprite(CustomizationType.Hair, "Hair_Down");
            backImg.sprite = FindCharSprite(CustomizationType.Hair, "HairBack_Down");
            downImg.sprite = FindCharSprite(CustomizationType.Cloth, "Down_0");
        }
        else
        {
            upImg.sprite = titleController.roomController.tilemapAreaInformation_Wall.GetEquipCustomItem().tileInformation.titleIconSprite;
            downImg.sprite = titleController.roomController.tilemapAreaInformation_Ground.GetEquipCustomItem().tileInformation.titleIconSprite;
        }
    }

    public void ClickThis()
    {
        if (customItemPanelType == CustomItemPanelUI.CustomItemPanelType.Character)
        {
            titleController.ClickButton_CustomCharacter();
        }
        else
        {
            titleController.ClickButton_CustomRoom();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonImage.SetDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.SetUp();
    }

    public Sprite FindCharSprite(CustomizationType customizationType , string spriteName)
    {
        SpriteCollector spriteCollector = titleController.characterMain.customizationController.customTypeToEquipInfo[customizationType].spriteChangers[0].mySpriteCollector;
        for (int i =0; i< spriteCollector.sprites.Length; i++)
        {
            if (spriteCollector.sprites[i].name == spriteName)
            {
                return spriteCollector.sprites[i];
            }
        }

        return null;
    }
}