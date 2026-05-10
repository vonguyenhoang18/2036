using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    private UIManager _uiManager => GameManager.Instance.UIManager;
    private InputManager _inputManager => GameManager.Instance.InputManager;
    private MapManager _mapManager => GameManager.Instance.MapManager;
    private ItemManager _itemManager => GameManager.Instance.ItemManager;
    private CharacterManager _characterManager => GameManager.Instance.CharacterManager;
    private AudioManager _audioManager => GameManager.Instance.AudioManager;
    private InventoryManager _inventoryManager => GameManager.Instance.InventoryManager;

    public void OnStartBtn()
    {
        _audioManager.PlaySound(AudioType.s_click);
        _mapManager.InitDangerZoneMap();
    }

    public void OnTutorialBtn()
    {
        _audioManager.PlaySound(AudioType.s_click);
        _uiManager.SetTutorialPanel();
    }

    public void OnQuitBtn()
    {
        _audioManager.PlaySound(AudioType.s_click);
        Application.Quit();
    }

    public void OnSettingBtn()
    {
        _audioManager.PlaySound(AudioType.s_click);
        _uiManager.SetSettingPanel(true);
    }
}
