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

    public Sprite selectedFlourish;
    public Sprite deselectedFlourish;

    public Image leftFlourish;
    public Image rightFlourish; 

	private GameListItem gameListItem;
	private GameScrollList GSL;

    //public string characterIconString;

    //public Launcher launcher;

	// Use this for initialization
	void Start () {
        print("START " + playerName.text);
		entry.onClick.AddListener (SelectGame);
    }

	public void Setup(GameListItem game, GameScrollList gameScrollList)
	{
        //gameListItem = game;
        playerName.text = game.playerName;
       // entry.onClick.AddListener(JoinGame);

        switch (game.characterIconString)
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

	//public void JoinGame()
	//{
 //       LauncherStatic.launcher.JoinGame();
 //       Debug.Log("Join " + playerName.text);
	//}

    public void SelectGame()
    {
        //LauncherStatic.launcher.deselectGames();
        if (leftFlourish.sprite == deselectedFlourish)
        {
            GSL.deselectGames();
            leftFlourish.sprite = selectedFlourish;
            rightFlourish.sprite = selectedFlourish;
            LauncherStatic.launcher.selectedRoomName = playerName.text;
            LauncherStatic.launcher.joinGameButton.interactable = true;
        }
        else
        {
            leftFlourish.sprite = deselectedFlourish;
            rightFlourish.sprite = deselectedFlourish;
            LauncherStatic.launcher.joinGameButton.interactable = false;
        }
    }

    public void DeselectGame()
    {
        leftFlourish.sprite = deselectedFlourish;
        rightFlourish.sprite = deselectedFlourish;
    }
}
