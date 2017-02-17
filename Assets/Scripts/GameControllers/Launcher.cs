using UnityEngine;
using UnityEngine.UI;

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
        public InputField inputField;

        #endregion

        #region Private Variables

        /// <summary>
        /// This client's version number. Users are separated from each other by gameversion (which allows 
        /// you to make breaking changes).
        /// </summary>
        string _gameVersion = "0.1";

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

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            PlayerPrefs.SetString("PlayerColor", "Purple");
            // #NotImportant
            // Force LogLevel
            PhotonNetwork.logLevel = Loglevel;

            // #Critical
            // We don't join the lobby. There is no need to join a lobby to get the list of rooms.
            PhotonNetwork.autoJoinLobby = false;

            // #Critical
            // This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients 
            // in the same room sync their level automatically
            PhotonNetwork.automaticallySyncScene = true;

            //TODO: check if this is actually affecting the number of attemped failed packets before disconnect 
            //-- 3 should be faster than default
            PhotonNetwork.MaxResendsBeforeDisconnect = 3;
            Debug.Log("MaxResendsBeforeDisconnect = " + PhotonNetwork.MaxResendsBeforeDisconnect);
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #endregion

        #region Photon.PunBehaviour CallBacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");

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
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

            // TODO: Need a better disconnect message
            Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

            // #Critical: We failed to join a random room, maybe none exists or they are all full. 
            // No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            // #Critical: We only load if we are the first player, else we rely on 
            // PhotonNetwork.automaticallySyncScene to sync our instance scene.
            if (PhotonNetwork.room.PlayerCount == 1)
            {
                Debug.Log("We load the 'Game'");

                // #Critical Load the Game
                // TODO: This will almost certainly be named something different
                PhotonNetwork.LoadLevel("Mal'sBoard");
            }
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
                    PhotonNetwork.playerName = "Worthy Wizard";
                }
                else
                {
                    PhotonNetwork.playerName = inputField.text;
                }

                // Keep track of the will to join a room, because when we come back from the game 
                // we will get a callback that we are connected, so we need to know what to do then
                isConnecting = true;

                progressLabel.SetActive(true);
                controlPanel.SetActive(false);

                // We check if we are connected or not, we join if we are, else we initiate the 
                // connection to the server.
                if (PhotonNetwork.connected)
                {
                    // #Critical We need to attempt joining a Random Room. If it fails, we'll get notified 
                    // in OnPhotonRandomJoinFailed() and we'll create one.
                    PhotonNetwork.JoinRandomRoom();
                }
                else
                {
                    // #Critical We must first and foremost connect to Photon Online Server.
                    PhotonNetwork.ConnectUsingSettings(_gameVersion);
                }
            }
            else
            {
                Debug.Log("You are not connected to the internet.");
            }
        }

        #endregion
    }
}