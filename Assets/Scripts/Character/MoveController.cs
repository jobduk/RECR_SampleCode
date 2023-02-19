using UnityEngine;

/// <summary>
/// 주인공 캐릭터의 이동 기능을 돕는 class
/// </summary>
public class MoveController : MonoBehaviour
{
    public Vector2Saver charPositionSaver = new Vector2Saver("charPositionSaver"); 
    public Nav2DController nav2DController;
    public GameObject flagObj;

    private void Awake()
    {
        nav2DController.moveDirEndAction += (Vector3 dir) => 
        {
            FlagOff(); 
            charPositionSaver.SetValue(transform.position); 
        };
        nav2DController.moveStartAction_TargetPosition += FlagOn;
    }

    public void LoadPosition()
    {
        transform.position = charPositionSaver.GetValue();
    }

    public bool isMoveLock;

    private void Update()
    {
        if (isMoveLock)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nav2DController.GetMoveCommand(clickPosition);
        }
    }
    
    void FlagOn(Vector3 clickPosition)
    {
        clickPosition.z = 0;
        flagObj.SetActive(true);
        flagObj.transform.position = clickPosition;
    }

    void FlagOff()
    {
        flagObj.SetActive(false);
    }

    public void SetMoveToEnd()
    {
        if (nav2DController.isMoving)
        {
            nav2DController.SetMoveToEnd();
        }
    }
}
