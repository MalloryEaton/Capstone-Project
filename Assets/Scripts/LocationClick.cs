using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationClick : MonoBehaviour {
    public short locationNumber;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    //detect click
    private void OnMouseDown()
    {
        Debug.Log("Location Number: " + locationNumber);
    }
}
