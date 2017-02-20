using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBoardUIController : MonoBehaviour {

    public GameObject exitButton;
    NetworkingController networking;
   

    void Start()
    {
        networking = FindObjectOfType(typeof(NetworkingController)) as NetworkingController;
    }

    public void exitToMenu()
    {
        if (PlayerPrefs.GetString("GameType") == "Network")
        {
            networking.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }

    }
}
