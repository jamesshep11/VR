using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : LivingEntity
{
    [HideInInspector]
    public enum Gender { Male, Female}

    [HideInInspector]
    public float hunger;
    [HideInInspector]
    public float sexDrive;
    [HideInInspector]
    public Gender gender;

    public GameObject target;
    
    public float viewRadius = 20f;

    [Header("Movement")]
    public float speed = 5f;
    public float stopDistance = 0.1f;
    public float boundsRadius = 0.27f;
    public float collisionAvoidDst = 5f;
    public float avoidCollisionWeight = 100f;
    public LayerMask obstacleMask;

    [Header("Food")]
    public LayerMask diet;
    [Range(0, 100)]
    public float eatRate = 10f;
    float feedRatecCorrection = 0.1f;

    [Header("Enemies")]
    public LayerMask enemies;
    public float cautionDist = 5f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        hunger = 0f;
        sexDrive = 0f;

        int rand = Random.Range(0, 100);
        if (rand <= 50) {
            gender = Gender.Male;
        } else {
            gender = Gender.Female;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        hunger += 2.0f * Time.deltaTime;
        sexDrive += 0.5f * Time.deltaTime;

        // Exit Condition
        if (Physics.CheckSphere(transform.position, cautionDist, enemies))
            GetComponent<Animator>().SetBool("Flee", true);
    }

    public virtual void Idle() {

    }

    public virtual GameObject Find(int layer) {
        Collider[] found = Physics.OverlapSphere(transform.position, viewRadius, layer);
        if (found.Length > 0) {
            int pos = Random.Range(0, found.Length);
            GameObject newTarget = found[pos].gameObject;
            if (newTarget.GetComponent<Terrain>() == null)
                return newTarget;
        }
        return null;
    }

    public virtual bool Chase() {
        if ((target.transform.position - transform.position).magnitude > stopDistance) {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            Vector3 acceleration = dir * speed;
            //Vector3 velocity = dir * (speed + 0.5f);      Increase speed when chasing mate

            // Move
            transform.forward = dir;
            transform.position += acceleration * Time.deltaTime;
            return false;
        }

        return true;
    }

    public virtual void Feed(LivingEntity targetStats) {
        hunger -= targetStats.feedWorth * eatRate * Time.deltaTime;
        target.transform.localScale -= targetStats.ogScale * eatRate / 100 * feedRatecCorrection;
    }

    public virtual void Mate() {
        ParticleSystem hearts = GetComponent<ParticleSystem>();
        Animator animator = GetComponent<Animator>();

        hearts.Play();
        waitThen(5, () => {
            hearts.Stop();

            animator.SetBool("Repro", false);
        });
    }

    public bool RequestMating(int layerIndex) {
        Animator targetAnimator = target.GetComponent<Animator>();
        return target.GetComponent<Animal>().MateRequested(gameObject, targetAnimator, layerIndex);
    }

    public virtual bool MateRequested(GameObject from, Animator targetAnimator, int layerIndex) {
        Animal stats = from.GetComponent<Animal>();
        AnimatorStateInfo stateinfo = targetAnimator.GetCurrentAnimatorStateInfo(layerIndex);
        // ignore if already mating
        if (stateinfo.IsName("Mate") || stateinfo.IsName("Flee"))
            return false;

        // Random chance of mating
        int chance = Random.Range(0, 2);
        if (chance == 0) return false;
        else {
            stats.target = from;
            targetAnimator.SetTrigger("Mate");
            return true;
        }
    }

    public virtual void Flee(Animator animator) {
        
    }
}
