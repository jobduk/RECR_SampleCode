using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pooling을 관리해주는 class
/// </summary>
public class PoolParent : MonoBehaviour
{
    public Transform thisTransform;
    public PoolObjectBase poolSample;
    public List<PoolObjectBase> poolList;

    public virtual PoolObjectBase DoPop()
    {
        PoolObjectBase tmp;

        if (poolList.Count == 0)
        {
            tmp = Instantiate(poolSample, thisTransform);
        }
        else
        {
            tmp = poolList[poolList.Count-1];
            poolList.RemoveAt(poolList.Count - 1);
        }

        popList.Add(tmp);
        tmp.poolParent = this;
        return tmp;
    }

    public T DoPop<T>() where T : PoolObjectBase
    {
        return DoPop() as T;
    }

    public void DoPush(PoolObjectBase pushedObject)
    {
        pushedObject.thisTransform.SetParent(thisTransform);
        popList.Remove(pushedObject);
        poolList.Add(pushedObject);
    }

    public List<PoolObjectBase> popList = new List<PoolObjectBase>();

    public void ClearAll()
    {
        int cnt = popList.Count;
        for (int i = 0; i < cnt; i++)
        {
            popList[popList.Count - 1].PushThis();
        }
    }

    private void Reset()
    {
        thisTransform = transform;
        poolSample = GetComponentInChildren<PoolObjectBase>();
    }
}