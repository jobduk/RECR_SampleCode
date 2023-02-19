using UnityEngine;

/// <summary>
/// singleton 기능들을 구현해놓은 class
/// </summary>
public class MySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                {
                    return null;
                }
            }
            return instance;
        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }
}