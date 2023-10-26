using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnState : StateMachineBehaviour
{
    public enum GizmoType { Never, SelectedOnly, Always }

    public GameObject prefab;
    public Transform parent = null;
    public float spawnRadius = 10f;
    public int minSpawnCount = 1;
    public int maxSpawnCount = 1;
    [Range(0, 1)]
    public float spawnProbability = 0.7f;
    public GizmoType showSpawnRegion;

    Transform transform;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform = animator.gameObject.transform;
        LivingEntity stats = animator.gameObject.GetComponent<LivingEntity>();

        // Check pobability of spawning
        float spawn = Random.Range(0f, 1f);
        if (spawn <= spawnProbability) {
            // Randomize child count
            int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
            for (int i = 0; i < spawnCount; i++) {
                // Get a random point within the spawn radius
                Vector3 pos = Random.insideUnitSphere * spawnRadius;
                pos += transform.position;

                // Map spawn point to mesh
                if (stats.grounded) {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(pos, out hit, spawnRadius, NavMesh.AllAreas))
                        pos = hit.position;
                    else
                        continue;
                }

                // Spawn child
                if (parent == null) parent = transform.parent;
                GameObject obj = prefab == null ? Instantiate(stats.prefab, parent) : Instantiate(prefab, parent);
                obj.transform.position = pos;
            }
        }
    }

    private void OnDrawGizmos() {
        if (showSpawnRegion == GizmoType.Always) {
            DrawGizmos();
        }
    }

    void OnDrawGizmosSelected() {
        if (showSpawnRegion == GizmoType.SelectedOnly) {
            DrawGizmos();
        }
    }

    void DrawGizmos() {

        Gizmos.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0.3f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }
}
