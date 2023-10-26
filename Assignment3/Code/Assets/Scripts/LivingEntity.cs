using System;
using System.Collections;
using UnityEngine;

public class LivingEntity : MonoBehaviour {

    public GameObject prefab;
    [HideInInspector]
    public enum StageOfLife { Baby, Adult, Elder, Dead }
    [HideInInspector]
    protected StageOfLife stage = StageOfLife.Baby;

    [HideInInspector]
    public bool grounded = false;
    [Range(0, 100)]
    public float startSize = 20f;

    #region StageOfLife Variables
    [Header("Baby")]
    public float babyDuration = 10f;
    [Range(0, 100)]
    public float babyGrowthRate = 5f;
    public Color babyColor = Color.white;
    
    [Header("Adult")]
    public float adultDuration = 20f;
    [Range(0, 100)]
    public float adultGrowthRate = 0.5f;
    public Color adultColor = Color.white;
    
    [Header("Elder")]
    public float elderDuration = 50f;
    [Range(0, 100)]
    public float elderGrowthRate = 0f;
    public Color elderColor = Color.white;

    [Header("Death")]
    public float deathDuration = 0f;
    [Range(0, 100)]
    public float deathGrowthRate = 0f;
    public Color deathColor = Color.white;
    #endregion

    [Header("Other")]
    public float feedWorth = 0f;
    [HideInInspector]
    public Vector3 ogScale;

    protected float curDuration;
    protected float curGrowthRate;
    protected Color curColor;

    protected bool dead;

    public virtual void Start() {
        ogScale = transform.localScale;
        startSize /= 100;
        transform.localScale = ogScale * startSize;
        StartCoroutine(LifeCycle());
    }

    public virtual void Update() {
        Grow(curGrowthRate/100);

        if (transform.position.y < -10)
            Die();
    }

    public virtual void Grow(float growthAmount ) {
        transform.localScale += ogScale * growthAmount / 100;
    }

    protected virtual void Die () {
        if (!dead)
            dead = true;
        GetComponent<Animator>().StopPlayback();
        Destroy(gameObject);
    }

    IEnumerator LifeCycle() {
        stage = StageOfLife.Baby;
        curDuration = babyDuration;
        curGrowthRate = babyGrowthRate;
        GetComponent<MeshRenderer>().material.color = babyColor;
        yield return new WaitForSeconds(curDuration);

        stage = StageOfLife.Adult;
        curDuration = adultDuration;
        curGrowthRate = adultGrowthRate;
        GetComponent<MeshRenderer>().material.color = adultColor;
        yield return new WaitForSeconds(curDuration);
        
        stage = StageOfLife.Elder;
        curDuration = elderDuration;
        curGrowthRate = elderGrowthRate;
        GetComponent<MeshRenderer>().material.color = elderColor;
        yield return new WaitForSeconds(curDuration);

        stage = StageOfLife.Dead;
        curDuration = deathDuration;
        curGrowthRate = deathGrowthRate;
        GetComponent<MeshRenderer>().material.color = deathColor;
        yield return new WaitForSeconds(curDuration);

        Die();
    }

    public void waitThen(float time, Action action) {
        StartCoroutine(wait(time, action));
    }

    IEnumerator wait(float time, Action action) {
        yield return new WaitForSeconds(time);

        action();
    }
}