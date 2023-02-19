using UnityEngine;

/// <summary>
/// ���ΰ� ĳ������ �پ��� component���� ��Ƴ��� class
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
