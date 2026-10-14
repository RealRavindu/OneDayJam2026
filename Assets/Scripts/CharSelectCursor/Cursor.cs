using UnityEngine;
using Unity.Netcode;

public class Cursor : NetworkBehaviour
{
    public Transform tetrisWaypointTransform, raccoonWaypointTransform;
    private Vector3 tetrisWaypoint, raccoonWaypoint, middlePoint;
    private PlayerCharacterInfo.player Player;
    private float currentAxisValue =0;
    private void Start()
    {
        tetrisWaypoint = tetrisWaypointTransform.position;
        raccoonWaypoint = raccoonWaypointTransform.position;
        middlePoint = transform.position;
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
        if(Input.GetAxisRaw("Vertical")<0 && currentAxisValue == 0) //down
        {
            UnselectCharacter();
        }
        currentAxisValue = (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).magnitude);
    }

    private void SelectCharacter(NetworkVariable<PlayerCharacterInfo.player> SelectedCharacter, Vector3 waypoint)
    {
        if (SelectedCharacter.Value == PlayerCharacterInfo.player.none)
        {
            Debug.Log("Selecting character");
            transform.position = waypoint;

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
            Debug.Log($"Can't go to waypoint {waypoint}, already selected by other player");
        }
    }

    private void UnselectCharacter()
    {
        Debug.Log("Unselecting character");
        transform.position = middlePoint;
        PlayerCharacterInfo.instance.UnselectForPlayerServerRpc(Player);
    }
}
