using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private static GameManager instance = null;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private InventoryManager inventoryManager;

    public UIManager UIManager => uiManager;

    public InputManager InputManager => inputManager;

    public MapManager MapManager => mapManager;

    public CharacterManager CharacterManager => characterManager;

    public AudioManager AudioManager => audioManager;
    public LevelManager LevelManager => levelManager;
    public InventoryManager InventoryManager => inventoryManager;

    void Awake()
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
