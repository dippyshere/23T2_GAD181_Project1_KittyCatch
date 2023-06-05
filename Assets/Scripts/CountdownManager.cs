using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float countdownDuration = 5f;

    private float countdownTimer;
    private bool isCountingDown;

    private void Start()
    {
        countdownTimer = countdownDuration;
        isCountingDown = true;
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (isCountingDown)
        {
            countdownTimer -= Time.unscaledDeltaTime;

            if (countdownTimer <= 0f)
            {
                countdownTimer = 0f;
                isCountingDown = false;
                Time.timeScale = 1f;
            }

            UpdateCountdownText();
        }
    }

    private void UpdateCountdownText()
    {
        int seconds = Mathf.CeilToInt(countdownTimer);
        if (seconds > 3)
        {
            countdownText.text = "Get Ready";
            return;
        }
        if (seconds == 0)
        {
            countdownText.text = "GO!";
            Destroy(this, 1f);
            return;
        }
        countdownText.text = seconds.ToString();
    }

    private void OnDestroy()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }    
    }
}

