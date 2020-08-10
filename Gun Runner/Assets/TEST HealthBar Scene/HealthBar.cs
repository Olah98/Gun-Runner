using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    
   // public Slider slider;

    public Text healthText;


  
    public void SetHealth(int health)
    {
        //THIS SECTION IS FOR USING A SLIDER
        //Setting the value of the slider bar to the value of health
        //slider.value = health;


        healthText.text = "Health: " + health;
        //print("Im running");
    }

    public void SetMaxHealth (int health)
    {
        //THIS SECTION IS FOR USING A SLIDER
        //Sets the max health, then sets the slider to that value
        // slider.maxValue = health;
        //slider.value = health;


        healthText.text = "Health: " + health;
    }

    
}
