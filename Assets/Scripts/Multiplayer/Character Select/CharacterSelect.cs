using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelect : NetworkBehaviour
{
    public static CharacterSelect instance;
    [SerializeField] Button RaccoonBtn, TetrisBtn;
    [SerializeField] private Cursor p1Cursor, p2Cursor;
    private NetworkVariable<bool> p1IsReady = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> p2IsReady = new NetworkVariable<bool>(false);
    [SerializeField] private float countdownTillGameBegins = 5;
    private Coroutine countdownToBeginGameCR;
    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (!IsServer) return;
        Debug.Log("Server is assigning cursors to players");
        p1Cursor.NetworkObject.ChangeOwnership(NetworkManager.ConnectedClientsIds[0]);
        p2Cursor.NetworkObject.ChangeOwnership(NetworkManager.ConnectedClientsIds[1]);

        p1IsReady.OnValueChanged += (previousValue, newValue) =>
        {
            ReadyCheck();

            if (newValue) //readied up
            {
                //visual update yet to be implemented. Need to signify readying up.
                //Thinking of selecting the button under whatever selection to show readied state.
                //check which button player has selected and select that button.
                if (PlayerCharacterInfo.instance.TetrisPlayer.Value == PlayerCharacterInfo.player.player1)
                {
                    TetrisBtn.Select();
                }else
                {
                    RaccoonBtn.Select();
                }
            } else
            {

            }
        };
        p2IsReady.OnValueChanged += (previousValue, newValue) =>
        {
            ReadyCheck();

            if (newValue) //readied up
            {
                //visual update yet to be implemented. Need to signify readying up.
                //Thinking of selecting the button under whatever selection to show readied state.
                //check which button player has selected and select that button.
            }
        };
    }

    [Rpc(SendTo.Server)]
    public void ChangeReadyStateServerRpc(RpcParams rpcParams = default)
    {
        PlayerCharacterInfo.player _player = PlayerCharacterInfo.player.none;
        ulong senderId = rpcParams.Receive.SenderClientId;
        if (senderId == p1Cursor.OwnerClientId)
        {
            _player = PlayerCharacterInfo.player.player1;
        }
        else if (senderId == p2Cursor.OwnerClientId)
        {
            _player = PlayerCharacterInfo.player.player2;
        }
        else return;


        if (!PlayerCharacterInfo.instance.HasPlayerMadeASelection(_player))
        {
            Debug.Log("Player has not made a selection yet");
            return;

        }

        if (_player == PlayerCharacterInfo.player.player1)
        {
            p1IsReady.Value = !p1IsReady.Value;
        }
        else
        {
            p2IsReady.Value = !p2IsReady.Value;
        }
    }

    public bool GetPlayerReadyState(PlayerCharacterInfo.player _player)
    {
        if (_player == PlayerCharacterInfo.player.player1)
        {
            return p1IsReady.Value;
        }
        else
        {
            return p2IsReady.Value;
        }
    }

    private void ReadyCheck()
    {
        if (p1IsReady.Value && p2IsReady.Value)
        {
            if (countdownToBeginGameCR == null)
            {
                countdownToBeginGameCR = StartCoroutine(CountdownAndBeginGame());
            }
        }
        else
        {
            if (countdownToBeginGameCR != null)
            {
                StopCoroutine(CountdownAndBeginGame());
            }
            countdownToBeginGameCR = null;
        }
    }

    private IEnumerator CountdownAndBeginGame()
    {
        float t = 0;
        while (t < countdownTillGameBegins)
        {
            t += Time.deltaTime;
            yield return null;
        }
        NetworkManager.Singleton.SceneManager.LoadScene("NewTetris", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
