using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Linq;
using System.Collections;

//HOSTING LOBBY SCRIPT
public class LobbyCodeDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeDisplay, waitingMessage;
    [SerializeField] private string waitingMessageString;
    [SerializeField] private float timeThreshold;
    private float time;
    private int i;
    [SerializeField] private Button backBtn;
    private void Start()
    {
        joinCodeDisplay.text = RelayConnector.Instance.joinCode;
        waitingMessage.text = waitingMessageString;
        StartCoroutine(WaitingForPlayersTextAnimation());
        backBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("MainMenuScene");
        });
    }


    private IEnumerator WaitingForPlayersTextAnimation()
    {
        int i = 0;
        while (true)
        {
            if (i < 3)
            {
                i++;
                waitingMessage.text = waitingMessage.text.Insert(waitingMessage.text.Length, ".");
            }
            else 
            {
                i = 0;
                waitingMessage.text = waitingMessageString;
            }
            yield return new WaitForSeconds(timeThreshold);
        }
    }

}
