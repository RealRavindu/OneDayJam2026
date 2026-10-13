using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : NetworkBehaviour
{
    [SerializeField] private Button RaccoonBtn, TetrisBtn;
    [SerializeField] private Cursor p1Cursor, p2Cursor;

    private void Start()
    {
        RaccoonBtn.onClick.AddListener(() =>
        {
            ClickedRaccoonButtonServerRpc();
        });
        TetrisBtn.onClick.AddListener(() =>
        {
            ClickedTetrisButtonServerRpc();
        });

        if (!IsServer) return;
        Debug.Log("Server is assigning cursors to players");
        //NetworkManager.SpawnManager.InstantiateAndSpawn(NetworkManager.Singleton.SpawnManager)
        p1Cursor.NetworkObject.ChangeOwnership(NetworkManager.ConnectedClientsIds[0]);
        p2Cursor.NetworkObject.ChangeOwnership(NetworkManager.ConnectedClientsIds[1]);
    }

    [ServerRpc]
    private void ClickedRaccoonButtonServerRpc()
    {
        Debug.Log($"User {NetworkManager.LocalClientId} has clicked the raccoon button");
    }
    [ServerRpc]
    private void ClickedTetrisButtonServerRpc()
    {
        Debug.Log($"User {NetworkManager.LocalClientId} has clicked the tetris button");
    }

}
