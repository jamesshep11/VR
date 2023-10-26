using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AddWood : NetworkBehaviour
{
    public float range = 10f;
    public float force = 50f;

    public GameObject projectile;
    public GameObject aimFor;
    private GameObject fire;

    private void Start() {
        fire = GameObject.Find("SparkEffect");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.X)) {
            DropWood();
        }
    }

    [ClientRpc]
    private void DropWood() {
        if (GetComponent<FirstPersonController>().action == FirstPersonController.Action.AddWood) {
            if (aimFor == null) {
                Vector3 startPos = transform.position + transform.forward;

                GameObject thrownObj = Instantiate(projectile, startPos, transform.rotation);
                NetworkServer.Spawn(thrownObj);
                thrownObj.GetComponent<Rigidbody>().AddForce(transform.forward * force);

                StartCoroutine(waitThenDestroy(2, thrownObj));

                Debug.Log(fire.transform.Find("Fire").name);
                fire.transform.Find("Fire").GetComponent<ParticleSystem>().Play();
                fire.transform.Find("FireParticle").GetComponent<ParticleSystem>().Play();
            } else {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, range)) {
                    if (hit.transform.name == aimFor.name) {
                        Vector3 startPos = transform.position + transform.forward;

                        GameObject thrownObj = Instantiate(projectile, startPos, transform.rotation);

                        Vector3 distance = transform.position - hit.transform.position;
                        float power = Mathf.Sqrt(distance.sqrMagnitude) * force;
                        thrownObj.GetComponent<Rigidbody>().AddForce(transform.forward * power);

                        StartCoroutine(waitThenDestroy(2, thrownObj));

                        Debug.Log(fire.transform.Find("Fire").name);
                        fire.transform.Find("Fire").GetComponent<ParticleSystem>().Play();
                        fire.transform.Find("FireParticle").GetComponent<ParticleSystem>().Play();
                    }
                }
            }
        }
    }

    IEnumerator waitThenDestroy(float seconds, GameObject destroy) {
        yield return new WaitForSeconds(seconds);

        Destroy(destroy);
    }

}
