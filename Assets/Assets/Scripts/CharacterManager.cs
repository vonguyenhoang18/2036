using System.Collections;
using System.Runtime.CompilerServices;
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
    public Transform Character => character;
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private ParticleSystem healingEffect;

    private bool _isDamaged = false;
    private float _drainTimer = 0f;
    private float _damagedTimer = 0f;
    private bool _isPaused = true;
    private bool _isSafeZone = false;

    public bool IsPaused => _isPaused;
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
        if (_isPaused) return;
        if (_isSafeZone) return;

        DrainHpOverTime();
        DamageOverTime();
    }

    public void Init()
    {
        IsMaskOn = true;
        _isDamaged = false;
        _drainTimer = 0f;
        _damagedTimer = 0f;
        character.position = Vector3.zero;
        

        SetMaskState();
        UIManager.Instance.DangerZonePanel.UpdateMaskState(IsMaskOn, true);
        SetHp(GameConstant.MAX_HP);
        ChangeDirection(Direction.Right);

        SetPause(false);
    }

    public void ChangeMaskState(bool state)
    {
        IsMaskOn = state;
        SetMaskState();
    }

    private void SetMaskState()
    {
        mask.SetActive(IsMaskOn);
    }

    public void ChangeMaskState()
    {
        AudioManager.Instance.PlaySound(AudioType.s_maskChange);
        IsMaskOn = !IsMaskOn;
        SetMaskState();
        UIManager.Instance.DangerZonePanel.UpdateMaskState(IsMaskOn, false);
    }

    private void SetHp(float hp)
    {
        CurrentHp = Mathf.Clamp(hp, 0, GameConstant.MAX_HP);
        UIManager.Instance.DangerZonePanel.UpdateHealthBar(CurrentHp / GameConstant.MAX_HP);
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
            if (!AudioManager.Instance.IsLoopPlaying(AudioType.s_walking))
                AudioManager.Instance.PlayLoopSound(AudioType.s_walking);
        }
        else
        {
            // Stop when idle
            AudioManager.Instance.StopLoopSound(AudioType.s_walking);
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
                AudioManager.Instance.PlaySound(AudioType.s_hurt);
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
            GameObject go = UIManager.Instance.ShowPopup(Popup.Result);
            go.GetComponent<PopupResult>().ShowResult(false);
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

    public void DamageOnce()
    {
        if (_isSafeZone) return;

        AddHp(-GameConstant.DAMAGE_AMOUNT);
        AudioManager.Instance.PlaySound(AudioType.s_hurt);
        StartCoroutine(DamageFlashRoutine());
    }

    private IEnumerator DamageFlashRoutine()
    {
        characterRenderer.color = Color.red;
        float elapsed = 0f;
        while (elapsed < GameConstant.DAMAGE_INTERVAL)
        {
            elapsed += Time.deltaTime;
            characterRenderer.color = Color.Lerp(Color.red, Color.white, elapsed / GameConstant.DAMAGE_INTERVAL);
            yield return null;
        }
        characterRenderer.color = Color.white;
    }

    public void Knockback(Vector2 direction)
    {
        StartCoroutine(KnockbackRoutine(direction));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction)
    {
        SetPause(true);
        Vector3 startPos = character.position;
        Vector3 endPos = startPos + (Vector3)(direction * GameConstant.KNOCKBACK_DISTANCE);
        float elapsed = 0f;

        while (elapsed < GameConstant.KNOCKBACK_DURATION)
        {
            elapsed += Time.deltaTime;
            character.position = Vector3.Lerp(startPos, endPos, elapsed / GameConstant.KNOCKBACK_DURATION);
            yield return null;
        }

        character.position = endPos;
        SetPause(false);
    }

    public void UseHealing()
    {
        if (InventoryManager.Instance.CanUseMedkit())
        {
            AudioManager.Instance.PlaySound(AudioType.s_healing);
            InventoryManager.Instance.UseMedkit();
            healingEffect.Play();

            AddHp(GameConstant.HEALING_AMOUNT);
        }
    }

    public void AddItemCount()
    {
        AudioManager.Instance.PlaySound(AudioType.s_pickUp);
        InventoryManager.Instance.AddMedkit();
    }

    public void SetPause(bool state)
    {
        _isPaused = state;
        if (_isPaused)
        {
            ChangeSpeed(0, 0);
        }
    }

    public void SetSafeZone(bool state)
    {
        _isSafeZone = state;
        if (_isSafeZone)
        {
            SetDamagedState(false);
        }
    }
}
