using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupExitSafeZone : MonoBehaviour
{
    public void OnYesBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        UIManager.Instance.HidePopup();
        MapManager.Instance.InitDangerZoneMap();
        CharacterManager.Instance.SetPause(false);
    }

    public void OnNoBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        UIManager.Instance.HidePopup();
        CharacterManager.Instance.SetPause(false);
    }
}
