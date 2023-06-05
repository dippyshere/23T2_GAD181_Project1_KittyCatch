using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("DemoLevel");
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
