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

    private void SetSelectedStage()
    {
        string stage = "";
        switch(index)
        {
            case 0:
                stage = "Forest";
                break;
            case 1:
                stage = "Graveyard";
                break;
            case 2:
                stage = "Desert";
                break;
            case 3:
                stage = "Volcano";
                break;
            case 4:
                stage = "Water";
                break;
            case 5:
                stage = "Tower";
                break;
        }
        PlayerPrefs.SetString("Stage", stage);
    }

    private void InitializeCameraPositions()
    {
        positionList.Add(new Vector3(-102f, 80f, -242f)); //forest
        positionList.Add(new Vector3(-7.1f, 84.9f, -146.6f)); //graveyard
        positionList.Add(new Vector3(145.2f, 70.5f, 23.2f)); //desert
        positionList.Add(new Vector3(235.5f, 98.7f, 72.6f)); //volcano
        positionList.Add(new Vector3(139.2f, 137.2f, 289.4f)); //water
        positionList.Add(new Vector3(409.1f, 75.6f, 226.3f)); //tower

        rotationList.Add(Quaternion.Euler(45.33f, -46.4f, 0f)); //forest
        rotationList.Add(Quaternion.Euler(42.5f, -48f, 0f)); //graveyard
        rotationList.Add(Quaternion.Euler(41.2f, -47.8f, 0f)); //desert
        rotationList.Add(Quaternion.Euler(36.1f, -46.5f, 0f)); //volcano
        rotationList.Add(Quaternion.Euler(39f, 135f, 0f)); //water
        rotationList.Add(Quaternion.Euler(15f, -62.61f, 0f)); //tower
    }

    public void Start()
    {
        //Level1.SetActive(false);
        //Level2.SetActive(false);
        index = 0;
        numberOfStages = 6;
        SetSelectedStage();
        //positionList.Add(new Vector3(47, 64, -70));
        //positionList.Add(new Vector3(89, 64, -42));
        //showLevel(0);
        //MainCam.transform.position = positionList[0];
    }

    public void cameraInit()
    {
        index = 0;
        MainCam.transform.position = positionList[0];
        MainCam.transform.rotation = rotationList[0];
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
        SetSelectedStage();
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
        SetSelectedStage();
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
