using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource bgmAudioSource;

    [Header("Volume Settings")]
    [Range(0, 1)] [SerializeField] float _maxBgmVolume = 1;
    [Range(0, 1)] public float _maxSfxVolume = 1;

    Coroutine fadeBGM;
    static SoundManager _instance;

    public float maxBgmVolume { get { return _maxBgmVolume; } }
    public static SoundManager instance { get { return _instance; } }

    public void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);

        SetBGMVolume(_maxBgmVolume);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Toca um Sound Effect
    /// </summary>
    /// <param name=""></param>
    public void PlaySFX(AudioClip clip,float volume = 1, float pitch = 1)
    {
        if (!clip)
            return;

        GameObject goTemp = new GameObject();
        AudioSource audioTemp = goTemp.AddComponent<AudioSource>();
        
        audioTemp.pitch = pitch;
        audioTemp.volume = _maxSfxVolume* volume;
        audioTemp.clip = clip;
        audioTemp.Play();
        Destroy(goTemp, clip.length + 0.1f);
    }

    public void PlayBGM(AudioClip audioClip, float volume = 1, float pitch = 1, bool loop = true)
    {
        if (fadeBGM != null)
            StopCoroutine(fadeBGM);

        if (audioClip)
        {
            if (audioClip == bgmAudioSource.clip && bgmAudioSource.loop && loop)
                return;

            bgmAudioSource.volume = _maxBgmVolume * volume;
            bgmAudioSource.pitch = pitch;
            bgmAudioSource.loop = loop;
            if (audioClip != bgmAudioSource.clip || !bgmAudioSource.loop)
            {
                bgmAudioSource.clip = audioClip;
                bgmAudioSource.Play();
            }
        }
        else
        {
            bgmAudioSource.clip = null;
            bgmAudioSource.Stop();
        }

    }

    internal void FadeBGM(float time)
    {
        if (fadeBGM != null)
            StopCoroutine(fadeBGM);
        fadeBGM = StartCoroutine(_FadeBGM(time));
    }

    private IEnumerator _FadeBGM(float time)
    {
        float decurredTime = 0;
        while(decurredTime < time)
        {
            bgmAudioSource.volume *= 0.95f;
            decurredTime += Time.deltaTime;
            yield return null;
        }
        bgmAudioSource.volume = 0;
    }

    /// <summary>
    /// Seta o volume máximo da BGM
    /// </summary>
    public void SetBGMVolume(float value)
    {
        _maxBgmVolume = value;
        bgmAudioSource.volume =  _maxBgmVolume;
    }
}
