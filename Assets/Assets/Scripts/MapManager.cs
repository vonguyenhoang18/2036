using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private DangerZone dangerZone;
    // [SerializeField] private SafeZone safeZone;
    
    public void InitDangerZoneMap()
    {
        dangerZone.InitMap();
    }

    public void InitSafeZoneMap()
    {
        
    }
}
