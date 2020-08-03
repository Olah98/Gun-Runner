using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
    //this is the main camera that fouces onto the player
    Camera playerCam;

    [Header("Player UI")]
    //THIS TEXT IS TEMPORARY AND NEEDS A NEW HOME. CANT STAY HERE FOREVER
    public GameObject Canvas;
    public Text cargoText;
    public Text ammoInMag;

    [Header("Player Vars")]
    //this is the playes move speed
    public float moveSpeed;
    //The starting value for the players health and a tracker for the players current health
    public int maxHealth;
    public int currentHealth;
    public bool hasCargo = false;
    public bool isPlayerAlive = true;
    //public GameObject gunLoc;//the location that the bullets are fired from
    public GameObject bulletPrefab;

    public PlayerWeapom weapon;

    [Header("Shooting Detection Range")]
    public float shootingDetectionRadius = 20f;
    Transform[] _proxEnemies;

    public weaponType gunType;

    public PlayerInventory myBag;

    public HealthBar healthBar;

    private void Awake()
    {
        playerCam = Camera.main;
        myBag = FindObjectOfType<PlayerInventory>();
        gunType = myBag.currnetWeapon;
        //gunLoc = this.transform.GetChild(0).gameObject;
        //This time scale is to resume the game after the Game Over Screen pops up. 
        //The time scale gets set to 0 when the game over screen pops up
        Time.timeScale = 1f;
    }

    private void Start()
    {
        //Sets player health to the Max Health value
        currentHealth = maxHealth;
        //Sets Health UI text to the Max Health value
        healthBar.SetMaxHealth(maxHealth);
        cargoText.text = "";
        ammoInMag.text = "Mag: " + weapon.ammoInMag.ToString();
        
    }


    void Update()
    {
        if (isPlayerAlive == true)
        {
            lookAround();
            moveAround();
            shootCurGun();
            changeGun();
        }
        ammoInMag.text = weapon.current.ToString() + ": " + weapon.ammoInMag.ToString();

    }


    //This is to check if the player has picked up the Cargo or not.
    //Also checks if a bullet has hit the player, if so it runs the TakeDamage function
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cargo")
        {
            hasCargo = true;
            Destroy(GameObject.FindWithTag("Cargo"));
            //HERE IS SOME OF THAT TEMPORARY TEXT THAT NEEDS A NEW HOME
            cargoText.text = ("Cargo Collected!");
            
        }

        if (other.gameObject.tag == "Bullet" && other.gameObject.GetComponent<Bullet>().type == shootType.enemy)
        {
            //Debug.Log(other.name);
            TakeDamage(other.GetComponent<Bullet>().damage);
        }
    }

    //This function takes in damage values for the player. It also updates the Health Bar UI 
    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        //Will check if player is dead, if so the player is disabled and End Screen UI pops up
        //Disables the players weapon, freezes the game, and activates the Game Over Screen
        if (currentHealth <= 0)
        {
            isPlayerAlive = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
            Time.timeScale = 0f;
            Canvas.transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    void lookAround()
    {
        // this is a simple way to turn the player around so they are always faceing the thing you want 
        // takes the angle of input and rotates it to that position using basic geomtry functions
        Vector2 dir = Input.mousePosition - playerCam.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

    void moveAround()
    {
        //for movment we are updating the vector 3 with 2 inputs 
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position = transform.position + movement * moveSpeed * Time.deltaTime;
    }

    void shootCurGun()
    {
        if (Input.GetMouseButton(0))
        {
            weapon.Shooting();
            ShootingDetection();
        }
    }

    void changeGun()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("change the gun");
            myBag.swapGun();
            gunType = myBag.currnetWeapon;
            weapon.checkWeapon();
        }
    }


    //shooting detection
    //show range of shooting detection (mainly for debugging)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingDetectionRadius);

    }

    //locate possible enemy transforms from colliders found in sphere
    private Transform[] collidersToTransforms(Collider[] colliders)
    {
        Transform[] transforms = new Transform[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            transforms[i] = colliders[i].transform;
        }
        return transforms;
    }

    //any enemies in radius will be alerted to shooting
    void ShootingDetection()
    {
        _proxEnemies = collidersToTransforms(Physics.OverlapSphere(transform.position, shootingDetectionRadius));
        foreach (Transform potentialTarget in _proxEnemies)
        {
            if (potentialTarget.gameObject.tag == "Enemy")
            {
                potentialTarget.gameObject.GetComponent<AIPathingBase>().setPOI(this.transform);
            }
        }
    }
}
