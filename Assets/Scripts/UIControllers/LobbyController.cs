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
        LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", true);

        StartCoroutine(LoadAsync(1));
    }

    private IEnumerator LoadAsync(int levelNum)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(levelNum);
        while (!async.isDone)
        {
            yield return null;
        }
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
