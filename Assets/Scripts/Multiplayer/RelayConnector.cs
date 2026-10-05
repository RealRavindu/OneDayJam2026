using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RelayConnector : MonoBehaviour
{
    public static RelayConnector Instance;
    public string joinCode;
    private async void Start()
    {
        Instance = this;

        DontDestroyOnLoad(this);

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => Debug.Log("Authentication has signed in, player ID: " + AuthenticationService.Instance.PlayerId);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        SceneManager.LoadScene(1);
    }
    public async void CreateRelay()
    {

        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            RelayServerData relayServerData = allocation.ToRelayServerData("dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            SceneManager.LoadScene("HostingLobby");
        } catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinRelay(string _joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(_joinCode);
            RelayServerData relayServerData = joinAllocation.ToRelayServerData("dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
            //tell host that you have connected and to proceed to character select screen here
            ProceedToCharacterSelectServerRPC();
            SceneManager.LoadScene("CharacterSelect");

        } catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    [ServerRpc]
    private void ProceedToCharacterSelectServerRPC()
    {
        Debug.Log("Proceeding to character select since a client has connected");
        SceneManager.LoadScene("CharacterSelect");
    }
}


