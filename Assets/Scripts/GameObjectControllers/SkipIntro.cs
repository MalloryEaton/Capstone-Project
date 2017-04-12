using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipIntro : MonoBehaviour
{
    public Image logo;

    private float growFactor;
    public float waitTime;

    void Start()
    {
        growFactor = 0.5f;
        waitTime = 0.1f;
        StartCoroutine(Scale());
    }

    IEnumerator Scale()
    {
        float timer = 0;
        while (logo.transform.localScale.x < 1)
        {
            timer += Time.deltaTime;
            logo.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
            yield return null;
        }

        timer = 0;
        yield return new WaitForSeconds(waitTime);
    }

    private void OnMouseDown()
    {
        SceneManager.LoadScene(17);
    }
}