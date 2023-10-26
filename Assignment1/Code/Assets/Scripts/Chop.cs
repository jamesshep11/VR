using Mirror;
using UnityEngine;

public class Chop : NetworkBehaviour
{

    public float range = 10f;

    public GameObject chopWith;
    public GameObject replaceWith;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) {
            DoChop();
        }
    }

    [ClientRpc]
    private void DoChop() {
        if (GetComponent<FirstPersonController>().action == FirstPersonController.Action.ChopTree) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, range)) {
                if (hit.transform.CompareTag("tree")) {
                    // play axe animation
                    chopWith.GetComponent<Animator>().Play("SwingAxe");
                    
                    // replace tree with stump
                    GameObject replacement = Instantiate(replaceWith, hit.transform.position, hit.transform.rotation);
                    replacement.transform.localScale = new Vector3(2, 2, 2);
                    Destroy(hit.transform.gameObject);
                    NetworkServer.Spawn(replacement);

                    // hide crosshair
                    GetComponent<FirstPersonController>().action = FirstPersonController.Action.None;
                    GetComponent<FirstPersonController>().ShowCrosshair(false);
                }
            }
        }
    }
}
