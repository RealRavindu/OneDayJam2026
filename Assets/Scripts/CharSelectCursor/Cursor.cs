using UnityEngine;
using Unity.Netcode;

public class Cursor : NetworkBehaviour
{
    public Transform tetrisWaypoint, raccoonWaypoint;
    private PlayerCharacterInfo.player Player;
    private float currentAxisValue =0;
    private void Start()
    {
        if (OwnerClientId == 0) Player = PlayerCharacterInfo.player.player1;
        else Player = PlayerCharacterInfo.player.player2;
    }
    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetAxisRaw("Horizontal") > 0 && currentAxisValue ==0) //right
        {
            SelectCharacter(PlayerCharacterInfo.instance.TetrisPlayer, tetrisWaypoint);
        }
        if (Input.GetAxisRaw("Horizontal") < 0 && currentAxisValue == 0) //left
        {
            SelectCharacter(PlayerCharacterInfo.instance.RaccoonPlayer, raccoonWaypoint);
        }
        currentAxisValue = Input.GetAxisRaw("Horizontal");
    }

    private void SelectCharacter(NetworkVariable<PlayerCharacterInfo.player> SelectedCharacter, Transform waypoint)
    {
        if (SelectedCharacter.Value == PlayerCharacterInfo.player.none)
        {
            transform.position = waypoint.position;
            if(SelectedCharacter == PlayerCharacterInfo.instance.RaccoonPlayer)
            {
                PlayerCharacterInfo.instance.SelectRaccoonForPlayerServerRpc(Player);
            } else
            {
                PlayerCharacterInfo.instance.SelectTetrisForPlayerServerRpc(Player);
            }
            
        }
        else
        {
            Debug.Log($"Can't go to waypoint {waypoint.gameObject.name}, already selected");
        }
    }
}
