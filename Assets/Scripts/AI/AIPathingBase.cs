using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

//directional enum (for random movements if patrol is off)
public enum direction
{
    up,
    down,
    left,
    right,
    stop
}

public enum status
{
    none,
    moving,
    standing,
    dead

}

public class AIPathingBase : MonoBehaviour
{
    /// <summary>
    /// This is the main AI script
    /// </summary>
    //ANIMATION
    public status currentStatus;

    //finds player that is closer and will move towards them
    //when there is not a player to track, it will either move in a patrol manner OR randomly up, down, left or right?

    Transform[] _nearbyEnemies;
    Transform[] _bulletsNear;

    [Header("Tracking and Pathfinding")]
    public NavMeshAgent agent;
    [Header("Rotation Waist that tracks poi")]
    public GameObject trackingObj;
    [Header("The Radius of The Attack Range")]
    //[Range(5, 20)]
    public float checkRadius = 10f; //10 seems to work well
    public float stopDistance;
    [Header("Bullet Detection Radius")]
    public float bulletDetectionRadius = 2f;
    [Header("Target")]
    private Transform _poi;
    private Vector3 distanceToTarget;
    [Header("Distance to target")]
    public float distance;
    [Header("How fast it moves")]
    public float speed;
    private float _currentUsedSpeed;
    //used for offering more varety in speeds randomly for enemy
    private float _speedMin;
    private float _speedMax;
    //how fast ai tracks target
    [Header("How fast ai tracks target")]
    public float trackingSpeed;
    [Header("How fast enemy rotates")]
    public float rotationSpeed = 5.0f;
    [Space(20)]

    [Header("Set enemy to idle or patrol idle")]
    public bool canPatrol = true;
    [Space(2)]
    [Header("Idle Movement var")]
    //amounts of time it moves in random directions before changing
    public float directionDurationMax;
    public float directionDurationMin;
    [Header("Current Direction facing")]
    public direction patrolDir;
    direction _opposite;
    direction _current;
    [Space(2)]
    //patrolling stuff
    [SerializeField]
    [Header("Patroling")]
    private bool _isDone = false;
    private Vector3 _lastPosition;
    private Vector3 _currentPosition;
    [Header("Patrol Path (must be DrawPath obj if patrol is on)")]
    public DrawPath pathToFollow;
    private NavMeshPath path;
    private bool _isPatroling;
    private int _currentIndexPatrol = 0;
    private float _reachDistancePatrol = 1.0f;
    private bool _isDonePatrol = false;
    private float _rangeStop;
    //distance before ai switches to next patrol target
    private float _reachDistance = 0.5f;
    //current patrol target
    private int _currentIndex = 0;

    [Space(5)]
    [Header("Health of enemy")]
    public int health = 100;
    
    [Space(15)]
    [Header("Fire Range")]
    public float fireRange = 9f;
    [Header("How often it fires")]
    public float fireRate;
    private float _fireRateSet;
    private float counter = 0f;
    [Header("Bullet prefab")]
    public GameObject projectile;
    public weaponType gunType;
    [Header("Equiped Gun Type")]
    public GameObject gun;
    
    private bool _firing = false;
    [Header("Fire durations")]
    public float triggerTimeMin = 1.0f;
    public float triggerTimeMax = 1.5f;
    //viewing player
    private bool LineOfSightMade = false;
    private bool InRangeToFire = false;
    private bool _attackCycle = false;
    private bool _nearMiss = false;

    private bool _singleShot = false;

    private EnemyAnimation _enemyAnimation;

    //patrol will follow path, once it reaches its last node it will either go to the first node or go backwards
    private void Awake()
    {
        _lastPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _rangeStop = Random.Range(0.4f, 0.7f);
        _fireRateSet = fireRate;
        fireRate = Random.Range(0.5f, _fireRateSet + (_fireRateSet * 0.1f));
        //SetGun();
        this.GetComponent<NavMeshAgent>().speed = speed;
        _speedMin = speed / 2;
        _speedMax = speed;
        _currentUsedSpeed = Random.Range(_speedMin, _speedMax);

        if(canPatrol)
        {
            directionDurationMax = 4f;
            directionDurationMin = 3f;

            //pick a random direction, and we will only use that one and its opposite to move back and forth
            float directionNum = Random.Range(0.0f, 1.0f);
            if (directionNum <= 0.25f)
            {
                patrolDir = direction.left;
                _current = direction.left;
                _opposite = direction.right;
            }   
            else if (directionNum <= 0.5f && directionNum > 0.25f)
            {
                patrolDir = direction.right;
                _current = direction.right;
                _opposite = direction.left;
            }   
            else if (directionNum <= 0.75f && directionNum > 0.5f)
            {
                patrolDir = direction.up;
                _current = direction.up;
                _opposite = direction.down;
            }    
            else if (directionNum > 0.75f)
            {
                patrolDir = direction.down;
                _current = direction.down;
                _opposite = direction.up;
            }
        }
        else
        {
            directionDurationMax = 2f;
            directionDurationMin = 0.5f;

            //we pick a random direction (no opposite cause it gets refreshed each time
            float directionNum = Random.Range(0.0f, 1.0f);
            if (directionNum <= 0.25f)
            {
                patrolDir = direction.left;
            }
            else if (directionNum <= 0.5f && directionNum > 0.25f)
            {
                patrolDir = direction.right;
            }
            else if (directionNum <= 0.75f && directionNum > 0.5f)
            {
                patrolDir = direction.up;
            }
            else if (directionNum > 0.75f)
            {
                patrolDir = direction.down;
            }
        }

        StartCoroutine(changeDirections());
    }

    // Update is called once per frame
    void Update()
    {
        //if target is destoryed or doesnt exist, we check for a target
        //while this happens enemy is moving
        if (_poi == null || !_poi.gameObject.activeInHierarchy)
        {
            //if(this.GetComponent<Rigidbody>().)

            if (!canPatrol)
            {
                patrolMovement();
                if (_current == direction.stop)
                    currentStatus = status.standing;
                else
                    currentStatus = status.moving;
            }
            else
            {
                patrolMovement2();

            }
                
            //aquire target
            _poi = GetClosestEnemy();

        }
        else
        {
            //we have target
            if (_attackCycle)
            {
                 this.GetComponent<NavMeshAgent>().speed = Random.Range(-(speed * 0.2f), speed - (speed * 0.5f));
                _attackCycle = false;
            }

            if (distance <= stopDistance)
            {
                this.GetComponent<NavMeshAgent>().speed = 0;
                currentStatus = status.standing;
            }
            else
            {
                this.GetComponent<NavMeshAgent>().speed = speed;
                currentStatus = status.moving;
            }

            //movement and line of sight
            if (CheckPOIDistance() || _nearMiss)
            {
                //has gun raised or something
                //if(!_firing && !_singleShot)
                   // currentStatus = status.agroMovement;

                _attackCycle = false;
                agent.isStopped = false;
                 
                //update path to target
                path = new NavMeshPath();
                if (NavMesh.CalculatePath(transform.position, _poi.position, NavMesh.AllAreas, path))
                {
                    //draw path
                    //set it
                    agent.SetPath(path);
                    for (int i = 0; i < path.corners.Length - 1; i++)
                    {
                        Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.green);
                    }
                }
                var rotation = Quaternion.LookRotation(_poi.position - trackingObj.transform.position);
                trackingObj.transform.rotation = Quaternion.Slerp(trackingObj.transform.rotation, rotation, Time.deltaTime * trackingSpeed);

                //if target is close enough and we have line of sight
                if (LineOfSight())
                {        
                    //shooting starts here
                    //while we can shoot, there will be a timer that runs between each shot
                    counter += Time.deltaTime;
                    if (counter >= fireRate)
                    {
                        if (gunType == weaponType.assaultRifle)
                            StartCoroutine(HoldTrigger());
                        else
                        {
                            StartCoroutine(SingleFire());
                            FireBullet();

                        }
                    }
                }
            }
        }

        //if someone shoots at enemy 
        FiredAt();

        //for automatic firing
        if(_firing)
        {
            //currentStatus = status.shooting;
            FireBullet();
        }
        
            

        if (health <= 0)
        {
            //enemy is dead
            OnDeath();
        }

        SetAnimation();
    }

    //when it dies
    void OnDeath()
    {
        //do stuff here
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet" && collision.gameObject.GetComponent<Bullet>().type == shootType.player)
        {
            //alerts enemy when it is hit by players bullet

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet" && other.GetComponent<Bullet>().type == shootType.player)
        {
            health -= other.GetComponent<Bullet>().damage;
            //alerts enemy when it is hit by players bullet

        }
    }

    //rooty shooty mc tooty
    void FireBullet()
    {
        //currentStatus = status.shooting;
        //new system
        if (gunType == weaponType.pistol || gunType == weaponType.assaultRifle || gunType == weaponType.DMR)
            gun.GetComponent<Weapon>().Shooting();
        else if (gunType == weaponType.shotgun)
            gun.GetComponent<ShotgunWeapon>().Shooting();

        //fire projectile forward (at player or poi)
        //set any variables HERE
        counter = 0.0f;
        //slight variation in firing
        fireRate = Random.Range(1f, _fireRateSet + (_fireRateSet * 0.1f));
    }

    //UNUSED
    void DoMovement()
    {
        currentStatus = status.moving;
        float distance = Vector3.Distance(path.corners[_currentIndex], transform.position);
        agent.SetDestination(Vector3.MoveTowards(transform.position, path.corners[_currentIndex], Time.deltaTime * speed * 100));

        if(path.corners[_currentIndex] - transform.position != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(path.corners[_currentIndex] - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

        if(distance <= _reachDistance && _currentIndex < path.corners.Length - 1)
        {
            if (!_isDone)
                _currentIndex++;
            else
                _isDone = true;
        }
    }

    //finds distance between enemy and poi, returns true if we are in range
    bool CheckPOIDistance()
    {
        bool attackMode = false;

        distanceToTarget = this.transform.position - _poi.transform.position;
        distance = distanceToTarget.magnitude;

        if (distanceToTarget.magnitude < fireRange)
        {
            attackMode = true;
        }
        else
            //Debug.Log("not in fire range");
        InRangeToFire = attackMode;
        
        return attackMode;
    }
    //if have line of sight of player
    bool LineOfSight()
    {
        bool hitTarget = false;
        RaycastHit lineOfSight;

        Vector3 forward = transform.TransformDirection(Vector3.forward) * fireRange;

        Vector3 eulerAngles = new Vector3(0, 4f, 0);
        Vector3 directionLeft = Quaternion.Euler(eulerAngles) * trackingObj.transform.forward;
        eulerAngles = new Vector3(0, -4f, 0);
        Vector3 directionRight = Quaternion.Euler(eulerAngles) * trackingObj.transform.forward;

        //Vector3 forwardV = trackingObj.transform.TransformDirection(trackingObj.transform.forward) * 10;
        Debug.DrawRay(trackingObj.transform.position, directionLeft * checkRadius, Color.red);
        Debug.DrawRay(trackingObj.transform.position, directionRight * checkRadius, Color.red);
        Debug.DrawRay(trackingObj.transform.position, trackingObj.transform.forward * checkRadius, Color.red);

        //if we hit something, check to see if its player
        if (Physics.Raycast(trackingObj.transform.position, trackingObj.transform.forward, out lineOfSight, Mathf.Infinity)|| Physics.Raycast(trackingObj.transform.position, directionLeft, out lineOfSight, Mathf.Infinity) || Physics.Raycast(trackingObj.transform.position, directionRight, out lineOfSight, Mathf.Infinity))
        {
            if (lineOfSight.collider.tag == "Player")
            {
                hitTarget = true;
            }
            else
                hitTarget = false;
        }
        LineOfSightMade = hitTarget;
   

        if (hitTarget == true)
            _attackCycle = false;

        return hitTarget;
    }

    bool Trackings()
    {
        bool hitTarget = false;
        RaycastHit tracking;

        Debug.DrawRay(transform.position, _poi.transform.position, Color.black);
        if(Physics.Raycast(transform.position, transform.TransformDirection(_poi.transform.position), out tracking, Mathf.Infinity))
        {
            if (tracking.collider.tag == "Player")
            {
                hitTarget = true;
            }
            else
                hitTarget = false;
        }

        return hitTarget;
    }

    //find target
    Transform GetClosestEnemy()
    {
        _nearbyEnemies = collidersToTransforms(Physics.OverlapSphere(transform.position, checkRadius));
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Pow(checkRadius, 2);
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in _nearbyEnemies)
        {
            if (potentialTarget.gameObject.tag == "Player")
            {
                float dSqrToTarget = getDistanceSqr(currentPosition, potentialTarget.position);
                
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                    
                }
            }
        }
        //Debug.Log(closestDistanceSqr);
        _attackCycle = true;
        return bestTarget;

    }

    //locate transforms from colliders found in sphere
    private Transform[] collidersToTransforms(Collider[] colliders)
    {
        Transform[] transforms = new Transform[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            transforms[i] = colliders[i].transform;
        }
        return transforms;
    }

    //show range of enemy (mainly for debugging)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, bulletDetectionRadius);
    }
  

    //simple math, get distance
    private float getDistanceSqr(Vector3 initialPoint, Vector3 targetPoint)
    {
        Vector3 directionToTarget = targetPoint - initialPoint;
        return directionToTarget.sqrMagnitude;
    }

    //set patrol path
    void patrolMovement2()
    {
        currentStatus = status.moving;
        float distance = Vector3.Distance(pathToFollow.pathPoints[_currentIndexPatrol].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, pathToFollow.pathPoints[_currentIndexPatrol].position, Time.deltaTime * speed);

        if (pathToFollow.pathPoints[_currentIndexPatrol].position - transform.position != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(pathToFollow.pathPoints[_currentIndexPatrol].position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

        if (distance <= _reachDistance && _currentIndexPatrol < pathToFollow.pathPoints.Count - 1)
        {
            if (!_isDonePatrol)
            {
                _currentIndexPatrol++;

                //Debug.Log(pathToFollow.pathPoints.Count);
                if (_currentIndexPatrol == pathToFollow.pathPoints.Count - 1)
                    _currentIndexPatrol = 0;
            }
            else
            {
                _isDonePatrol = true;
            }
        }
    }

    //random movement
    void patrolMovement()
    {
        switch (_current)
        {
            case direction.up:
                //z forward
                //agent.transform.Translate(Vector3.forward * _currentUsedSpeed * Time.deltaTime);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
                break;
            case direction.down:
                //z backwards
                //agent.transform.Translate(Vector3.back * _currentUsedSpeed * Time.deltaTime);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
                break;
            case direction.left:
                //x left
                //agent.transform.Translate(Vector3.left * _currentUsedSpeed * Time.deltaTime);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90f, transform.eulerAngles.z);
                break;
            case direction.right:
                //x right
                //agent.transform.Translate(Vector3.right * _currentUsedSpeed * Time.deltaTime);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90, transform.eulerAngles.z);
                break;
            default:
                break;
        }

        if(_current != direction.stop)
            agent.transform.Translate(Vector3.forward * _currentUsedSpeed * Time.deltaTime);
    }
    IEnumerator changeDirections()
    {
        
        yield return new WaitForSeconds(Random.Range(directionDurationMin, directionDurationMax));
        //wait a few seconds, then change directions
        _current = direction.stop;
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));

        if (canPatrol)
        {
            _currentUsedSpeed = Random.Range(_speedMin, _speedMax);
            direction temp = patrolDir;
            patrolDir = _opposite;
            _current = _opposite;
            _opposite = temp;
            
        }
        else
        {
            //we pick a random direction (no opposite cause it gets refreshed each time
            float directionNum = Random.Range(0.0f, 1.0f);
            if (directionNum <= 0.25f)
            {
                _current = direction.left;
            }
            else if (directionNum <= 0.5f && directionNum > 0.25f)
            {
                _current = direction.right;
            }
            else if (directionNum <= 0.75f && directionNum > 0.5f)
            {
                _current = direction.up;
            }
            else if (directionNum > 0.75f)
            {
                _current = direction.down;
            }
        }

        StartCoroutine(changeDirections());
    }

    //function uses externally 
    public void setPOI(Transform poi)
    {
        //when player fires shot and enemy is in range, it sets the poi
        _poi = poi;
    }

    //some weapons have continueous fire (like assaault and dmrs)
    IEnumerator HoldTrigger()
    {
        //currentStatus = status.shooting;
        _firing = true;
        yield return new WaitForSeconds(Random.Range(triggerTimeMin, triggerTimeMax));
        _firing = false;
    }

    IEnumerator SingleFire()
    {
        _singleShot = true;
        //currentStatus = status.shooting;
        yield return new WaitForSeconds(0.5f);
        _singleShot = false;
    }

    //if someone shoots at enemy, and bullets almost hit or do, enemy will know
    public void FiredAt()
    {
        
        _bulletsNear = collidersToTransforms(Physics.OverlapSphere(transform.position, bulletDetectionRadius));
        foreach (Transform potentialTarget in _bulletsNear)
        {
            if (potentialTarget.gameObject.tag == "Bullet")
            {
                //Debug.Log("near miss");
                //Debug.Log(potentialTarget.GetComponent<Bullet>().shooter.tag);
                //if player shot bullet
                if (potentialTarget.GetComponent<Bullet>().shooter.tag == "Player")
                {
                    //if enemy is way to far away, enemy wont still have target
                    float dSqrToTarget = getDistanceSqr(transform.position, potentialTarget.position);
                    if (dSqrToTarget < 30f)
                    {
                        //Debug.Log("target acquired");
                        _poi = potentialTarget.GetComponent<Bullet>().shooter.transform;
                        //if (_attackCycle != true)
                        _attackCycle = true;
                        //bypass range requirement to track poi. Enemy was attacts and will go that that location
                        _nearMiss = true;
                    }
                }
            }
        }
    }

    void SetAnimation()
    {
        //based on status enum
        switch (currentStatus)
        {
            case status.none:
                _enemyAnimation.IsIdling();
                break;
            case status.moving:
                _enemyAnimation.IsRunning();
                break;
            case status.standing:
                _enemyAnimation.IsIdling();
                break;
            default:
                break;
        }
    }
}
