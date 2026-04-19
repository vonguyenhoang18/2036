using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject mask;
    [SerializeField] private Transform character;
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private ParticleSystem healingEffect;

    private bool _isDamaged = false;
    private float _drainTimer = 0f;
    private float _damagedTimer = 0f;

    private AudioManager _audioManager => GameManager.Instance.AudioManager;
    private LevelManager _levelManager => GameManager.Instance.LevelManager;
    private InventoryManager _inventoryManager => GameManager.Instance.InventoryManager;

    public float CurrentHp { get; private set; }
    public bool IsMaskOn { get; private set; }

    public void CharacterInit()
    {
        IsMaskOn = false;
        _isDamaged = false;
        _drainTimer = 0f;
        _damagedTimer = 0f;
        transform.position = Vector3.zero;
        

        SetMaskState();
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
    }

    private void SetHp(float hp)
    {
        CurrentHp = Mathf.Clamp(hp, 0, GameConstant.MAX_HP);
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
            if (!_audioManager.IsPlaying(AudioType.s_walking))
                _audioManager.PlayLoopSound(AudioType.s_walking);
        }
        else
        {
            // Stop when idle
            _audioManager.StopSound(AudioType.s_walking);
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
        CurrentHp = Mathf.Clamp(CurrentHp + amount, 0, GameConstant.MAX_HP);
        if (CurrentHp == 0)
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

    public void PlayHealingEffect()
    {
        _audioManager.PlaySound(AudioType.s_healing);
        healingEffect.Play();
    }

    public void TriggerWinLevel()
    {
        _levelManager.WinLevel();
    }

    private void TriggerLoseLevel()
    {
        _levelManager.LoseLevel();
    }

    public void AddItemCount()
    {
        _audioManager.PlaySound(AudioType.s_pickUp);
        _inventoryManager.AddItemCount();
    }
}
