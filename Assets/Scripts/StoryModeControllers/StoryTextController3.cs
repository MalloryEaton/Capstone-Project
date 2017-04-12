using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryTextController3 : MonoBehaviour
{
    private List<string> TextList;
    private AutoTypeMainBox autoTypeMain;
    private AutoTypeLeftBox autoTypeLeft;
    private AutoTypeRightBox autoTypeRight;

    private int textIndex;

    public List<Camera> cameras;
    public List<Light> mainLights;
    public List<Light> desertLights;

    public GameObject mainTextBox;
    public GameObject rightTextBox;
    public GameObject leftTextBox;

    private Vector3 mainTextOriginalPosition;
    private Vector3 rightTextOriginalPosition;
    private Vector3 leftTextOriginalPosition;

    private string textbox;

    private GameObject LoadingPanel;

    private List<List<Light>> lightsList;

    private bool isFirstTime = true;

    private GameObject yellowMage;
    
    void Start()
    {
        yellowMage = GameObject.Find("YellowMage");
        if (PlayerPrefs.GetInt("StoryStage") == 3)
        {
            isFirstTime = false;
            transform.position = new Vector3(14, 16, 5);
            transform.Rotate(0, 130, 0);
        }
        mainTextOriginalPosition = mainTextBox.GetComponent<RectTransform>().position;
        rightTextOriginalPosition = mainTextBox.GetComponent<RectTransform>().position;
        leftTextOriginalPosition = mainTextBox.GetComponent<RectTransform>().position;
        TextList = new List<string>();
        InitializeTextList();
        if (isFirstTime)
            InstantiateShine("Black");
        else
            InstantiateShine("White");
        textbox = "main";
        lightsList = new List<List<Light>>();
        SetUpLightsList();
        if (isFirstTime)
            SetUpCameras("Main");
        else
            SetUpCameras("Desert");
        SetUpTextBoxes("main");
        LoadingPanel = GameObject.Find("LoadingPanel");
        textIndex = 0;
        autoTypeMain = FindObjectOfType(typeof(AutoTypeMainBox)) as AutoTypeMainBox;
        autoTypeLeft = FindObjectOfType(typeof(AutoTypeLeftBox)) as AutoTypeLeftBox;
        autoTypeRight = FindObjectOfType(typeof(AutoTypeRightBox)) as AutoTypeRightBox;
        yellowMage.SetActive(false);
        if (isFirstTime)
            autoTypeMain.StartText(TextList[0]);
        else
        {
            yellowMage.SetActive(true);
            textbox = "right";
            SetUpTextBoxes("right");

            autoTypeRight.StartText(TextList[0]);
        }
    }

    private void InstantiateShine(string color)
    {
        GameObject shrine = Instantiate(Resources.Load(@"ShrinesTutorial\Shrine" + color, typeof(GameObject)) as GameObject);
        shrine.transform.position = new Vector3(12, 0, 12);
    }

    private void SetUpLightsList()
    {
        lightsList.Add(mainLights);
        lightsList.Add(desertLights);
    }

    private void SetUpCameras(string cam)
    {
        foreach (List<Light> l in lightsList)
        {
            foreach (Light light in l)
            {
                light.enabled = false;
            }
        }

        foreach (Camera c in cameras)
        {
            c.enabled = false;
        }

        if (cam == "Main")
        {
            cameras[0].enabled = true;
            foreach (Light light in mainLights)
            {
                light.enabled = true;
            }
        }
        else if (cam == "Desert")
        {
            cameras[1].enabled = true;
            foreach (Light light in desertLights)
            {
                light.enabled = true;
            }
        }
    }

    private void InitializeTextList()
    {
        if (isFirstTime)
        {
            TextList.Add("Targus made his way towards the desert. If this figure was involved with the undead, they would probably be very interested in the mummies beneath the sand.");
            TextList.Add("Targus, however, was not a huge fan of the journey, particularly the sand.");
            TextList.Add("I don't like sand. It's coarse and rough and irritating... and it gets everywhere.");
            TextList.Add("You had better get used to it. The desert will be your final resting place!");
            TextList.Add("Oh, bother. Here we go again...");
            TextList.Add("");
        }
        else
        {
            TextList.Add("Targus! It sure is good to see you, now that I have control over my own mind.");
            TextList.Add("You’ll have to forgive me for the threats against your life and such. I was... ah... not myself.");
            TextList.Add("No worries, Merwin, I’m already a bit familiar with what's going on. Do you remember who it was that attacked you?");
            TextList.Add("Eh... Only vaguely. I do remember him (in typical villain fashion) describing his master plan to no one in particular.");
            TextList.Add("Oh, do tell!");
            TextList.Add("My mind is still hazy, but I think he mentioned something about enslaving all the king's sorcerers!");
            TextList.Add("What about all the king's men?");
            TextList.Add("What?");
            TextList.Add("Nevermind. I must track this guy down before he can do any more harm to us.");
            TextList.Add("Good luck!");
            TextList.Add("");
        }
    }

    private void SceneLogic()
    {
        #region FirstTime
        if (isFirstTime)
        {
            if (textIndex == 2)
            {
                SetUpCameras("Desert");
                transform.position = new Vector3(15, 16, 9);
                transform.Rotate(0, 130, 0);
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 3)
            {
                yellowMage.SetActive(true);
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 4)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 5)
            {
                LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
                SetUpTextBoxes("none");
                PlayerPrefs.SetString("Player1Color", "White");
                PlayerPrefs.SetString("Player2Color", "Yellow");
                PlayerPrefs.SetString("Difficulty", "Medium");
                PlayerPrefs.SetString("GameType", "Story");
                StartCoroutine(LoadAsync(6));
            }
            else
            {
                if (textbox == "main")
                {
                    autoTypeMain.autoType = true;
                    autoTypeMain.StartText(TextList[textIndex]);
                }
                if (textbox == "right")
                {
                    autoTypeRight.autoType = true;
                    autoTypeRight.StartText(TextList[textIndex]);
                }
                if (textbox == "left")
                {
                    autoTypeLeft.autoType = true;
                    autoTypeLeft.StartText(TextList[textIndex]);
                }
            }
        }
        #endregion
        #region SecondTime
        else
        {
            if (textIndex == 2)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 3)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 4)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 5)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 6)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 7)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 8)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 9)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 10)
            {
                LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
                StartCoroutine(LoadAsync(14));
            }
            else
            {
                if (textbox == "main")
                {
                    autoTypeMain.autoType = true;
                    autoTypeMain.StartText(TextList[textIndex]);
                }
                if (textbox == "right")
                {
                    autoTypeRight.autoType = true;
                    autoTypeRight.StartText(TextList[textIndex]);
                }
                if (textbox == "left")
                {
                    autoTypeLeft.autoType = true;
                    autoTypeLeft.StartText(TextList[textIndex]);
                }
            }
        }
        #endregion
    }

    private void SetUpTextBoxes(string box)
    {
        if (box == "main")
        {
            mainTextBox.transform.position = mainTextOriginalPosition;
            leftTextBox.transform.position = new Vector3(leftTextOriginalPosition.x, 1000, 0);
            rightTextBox.transform.position = new Vector3(rightTextOriginalPosition.x, 1000, 0);
        }
        else if (box == "left")
        {
            mainTextBox.transform.position = new Vector3(mainTextOriginalPosition.x, 1000, 0);
            leftTextBox.transform.position = leftTextOriginalPosition;
            rightTextBox.transform.position = new Vector3(rightTextOriginalPosition.x, 1000, 0);
        }
        else if (box == "right")
        {
            mainTextBox.transform.position = new Vector3(mainTextOriginalPosition.x, 1000, 0);
            leftTextBox.transform.position = new Vector3(leftTextOriginalPosition.x, 1000, 0);
            rightTextBox.transform.position = rightTextOriginalPosition;
        }
        else if (box == "none")
        {
            mainTextBox.transform.position = new Vector3(mainTextOriginalPosition.x, 1000, 0);
            leftTextBox.transform.position = new Vector3(leftTextOriginalPosition.x, 1000, 0);
            rightTextBox.transform.position = new Vector3(rightTextOriginalPosition.x, 1000, 0);
        }
    }

    private void OnMouseDown()
    {
        if (textbox == "main")
        {
            if (autoTypeMain.autoType)
                autoTypeMain.autoType = false;
            else
            {
                textIndex++;
                SceneLogic();
            }
        }
        else if (textbox == "right")
        {
            if (autoTypeRight.autoType)
                autoTypeRight.autoType = false;
            else
            {
                textIndex++;
                SceneLogic();
            }
        }
        else if (textbox == "left")
        {
            if (autoTypeLeft.autoType)
                autoTypeLeft.autoType = false;
            else
            {
                textIndex++;
                SceneLogic();
            }
        }
    }

    private IEnumerator LoadAsync(int levelNum)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(levelNum);
        while (!async.isDone)
        {
            yield return null;
        }
    }
}