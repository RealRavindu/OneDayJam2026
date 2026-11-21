using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class ClientConnectionChecker : NetworkBehaviour
{
    //This class handles syncing the client and the host.
    private void Start()
    {
        Debug.Log($"I am Host: {IsHost}");
        if (IsHost) return;

        Debug.Log($"Running client loaded event");
        StartCoroutine(StartGameAfterOneSecond());
    }
    private IEnumerator StartGameAfterOneSecond()
    {
        yield return new WaitForSeconds(1);

        HandleClientLoadedRpc();
    }
    [Rpc(SendTo.Everyone)]
    private void HandleClientLoadedRpc()
    {
        Debug.Log("EVENT");
       EventCore.OnClientLoaded?.Invoke();
    }
}
