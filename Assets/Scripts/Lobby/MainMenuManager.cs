using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string joinCode;
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void ClickedHost()
    {
        joinCode = StartHostWithRelay(1, "udp").ToString();
    }

    public void ClickedJoin()
    {
        _ = StartClientWithRelay(joinCode, "udp");
    }

    public async Task<string> StartHostWithRelay(int maxConnections, string connectionType)
    {
        await UnityServices.InitializeAsync();
        Debug.Log("Unity services initialized");
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        var allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        Debug.Log("allocation data set");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, connectionType));
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        Debug.Log("join code gotted");
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    public async Task<bool> StartClientWithRelay(string joinCode, string connectionType)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, connectionType));
        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }

    private void Update()
    {
        if (NetworkManager.Singleton.IsHost) Debug.Log("I am hosting");
        if (NetworkManager.Singleton.IsClient) Debug.Log("I am clienting");
        if (NetworkManager.Singleton.IsConnectedClient) Debug.Log("I am connected clienting?");
        if (NetworkManager.Singleton.IsServer) Debug.Log("I am servering");
        if (NetworkManager.Singleton.IsApproved) Debug.Log("I am approved?");
    }


}
