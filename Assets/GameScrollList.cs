using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class GameListItem
{
	public string playerName;
	//public Sprite characterIcon;
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
			newEntry.transform.SetParent (contentPanel);

			GameEntryScript listItem = newEntry.GetComponent<GameEntryScript> ();
			listItem.Setup (game, this);
		}
	}

}
