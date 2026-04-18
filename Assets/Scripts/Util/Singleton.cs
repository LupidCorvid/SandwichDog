using UnityEngine;

// courtesy of https://unity.com/resources/level-up-your-code-with-game-programming-patterns
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static bool isDestroyed = false;

    private static T instance;
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = (T)FindFirstObjectByType(typeof(T));
                //if (instance)
                //{
                    //Debug.Log("found preexisting instance for " + typeof(T).Name);
                //}
                if (!instance && !isDestroyed)
                {
                    Debug.Log("Setup new instance of " + typeof(T).Name);
                    SetupInstance();
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        RemoveDuplicates();
    }

    private void OnDestroy()
    {
        isDestroyed = true;
        if (instance)
        {
            CleanupSingleton();
        }
    }

    /// <summary>
    /// Overrideable func so Singletons can clean up any of its own data from the scene if needed upon shutdown.
    /// </summary>
    protected virtual void CleanupSingleton()
    {

    }

    private static void SetupInstance()
    {
        instance = (T)FindFirstObjectByType(typeof(T));

        if (!instance)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = typeof(T).Name;
            instance = gameObj.AddComponent<T>();
            DontDestroyOnLoad(gameObj);
        }
    }

    private void RemoveDuplicates()
    {
        if (!instance)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Duplicate " + gameObject.name + " Singleton in scene.");
            Destroy(gameObject);
        }
    }
}