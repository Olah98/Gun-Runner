using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerVars : MonoBehaviour
{
    private static PlayerVars _instance;//creats a ref in script

    public static PlayerVars Instance { get { return _instance; } }// allows anythin in the scene to ref this script

    public int health = 0;
    public int credits;

    [Header("Gun Data")]
    public WeaponData gunObj;

    [Header("Guns moving forward")]
    public WeaponInstance slot1 = new WeaponInstance();
    public WeaponInstance slot2 = new WeaponInstance();

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(this.gameObject); } else { _instance = this; }//destroys any player inventorys that are not the orignal one

        DontDestroyOnLoad(gameObject);//makes this stay acrossed level loads so vars stay consistant

    }

    private void Start()
    {
        if(FindObjectOfType<Player_Controller>().isActiveAndEnabled)
        {
            print("Found the Player");
            Player_Controller playerInstance = FindObjectOfType<Player_Controller>();
            if (health == 0) { health = playerInstance.maxHealth; }
            playerInstance.currentHealth = health;
            
        }
        
        if (FindObjectOfType<PlayerInventory>().isActiveAndEnabled)
        {
            print("Found the Player's Bag");
            PlayerInventory bagInstance = FindObjectOfType<PlayerInventory>();
            

            if (slot1.weapontype == weaponType.none)
            {
               slot1 = gunObj.FindWeapon(1);
               print("pistol set");
            }
            bagInstance.SetGun1(slot1);
            bagInstance.SetGun2(slot2);

        }
        
    }



}
