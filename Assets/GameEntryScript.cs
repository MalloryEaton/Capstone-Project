using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.EnsorcelledStudios.Runic;

public class GameEntryScript : MonoBehaviour {

	public Button entry;
	public Text playerName;
	public Image characterIcon;

	private GameListItem gameListItem;
	private GameScrollList GSL;

	// Use this for initialization
	void Start () {
		entry.onClick.AddListener (JoinGame);
	}

	public void Setup(GameListItem game, GameScrollList gameScrollList)
	{
		gameListItem = game;
		playerName.text = gameListItem.playerName;
		//characterIcon.sprite = gameListItem.characterIcon;

		GSL = gameScrollList;
	}

	public void JoinGame()
	{
        LauncherStatic.launcher.JoinGame(playerName.text);
	}
}
