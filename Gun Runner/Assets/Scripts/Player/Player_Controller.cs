using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum playerStatus
{
    standing,
    moving,
    firing
}

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
    //public GameObject bulletPrefab;

    public PlayerWeapom weapon;

    [Header("Shooting Detection Range")]
    public float shootingDetectionRadius = 20f;
    Transform[] _proxEnemies;

    //public weaponType gunType;

    public PlayerInventory myBag;

    //weapon data
    [SerializeField]
    public WeaponData weaponData;

    public HealthBar healthBar;

    bool damageCoolDown = false;
    float damangeCoolDownTimer = 1.0f;

    public bool CanPickUp = false;
    public GameObject crate;

    private void Awake()
    {
        playerCam = Camera.main;
        myBag = FindObjectOfType<PlayerInventory>();

        //SetPlayerGunData();
       
        //This time scale is to resume the game after the Game Over Screen pops up. 
        //The time scale gets set to 0 when the game over screen pops up
        Time.timeScale = 1f;
    }

    private void Start()
    {
        if (FindObjectOfType<PlayerVars>().isActiveAndEnabled)
        {
            print("Found the Player");
            //Player_Controller playerInstance = FindObjectOfType<Player_Controller>();
            if (PlayerVars.Instance.health == 0) { PlayerVars.Instance.health = maxHealth; }
            currentHealth = PlayerVars.Instance.health;

        }
        //Sets player health to the Max Health value
        //currentHealth = maxHealth;
        //Sets Health UI text to the Max Health value
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        cargoText.text = "";
        ammoInMag.text = "Mag: " + weapon.ammoInMag.ToString();
        weapon.checkWeapon();
    }

    void Update()
    {
        if (isPlayerAlive == true)
        {
            lookAround();
            moveAround();
            shootCurGun();
            changeGun();
            PickUpGun();
        }
        ammoInMag.text = weapon.current.ToString() + ": " + weapon.ammoInMag.ToString();

    }

    //This is to check if the player has picked up the Cargo or not.
    //Also checks if a bullet has hit the player, if so it runs the TakeDamage function
    private void OnTriggerEnter(Collider other)
    {
        //because collision with level exit is in another script for some reason, it will call function in this script to save character info , then load


        if (other.gameObject.tag == "Cargo")
        {
            //hasCargo = true;
            //Destroy(GameObject.FindWithTag("Cargo"));
            //HERE IS SOME OF THAT TEMPORARY TEXT THAT NEEDS A NEW HOME
            cargoText.text = ("Cargo Collected!");
            
        }

        if (other.gameObject.tag == "Bullet" && other.gameObject.GetComponent<Bullet>().type == shootType.enemy)
        {
            //Debug.Log(other.name);
            TakeDamage(other.GetComponent<Bullet>().damage);
        }

        if(other.gameObject.tag == "HealthPack")
        {
            //gives one health
            currentHealth++;
            //sethealth
            healthBar.SetHealth(currentHealth);
        }
        if(other.gameObject.tag == "AmmoPack")
        {
            //gives 2 mags for both guns
            myBag.slot1.totalAmmo += 2 * myBag.slot1.magSize;
            if (myBag.slot2.weapontype != weaponType.none)
                myBag.slot2.totalAmmo += 2 * myBag.slot2.magSize;

            //could add more ammo to cargo?
        }
        if (other.gameObject.tag == "WeaponDrop")
        {
            if (other.GetComponent<WeaponDrop>().canPickUp)
            {
                Debug.Log("Pless e");
                crate = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "WeaponDrop")
        {
            crate = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //ignore collisions to fix some physics problems
        if (collision.gameObject.tag == "GrenadeLauncherAmmo")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
        }
    }

    //This function takes in damage values for the player. It also updates the Health Bar UI 
    void TakeDamage(int damage)
    {
        if (damageCoolDown == false)
        {
            currentHealth--;

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
            StartCoroutine(damangeCoolDownIE());
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
        if(movement != Vector3.zero)
        {
            //PLAY MOVEMENT ANIMATION HERE

        }
        transform.position = transform.position + movement * moveSpeed * Time.deltaTime;
    }

    void shootCurGun()
    {
        if (Input.GetMouseButton(0))
        {
            //fire animation here

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
            //gunType = myBag.currnetWeapon.weapontype;
            weapon.checkWeapon();
        }
    }

    void PickUpGun()
    {
        if (crate != null && Input.GetKeyDown(KeyCode.E))
        {
            if (crate.GetComponent<WeaponDrop>().cargoWeapon)
            {
                myBag.cargo = crate.GetComponent<WeaponDrop>().weapon;
                Debug.Log("Picked up cargo");
            }
            else if (myBag.slot2.weapontype == weaponType.none && crate.GetComponent<WeaponDrop>().weapon.weapontype == myBag.slot1.weapontype)
            {
                myBag.slot1.totalAmmo += crate.GetComponent<WeaponDrop>().weapon.totalAmmo;
                Debug.Log("Extra ammo, casue same gun dropped");
            }
            else if (myBag.slot2.weapontype == weaponType.none && crate.GetComponent<WeaponDrop>().weapon.weapontype != myBag.slot1.weapontype)
            {
                myBag.slot2 = crate.GetComponent<WeaponDrop>().weapon;
                Debug.Log("Picked up secondary"); //no secondary before so automaticlly go there
            }
            else
            {
                //replaces gun in currently equiped slot
                //player can choose (if the same gun is what dropped then give ammo)
                if (myBag.currentlyEquiped == current.slot1 && crate.GetComponent<WeaponDrop>().weapon.weapontype != myBag.slot1.weapontype)
                    myBag.slot1 = crate.GetComponent<WeaponDrop>().weapon;
                else if (myBag.currentlyEquiped == current.slot2 && crate.GetComponent<WeaponDrop>().weapon.weapontype != myBag.slot2.weapontype)
                    myBag.slot2 = crate.GetComponent<WeaponDrop>().weapon;
                else if (myBag.currentlyEquiped == current.slot2 && crate.GetComponent<WeaponDrop>().weapon.weapontype == myBag.slot2.weapontype)
                    myBag.slot2.totalAmmo += crate.GetComponent<WeaponDrop>().weapon.totalAmmo;
                else if (myBag.currentlyEquiped == current.slot1 && crate.GetComponent<WeaponDrop>().weapon.weapontype == myBag.slot1.weapontype)
                    myBag.slot1.totalAmmo += crate.GetComponent<WeaponDrop>().weapon.totalAmmo;

                Debug.Log("replaced");
            }
            Destroy(crate.gameObject);
            crate = null;
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

    //WILL NEED TO BE UPDATED:
    //temporary hardcoded
    //future update will have players choose guns before level starts and they will be set here
    void SetPlayerGunData()
    {
        //can find using item number or by its name (must be exact)
        //myBag.SetGun1(weaponData.FindWeapon(1));
        //myBag.SetGun2(weaponData.FindWeapon(7));
        //myBag.SetCargo(weaponData.FindWeapon("Grenade Launcher"));
        weapon.checkWeapon();
    }

    private void OnDestroy()
    {
        PlayerVars.Instance.health = currentHealth;
    }

    //player cannot take damange to often
    IEnumerator damangeCoolDownIE()
    {
        damageCoolDown = true;
        yield return new WaitForSeconds(damangeCoolDownTimer);
        damageCoolDown = false;
    }

    
}
