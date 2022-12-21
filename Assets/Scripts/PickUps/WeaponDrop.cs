using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    /// <summary>
    /// Weapon Drops
    /// Dylan Loe
    /// 
    /// Updated June 15, 2020
    /// 
    /// - Possible weapons for players to pick up and use
    /// </summary>
    
    public WeaponInstance weapon;

    [Header("how close player is to pick up gun")]
    public float pickUpRadius;

    public bool canPickUp;
    public bool cargoWeapon;

    //public Canvas canvas;
    //public Text text;
    private TextMesh mText;
    //TextMesh valueText;
    //TextMesh text2;

    Transform[] _player;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        //canvas.transform.position = this.transform.position;
        //text.transform.position = this.transform.position;
        //canvas. = Camera.current;
        mText = GetComponentInChildren<TextMesh>();
        //mText.transform.localScale = mText.transform.localScale * -1;
    }

    private void Update()
    {
        canPickUp = DetectPlayer();
        //valueText.transform.rotation = Quaternion.Euler(90, 0, 0);

        if(canPickUp)
        {
            //show text
            //mText.transform.LookAt(cam.transform.position);
            
            mText.gameObject.SetActive(true);
        }
        else
        {
            //turn off text
            mText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }

    //locate possible player from colliders found in sphere
    private Transform[] collidersToTransforms(Collider[] colliders)
    {
        Transform[] transforms = new Transform[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            transforms[i] = colliders[i].transform;
        }
        return transforms;
    }

    bool DetectPlayer()
    {
        _player = collidersToTransforms(Physics.OverlapSphere(transform.position, pickUpRadius));
        foreach (Transform potentialPlayer in _player)
        {
            if (potentialPlayer.gameObject.tag == "Player")
            {
                
                return true;
            }
        }
        return false;
    }
}
