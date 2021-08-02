using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] string scoreString = "SCORE ";
    [SerializeField] string highScoreString = "HIGH SCORE ";
    [SerializeField] Text score;
    [SerializeField] Text highScore;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject[] lives;

    public static LevelUI instance;
    

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

    }
    private void Start()
    {
        gameOver.SetActive(false);
        SetScore();
        SetHiScore();
        SetLives();
    }


    internal void SetScore()
    {
        this.score.text = string.Concat(scoreString, GameManager.instance.score.ToString());
    }

    internal void SetHiScore()
    {
        this.highScore.text = string.Concat(highScoreString, GameManager.instance.highScore.ToString());
    }

    internal void SetLives()
    {
        for (int i = 0; i < this.lives.Length; i++)
        {
            if (i < GameManager.instance.lives)
                this.lives[i].SetActive(true);
            else
                this.lives[i].SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }
}
