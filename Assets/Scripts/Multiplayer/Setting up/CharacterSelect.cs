using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : NetworkBehaviour
{
    [SerializeField] private Button RaccoonBtn, TetrisBtn;

    private void Start()
    {
        RaccoonBtn.onClick.AddListener(() =>
        {
            ClickedRaccoonButtonServerRpc();
        });

        if (!IsServer) return;
        //NetworkManager.SpawnManager.InstantiateAndSpawn(NetworkManager.Singleton.SpawnManager)
    }

    [ServerRpc]
    private void ClickedRaccoonButtonServerRpc()
    {
        Debug.Log($"User {NetworkManager.LocalClientId} has clicked the raccoon button");
    }
    
}
