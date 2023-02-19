using UnityEngine;
using DG.Tweening;

/// <summary>
/// Title에서 Object들의 상호작용을 돕는 class
/// </summary>
public class TitleController : MonoBehaviour
{
    public CameraController cameraController;
    public CharacterMain characterMain;
    public RoomController roomController;

    public enum TitleState
    {
        Main,
        CustomizationCharacter,
        CustomizationRoom
    }

    public TitleState titleState;

    public void Init()
    {
        titleState = TitleState.Main;
        customPanel_Character.Init();
        customPanel_Room.Init();
        GameManager.Instance.titleController.customPanelButton_Room.Renewal();
        GameManager.Instance.titleController.customPanelButton_Character.Renewal();
        roomController.Init();
    }

    public RectTransform customRoomButtonRectTransform;
    public RectTransform customCharButtonRectTransform;

    public CustomItemPanelUI customPanel_Character;
    public CustomItemPanelUI customPanel_Room;

    public void ClickButton_CustomCharacter()
    {
        titleState = TitleState.CustomizationCharacter;
        customPanel_Character.PlayOnAnimation();
    }

    public void ClickButton_CustomRoom()
    {
        titleState = TitleState.CustomizationRoom;
        customPanel_Room.PlayOnAnimation();
    }

    public GameObject backButtonGameObject;
    public RectTransform backButtonRectTransform;

    bool isBackButtonOn;

    public void BackButtonOn(float moveTime, float delayTime = 0, Ease ease = Ease.OutQuad)
    {
        isBackButtonOn = true;
        backButtonGameObject.SetActive(true);
        backButtonRectTransform.DOAnchorPosY(-24, moveTime).SetDelay(delayTime).SetEase(ease);
    }

    public void BackButtonOff(float moveTime, float delayTime = 0, Ease ease = Ease.OutQuad)
    {
        isBackButtonOn = false;
        backButtonRectTransform.DOAnchorPosY(64, moveTime).OnComplete(() => { if (!isBackButtonOn) backButtonGameObject.SetActive(false); }).SetEase(ease).SetDelay(delayTime);
    }

    float baseAniTime = 0.2f;

    public void ClickButton_Back()
    {
        if (!isBackButtonOn)
            return;

        if (titleState == TitleState.CustomizationCharacter)
        {
            customPanel_Character.PlayOffAnimation();
            characterMain.moveController.isMoveLock = false;
        }
        else if (titleState == TitleState.CustomizationRoom)
        {
            customPanel_Room.PlayOffAnimation();
            characterMain.moveController.isMoveLock = false;
        }

        float titleOnDelay = (baseAniTime + 0.2f)*0.5f;
        customRoomButtonRectTransform.DOAnchorPosY(-101, baseAniTime).SetDelay(titleOnDelay);
        customCharButtonRectTransform.DOAnchorPosY(-101, baseAniTime).SetDelay(titleOnDelay);

        titleState = TitleState.Main; ;
    }

    public CustomPanelButton customPanelButton_Room;
    public CustomPanelButton customPanelButton_Character;
}