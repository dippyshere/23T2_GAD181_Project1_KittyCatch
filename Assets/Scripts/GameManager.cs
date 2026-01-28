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
    public CountdownManager countdownManager;
    public GameObject confettiPrefab;
    public GameObject explosionPrefab;
    public GameObject deathPrefab;
    public bool isInfiniteGame;

    private float timer;
    public bool isGameOver;

    private void Start()
    {
        timer = gameTime;
        isGameOver = false;
        Invoke(nameof(SetInitialTimer), 0.1f);
    }

    private void SetInitialTimer()
    {
        timer = isInfiniteGame ? 0f : gameTime;
    }

    private void FixedUpdate()
    {
        if (isGameOver)
        {
            return;
        }

        if (isInfiniteGame)
            timer += Time.deltaTime;
        else
            timer -= Time.deltaTime;
        UpdateTimerText();

        if (!isInfiniteGame && timer <= 0f)
        {
            OutOfTime();
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
        countdownManager.countdownText.gameObject.SetActive(false);
        Instantiate(deathPrefab, basketController.transform.position + new Vector3(0f, -1.85f, 0f), Quaternion.Euler(-90f, 0f, 0f));
        if (isInfiniteGame)
        {
            Instantiate(confettiPrefab, transform);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OutOfTime()
    {
        isGameOver = true;
        outOfTimePanel.SetActive(true);
        quitButton.SetActive(true);
        replayButton.SetActive(true);
        basketController.isMoving = false;
        countdownManager.countdownText.gameObject.SetActive(false);
        Time.timeScale = 0f;
        Instantiate(confettiPrefab, transform);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

