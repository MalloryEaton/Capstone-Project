using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour {

    private void Start()
    {
        print("Load Lobby");
        PlayerPrefs.SetString("GameType", "Network");

        PlayerPrefs.SetString("Player1Color", "Green");
        PlayerPrefs.SetString("Player2Color", "Purple");
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
