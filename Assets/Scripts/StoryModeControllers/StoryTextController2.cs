using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryTextController2 : MonoBehaviour
{
    private List<string> TextList;
    private AutoTypeMainBox autoTypeMain;
    private AutoTypeLeftBox autoTypeLeft;
    private AutoTypeRightBox autoTypeRight;

    private int textIndex;

    public List<Camera> cameras;
    public List<Light> mainLights;
    public List<Light> graveyardLights;

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

    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.GetInt("StoryStage") == 2)
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
            SetUpCameras("Graveyard");
        SetUpTextBoxes("main");
        LoadingPanel = GameObject.Find("LoadingPanel");
        textIndex = 0;
        autoTypeMain = FindObjectOfType(typeof(AutoTypeMainBox)) as AutoTypeMainBox;
        autoTypeLeft = FindObjectOfType(typeof(AutoTypeLeftBox)) as AutoTypeLeftBox;
        autoTypeRight = FindObjectOfType(typeof(AutoTypeRightBox)) as AutoTypeRightBox;

        if (isFirstTime)
            autoTypeMain.StartText(TextList[0]);
        else
        {
            GameObject mage = Instantiate(Resources.Load(@"MagesForBoard\GreenMage", typeof(GameObject)) as GameObject);
            mage.transform.position = new Vector3(4, 0, -4);
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
        lightsList.Add(graveyardLights);
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
        else if (cam == "Graveyard")
        {
            cameras[1].enabled = true;
            foreach (Light light in graveyardLights)
            {
                light.enabled = true;
            }
        }
    }

    private void InitializeTextList()
    {
        if (isFirstTime)
        {
            TextList.Add("After tangling with Sebastian in the forest, Targus set out ");
        }
        else
        {
            TextList.Add("Owwwww..... Targus, is that you? What just happened?");
            TextList.Add("Sebastian! It seems you have come to your senses. Have you been eating anything different lately? Perhaps something in the mushroom family?");
            TextList.Add("I honestly don't remember anything. I wish I could be of more help!");
            TextList.Add("No worries. I must be going now. I've want to meet with the other sorcerers on my way home.");
            TextList.Add("Say hello to everyone for me. Oh, and let me know if you ever need some healthy additions to your diet!");
            TextList.Add("Thanks, Sebastian. I'll get back to you on that. I'll honestly probably be sticking to my favorite snacks and pastries.");
        }
    }

    private void SceneLogic()
    {
        if (isFirstTime)
        {
            if (textIndex == 3)
            {
                SetUpCameras("Graveyard");
                transform.position = new Vector3(-25, 16, 38);
                transform.Rotate(0, 130, 0);
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 5)
            {
                GameObject mage = Instantiate(Resources.Load(@"MagesForBoard\PurpleMage", typeof(GameObject)) as GameObject);
                mage.transform.position = new Vector3(4, 0, -4);
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 6)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 7)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 8)
            {
                SetUpTextBoxes("none");
                PlayerPrefs.SetString("Player1Color", "White");
                PlayerPrefs.SetString("Player2Color", "Purple");
                PlayerPrefs.SetInt("StoryStage", 2);
                LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
                StartCoroutine(LoadAsync(5));
            }
            else
            {
                if (textbox == "main")
                    autoTypeMain.StartText(TextList[textIndex]);
                if (textbox == "right")
                    autoTypeRight.StartText(TextList[textIndex]);
                if (textbox == "left")
                    autoTypeLeft.StartText(TextList[textIndex]);
            }
        }
        else
        {
            if (textIndex == 1)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 2)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 3)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 4)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 5)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 6)
            {
                LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
                StartCoroutine(LoadAsync(12));
            }
            else
            {
                if (textbox == "main")
                    autoTypeMain.StartText(TextList[textIndex]);
                if (textbox == "right")
                    autoTypeRight.StartText(TextList[textIndex]);
                if (textbox == "left")
                    autoTypeLeft.StartText(TextList[textIndex]);
            }
        }
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
                autoTypeMain.autoType = true;
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
                autoTypeRight.autoType = true;
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
                autoTypeLeft.autoType = true;
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