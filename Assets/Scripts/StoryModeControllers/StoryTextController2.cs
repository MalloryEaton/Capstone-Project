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

    private GameObject purpleMage;
    
    void Start()
    {
        purpleMage = GameObject.Find("PurpleMage");
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
        purpleMage.SetActive(false);
        if (isFirstTime)
            autoTypeMain.StartText(TextList[0]);
        else
        {
            purpleMage.SetActive(true);
            textbox = "left";
            SetUpTextBoxes("left");

            autoTypeLeft.StartText(TextList[0]);
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
            TextList.Add("After tangling with Sebastian in the forest, Targus sets out again on his journey home. On the way, he makes another detour, stopping to visit his friend in the graveyard.");
            TextList.Add("Surely Sebastian's fit of madness was just an unfortunate encounter with the wrong fungus, right?");
            TextList.Add("I'm sure glad that Sebastian is back to normal. That was frightening!");
            TextList.Add("I'll have to tell Sir Gilbaard about it. He could use a good laugh; crypt keepers are always so... grave.");
            TextList.Add("Good evening sir, you look like death walking. May I interest you in our premium package?");
            TextList.Add("Hey, Sir Gilbaard. How's life? Or, well, the lack thereof?");
            TextList.Add("I can assure you that it is a steal of a deal!");
            TextList.Add("I’m afraid I have no idea what you’re talking about.");
            TextList.Add("I'm talking about a free casket! As a special bonus, I'll provide the means for you to get there!");
            TextList.Add("");
        }
        else
        {
            TextList.Add("Sir Gilbaard, are you alright?");
            TextList.Add("It seems I'll be staying among the living for now. Well... still among the dead.");
            TextList.Add("It can't be coincidence that both you and Sebastian tried to attack me. Can you remember anything?");
            TextList.Add("Hmmm... I think I can remember a fellow coming through here. He seemed particularly interested in the undead.");
            TextList.Add("Interesting. Can you remember anything else?");
            TextList.Add("I'm afraid not.");
            TextList.Add("Well... I'm on my way to meet with Merwin Etherfrost. Maybe he can help us figure out what is going on.");
            TextList.Add("Hopefully, I won't be walking into a showdown in the desert...");
            TextList.Add("I wish you luck, dear Targus. I must get back to my duties, tending to the dead.");
            TextList.Add("Be sure to get out every once in a while, talk to some people. Preferably those with heartbeats.");
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
                SetUpCameras("Graveyard");
                transform.position = new Vector3(15, 16, 9);
                transform.Rotate(0, 130, 0);
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 4)
            {
                purpleMage.SetActive(true);
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
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 8)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 9)
            {
                LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
                SetUpTextBoxes("none");
                PlayerPrefs.SetString("Player1Color", "White");
                PlayerPrefs.SetString("Player2Color", "Purple");
                PlayerPrefs.SetString("Difficulty", "Easy");
                PlayerPrefs.SetString("GameType", "Story");
                StartCoroutine(LoadAsync(5));
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
            if (textIndex == 1)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 2)
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
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 8)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 9)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 10)
            {
                LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
                StartCoroutine(LoadAsync(13));
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