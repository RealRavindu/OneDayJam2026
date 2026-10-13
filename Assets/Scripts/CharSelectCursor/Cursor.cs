using UnityEngine;
using Unity.Netcode;

public class Cursor : NetworkBehaviour
{
    public Transform tetrisWaypoint, raccoonWaypoint;
    private player Player;
    private void Start()
    {
        if (OwnerClientId == 0) Player = player.player1;
        else Player = player.player2;
    }
    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetAxisRaw("Horizontal") > 0) //right
        {
            SelectCharacter(PlayerCharacterInfo.instance.TetrisPlayer);
        }
        if (Input.GetAxisRaw("Horizontal") < 0) //left
        {
            SelectCharacter(PlayerCharacterInfo.instance.RaccoonPlayer);
        }
    }

    private void SelectCharacter(NetworkVariable<player> SelectedCharacter)
    {
        if (SelectedCharacter.Value == player.none)
        {
            transform.position = raccoonWaypoint.position;
            //ChangeWhoSelectedCharacterServerRpc(SelectedCharacter);
            SelectedCharacter.Value = Player;
        }
        else
        {
            Debug.Log("Can't go to raccoon, already selected");
        }
    }

    [ServerRpc(RequireOwnership =false)]
    private void ChangeWhoSelectedCharacterServerRpc(player SelectedCharacter)
    {
        SelectedCharacter = Player;
    }
}
