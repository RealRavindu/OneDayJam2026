using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public class PlayerCharacterInfo : NetworkBehaviour
{
    public NetworkVariable<player> TetrisPlayer = new NetworkVariable<player>(player.none);
    public NetworkVariable<player> RaccoonPlayer = new NetworkVariable<player>(player.none);
    public static PlayerCharacterInfo instance;
    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

    }
    [Rpc(SendTo.Server)]
    public void SelectTetrisForPlayerServerRpc(player _player)
    {
        if (RaccoonPlayer.Value == _player)
        {
            RaccoonPlayer.Value = player.none;
        }
        TetrisPlayer.Value = _player;
    }
    [Rpc(SendTo.Server)]
    public void SelectRaccoonForPlayerServerRpc(player _player)
    {
        if (TetrisPlayer.Value == _player)
        {
            TetrisPlayer.Value = player.none;
        }
        RaccoonPlayer.Value = _player;
    }
    [Rpc(SendTo.Server)]
    public void UnselectForPlayerServerRpc(player _player)
    {
        if (TetrisPlayer.Value == _player)
        {
            TetrisPlayer.Value = player.none;
        }
        if (RaccoonPlayer.Value == _player)
        {
            RaccoonPlayer.Value = player.none;
        }
    }
    public bool HasPlayerMadeASelection(player _player)
    {
        return (TetrisPlayer.Value == _player || RaccoonPlayer.Value == _player);
    }
    public character WhoDidPlayerSelect(player _player)
    {
        if (TetrisPlayer.Value == _player) return character.tetris;
        else if (RaccoonPlayer.Value == _player) return character.raccoon;
        else return character.none;
    }
    public enum player
    {
        player1, player2, none
    }

    public enum character
    {
        raccoon, tetris, none
    }
}



