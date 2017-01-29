using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour {
    private float startY;
    private float amplitude = 1;
    private float speed = 5;

    private void Start()
    {
        startY = transform.position.y;
    }

    private void Update()
    {
        //transform.position = new Vector3(transform.position.x, startY + amplitude * Mathf.Sin(speed * Time.time), transform.position.z);
    }
}
