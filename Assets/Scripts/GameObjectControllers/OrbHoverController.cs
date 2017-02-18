using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHoverController : MonoBehaviour {
    private float startY = 1.5f;
    private float amplitude = 0.5f;
    private float speed = 5;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, startY + amplitude * Mathf.Sin(speed * Time.time), transform.position.z);
    }
}
