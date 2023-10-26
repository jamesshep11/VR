using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Animal
{
    [Header("General Settings")]
    public float walkZone = 100;
    public bool debug;

    [Header("Hunting Settings")]
    public Material sharkMaterial;

    Vector3 vel, targetPos, targetCurent, targetRandom, targetFlock;
    float huntTime, huntSpeed, speedSh, acselSh, rotationSpeed;
    bool hunting;
    static WaitForSeconds delay0 = new WaitForSeconds(6.0f);

    Animator animator;
    AnimatorStateInfo animStateInfo;

    public override void Start() {
        huntSpeed = 1.0f;
        rotationSpeed = 1f;

        animator = GetComponent<Animator>();

        StartCoroutine(RandomVector());
        targetCurent = targetRandom;
    }

    public override void Update() {
        animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.IsName("Chase"))
            hunting = true;
        else
            hunting = false;

        if (acselSh >= 0) {
            speedSh += Time.deltaTime * acselSh;
            sharkMaterial.SetFloat("_ScriptControl", speedSh);
        }

        DebugPath();
    }

    public override void Idle() {
        // Wander anround randomly
        if (huntTime > 0) huntTime -= Time.deltaTime;
        if (huntSpeed > 1.0f) huntSpeed -= Time.deltaTime * 2.5f;
        if (acselSh > 0) acselSh -= Time.deltaTime * 0.1f;
        if (rotationSpeed > 1f) rotationSpeed -= Time.deltaTime * 1f;

        targetCurent = targetRandom;
        targetPos = Vector3.SmoothDamp(targetPos, targetCurent, ref vel, 3.0f);
        Vector3 dir = (targetPos - transform.position);
        transform.forward = Vector3.RotateTowards(transform.forward, dir, Time.deltaTime * rotationSpeed, 0f);
        transform.Translate(Vector3.forward * Time.deltaTime * huntSpeed * speed);
    }

    public override GameObject Find(int layer) {
        Collider[] found = Physics.OverlapSphere(transform.position, viewRadius, layer);
        if (found.Length > 0) {
            int pos = Random.Range(0, found.Length);
            GameObject newTarget = found[pos].gameObject;
            if (newTarget.GetComponent<Terrain>() == null)
                return newTarget;
        }
        Idle();
        return null;
    }

    public override bool Chase() {
        if (hunting) {
            if (huntTime < 0.5f) huntTime += Time.deltaTime * 0.5f;
            else return true;

            targetCurent = target.transform.position;
            if (huntSpeed < 10.1f) huntSpeed += Time.deltaTime * 2.5f;
            if (acselSh < 0.6f) acselSh += Time.deltaTime * 0.1f;
            if (rotationSpeed < 2.5f) rotationSpeed += Time.deltaTime * 1f;

            Hunt();
            return false;
        } else 
            return base.Chase();
    }

    void Hunt() {
        targetPos = targetCurent;
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.forward = Vector3.RotateTowards(transform.forward, dir, Time.deltaTime * rotationSpeed, 0f);
        transform.Translate(Vector3.forward * Time.deltaTime * huntSpeed * speed);
    }

    public override void Feed(LivingEntity targetStats) {
        hunger -= targetStats.feedWorth * eatRate / 100 * Time.deltaTime;
        Destroy(targetStats.gameObject);
    }

    IEnumerator RandomVector() {
        while (true) {
            targetRandom = Random.insideUnitSphere * walkZone;
            targetRandom.y = Mathf.Clamp(targetRandom.y, 10, 30);
            yield return delay0;
        }
    }

    void DebugPath() {
        if (debug == true) Debug.DrawLine(transform.position, targetCurent);
    }
}
