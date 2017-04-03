using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoType : MonoBehaviour {

    private float letterPause;
    public AudioClip sound;

    public string message;
    
    // Use this for initialization
    void Start()
    {
        letterPause = 0.03f;
        message = "In a world far away from our own lies the Kingdom Of Derraveth. This kingdom thrives on the energy given off by ancient magic artifacts which the citizens call shrines. The king of Derraveth employs seven sorcerers who protect the shrines, using magic to keep evil-doers at bay.";
        GetComponent<Text>().text = "";
        StartCoroutine(TypeText());
    }

    public void StartText(string text)
    {
        message = text;
        GetComponent<Text>().text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            GetComponent<Text>().text += letter;
            //if (sound)
            //    audio.PlayOneShot(sound);
            yield return 0;
            yield return new WaitForSeconds(letterPause);
        }
    }
}
