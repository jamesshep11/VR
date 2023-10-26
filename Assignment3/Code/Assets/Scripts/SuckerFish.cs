using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SuckerFish : Animal
{
    NavMeshAgent agent;

    public float wanderTimer = 3f;

    GameObject player;
    GameObject ground;
    float visibleRange = 30f;
    float safeDistance = 10f;
    float playerDistance;

    float refreshTime;

    public override void Start() {
        base.Start();

        agent = gameObject.GetComponent<NavMeshAgent>();
        ground = GameObject.FindGameObjectWithTag("Ground");
        grounded = true;

        refreshTime = 0f;
    }

    public override void Idle() {
        // Wander anround randomly
        refreshTime -= Time.deltaTime;
        if (refreshTime <= 0) {
            Vector3 newPos = RandomNavSphere(transform.position, viewRadius, -1);
            agent.SetDestination(newPos);
            refreshTime = wanderTimer;
        }
    }

    public override bool Chase() {
        refreshTime -= Time.deltaTime;
        if (refreshTime <= 0) {
            agent.SetDestination(target.transform.position);
            refreshTime = 0.5f;
        }

        return agent.remainingDistance <= agent.stoppingDistance;
    }

    public override void Flee(Animator animator) {
        Collider[] enemyColliders = Physics.OverlapSphere(gameObject.transform.position, visibleRange, enemies);

        if (enemyColliders.Length > 0) {
            playerDistance = closestEnemy(enemyColliders);

            if (InVisibleRange() && !IsHidden())
                FindHidingPlace();
            refreshTime = wanderTimer;
        } else {
            refreshTime -= Time.deltaTime;
            if (refreshTime <= 0)
                animator.SetBool("Flee", false);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    float closestEnemy(Collider[] enemies) {
        player = enemies[0].gameObject;
        float distance = visibleRange;
        float curDist;
        foreach (Collider enemy in enemies) {
            curDist = (gameObject.transform.position - enemy.transform.position).magnitude;
            if (curDist < distance) {
                distance = curDist;
                player = enemy.gameObject;
            }
        }

        return distance;
    }

    bool FindHidingPlace() {
        // get all the vectors on the edge of the Navmesh
        var borderVectors = ground.GetComponent<GetNavmeshEdges>().BorderVectors;

        // load into objects with distance from player
        var bvects = borderVectors.Select(v => new NavEdge {
            Distance = (v - gameObject.transform.position).magnitude,
            Position = v
        }).OrderBy(a => a.Distance); ;

        foreach (var e in bvects) {
            // is hidden and a safe distance from the player
            var dist = (e.Position - player.transform.position).magnitude;
            if (IsPosHidden(e.Position)) {
                // move to static location
                agent.SetDestination(e.Position);
                return true;
            }
        }
        // could not find a hiding place
        return false;
    }

    bool InVisibleRange() {
        var inRange = playerDistance <= visibleRange;

        return inRange;
    }

    bool IsHidden() {
        var ishidden = IsPosHidden(gameObject.transform.position);
        return ishidden;
    }

    bool IsPosHidden(Vector3 pos) {
        var distToPlayer = player.transform.position - pos;

        RaycastHit hit;
        bool seeWall = false;

        if (Physics.Raycast(pos, distToPlayer, out hit)) {
            seeWall = (hit.collider.gameObject.layer == LayerMask.NameToLayer("Solid"));
        }

        var seePlayer = distToPlayer.magnitude < visibleRange && !seeWall;
        var tooClose = distToPlayer.magnitude < safeDistance;

        return !seePlayer && !tooClose;
    }
}

public class NavEdge
{
    public Vector3 Position { set; get; }
    public float Distance { set; get; }
}
