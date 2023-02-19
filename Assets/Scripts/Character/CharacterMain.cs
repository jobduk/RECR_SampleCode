using UnityEngine;

/// <summary>
/// 주인공 캐릭터의 다양한 component들을 모아놓는 class
/// </summary>
public class CharacterMain : MonoBehaviour
{
    public MoveController moveController;
    public Nav2DController nav2DController;
    public AnimatorController animatorController;
    public CustomizationController customizationController;

    public void Init()
    {
        moveController.LoadPosition();
        customizationController.Init();
    }
}
