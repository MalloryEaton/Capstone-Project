using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomIntoStage : MonoBehaviour
{
    private float zoomSpeed = 25f;

    public GameObject forest;
    public GameObject graveyard;
    public GameObject desert;
    public GameObject volcano;
    public GameObject water;
    public GameObject tower;

    private GameObject target;

    private bool zoom = false;

    void Start()
    {
        target = water;
    }

    private void Update()
    {
        if (zoom)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * zoomSpeed);
            transform.LookAt(target.transform);
        }
    }
}
