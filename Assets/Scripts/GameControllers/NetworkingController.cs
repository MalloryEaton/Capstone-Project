using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class NetworkingController : Photon.PunBehaviour
{
    /*---------------------------------------------------------------------
    || NETWORKING VARIABLES
    -----------------------------------------------------------------------*/
    public InputField chatInputField;
    private GameLogicController gameLogicController;
    public GameBoardUIController gameBoardUI;

    public bool isMasterClient;
    public bool playerDisconnected;

    public string opponentName;
    public string stageToLoad;

    public short moveTo;
    public short moveFrom;
    public short removeFrom;

    public string otherPlayerColor;

    /*---------------------------------------------------------------------
    || NETWORKING FUNCTIONS
    -----------------------------------------------------------------------*/
    void Start()
    {
        gameLogicController = FindObjectOfType(typeof(GameLogicController)) as GameLogicController;
    }

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        // Scene 1 is the network lobby
        // TODO: Load the correct scene.
        SceneManager.LoadScene(2);
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        // If one player disconnects, we disconnect the other player and alert them
        Debug.Log("OnPhotonPlayerDisconnected(): " + other.NickName); // Seen when other disconnects

        if(gameLogicController.playerForfeit != "other")
        {
            LeaveRoom();
        }
        
    }

    public override void OnDisconnectedFromPhoton()
    {
        // This will need to take us back to the original connection
        // menu, not back to the lobby.

        // TODO: Need a better disconnect message
        Debug.Log("You have been disconnected from the server.");

        if (playerDisconnected)
        {
            // TODO: Add a disconnect notification in the UI.
            Debug.Log("Please check to see if you are connected to the internet.");
        }
    }

    public override void OnConnectionFail(DisconnectCause cause)
    {
        playerDisconnected = true;
    }

    public bool DetermineIfMasterClient()
    {
        return PhotonNetwork.isMasterClient;
    }

    public void LeaveRoom()
    {
        LeanTween.cancelAll();

        PhotonNetwork.LeaveRoom();
    }

    public void SendName()
    {
        photonView.RPC("ReceiveName", PhotonTargets.Others, PhotonNetwork.playerName);
        Debug.Log("send name");
    }

    public void SendColor()
    {
        photonView.RPC("ReceiveColor", PhotonTargets.Others, PlayerPrefs.GetString("PlayerColor"));
    }

    public void SendMove()
    {
        photonView.RPC("ReceiveMove", PhotonTargets.Others, moveTo, moveFrom, removeFrom);
    }

    public void SendChat()
    {
        string sentMessage;
        sentMessage = gameBoardUI.chatInput.text;
        sentMessage = sentMessage.Trim();
        if (sentMessage != "")
        {
            photonView.RPC("ReceiveChat", PhotonTargets.Others, sentMessage, PhotonNetwork.playerName);
            gameBoardUI.addMessage(PhotonNetwork.playerName, sentMessage);
        }
    }

    public void SendForfeit()
    {
        // TODO: Call this function from wherever Forfeit is called
        photonView.RPC("ReceiveForfeit", PhotonTargets.Others);
    }

    public void ResetNetworkValues()
    {
        moveTo = moveFrom = removeFrom = -1;
    }

    [PunRPC]
    public void ReceiveName(string opName)
    {       
        opponentName = opName;
        Debug.Log(opponentName + " received!");
        //opponentNameText.text = opponentName;
    }

    [PunRPC]
    public void ReceiveColor(string color)
    {
        otherPlayerColor = color;
    }

    [PunRPC]
    public void ReceiveMove(short moveTo, short moveFrom, short removeFrom)
    {
        this.moveTo = moveTo;
        this.moveFrom = moveFrom;
        this.removeFrom = removeFrom;

        if (moveTo != -1)
        {
            // Place opponent rune
            if (moveFrom == -1)
            {
                gameLogicController.PlacementPhase(moveTo);
            }
            else
            {
                gameLogicController.runeFromLocation = moveFrom;
                gameLogicController.MovementPhase_Place(moveTo);
            }            
        }
    }

    [PunRPC]
    public void ReceiveChat(string receivedMessage, string name)
    {
        Debug.Log("Opponent: " + receivedMessage);
        Debug.Log(opponentName);
        //chatInputField.text = opponentName + ": " + receivedMessage;
        gameBoardUI.addMessage(name, receivedMessage);
        if(gameBoardUI.chatInput.GetComponent<Animator>().GetBool("isDisplayed") == false)
        {
            gameBoardUI.displayNotification();
        }

    }
    
    [PunRPC]
    public void ReceiveForfeit()
    {
        gameLogicController.playerForfeit = "other";
        //Display other player forfeit message
        
        StartCoroutine(forfeitCycle());
        StartCoroutine(gameOverDelay());
    } 

    IEnumerator forfeitCycle()
    {
        gameBoardUI.displayForfeit();
        yield return new WaitForSeconds(2f);
        gameBoardUI.hideForfeit();
    }

    IEnumerator gameOverDelay()
    {
        yield return new WaitForSeconds(2f);
        gameLogicController.GameOver();
    }
}
