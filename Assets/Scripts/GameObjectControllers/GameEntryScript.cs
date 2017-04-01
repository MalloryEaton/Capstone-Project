using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.EnsorcelledStudios.Runic;

public class GameEntryScript : MonoBehaviour {

	public Button entry;
	public Text playerName;
	public Image characterIcon;

    public Sprite redSprite;
    public Sprite blueSprite;
    public Sprite blackSprite;
    public Sprite greenSprite;
    public Sprite yellowSprite;
    public Sprite orangeSprite;
    public Sprite purpleSprite;
    public Sprite whiteSprite; 

	private GameListItem gameListItem;
	private GameScrollList GSL;

    //public Launcher launcher;

	// Use this for initialization
	void Start () {
        print("START " + playerName.text);
		entry.onClick.AddListener (JoinGame);
    }

	public void Setup(GameListItem game, GameScrollList gameScrollList)
	{
		gameListItem = game;
		playerName.text = gameListItem.playerName;
       // entry.onClick.AddListener(JoinGame);

        switch (gameListItem.characterIconString)
        {
            case "Red":
                characterIcon.sprite = redSprite;
                break;
            case "Green":
                characterIcon.sprite = greenSprite;
                break;
            case "Blue":
                characterIcon.sprite = blueSprite;
                break;
            case "Orange":
                characterIcon.sprite = orangeSprite;
                break;
            case "Black":
                characterIcon.sprite = blackSprite;
                break;
            case "White":
                characterIcon.sprite = whiteSprite;
                break;
            case "Yellow":
                characterIcon.sprite = yellowSprite;
                break;
            case "Purple":
                characterIcon.sprite = purpleSprite;
                break;
        }
        
		//characterIcon.sprite = gameListItem.characterIcon;

		GSL = gameScrollList;
	}

	public void JoinGame()
	{
        LauncherStatic.launcher.JoinGame(playerName.text);
        Debug.Log("Join " + playerName.text);
	}
}
