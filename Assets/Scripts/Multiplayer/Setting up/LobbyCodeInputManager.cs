using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyCodeInputManager : MonoBehaviour
{
    public Button connectBtn, backBtn;
    public TMP_InputField inputField;
    private void Start()
    {
        inputField.onValueChanged.AddListener((string value) =>
        {
            inputField.SetTextWithoutNotify(value.ToUpper());
            if (value.Length == 6)
            {
                connectBtn.interactable = true;
            } else
            {
                connectBtn.interactable = false;
            }
        });
        connectBtn.onClick.AddListener(() =>
        {
            RelayConnector.Instance.JoinRelay(inputField.text);
        });

        backBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
    }

}
