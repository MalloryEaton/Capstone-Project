using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour {

    public Camera MainCam;
    public List<Vector3> positionList;
    public List<Quaternion> rotationList;
    private int index;
    private int numberOfStages;
    public GameObject Level1;
    public GameObject Level2;

    private void Awake()
    {
        InitializeCameraPositions();
    }

    private void InitializeCameraPositions()
    {
        positionList.Add(new Vector3(47, 64, -70));
        positionList.Add(new Vector3(113.3f, 34f, 48.6f));
        positionList.Add(new Vector3(291f, 226f, 751f));
        positionList.Add(new Vector3(139.2f, 137.2f, 289.4f));
        positionList.Add(new Vector3(409.1f, 75.6f, 226.3f));

        rotationList.Add(Quaternion.Euler(36.03f, -33.65f, 0f));
        rotationList.Add(Quaternion.Euler(41.7f, -47f, 0f));
        rotationList.Add(Quaternion.Euler(38.6f, 135f, 0f));
        rotationList.Add(Quaternion.Euler(39f, 135f, 0f));
        rotationList.Add(Quaternion.Euler(15f, -62.61f, 0f));
    }

    public void Start()
    {
        //Level1.SetActive(false);
        //Level2.SetActive(false);
        print(MainCam.transform.position);
        index = 0;
        numberOfStages = 5;
        //positionList.Add(new Vector3(47, 64, -70));
        //positionList.Add(new Vector3(89, 64, -42));
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
            MainCam.transform.rotation = rotationList[index];
            //hideLevel(index - 1);
        }
        else
        {
            index = 0;
            //showLevel(index);
            print(index);
            MainCam.transform.position = positionList[index];
            MainCam.transform.rotation = rotationList[index];
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
            MainCam.transform.rotation = rotationList[index];
            //hideLevel(index + 1);
        }
        else
        {
            index = numberOfStages - 1;
            //showLevel(index);
            print(index + "position: " + positionList[index].x + ", " + positionList[index].y + ", " + positionList[index].z);
            MainCam.transform.position = positionList[index];
            MainCam.transform.rotation = rotationList[index];
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
