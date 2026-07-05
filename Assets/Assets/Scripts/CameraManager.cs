using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private bool _following;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void LateUpdate()
    {
        if (!_following) return;
        if (CharacterManager.Instance == null) return;
        Transform character = CharacterManager.Instance.Character;
        Vector3 pos = Camera.main.transform.position;
        pos.x = character.position.x;
        pos.y = character.position.y + 1f;
        Camera.main.transform.position = pos;
    }

    public void InitSafeZone()
    {
        _following = false;
        Vector3 pos = Camera.main.transform.position;
        pos.x = 0f;
        pos.y = 1f;
        Camera.main.transform.position = pos;
    }

    public void InitDangerZone()
    {
        _following = true;
    }
}
