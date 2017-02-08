using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour {

    public Camera MainCam;
    public List<Vector3> positionList;
    private int index;

    public void Start()
    {
        print(MainCam.transform.position);
        index = 0;
        positionList.Add(new Vector3(47, 64, -70));
        positionList.Add(new Vector3(89, 64, -42));
    }
    
    public void moveCameraRight()
    {
        if(index < positionList.Count-1)
        {
            index += 1;
            print(index);
            MainCam.transform.position = positionList[index];
        }
    }

    public void moveCameraLeft()
    {
        if(index > 0)
        {
            index -= 1;
            print(index + "position: " + positionList[index].x + ", " + positionList[index].y + ", " + positionList[index].z);
            MainCam.transform.position = positionList[index];
        }
    }
}
