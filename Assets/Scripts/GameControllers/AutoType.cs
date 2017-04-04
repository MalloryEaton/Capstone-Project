using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoType : MonoBehaviour {

    private float normalSpeed;
    private float fastSpeed;
    public AudioClip sound;

    private bool speedUp = false;

    public bool autoType = true;

    //public string message;
    
    // Use this for initialization
    void Start()
    {
        normalSpeed = 0.03f;
        fastSpeed = 0.001f;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            speedUp = true;
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            speedUp = false;
        }
    }

    public void StartText(string text)
    {
        //message = text;
        GetComponent<Text>().text = "";
        StopAllCoroutines();
        StartCoroutine(TypeText(text));
    }

    IEnumerator TypeText(string message)
    {
        foreach (char letter in message.ToCharArray())
        {
            if (autoType)
            {
                GetComponent<Text>().text += letter;
                //if (sound)
                //    audio.PlayOneShot(sound);
                //yield return 0;
                if(speedUp)
                    yield return new WaitForSeconds(fastSpeed);
                else
                    yield return new WaitForSeconds(normalSpeed);
            }
            else
            {
                GetComponent<Text>().text = message;
                yield return 0;
            }
        }
        autoType = false;
    }
}
