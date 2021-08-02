using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void GameManagerEvent();

    [SerializeField] int initialLives;
    [SerializeField] int dotScore;
    [SerializeField] int energyzerScore;
    [SerializeField] int killEnemyScore;
    [Header("Sounds")]
    [SerializeField] AudioClip levelStartSong;
    [SerializeField] AudioClip victorySong;
    [SerializeField] AudioClip[] munchSounds;
    [SerializeField] AudioClip eatGhostSound;
    [SerializeField] AudioClip[] deathSounds;

    int munchSoundIndex;
    int dots;
    string _playerTag = "Player";
    string dotTag = "Dot";
    string energyzerTag = "Energyzer";
    Enemy[] enemies;
    Coroutine deathCoroutine;

    public string playerTag => _playerTag;
    public int lives { get; private set; }
    public int score { get; private set; }
    public int highScore { get; private set; }

    public static GameManager instance;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        lives = initialLives;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SetScore(0);
        Initialize();
    }

    private void Initialize()
    {
        dots = GameObject.FindGameObjectsWithTag(dotTag).Length + GameObject.FindGameObjectsWithTag(energyzerTag).Length;
        enemies = FindObjectsOfType<Enemy>();
        StartCoroutine(StartLevel());
    }

    

    private void SetLives(int value)
    {
        lives = value;
        LevelUI.instance.SetLives();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Initialize();
    }

    IEnumerator StartLevel()
    {
        deathCoroutine = null;
        munchSoundIndex = 0;
        Player.instance.Initialize();
        foreach (var item in enemies)
            item.Initialize();

        bool fadeInEnd = false;
        ScreenFader.instance.FadeIn(.5f,()=> { fadeInEnd = true; });
        while (!fadeInEnd)
            yield return null;

        SoundManager.instance.PlayBGM(levelStartSong, 1, 1, false);
        yield return new WaitForSeconds(levelStartSong.length);

        Player.instance.StartLevel();
        foreach (var item in enemies)
            item.StartLevel();
    }
    public void LoseLife()
    {
        if(deathCoroutine == null)
            deathCoroutine = StartCoroutine(_LoseLife());
    }

    private IEnumerator _LoseLife()
    {
        foreach (var item in enemies)
            item.ToggleMovement(false);

        Player.instance.Kill();
        SoundManager.instance.PlaySFX(deathSounds[0]);
        yield return new WaitForSeconds(deathSounds[0].length);
        SoundManager.instance.PlaySFX(deathSounds[1]);
        Player.instance.Vanish();
        yield return new WaitForSeconds(deathSounds[1].length);

        bool canContinue = false;
        

        SetLives(lives-1);
        if (lives < 1)
        {
            LevelUI.instance.ShowGameOver();
            while (!Input.anyKey)
                yield return null;

            ScreenFader.instance.FadeOut(.5f, () => { canContinue = true; });

            while (!canContinue)
                yield return null;

            GameOver();

        }
        else
        {
            ScreenFader.instance.FadeOut(.5f, () => { canContinue = true; });

            while (!canContinue)
                yield return null;

            ResetLevel();
        }
    }

    private void ResetLevel()
    {
        StartCoroutine(StartLevel());
    }

    private void GameOver()
    {
        lives = initialLives;
        SetScore(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GetDot()
    {
        SetScore(score + dotScore);
        DecreaseDots();
        PlayMunchSound();
    }

    public void KillEnemy()
    {
        SetScore(score + killEnemyScore);
        SoundManager.instance.PlaySFX(eatGhostSound);
    }

    public void GetEnergyzer()
    {
        SetScore(score + energyzerScore);
        DecreaseDots();
        PlayMunchSound();
        foreach (var item in enemies)
        {
            item.Frighten();
        }
    }
    private void DecreaseDots()
    {
        dots--;
        if (dots <= 0)
            StartCoroutine(Victory());
    }

    private IEnumerator Victory()
    {
        for (int i = 0; i < enemies.Length; i++)
            Destroy(enemies[i].gameObject);

        Player.instance.Stop();

        SoundManager.instance.PlayBGM(victorySong, 1, 1, false);
        yield return new WaitForSeconds(victorySong.length);

        bool canContinue = false;
        ScreenFader.instance.FadeOut(.5f, () => { canContinue = true; });

        while (!canContinue)
            yield return null;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetScore(int value)
    {
        score = value;
        LevelUI.instance.SetScore();
        if (highScore <= score)
        {
            highScore = score;
            LevelUI.instance.SetHiScore();
        }
    }

    public void PlayMunchSound()
    {
        SoundManager.instance.PlaySFX(munchSounds[munchSoundIndex]);
        munchSoundIndex++;
        if (munchSoundIndex >= munchSounds.Length)
            munchSoundIndex = 0;

    }

}
