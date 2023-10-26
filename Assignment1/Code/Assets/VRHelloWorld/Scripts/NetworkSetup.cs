using Mirror;
using UnityEngine;

/// <summary>
/// The purpose of this class is to create a player object based on the type of
/// connection that has been established. If the connection is from the host:client
/// device, then the hostPlayerPrefab will be used. If the connection is from the
/// client only device, then the remoteClientPlayerPrefab will be used instead.
/// </summary>
public class NetworkSetup : NetworkManager
{
    [Header("Setup")]
    [Tooltip("The prefab that should be spawned as the HOST device's player.")]
    public GameObject hostPlayerPrefab;

    [Tooltip("The prefab that should be spawned as the CLIENT device's player.")]
    public GameObject remoteClientPlayerPrefab;

    // Called when the client wishes to add a player object to the scene.
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // base.OnServerAddPlayer(conn);

        // Is the connection on a device that ONLY has a client?
        // Typically the host:client device will have a connection ID == 0.
        bool isClientOnly = conn.connectionId != 0;

        // Determine which prefab to use (based on whether host:client or client only.
        GameObject prefab = isClientOnly ? remoteClientPlayerPrefab : hostPlayerPrefab;

        // Instantiate the object.
        GameObject player = Instantiate(prefab);

        // Create the player object associated with the connection.
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
