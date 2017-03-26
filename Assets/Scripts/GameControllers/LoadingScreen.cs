using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {
    public GameObject LoadingPanel;
    
    void Start ()
    {
        LoadingPanel = GameObject.Find("LoadingPanel");
        LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        int sceneToLoad = 1;
        StartCoroutine(LoadAsync(sceneToLoad));
    }

    private IEnumerator LoadAsync(int levelNum)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(levelNum);
        while (!async.isDone)
        {
            yield return null;
        }
    }
}
