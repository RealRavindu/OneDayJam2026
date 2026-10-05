using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LobbyCodeDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeDisplay, waitingMessage;

    [SerializeField] private float timeThreshold;
    private float time;
    private int i;
    [SerializeField] private Button backBtn;
    private void Start()
    {
        joinCodeDisplay.text = RelayConnector.Instance.joinCode;

        backBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("MainMenuScene");
        });
    }


    private void Update()
    {
        time += Time.deltaTime;
        if (time > timeThreshold)
        {
            i++;
            time = 0;
            switch (i)
            {
                case 0:
                    waitingMessage.text = "Waiting for player.";
                    break;
                case 1:
                    waitingMessage.text = "Waiting for player..";
                    break;
                case 2:
                    waitingMessage.text = "Waiting for player...";
                    break;
                default:
                    i = 0; 
                    break;
            }
        }
    }
}
