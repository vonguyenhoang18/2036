using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance
    {
        get { return instance; }
    }

    private static InventoryManager instance = null;

    private int _medkitCount = 0;

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
        UIManager.Instance.DangerZonePanel.UpdateMedkitCount(_medkitCount);
    }

    public void UseMedkit()
    {
        if (_medkitCount > 0)
        {
            _medkitCount--;
            UIManager.Instance.DangerZonePanel.UpdateMedkitCount(_medkitCount);
        }
    }

    public bool CanUseMedkit()
    {
        return _medkitCount > 0;
    }

    public void SetMedKit(int value)
    {
        _medkitCount = value;
    }
}
