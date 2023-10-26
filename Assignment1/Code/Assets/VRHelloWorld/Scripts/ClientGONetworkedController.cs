using UnityEngine;
using Mirror;
using Mirror.Discovery;

public class ClientGONetworkedController : NetworkBehaviour
{
    // A reference to the VR Camera, named "VRCamera" that is has the
    // Vuforia head tracking and depth rendering components attached.
    private Camera VRCamera;

    // The position of the "head" on the hostGO to which the VR Camera
    // will be attached.
    public Transform hostGOAttachmentPoint;

    private void FindVRCamera()
    {
        Debug.Log("Finding VR Camera in scene...");

        VRCamera = GameObject.Find("VRCamera").GetComponent<Camera>();

        if (VRCamera == null)
            Debug.LogError("Could not find VR Camera in the current scene.");
    }

    private void AttachVRCamera()
    {
        Debug.Log("Attaching VR Camera to HostGO head attachment point...");

        // Find the attachment point of the HostGO.
        hostGOAttachmentPoint = GameObject.FindGameObjectWithTag("AttachmentPoint").transform;

        // Attach VRCamera's parent to HostGO's attachment point
        VRCamera.transform.parent.parent = hostGOAttachmentPoint;
        VRCamera.transform.parent.localPosition = Vector3.zero;
        VRCamera.transform.parent.localRotation = Quaternion.identity;

        // Just for neatness, parent the ClientGO to the HostGO as well.
        gameObject.transform.parent = hostGOAttachmentPoint.parent;
        gameObject.transform.parent.localPosition = Vector3.zero;
        gameObject.transform.parent.localRotation = Quaternion.identity;
    }

    private void HideHUDs()
    {
        NetworkManagerHUD nwHUD = GameObject.FindObjectOfType<NetworkManagerHUD>();
        nwHUD.enabled = false;

        NetworkDiscoveryHUD nwdHUD = GameObject.FindObjectOfType<NetworkDiscoveryHUD>();
        nwdHUD.enabled = false;

        GameObject.Find("Cube").GetComponent<MeshRenderer>().enabled = false;
    }

    // Use this for initialization on the CLIENT device.
    void Start()
    {
        if (isServer)
            return;

        // Configure scene of CLIENT.
        FindVRCamera();
        AttachVRCamera();
        HideHUDs();

        // Switch on VR.
        VrModeController vrc = GameObject.FindObjectOfType<VrModeController>();
        vrc.EnterVR();
    }
}
