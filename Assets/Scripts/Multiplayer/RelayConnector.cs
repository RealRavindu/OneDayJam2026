using UnityEngine;
using Unity.Netcode;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using TMPro;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.VisualScripting;

public class RelayConnector : NetworkBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);

    }
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => Debug.Log("Authentication has signed in, player ID: " + AuthenticationService.Instance.PlayerId);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    public TextMeshProUGUI joinCodeTextBox;

    [Serialize]
    public string joinCode
    {
        get { return _joinCode; }
        set
        {
            _joinCode = value;
            joinCodeTextBox.text = _joinCode;
        }
    }
    private string _joinCode;
    public async void CreateRelay()
    {

        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            RelayServerData relayServerData = allocation.ToRelayServerData("dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            //MainMenuManager.instance.ShowHostingLobbyGroup();
        } catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinRelay()
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerData relayServerData = joinAllocation.ToRelayServerData("dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();

        } catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
