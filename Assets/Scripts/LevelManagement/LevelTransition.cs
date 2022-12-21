using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    /// <summary>
    /// Level Transition
    /// Dylan Loe
    /// 
    /// Updated June 15, 2020
    /// 
    /// - When levels need to be changed
    /// </summary>
    
    public string nextLevel;

    public Player_Controller playerReference;

    public void loadLevel ()
    {
        SceneManager.LoadScene(nextLevel);
    }

    private void OnTriggerEnter(Collider other)
    {
        //This is the check if the player has hit the exit trigger AND if they have the cargo. If both are true, the player can load the next level
        if (other.tag == "Player")
        {
            if (playerReference.hasCargo == true)
            {
                loadLevel();
            }
            
            else
            {
                print("No Cargo for you");
            }
        }
    }
}
