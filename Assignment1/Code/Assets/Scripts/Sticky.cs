using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Wood-10tier log_s") {
            GetComponent<Rigidbody>().isKinematic = true;
            var stickyObj = gameObject.AddComponent<FixedJoint>();
            stickyObj.connectedBody = collision.rigidbody;

            Destroy(this);
        }
    }
}
