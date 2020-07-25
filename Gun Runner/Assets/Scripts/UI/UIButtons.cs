using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        SceneManager.LoadScene("Tutorial Level");
    }

    public void LoadTitleScreen()
    {
        SceneManager.LoadScene("HomeScreen");
    }
}
