using System.Collections;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 가구 설치 방법을 설명해주는 튜토리얼을 보여주는 class
/// </summary>
public class TutorialFurniture : MonoBehaviour
{
    public GameObject tutorialFingerObject;
    public Transform tutorialFingerTransform;
    public Animator tutorialFingerAnimator;

    bool isOn;

    public BoolSaver isSeeTutorialFurnitureSaver = new BoolSaver("isSeeTutorialFurniture");

    public void TryPlayTutorial()
    {
        if (!isSeeTutorialFurnitureSaver.GetValue())
            OnTutorialPanel();
    }

    void OnTutorialPanel()
    {
        isOn = true;
        gameObject.SetActive(true);
        tutorialFingerAnimator.ResetTrigger("DoClick");
        tutorialFingerObject.SetActive(true);
        targetFurniture = BuildManager.Instance.furnitureList[0];
        StartCoroutine(TutorialRoutine());
    }

    void OffTutorialPanel()
    {
        isOn = false;
        gameObject.SetActive(false);
        tutorialFingerObject.SetActive(false);
        targetFurniture.thisObject.SetActive(true);
        DisableTutorialFurniture();
        trashCan.DoOff(true);
    }


    Furniture targetFurniture;
    IEnumerator TutorialRoutine()
    {
        while (true)
        {
            yield return GoToTrashCan();
        }
    }

    float tutorialFingerSpeed = 180f;

    public TrashCan trashCan;
    Furniture tutorialFurniture;

    IEnumerator GoToTrashCan()
    {
        targetFurniture.thisObject.SetActive(true);
        targetFurniture.gameObject.SetActive(true);
        tutorialFingerAnimator.Play("Idle");
        tutorialFingerTransform.position = targetFurniture.thisTransform.position;

        yield return new WaitForSeconds(0.5f);
        tutorialFingerAnimator.SetTrigger("DoClick");
        yield return new WaitForSeconds(0.5f);
        trashCan.DoOn();

        targetFurniture.thisObject.SetActive(false);

        tutorialFurniture = targetFurniture.DoPop<Furniture>();
        tutorialFingerTransform.position = targetFurniture.thisTransform.position;
        tutorialFurniture.thisTransform.position = targetFurniture.thisTransform.position;
        tutorialFurniture.SetPreview();
        tutorialFurniture.sortingGroup.sortingOrder = 2;
        tutorialFurniture.gameObject.SetActive(true);

        tutorialFurniture.thisTransform.DOMove(trashCan.transform.position + new Vector3(0, -48f, 0), tutorialFingerSpeed).
            SetSpeedBased().SetEase(Ease.Linear);
        yield return tutorialFingerTransform.DOMove(trashCan.transform.position + new Vector3(0, -48f, 0), tutorialFingerSpeed).
            SetSpeedBased().SetEase(Ease.Linear).WaitForCompletion();

        trashCan.CoverOpen();
        yield return new WaitForSeconds(0.5f);
        tutorialFingerAnimator.Play("Idle");
        trashCan.DoOff();
        trashCan.CoverClose();
        DisableTutorialFurniture();
        yield return new WaitForSeconds(0.5f);

        yield break;
    }

    void DisableTutorialFurniture()
    {
        if (tutorialFurniture != null)
        {
            tutorialFurniture.PushThis();
            trashCan.CoverClose();
        }
    }

    public void ClickYesButton()
    {
        if (isOn)
        {
            isSeeTutorialFurnitureSaver.SetValue(true);
            OffTutorialPanel();
        }
    }
}