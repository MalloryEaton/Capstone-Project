using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class ButtonController : MonoBehaviour {

    public GameObject Menu;

	public void StartGame (string scene)
    {
        Menu.SetActive(false);
    }
}
