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
    public static MainMenuManager instance;
    Action pressedEscape;
    //scenarios:
    //started hosting: back to main menu. quit host. Hide showCodeToPrendsGroup
    //started joining: back to main menu. Erase inputField text. Hide inputFieldGroup.
    //at main menu: quit (yet to be implemented)
    [SerializeField] private CanvasGroup hostingLobbyGroup;
    [SerializeField] private CanvasGroup inputFieldGroup;
    [SerializeField] private CanvasGroup buttonsGroup;
    [SerializeField] private int numOfCharactersInAJoinCode;
    [SerializeField] private Button connectButton;
    [SerializeField] private TMP_InputField inputField;
    private string joinCodeInput
    {
        get { return inputField.text; }
        set
        {
            if (value.Length == numOfCharactersInAJoinCode) connectButton.interactable = true;
            else connectButton.interactable = false;
        }
    }

    private void Start()
    {
        instance = this;
        pressedEscape += ShowButtonsGroup;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pressedEscape.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Return) && joinCodeInput.Length == numOfCharactersInAJoinCode)
        {
            connectButton.onClick.Invoke();
            HideInputFieldGroup();
        }

    }
    
    public void ShowHostingLobbyGroup()
    {
        hostingLobbyGroup.alpha = 1;
        hostingLobbyGroup.blocksRaycasts = true;
        HideButtonsGroup();
        pressedEscape += HideHostingLobbyGroup;
    }
    public void HideHostingLobbyGroup()
    {
        hostingLobbyGroup.blocksRaycasts = false;
        hostingLobbyGroup.alpha = 0;
        NetworkManager.Singleton.Shutdown();
        pressedEscape -= HideHostingLobbyGroup;
    }

    public void ShowInputFieldGroup()
    {
        inputFieldGroup.alpha = 1;
        inputFieldGroup.blocksRaycasts = true;
        HideButtonsGroup();
        pressedEscape += HideInputFieldGroup;
    }

    public void HideInputFieldGroup()
    {
        inputFieldGroup.blocksRaycasts = false;
        inputFieldGroup.alpha = 0;
        pressedEscape -= HideInputFieldGroup;
        NetworkManager.Singleton.Shutdown();
    }
    public void ShowButtonsGroup()
    {
        buttonsGroup.blocksRaycasts = true;
        buttonsGroup.alpha = 1;
    }
    public void HideButtonsGroup()
    {
        buttonsGroup.blocksRaycasts = false;
        buttonsGroup.alpha = 0;
    }

}
