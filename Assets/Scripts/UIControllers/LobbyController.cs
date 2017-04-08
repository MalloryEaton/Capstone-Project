using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour {

    public GameObject LoadingScreen;
    public Button joinGameButton;
    public GameObject waitingForOpponent;

    private void Start()
    {
        print("Load Lobby");
        PlayerPrefs.SetString("GameType", "Network");
        joinGameButton.interactable = false;
        waitingForOpponent.SetActive(false);
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void displayWaitingForOpponent()
    {
        waitingForOpponent.SetActive(true);
        waitingForOpponent.GetComponent<Animator>().SetBool("isDisplayed", true);
    }

    public void hideWaitingForOpponent()
    {
        waitingForOpponent.GetComponent<Animator>().SetBool("isDisplayed", false);
        waitingForOpponent.SetActive(false);
    }

}
