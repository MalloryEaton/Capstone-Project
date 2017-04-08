using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour {

    public GameObject LoadingScreen;
    public Button joinGameButton;

    private void Start()
    {
        print("Load Lobby");
        PlayerPrefs.SetString("GameType", "Network");
        joinGameButton.interactable = false;
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
