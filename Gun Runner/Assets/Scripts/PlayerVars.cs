using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerVars : MonoBehaviour
{
    private static PlayerVars _instance;//creats a ref in script

    public static PlayerVars Instance { get { return _instance; } }// allows anythin in the scene to ref this script

    public float health;
    public int credits;

    [Header("Guns moving forward")]
    public WeaponData slot1;
    public WeaponData slot2;

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(this.gameObject); } else { _instance = this; }//destroys any player inventorys that are not the orignal one

        DontDestroyOnLoad(gameObject);//makes this stay acrossed level loads so vars stay consistant

    }

    private void Start()
    {
        if(FindObjectOfType<Player_Controller>().isActiveAndEnabled)
        {
            print("I SEEEEEE YOUUUUUUUUUUUUUUUUUU");
        }
    }



}
