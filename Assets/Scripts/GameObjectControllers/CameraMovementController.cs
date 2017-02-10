using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour {

    public Camera MainCam;
    public List<Vector3> positionList;
    private int index;
    public GameObject Level1;
    public GameObject Level2;

    public void Start()
    {
        //Level1.SetActive(false);
        //Level2.SetActive(false);
        print(MainCam.transform.position);
        index = 0;
        positionList.Add(new Vector3(47, 64, -70));
        positionList.Add(new Vector3(89, 64, -42));
        //showLevel(0);
        //MainCam.transform.position = positionList[0];
    }
    
    public void moveCameraRight()
    {
        if(index < positionList.Count-1)
        {
            index += 1;
            //showLevel(index);
            print(index);
            MainCam.transform.position = positionList[index];
            //hideLevel(index - 1);
        }
    }

    public void moveCameraLeft()
    {
        if(index > 0)
        {
            index -= 1;
            //showLevel(index);
            print(index + "position: " + positionList[index].x + ", " + positionList[index].y + ", " + positionList[index].z);
            MainCam.transform.position = positionList[index];
            //hideLevel(index + 1);
        }
    }

    public void hideLevel(int level)
    {
        switch (level)
        {
            case 0:
                Level1.SetActive(false);
                break;
            case 1:
                Level2.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void showLevel(int level)
    {
        switch (level)
        {
            case 0:
                Level1.SetActive(true);
                break;
            case 1:
                Level2.SetActive(true);
                break;
            default:
                break;
        }
    }
}
