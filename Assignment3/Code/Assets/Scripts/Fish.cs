using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Animal
{
    Boid boid;

    public override void Start() {
        base.Start();

        boid = GetComponent<Boid>();
        boid.flocking = true;
    }

    public override void Idle() {
        boid.acceleration = calcAcceleration();
    }

    public override void Flee(Animator animator) {
        Collider[] enemies = Physics.OverlapSphere(boid.position, boid.settings.cautionRadius, boid.settings.enemyMask);

        if (enemies.Length > 0) {
            Vector3 fleeVector = Vector3.zero;

            foreach (Collider enemy in enemies)
                fleeVector += (boid.position - enemy.transform.position);
            fleeVector /= enemies.Length;

            boid.acceleration += SteerTowards(fleeVector) * boid.settings.fleeWeight;
        } else {
            animator.SetBool("Flee", false);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Vector3 calcAcceleration() {
        Vector3 acceleration = Vector3.zero;

        if (boid.target != null) {
            Vector3 offsetToTarget = (boid.target.position - boid.position);
            acceleration = SteerTowards(offsetToTarget) * boid.settings.targetWeight;
        }

        if (boid.numPerceivedFlockmates != 0) {
            boid.centreOfFlockmates /= boid.numPerceivedFlockmates;

            Vector3 offsetToFlockmatesCentre = (boid.centreOfFlockmates - boid.position);

            var alignmentForce = SteerTowards(boid.avgFlockHeading) * boid.settings.alignWeight;
            var cohesionForce = SteerTowards(offsetToFlockmatesCentre) * boid.settings.cohesionWeight;
            var seperationForce = SteerTowards(boid.avgAvoidanceHeading) * boid.settings.seperateWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }

        return acceleration;
    }

    Vector3 SteerTowards(Vector3 vector) {
        Vector3 v = vector.normalized * boid.settings.maxSpeed - boid.velocity;
        return Vector3.ClampMagnitude(v, boid.settings.maxSteerForce);
    }
}
