using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Pause Menu
    /// Dylan Loe
    /// 
    /// Updated June 15, 2020
    /// 
    /// - Player Pause Menu
    /// </summary>

    //to see if the game is paused or not
    public static bool GameIsPaused = false;

    //this is the UI that will show when the game is paused
    public GameObject pauseMenuUI;

    //so that you can change the Main Menu Screen (you will need to type in the name of the scene in the inspector)
    public string MainMenuScreen;

    // Update is called once per frame
    void Update()
    {
        //in order to pause the game, the player will press the ESC key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    //this is what happens when the game is resumed
    public void Resume()
    {
        //turn off the Pause Menu UI when the game is not paused
        pauseMenuUI.SetActive(false);

        //unfreezes the time
        Time.timeScale = 1f; //1f is normal time

        //makes sure to set that the game is not paused
        GameIsPaused = false;
    }
    //this is what happens when the game is paused
    void Pause()
    {
        //turn on the Pause Menu UI, as referenced above
        pauseMenuUI.SetActive(true);

        //freezes time in the game
        Time.timeScale = 0f;

        //references that the game is paused (bool)
        GameIsPaused = true;
    }
    //this is how you are taken back to the main menu from the pause screen
    public void MainMenu()
    {
        //displays this in the console when the button is clicked
        Debug.Log("Loading menu...");

        //makes sure that the game isn't still paused when you return to the menu
        Time.timeScale = 1f;

        //takes you to the menu when you click the button
        SceneManager.LoadScene(MainMenuScreen);
    }
    //this is how you quit the game from the pause menu
    public void QuitGame()
    {
        //displays this in the console when the button is clicked
        Debug.Log("Quitting game...");

        //this will quit the game from the pause screen
        Application.Quit();
    }
    //this is how you get to the feedback form from the pause menu
    public void FeedbackForm()
    {
        //displays this in the console when the button is clicked
        Debug.Log("Navigating to feedback form...");

        //this will take you to the feedback form from the pause screen
        Application.OpenURL("https://forms.gle/g3yeaueWbfiEmBze7");
    }
}
