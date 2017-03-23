using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEntryScript : MonoBehaviour {

	public Button entry;
	public Text playerName;
	public Image characterIcon;

	private GameListItem gameListItem;
	private GameScrollList GSL;

	// Use this for initialization
	void Start () {
		
	}

	public void Setup(GameListItem game, GameScrollList gameScrollList)
	{
		gameListItem = game;
		playerName.text = gameListItem.playerName;
		//characterIcon.sprite = gameListItem.characterIcon;

		GSL = gameScrollList;
	}
}
