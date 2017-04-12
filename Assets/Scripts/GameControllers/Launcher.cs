using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
//using System.Collections;
using ExitGames.Client.Photon;

namespace Com.EnsorcelledStudios.Runic
{
    public class Launcher : Photon.PunBehaviour
    {
        #region Public Variables

        /// <summary>
        /// The PUN loglevel. 
        /// </summary>
        public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

        public GameObject controlPanel;
        public GameObject progressLabel;
        public GameObject LoadingScreen;
        public InputField inputField;
        public GameObject gamesPanel;
        public GameScrollList scrollList;
        private Hashtable playerProperties = new Hashtable();
        private string[] roomProperties = new string[2];
        public GameEntryScript gameEntry;

        //List that will contain available netwrok games
        //GameListItem defined in GameScrollList.cs
        public List<GameListItem> gameList;
        public string selectedRoomName;
        public Button joinGameButton;
        public LobbyController lobbyUI;
        public Text noNameAlert;

        #endregion

        #region Private Variables

        /// <summary>
        /// This client's version number. Users are separated from each other by gameversion (which allows 
        /// you to make breaking changes).
        /// </summary>
        string _gameVersion = "0.2";

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, 
        /// and so new room will be created.
        /// </summary>   
        public byte MaxPlayersPerRoom = 2;

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several 
        /// callbacks from Photon, we need to keep track of this to properly adjust the behavior when we 
        /// receive call back by Photon. Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;

        bool playerDisconnected = false;

        short randomStage;

        // We will keep available games in this array.
        RoomInfo[] roomInfo;

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            PlayerPrefs.SetString("GameType", "Network");
            // #NotImportant
            // Force LogLevel
            PhotonNetwork.logLevel = Loglevel;

            // #Critical
            // We need to join the lobby to get a list of rooms.
            PhotonNetwork.autoJoinLobby = true;

            // #Critical
            // This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients 
            // in the same room sync their level automatically
            PhotonNetwork.automaticallySyncScene = true;

            //TODO: check if this is actually affecting the number of attemped failed packets before disconnect 
            //-- 3 should be faster than default
            PhotonNetwork.MaxResendsBeforeDisconnect = 3;
            Debug.Log("MaxResendsBeforeDisconnect = " + PhotonNetwork.MaxResendsBeforeDisconnect);

            PhotonNetwork.QuickResends = 3;

            PhotonNetwork.CrcCheckEnabled = true;
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            controlPanel.SetActive(true);
            LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", false);
            LauncherStatic.launcher = this;
        }

        #endregion

        #region Photon.PunBehaviour CallBacks

        public override void OnReceivedRoomListUpdate()
        {
            // If we're in a lobby, update the list of available games.
            if (PhotonNetwork.insideLobby == true)
            {
                gameList.Clear();

                roomInfo = PhotonNetwork.GetRoomList();

                foreach (RoomInfo room in roomInfo)
                {
                    if (room.PlayerCount != room.MaxPlayers)
                    {
                        GameListItem game = new GameListItem();
                        //GameEntryScript game = Instantiate(gameEntry);
                        // Hide the unique ID from the screen
                        game.playerName = room.Name;
                        // This code works for accessing custom properties
                        game.characterIconString = room.CustomProperties["color"].ToString();
                        //game.stageIconString = room.CustomProperties["stage"].ToString();
                        gameList.Add(game);
                    }
                }

                scrollList.clearList();
                scrollList.addGames(gameList);
            }
        }

        public override void OnJoinedLobby()
        {
            LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", false);
            controlPanel.SetActive(false);
            gamesPanel.SetActive(true);
            OnReceivedRoomListUpdate();
        }

        // DEPRECATED: We are no longer using this.
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() was called by PUN");

            // We don't want to do anything if we are not attempting to join a room. 
            // This case where isConnecting is false is typically when you lost or quit the game, 
            // when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnPhotonRandomJoinFailed()
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnectedFromPhoton()
        {
            // This will need to take us back to the original connection
            // menu, not back to the lobby.

            LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", false);
            controlPanel.SetActive(true);

            // TODO: Need a better disconnect message
            Debug.Log("You have been disconnected from the server.");

            if (playerDisconnected)
            {
                // TODO: Add a disconnect notification in the UI.
                Debug.Log("Please check to see if you are connected to the internet.");
            }
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            // TODO: We will need a UI message here that says you tried to join a room and the
            // action failed.
            Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. Room not available.");

            LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", false);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room.");
            LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", false);
            // #Critical: We only load if we are the first player, else we rely on 
            // PhotonNetwork.automaticallySyncScene to sync our instance scene.
            if (PhotonNetwork.room.PlayerCount == 1)
            {
                lobbyUI.displayWaitingForOpponent();
            }
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer other)
        {
            Debug.Log("Player connected: " + other.NickName); // Not seen if you're the player connecting

            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // Called before OnPhotonPlayerDisconnected

                PhotonNetwork.room.IsOpen = false;
                PhotonNetwork.room.IsVisible = false;

                LoadArena();
            }

            lobbyUI.hideWaitingForOpponent();
        }

        public override void OnConnectionFail(DisconnectCause cause)
        {
            playerDisconnected = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start the connection process. 
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            // Check if the user is connected to the internet. If not, let them know
            if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                // Get the player name and save it
                if (inputField.text.Trim() == "")
                {
                    // TODO: Alert the player that they need to put in a username.
                    noNameAlert.GetComponent<Animator>().SetBool("isDisplayed", true);
                }
                else
                {
                    PhotonNetwork.playerName = inputField.text;

                    LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", true);
                    controlPanel.SetActive(false);

                    gamesPanel.SetActive(true);

                    // Keep track of the will to join a room, because when we come back from the game 
                    // we will get a callback that we are connected, so we need to know what to do then
                    isConnecting = true;

                    // We check if we are connected or not, we join if we are, else we initiate the 
                    // connection to the server.
                    if (!PhotonNetwork.connected)
                    {
                        // #Critical We must first and foremost connect to Photon Online Server.
                        PhotonNetwork.ConnectUsingSettings(_gameVersion);
                    }

                }
            }
            else
            {
                // TODO: We need a popup for this. And some others.
                Debug.Log("You are not connected to the internet.");
            }
        }

        // JoinGame and CreateGame are new functions that we will use in the lobby,
        // depending on whether or not the user wants to host or join.
        public void JoinGame()
        {
            if (PhotonNetwork.insideLobby)
            {
                LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", true);
                print("Join " + selectedRoomName);
                PhotonNetwork.JoinRoom(selectedRoomName);
            }
        }

        public void CreateGame()
        {
            GetRandomStage();

            playerProperties.Clear();

            // We need to access the player's chosen color and stage here.
            playerProperties.Add("color", PlayerPrefs.GetString("PlayerColor"));
            playerProperties.Add("stage", PlayerPrefs.GetString("Stage"));
            roomProperties[0] = "color";
            roomProperties[1] = "stage";

            // Change room name to be a unique ID
            PhotonNetwork.CreateRoom(PhotonNetwork.playerName, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom, CustomRoomProperties = playerProperties, CustomRoomPropertiesForLobby = roomProperties }, null);
        }

        private void LoadArena()
        {
            switch (randomStage)
            {
                case 0:
                    PhotonNetwork.LoadLevel("ForestGameBoard");
                    break;
                case 1:
                    PhotonNetwork.LoadLevel("GraveyardGameBoard");
                    break;
                case 2:
                    PhotonNetwork.LoadLevel("DesertGameBoard");
                    break;
                case 3:
                    PhotonNetwork.LoadLevel("VolcanoGameBoard");
                    break;
                case 4:
                    PhotonNetwork.LoadLevel("WaterGameBoard");
                    break;
                case 5:
                    PhotonNetwork.LoadLevel("TowerGameBoard");
                    break;
            }
        }

        public void DisconnectFromRoom()
        {
            PhotonNetwork.LeaveRoom();
            lobbyUI.hideWaitingForOpponent();
        }

        public void DisconnectFromPhoton()
        {
            PhotonNetwork.Disconnect();

            gamesPanel.SetActive(false);
            // backButton.SetActive(false);

            // TODO: This should bring back up the login stuff.
        }

        #endregion

        #region Private Methods

        private void GetRandomStage()
        {
            randomStage = (short)Random.Range(0, 5);

            switch (randomStage)
            {
                case 0:
                    PlayerPrefs.SetString("Stage", "Forest");
                    break;
                case 1:
                    PlayerPrefs.SetString("Stage", "Graveyard");
                    break;
                case 2:
                    PlayerPrefs.SetString("Stage", "Desert");
                    break;
                case 3:
                    PlayerPrefs.SetString("Stage", "Volcano");
                    break;
                case 4:
                    PlayerPrefs.SetString("Stage", "Water");
                    break;
                case 5:
                    PlayerPrefs.SetString("Stage", "Tower");
                    break;
            }
        }

        public void hideNoNameAlert()
        {
            if(noNameAlert.GetComponent<Animator>().GetBool("isDisplayed") == true)
            {
                noNameAlert.GetComponent<Animator>().SetBool("isDisplayed", false);
            }
        }

        #endregion
    }
}