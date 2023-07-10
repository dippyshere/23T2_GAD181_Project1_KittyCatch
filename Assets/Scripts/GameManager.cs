using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gameTime = 10.2f;
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;
    public GameObject outOfTimePanel;
    public GameObject quitButton;
    public GameObject replayButton;
    public GameObject retryButton;
    public BasketController basketController;

    private float timer;
    private bool isGameOver;

    private void Start()
    {
        timer = gameTime;
        isGameOver = false;
    }

    private void FixedUpdate()
    {
        if (!isGameOver)
        {
            timer -= Time.deltaTime;
            UpdateTimerText();

            if (timer <= 0f)
            {
                OutOfTime();
            }
        }
    }

    private void UpdateTimerText()
    {
        //if (timer > 10f)
        //{
        //    timerText.text = "10.0s";
        //    return;
        //}
        if (timer <= 10f)
        {
            timerText.text = timer.ToString("F2") + "s";
        }
        else
        {
            timerText.text = timer.ToString("F1") + "s";
        }
    }


    public void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        quitButton.SetActive(true);
        retryButton.SetActive(true);
        basketController.isMoving = false;
        Time.timeScale = 0f;
    }

    public void OutOfTime()
    {
        isGameOver = true;
        outOfTimePanel.SetActive(true);
        quitButton.SetActive(true);
        replayButton.SetActive(true);
        basketController.isMoving = false;
        Time.timeScale = 0f;
    }
}

