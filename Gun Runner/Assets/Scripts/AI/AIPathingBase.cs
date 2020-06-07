using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AIPathingBase : MonoBehaviour
{
    //finds player that is closer and will move towards them

    Transform[] _nearbyEnemies;

    public NavMeshAgent agent;

    [Header("The Radius of The Attack Range")]
    public float checkRadius = 10f;

    public int health = 100;

    private Transform _poi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_poi == null) //&& !_poi.gameObject.activeInHierarchy)
        {
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
                //Debug.Log("Slinger Found target: " + dSqrToTarget);
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

    private Transform[] collidersToTransforms(Collider[] colliders)
    {
        Transform[] transforms = new Transform[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            transforms[i] = colliders[i].transform;
        }
        return transforms;
    }

    //show range of enemy (mainly for debugging
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
}
