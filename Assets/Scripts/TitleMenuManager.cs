using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleMenuManager : MonoBehaviour
{
    public void Start()
    {
        if (Application.isMobilePlatform)
        {
            Application.targetFrameRate = Mathf.CeilToInt((float)Screen.currentResolution.refreshRateRatio.value);
        }
        else
        {
            Application.targetFrameRate = -1;
        }

        InputSystem.settings.SetInternalFeatureFlag("USE_OPTIMIZED_CONTROLS", true);
        InputSystem.settings.SetInternalFeatureFlag("USE_READ_VALUE_CACHING", true);
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("DemoLevel");
    }

    public void StartNewGamePlus()
    {
        SceneManager.LoadScene("NewGamePlusLevel");
    }

    public void QuitGame()
    {
        // Only works in standalone builds, not in the Unity Editor
        Application.Quit();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
