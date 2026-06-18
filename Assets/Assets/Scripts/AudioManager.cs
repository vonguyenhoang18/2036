using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AudioType
{
    //Sound
    s_click,
    s_healing,
    s_hurt,
    s_maskChange,
    s_pickUp,
    s_breathing,
    s_walking,
    s_winGame,
    s_loseGame,
    //Music
    m_gameplay,
    m_mainMenu,
    m_ending
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get { return instance; }
    }

    private static AudioManager instance = null;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource loopSource;

    [Header("Volume")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float soundVolume = 1f;

    [SerializeField] private AudioClip s_click;
    [SerializeField] private AudioClip s_healing;
    [SerializeField] private AudioClip s_hurt;
    [SerializeField] private AudioClip s_maskChange;
    [SerializeField] private AudioClip s_pickUp;
    [SerializeField] private AudioClip s_breathing;
    [SerializeField] private AudioClip s_walking;
    
    [SerializeField] private AudioClip s_winGame;
    [SerializeField] private AudioClip s_loseGame;
    
    [SerializeField] private AudioClip m_gameplay;
    [SerializeField] private AudioClip m_mainMenu;
    [SerializeField] private AudioClip m_ending;

    private bool _musicEnabled = true;
    private bool _soundEnabled = true;

    public bool MusicEnabled => _musicEnabled;

    public bool SoundEnabled => _soundEnabled;

    private const string MUSIC_KEY = "MUSIC_ENABLED";
    private const string SOUND_KEY = "SOUND_ENABLED";

    private Dictionary<string, AudioClip> _clipCache = new();

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    public void Init()
    {
        musicSource.loop = true;
        soundSource.loop = false;
        loopSource.loop = true;

        LoadSetting();
        ApplySetting();
    }

    /* ================= LOAD / APPLY ================= */

    private void LoadSetting()
    {
        _musicEnabled = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
        _soundEnabled = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
    }

    private void ApplySetting()
    {
        musicSource.mute = !_musicEnabled;
        soundSource.mute = !_soundEnabled;
        loopSource.mute = !_soundEnabled;
    }

    /* ================= SOUND ================= */

    public void PlaySound(AudioType type)
    {
        AudioClip clip = GetAudioClip(type);
        if (clip != null)
        {
            PlaySound(clip);
        }
    }

    public void PlayLoopSound(AudioType type)
    {
        AudioClip clip = GetAudioClip(type);
        if (clip != null)
        {
            PlayLoopSound(clip);
        }
    }

    public void StopLoopSound(AudioType type)
    {
        AudioClip clip = GetAudioClip(type);
        if (clip != null)
        {
            StopLoopSound(clip);
        }
    }

    public void StopSound(AudioType type)
    {
        AudioClip clip = GetAudioClip(type);
        if (clip != null)
        {
            StopSound(clip);
        }
    }

    public bool IsLoopPlaying(AudioType type)
    {
        AudioClip clip = GetAudioClip(type);
        if (clip != null)
        {
            return IsLoopPlaying(clip);
        }
        return false;
    }

    public void PlayWalkingSound()
    {

    }

    public void PlayMusic(AudioType type)
    {
        AudioClip clip = GetAudioClip(type);
        if (IsMusicPlaying(clip)) return;
        if (clip != null)
        {
            PlayMusic(clip);
        }
    }

    private AudioClip GetAudioClip(AudioType type)
    {
        switch (type)
        {
            case AudioType.s_click: return s_click;
            case AudioType.s_healing: return s_healing;
            case AudioType.s_hurt: return s_hurt;
            case AudioType.s_maskChange: return s_maskChange;
            case AudioType.s_pickUp: return s_pickUp;
            case AudioType.s_breathing: return s_breathing;
            case AudioType.s_walking: return s_walking;
            case AudioType.s_winGame: return s_winGame;
            case AudioType.s_loseGame: return s_loseGame;
            case AudioType.m_gameplay: return m_gameplay;
            case AudioType.m_mainMenu: return m_mainMenu;
            case AudioType.m_ending: return m_ending;
            default: return null;
        }
    }

    /* ================= FADING ================= */

    private IEnumerator FadeInMusic(AudioClip clip, float time)
    {
        musicSource.volume = 0f;
        musicSource.clip = clip;
        musicSource.Play();

        float t = 0f;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(0f, musicVolume, t / time);
            yield return null;
        }

        musicSource.volume = musicVolume;
    }

    private IEnumerator FadeOutMusic(float time)
    {
        float startVolume = musicSource.volume;
        float t = 0f;

        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / time);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = musicVolume;
    }

    /* ================= PLAYING ================= */

    public void PlaySound(AudioClip clip)
    {
        if (clip == null || !_soundEnabled) return;
        soundSource.PlayOneShot(clip, soundVolume);
    }

    public void PlayLoopSound(AudioClip clip)
    {
        if (clip == null || !_soundEnabled) return;
        loopSource.clip = clip;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void StopSound(AudioClip clip)
    {
        if (clip == null) return;
        if (soundSource.clip == clip && soundSource.isPlaying)
        {
            soundSource.Stop();
            soundSource.clip = null;
            soundSource.loop = false;
        }
    }

    public void StopLoopSound(AudioClip clip)
    {
        if (clip == null) return;
        if (loopSource.clip == clip && loopSource.isPlaying)
        {
            loopSource.Stop();
            loopSource.clip = null;
            loopSource.loop = false;
        }
    }

    public bool IsMusicPlaying(AudioClip clip)
    {
        return clip != null && musicSource.clip == clip;
    }

    public bool IsLoopPlaying(AudioClip clip)
    {
        return clip != null && loopSource.clip == clip && loopSource.isPlaying;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (_musicEnabled)
        {
            musicSource.volume = 1f;
            musicSource.clip = clip;
            musicSource.Play();
        }
        else
        {
            musicSource.volume = 0f;
            musicSource.Stop();
            musicSource.clip = null;
        }
    }

    /* ================= TOGGLE ================= */

    public void ToggleMusic(bool enable)
    {
        _musicEnabled = enable;
        musicSource.mute = !enable;
        PlayerPrefs.SetInt(MUSIC_KEY, enable ? 1 : 0);
    }

    public void ToggleSound(bool enable)
    {
        _soundEnabled = enable;
        soundSource.mute = !enable;
        loopSource.mute = !enable;
        PlayerPrefs.SetInt(SOUND_KEY, enable ? 1 : 0);
    }

    /* ================= VOLUME ================= */

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        musicSource.volume = musicVolume;
    }

    public void SetSoundVolume(float value)
    {
        soundVolume = Mathf.Clamp01(value);
        soundSource.volume = soundVolume;
        loopSource.volume = soundVolume;
    }
}
