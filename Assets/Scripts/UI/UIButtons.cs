using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    /// <summary>
    /// UI Behavior
    /// Dylan Loe
    /// 
    /// Updated June 15, 2020
    /// 
    /// - UI buttons for menus
    /// 
    /// </summary>

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void OpenHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void LoadTitleScreen()
    {
        SceneManager.LoadScene("HomeScreen");
    }
}
