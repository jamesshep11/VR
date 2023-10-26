using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : LivingEntity {
    
    public float SpawnIntervals = 50f;
    public List<StageOfLife> SpawnStages;

    float timer = 0;
    Animator animator;

    public override void Start() {
        base.Start();

        grounded = true;
        animator = GetComponent<Animator>();
    }

    public override void Update() {
        base.Update();

        if(SpawnStages.Contains(stage))
            if (timer <= 0) {
                animator.SetTrigger("Repro");
                timer = SpawnIntervals;
            } else timer -= Time.deltaTime;

        if (transform.localScale.magnitude < (ogScale * startSize).magnitude)
            Die();

        if (transform.position.y < 0)
            Die();
    }
}