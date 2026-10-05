using System;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : NetworkBehaviour
{
    public Button HostBtn, JoinBtn, QuitBtn;
    private void Start()
    {
        HostBtn.onClick.AddListener(() =>
        {
            RelayConnector.Instance.CreateRelay();
        });

        JoinBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("JoiningLobby");
        });
        QuitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
