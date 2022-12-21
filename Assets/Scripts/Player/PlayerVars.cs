using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerVars : MonoBehaviour
{
    /// <summary>
    /// Player Weapon Loadouts Variables
    /// Dylan Loe
    /// 
    /// Updated June 15, 2020
    /// 
    /// - player weapon inventory
    /// </summary>
    
    private static PlayerVars _instance;//creats a ref in script

    public static PlayerVars Instance { get { return _instance; } }// allows anythin in the scene to ref this script

    public int health = 0;
    public int credits;

    [Header("Guns moving forward")]
    public WeaponInstance slot1 = new WeaponInstance();
    public WeaponInstance slot2 = new WeaponInstance();

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(this.gameObject); } else { _instance = this; }//destroys any player inventorys that are not the orignal one

        DontDestroyOnLoad(gameObject);//makes this stay acrossed level loads so vars stay consistant
    }

}
