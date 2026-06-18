using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance
    {
        get { return instance; }
    }

    private static InventoryManager instance = null;

    private int _medkitCount = 0;

    private UIManager _uiManager => UIManager.Instance;

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

    public void AddMedkit()
    {
        _medkitCount++;
        _uiManager.DangerZonePanel.UpdateMedkitCount(_medkitCount);
    }

    public void UseMedkit()
    {
        if (_medkitCount > 0)
        {
            _medkitCount--;
            _uiManager.DangerZonePanel.UpdateMedkitCount(_medkitCount);
        }
        else
        {
            Debug.Log("No medkits available to use.");
        }
    }

    public bool CanUseMedkit()
    {
        return _medkitCount > 0;
    }
}
