using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class LobbySelection : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] PlayerType playerType;
    public Selected playerInfo;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Mouse clicked");
        if (!playerInfo.isAlreadySelected && IsOwner)
        {
            playerInfo.isAlreadySelected = true;
            Debug.Log($"Player is owner and flipping ownership of this object to true {playerInfo.isAlreadySelected}");
        }
    }
}
public class Selected : NetworkBehaviour
{
    public bool isAlreadySelected = false;
}
public enum PlayerType
{
    TetrisPlayer, RaccoonPlayer
}
