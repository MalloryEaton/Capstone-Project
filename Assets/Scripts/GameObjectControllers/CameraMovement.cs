using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    private float rotationSpeed = 25f;

    private GameObject target;
    
    void Start () {
        target = GameObject.FindGameObjectWithTag("Center");
    }
    
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Time.deltaTime * rotationSpeed);
            transform.LookAt(target.transform);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Time.deltaTime * rotationSpeed);
            transform.LookAt(target.transform);
        }
    }
}
