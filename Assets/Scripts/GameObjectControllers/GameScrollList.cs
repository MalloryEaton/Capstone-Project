using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class GameListItem
{
	public string playerName;
    public string characterIconString;
	public Sprite characterIcon;
    public Sprite stageIcon;
}


public class GameScrollList : MonoBehaviour {
	//public List<GameListItem> gameList;
	public Transform contentPanel;
	//public GameScrollList gameList;
	public ListObjectPool listObjectPool;

	// Use this for initialization
	void Start () {
		//RefreshDisplay ();
	}

	//public void RefreshDisplay(){
	//	addGames ();
	//}
	
	public void addGames(List <GameListItem> list){
		for (int i = 0; i < list.Count; i++) {
			GameListItem game = list [i];
			GameObject newEntry = listObjectPool.GetObject ();
			//newEntry.transform.SetParent (contentPanel);

			GameEntryScript listItem = newEntry.GetComponent<GameEntryScript> ();
			listItem.Setup (game, this);
            listItem.transform.SetParent(contentPanel);
		}
	}

    public void deselectGames()
    {
        foreach(GameEntryScript game in contentPanel.GetComponentsInChildren<GameEntryScript>())
        {
            game.DeselectGame();
        }
    }

    public void clearList()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            listObjectPool.ReturnObject(toRemove);
        }
    }

}
