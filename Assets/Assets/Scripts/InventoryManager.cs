using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private int _medkitCount = 0;

    private UIManager _uiManager => GameManager.Instance.UIManager;

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
