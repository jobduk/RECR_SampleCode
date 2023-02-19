using UnityEngine;

/// <summary>
/// 주인공 캐릭터의 애니메이션 기능을 돕는 class
/// </summary>
public class AnimatorController : MonoBehaviour
{
    public Nav2DController nav2DController;
    public Animator animator;

    private void Awake()
    {
        nav2DController.moveDirStartAction += MoveStartAction;
        nav2DController.moveDirEndAction += MoveEndAction;
        nav2DController.movingDirAction += AnimationDirSet;
    }

    string isMovingStr = "IsMoving";

    public void MoveStartAction(Vector3 vector3)
    {
        AnimationDirSet(vector3);
        animator.SetBool(isMovingStr, true);
    }

    public void MoveEndAction(Vector3 vector3)
    {
        animator.SetBool(isMovingStr, false);
    }

    public Transform xFlipTransform;

    public void AnimationDirSet(Vector3 vector3)
    {
        float targetX;
        float targetY;
        if (vector3.x > 0)
            targetX = 1;
        else if (vector3.x < 0)
            targetX = -1;
        else
            targetX = 0;

        if (vector3.y > 0)
            targetY = 1;
        else if (vector3.y < 0)
            targetY = -1;
        else
            targetY = 0;

        if(targetX > 0)
        {
            xFlipTransform.localEulerAngles = Vector3.up * 180f;
            targetX = -1;
        }
        else
        {
            xFlipTransform.localEulerAngles = Vector3.zero;
        }
        animator.SetFloat("X", targetX);
        animator.SetFloat("Y", targetY);
    }
}
