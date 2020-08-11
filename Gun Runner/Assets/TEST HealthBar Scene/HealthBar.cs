using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    
   // public Slider slider;

    //public Text healthText;

    public Sprite healthHeartFull; //4 health
    public Sprite healthHeartEmpty;
    public Sprite healthBarFull;
    public Sprite healthBarEmpty;

    public Image heart;
    public Image bar1;
    public Image bar2;
    public Image bar3;
    public Image bar4;
  
    public void SetHealth(int health)
    {
        //THIS SECTION IS FOR USING A SLIDER
        //Setting the value of the slider bar to the value of health
        //slider.value = health;

        //healthText.text = "Health: " + health;
        switch (health)
        {
            case 4:
                heart.sprite = healthHeartFull;
                bar1.sprite = healthBarFull;
                bar2.sprite = healthBarFull;
                bar3.sprite = healthBarFull;
                bar4.sprite = healthBarFull;
                break;
            case 3:
                heart.sprite = healthHeartFull;
                bar1.sprite = healthBarFull;
                bar2.sprite = healthBarFull;
                bar3.sprite = healthBarFull;
                bar4.sprite = healthBarEmpty;
                break;
            case 2:
                heart.sprite = healthHeartFull;
                bar1.sprite = healthBarFull;
                bar2.sprite = healthBarFull;
                bar3.sprite = healthBarEmpty;
                bar4.sprite = healthBarEmpty;
                break;
            case 1:
                heart.sprite = healthHeartFull;
                bar1.sprite = healthBarFull;
                bar2.sprite = healthBarEmpty;
                bar3.sprite = healthBarEmpty;
                bar4.sprite = healthBarEmpty;
                break;
            case 0:
                heart.sprite = healthHeartEmpty;
                bar1.sprite = healthBarEmpty;
                bar2.sprite = healthBarEmpty;
                bar3.sprite = healthBarEmpty;
                bar4.sprite = healthBarEmpty;
                break;
            default:
                break;
        }


        //print("Im running");
    }

    public void SetMaxHealth (int health)
    {
        //THIS SECTION IS FOR USING A SLIDER
        //Sets the max health, then sets the slider to that value
        // slider.maxValue = health;
        //slider.value = health;


        //healthText.text = "Health: " + health;
    }

    
}
