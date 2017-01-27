using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    //public float speed = 20f;
    //public float rotationSpeed = 2.0f;
    //public float inOutSpeed = 10.0f;

    //private float yaw = 135.0f;
    //private float pitch = 51.0f;

    //private bool moveCameraIsActive = false;
    //private bool keyCPressed = false;

    private float rotationSpeed = 25f;

    private GameObject target;

    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("Center");
    }

	// Update is called once per frame
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

        //if (Input.GetKey(KeyCode.C))
        //{
        //    keyCPressed = !keyCPressed;
        //    if(keyCPressed)
        //    {
        //        moveCameraIsActive = !moveCameraIsActive;
        //    }
        //}

        //if(moveCameraIsActive)
        //{
        //    if (Input.GetKey(KeyCode.D))
        //    {
        //        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        //    }
        //    if (Input.GetKey(KeyCode.A))
        //    {
        //        transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        //    }
        //    if (Input.GetKey(KeyCode.S))
        //    {
        //        transform.Translate(new Vector3(0, 0, -inOutSpeed * Time.deltaTime));
        //    }
        //    if (Input.GetKey(KeyCode.W))
        //    {
        //        transform.Translate(new Vector3(0, 0, inOutSpeed * Time.deltaTime));
        //    }
        //    if (Input.GetKey(KeyCode.LeftShift))
        //    {
        //        transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        //    }
        //    if (Input.GetKey(KeyCode.Space))
        //    {
        //        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        //    }

        //    yaw += rotationSpeed * Input.GetAxis("Mouse X");
        //    pitch -= rotationSpeed * Input.GetAxis("Mouse Y");

        //    transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        //}
    }
}
