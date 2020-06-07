using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


public enum direction
{
    up,
    down,
    left,
    right,
    stop
}

public class AIPathingBase : MonoBehaviour
{
    //finds player that is closer and will move towards them
    //when there is not a player to track, it will either move in a patrol manner OR randomly up, down, left or right?

    Transform[] _nearbyEnemies;

    public NavMeshAgent agent;

    [Header("The Radius of The Attack Range")]
    public float checkRadius = 10f;

    public int health = 100;

    public Transform _poi;

    public float directionDurationMax;
    public float directionDurationMin;

    public direction patrolDir;
    direction _opposite;
    direction _current;
    public bool canPatrol = true;

    public float speed;
    private float _currentUsedSpeed;
    private float _speedMin;
    private float _speedMax;

    //patrol will be either moving one direction or anthor

    // Start is called before the first frame update
    void Start()
    {
        speed = this.GetComponent<NavMeshAgent>().speed;
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
        if(_poi == null || !_poi.gameObject.activeInHierarchy)
        {
            patrolMovement();
            //aquire target
            _poi = GetClosestEnemy();

        }
        else
        {
            //we have target
            //move towards target
            agent.SetDestination(_poi.position);
        }
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
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

    //simple math, get distance
    private float getDistanceSqr(Vector3 initialPoint, Vector3 targetPoint)
    {
        Vector3 directionToTarget = targetPoint - initialPoint;
        return directionToTarget.sqrMagnitude;
    }

    void patrolMovement()
    {
        switch (_current)
        {
            case direction.up:
                //z forward
                agent.transform.Translate(Vector3.forward * _currentUsedSpeed * Time.deltaTime);
                break;
            case direction.down:
                //z backwards
                agent.transform.Translate(Vector3.back * _currentUsedSpeed * Time.deltaTime);
                break;
            case direction.left:
                //x left
                agent.transform.Translate(Vector3.left * _currentUsedSpeed * Time.deltaTime);
                break;
            case direction.right:
                //x right
                agent.transform.Translate(Vector3.right * _currentUsedSpeed * Time.deltaTime);
                break;
            default:
                break;
        }

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
}
