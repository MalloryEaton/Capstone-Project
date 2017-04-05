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

    public bool isMasterClient;

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
        SceneManager.LoadScene(1);
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        // If one player disconnects, we disconnect the other player and alert them
        Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // Seen when other disconnects

        LeaveRoom();
    }

    public bool DetermineIfMasterClient()
    {
        return PhotonNetwork.isMasterClient;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SendName()
    {
        photonView.RPC("ReceieveName", PhotonTargets.Others, PhotonNetwork.playerName);
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
        sentMessage = chatInputField.text;
        sentMessage = sentMessage.Trim();
        if (sentMessage != "")
        {
            photonView.RPC("ReceiveChat", PhotonTargets.Others, sentMessage);
        }
    }

    public void ResetNetworkValues()
    {
        moveTo = moveFrom = removeFrom = -1;
    }

    [PunRPC]
    public void ReceiveName(string opponentName)
    {
        this.opponentName = opponentName;
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
    public void ReceiveChat(string receivedMessage)
    {
        Debug.Log("Opponent: " + receivedMessage);
        chatInputField.text = opponentName + ": " + receivedMessage;
    }    
}
