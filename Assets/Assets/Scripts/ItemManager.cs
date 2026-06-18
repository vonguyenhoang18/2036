using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance
    {
        get { return instance; }
    }

    private static ItemManager instance = null;

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
