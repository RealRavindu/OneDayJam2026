using Unity.Netcode;
using UnityEngine;

public class PlayerCharacterInfo : NetworkBehaviour
{
    public NetworkVariable<player> TetrisPlayer = new NetworkVariable<player>(player.none);
    public NetworkVariable<player> RaccoonPlayer = new NetworkVariable<player>(player.none);
    public static PlayerCharacterInfo instance;
    private void Start()
    {
        instance = this;
        DontDestroyOnLoad(this);

        TetrisPlayer.OnValueChanged += (previousValue, newValue) =>
        {
            if(RaccoonPlayer.Value == newValue)
            {
                RaccoonPlayer.Value = player.none;
            }
        };
        RaccoonPlayer.OnValueChanged += (previousValue, newValue) =>
        {
            if (TetrisPlayer.Value == newValue)
            {
                TetrisPlayer.Value = player.none;
            }
        };
    }
    
}
public enum player
{
    player1, player2, none
}
