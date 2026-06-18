using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance
    {
        get { return instance; }
    }

    private static CharacterManager instance = null;

    [SerializeField] private GameObject mask;
    [SerializeField] private Transform character;
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private ParticleSystem healingEffect;

    private bool _isDamaged = false;
    private float _drainTimer = 0f;
    private float _damagedTimer = 0f;

    private UIManager _uiManager => UIManager.Instance;
    private MapManager _mapManager => MapManager.Instance;
    private AudioManager _audioManager => AudioManager.Instance;
    private InventoryManager _inventoryManager => InventoryManager.Instance;

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

    public float CurrentHp { get; private set; }
    public bool IsMaskOn { get; private set; }

    private void Update()
    {
        DrainHpOverTime();
    }

    public void Init()
    {
        IsMaskOn = false;
        _isDamaged = false;
        _drainTimer = 0f;
        _damagedTimer = 0f;
        character.position = Vector3.zero;
        

        SetMaskState();
        _uiManager.DangerZonePanel.UpdateMaskState(IsMaskOn);
        SetHp(GameConstant.MAX_HP);
        ChangeDirection(Direction.Right);
    }

    private void SetMaskState()
    {
        mask.SetActive(IsMaskOn);
    }

    public void ChangeMaskState()
    {
        _audioManager.PlaySound(AudioType.s_maskChange);
        IsMaskOn = !IsMaskOn;
        SetMaskState();
        _uiManager.DangerZonePanel.UpdateMaskState(IsMaskOn);
    }

    private void SetHp(float hp)
    {
        CurrentHp = Mathf.Clamp(hp, 0, GameConstant.MAX_HP);
        _uiManager.DangerZonePanel.UpdateHealthBar(CurrentHp / GameConstant.MAX_HP);
    }

    public void ChangeDirection(Direction direction)
    {
        Vector3 scale = characterRenderer.transform.localScale;
        switch (direction)
        {
            case Direction.Left:
                scale.x = -1f;
                break;
            case Direction.Right:
                scale.x = 1f;
                break;
        }
        characterRenderer.transform.localScale = scale;
    }

    public void ChangeSpeed(float x, float y)
    {
        Vector3 movement = new Vector3(x, y, 0f);
        movement = movement.normalized;
        character.position += movement * GameConstant.PLAYER_SPEED * Time.deltaTime;
        characterAnimator.SetFloat("Speed", movement.magnitude);
        if (movement.magnitude > 0)
        {
            // Only play if not already playing
            if (!_audioManager.IsLoopPlaying(AudioType.s_walking))
                _audioManager.PlayLoopSound(AudioType.s_walking);
        }
        else
        {
            // Stop when idle
            _audioManager.StopLoopSound(AudioType.s_walking);
        }
    }

    public void DrainHpOverTime()
    {
        _drainTimer += Time.deltaTime;
        if (_drainTimer >= GameConstant.DRAIN_INTERVAL)
        {
            if (IsMaskOn)
            {
                AddHp(-GameConstant.DRAIN_MASK_ON_AMOUNT);
            }
            else
            {
                AddHp(-GameConstant.DRAIN_MASK_OFF_AMOUNT);
            }
            _drainTimer -= 1f;
        }
    }

    public void DamageOverTime()
    {
        if (_isDamaged)
        {
            if (_damagedTimer == 0)
            {
                AddHp(-GameConstant.DAMAGE_OVER_TIME_AMOUNT);
                _audioManager.PlaySound(AudioType.s_hurt);
            }

            if (_damagedTimer >= GameConstant.DAMAGE_INTERVAL * 2f)
            {
                _damagedTimer = 0f;
            }
            _damagedTimer += Time.deltaTime;
            if (_damagedTimer < GameConstant.DAMAGE_INTERVAL)
            {
                characterRenderer.color = Color.red;
            }
            else
            {
                characterRenderer.color = Color.white;
            }
        }
    }

    public void AddHp(float amount)
    {
        float currentHp = Mathf.Clamp(CurrentHp + amount, 0, GameConstant.MAX_HP);
        SetHp(currentHp);
        if (currentHp == 0)
        {
            TriggerLoseLevel();
        }
    }

    public void SetDamagedState(bool state)
    {
        _isDamaged = state;
        if (!_isDamaged)
        {
            characterRenderer.color = Color.white;
            _damagedTimer = 0;
        }
    }

    public void UseHealing()
    {
        if (_inventoryManager.CanUseMedkit())
        {
            _audioManager.PlaySound(AudioType.s_healing);
            _inventoryManager.UseMedkit();
            healingEffect.Play();

            AddHp(GameConstant.HEALING_AMOUNT);
        }
    }

    public void TriggerWinLevel()
    {
        _mapManager.WinLevel();
    }

    private void TriggerLoseLevel()
    {
        _mapManager.LoseLevel();
    }

    public void AddItemCount()
    {
        _audioManager.PlaySound(AudioType.s_pickUp);
        _inventoryManager.AddMedkit();
    }
}
