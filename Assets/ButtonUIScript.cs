using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUIScript : MonoBehaviour {

    public Text text;

    private void OnPointerEnter()
    {
        text.color = Color.grey;
        print("Text enter");
    }
}
