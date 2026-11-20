using Unity.Netcode;
using UnityEngine;

public class CharOwnershipManager : MonoBehaviour
{
    public NetworkObject Raccoon, TetrisController, TetrisManager;
    private void Start()
    {
        if (!NetworkManager.Singleton.IsHost) return;
        SetOwnershipOfCharactersToRelevantPlayers();
        
    }
    

    private void SetOwnershipOfCharactersToRelevantPlayers()
    {
        Debug.Log("Setting ownership");
        if(PlayerCharacterInfo.instance.RaccoonPlayer.Value == PlayerCharacterInfo.player.player1)
        {
            Debug.Log("Player 1 (Host) is Raccoon");
            Raccoon.ChangeOwnership(NetworkManager.Singleton.LocalClientId);
            TetrisController.ChangeOwnership(NetworkManager.Singleton.ConnectedClientsIds[1]);
            TetrisManager.ChangeOwnership(NetworkManager.Singleton.ConnectedClientsIds[1]);
        } else
        {
            Debug.Log("Player 1 (Host) is Tetris");
            Raccoon.ChangeOwnership(NetworkManager.Singleton.ConnectedClientsIds[1]);
            TetrisController.ChangeOwnership(NetworkManager.Singleton.LocalClientId);
            TetrisManager.ChangeOwnership(NetworkManager.Singleton.LocalClientId);
        }
    }
}
