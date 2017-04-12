using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryTextController4 : MonoBehaviour
{
    private List<string> TextList;
    private AutoTypeMainBox autoTypeMain;
    private AutoTypeLeftBox autoTypeLeft;
    private AutoTypeRightBox autoTypeRight;

    private int textIndex;

    public List<Camera> cameras;
    public List<Light> mainLights;
    public List<Light> volcanoLights;

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

    private GameObject redMage;
    private GameObject whiteMage;
    private GameObject flyRock;
    private GameObject lavaPlume;

    void Start()
    {
        redMage = GameObject.Find("RedMage");
        whiteMage = GameObject.Find("WhiteMage");
        flyRock = GameObject.Find("FlyRock");
        lavaPlume = GameObject.Find("LavaPlume");
        if (PlayerPrefs.GetInt("StoryStage") == 4)
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
            SetUpCameras("Volcano");
        SetUpTextBoxes("main");
        LoadingPanel = GameObject.Find("LoadingPanel");
        textIndex = 0;
        autoTypeMain = FindObjectOfType(typeof(AutoTypeMainBox)) as AutoTypeMainBox;
        autoTypeLeft = FindObjectOfType(typeof(AutoTypeLeftBox)) as AutoTypeLeftBox;
        autoTypeRight = FindObjectOfType(typeof(AutoTypeRightBox)) as AutoTypeRightBox;
        redMage.SetActive(false);
        if (isFirstTime)
            autoTypeMain.StartText(TextList[0]);
        else
        {
            redMage.SetActive(true);
            textbox = "right";
            SetUpTextBoxes("right");
            autoTypeRight.autoType = true;
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
        lightsList.Add(volcanoLights);
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
        else if (cam == "Volcano")
        {
            cameras[1].enabled = true;
            foreach (Light light in volcanoLights)
            {
                light.enabled = true;
            }
        }
    }

    private void InitializeTextList()
    {
        if (isFirstTime)
        {
            TextList.Add("To find his next friend, Targus had to travel over the seas to the volcanic island of Goragundi, named after the volcano which created it.");
            TextList.Add("He hoped to find Quin Zoltan, the famous fire sorecerer and protector of the island, before someone else did.");
            TextList.Add("I’m certain Quin is around here somewhere. He’s always been a homebody.");
            TextList.Add("Hey, you! Come here! Want to take a dip in my state-of-the-art hot tub?");
            TextList.Add("Don’t mind the red coloring. That’s just, um, colored dye.");
            TextList.Add("Very tempting, but I'm going to have to pass.");
            TextList.Add("Hmph, it appears you aren’t as stupid as you look!");
            TextList.Add("");
        }
        else
        {
            TextList.Add("Targus, I remember everything! The one enslaving the minds of the king's sorcerers in none other than Iver Hagroot!");
            TextList.Add("It seems he has come back with some new skills. He dabbling in necromancy now!");
            TextList.Add("What?! I always knew he would make a return.");
            TextList.Add("He is stronger now than ever before! I was no match for him, Targus. You have to stop this barbarian!");
            TextList.Add("I’ve defeated him once, and I can do it again! Where do you think will he go next?");
            TextList.Add("To the floating isle of Lapucha, I would assume. He's probably already gotten to our friend, Fariday.");
            TextList.Add("I must get there as quickly as possible, then.");
            TextList.Add("Here, hang on to this rock.");
            TextList.Add("What are you doing? I suppose you are going to cause the volcano to blast me into th-- AAAAAAAHHHHHH!!!");
            TextList.Add("");
            TextList.Add("Precisely!");
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
                SetUpCameras("Volcano");
                transform.position = new Vector3(15, 16, 9);
                transform.Rotate(0, 130, 0);
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 3)
            {
                redMage.SetActive(true);
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 5)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 6)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 7)
            {
                LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
                SetUpTextBoxes("none");
                PlayerPrefs.SetString("Player1Color", "White");
                PlayerPrefs.SetString("Player2Color", "Red");
                PlayerPrefs.SetString("Difficulty", "Medium");
                PlayerPrefs.SetString("GameType", "Story");
                StartCoroutine(LoadAsync(7));
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
                autoTypeLeft.autoType = false;
                LeanTween.moveY(flyRock, 50, 0.4f);
                LeanTween.moveY(whiteMage, 50, 0.4f);
                LeanTween.moveY(lavaPlume, 100, 1f);
            }
            else if (textIndex == 10)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 11)
            {
                LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
                StartCoroutine(LoadAsync(15));
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
        print(textIndex);
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