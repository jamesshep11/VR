using Mirror;
using UnityEngine;

public class Gun : NetworkBehaviour
{
    public float range = 10f;
    public float force = 50f;

    public GameObject projectile;
    public GameObject aimFor;

    private void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) {
            Shoot();
        }
    }

    [ClientRpc]
    private void Shoot() {
        if (GetComponent<FirstPersonController>().action == FirstPersonController.Action.ThrowAxe) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, range)) {
                if (aimFor == null || hit.transform.name == aimFor.name) {
                    Vector3 startPos = transform.position + transform.forward;

                    GameObject thrownObj = Instantiate(projectile, startPos, new Quaternion(transform.rotation.x, transform.rotation.y - 1f, transform.rotation.z, transform.rotation.w));

                    thrownObj.GetComponent<Rigidbody>().isKinematic = false;
                    Vector3 distance = transform.position - hit.transform.position;
                    thrownObj.GetComponent<Rigidbody>().AddForce(transform.forward * force);

                    NetworkServer.Spawn(thrownObj);
                }
            }
        }
    }
}
