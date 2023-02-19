using UnityEngine;

/// <summary>
/// Pooling하고 싶은 Object에 사용되는 class
/// </summary>
public class PoolObjectBase : MonoBehaviour
{
    public PoolParent poolParent;
    public GameObject thisObject;
    public Transform thisTransform;

    public virtual void PushThis()
    {
        thisObject.SetActive(false);
        poolParent.DoPush(this);
    }

    public PoolObjectBase DoPop()
    {
        return poolParent.DoPop();
    }

    public T DoPop<T>() where T : PoolObjectBase
    {
        return DoPop() as T;
    }

    private void Reset()
    {
        thisObject = gameObject;
        thisTransform = transform;
        if (transform.parent != null)
            if (transform.parent.GetComponent<PoolParent>())
                poolParent = transform.parent.GetComponent<PoolParent>();
    }
}
