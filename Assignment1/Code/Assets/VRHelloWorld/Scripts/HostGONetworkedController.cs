using Mirror;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player object controller that will be created on the host machine (desktop).
/// It will be controlled using the keyboard. The game object has an attachment 
/// point to which the VR Camera (client) object will be attached. This child
/// object must be a child of hostGO and be tagged as "AttachmentPoint".
/// </summary>
public class HostGONetworkedController : NetworkBehaviour
{
    // Update is called once per frame.
    void Update()
    {
        // If this instance is not the host instance, exit.
        if (!isServer)
            return;

        // This is the host instance, so allow the user to control it using
        // the keyboard.

        // Get amount the left-right (horizontal) and up-down (vertical)
        // controls have been pressed.
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        // Rotate and move forward (if host).
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }
}