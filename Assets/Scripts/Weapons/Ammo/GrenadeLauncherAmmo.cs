using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncherAmmo : MonoBehaviour
{
    /// <summary>
    /// Grenade Launcher Ammo Behavior
    /// Dylan Loe
    /// 
    /// Updated June 15, 2020
    /// 
    /// - if it hits an enemy it blows up
    /// - otherwise it runs a countdown then blows up
    /// - will use physics to roll balll around
    /// 
    /// Questions:
    /// - damage player?
    /// 
    /// </summary>

    public GameObject hitEffect;
    public float explosionRadius = 5;
    public float timer = 10;
    public int damage = 5;

    //percentage of decrement ( multiply by 10%)
    float counterVal = 1; 
    float  counterDecrement;
    Transform[] _proxEnemies;
    float counterS;
    bool colorTick = true;

    float tick = 1;

    public Material defaultC;

    public shootType type;

    public GameObject tempExplosionObj;

    private void Start()
    {
        counterDecrement = (counterVal/timer);
        counterS = 0;
        //starts countdown
        StartCoroutine(CountDown());
    }

    private void Update()
    {
        if (tick > -20)
        {
            if (counterS >= tick)
            {
                if (colorTick)
                {
                    this.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    colorTick = false;
                }
                else
                {
                    this.gameObject.GetComponent<Renderer>().material.color = defaultC.color;
                    colorTick = true;
                }

                tick -= counterDecrement;
                counterDecrement = tick*(counterVal / timer);
                counterS = 0;
            }
            counterS += Time.deltaTime;
        }
        else
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(timer);
        Detonation();
    }


    //for debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "GrenadeLauncherAmmo")
        {
            Physics.IgnoreCollision(other.GetComponent<Collider>(), this.GetComponent<Collider>());
        }
    }

    void Detonation()
    {
        ///temp explosiion
        tempExplosionObj.GetComponent<MeshRenderer>().enabled = true;
        Debug.Log("BOOM");

        ///
        //PARTILE EFFECT GOES HERE
        ///

        //will check radius around this object
        //if enemy enemies are in it, they die
        _proxEnemies = collidersToTransforms(Physics.OverlapSphere(transform.position, explosionRadius));
        foreach (Transform potentialTarget in _proxEnemies)
        {
            if (potentialTarget.gameObject.tag == "Enemy")
            {
                Debug.Log(potentialTarget.gameObject.name);
                potentialTarget.gameObject.GetComponent<AIPathingBase>().health -= damage;
            }
        }

        StartCoroutine(showExplosion());
        //will maybe have particle effect too!
    }

    //explosion (TEMP)
    IEnumerator showExplosion()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
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
}
