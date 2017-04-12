using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryTextController5 : MonoBehaviour
{
    private List<string> TextList;
    private AutoTypeMainBox autoTypeMain;
    private AutoTypeLeftBox autoTypeLeft;
    private AutoTypeRightBox autoTypeRight;
    private AutoTypeRightBox autoTypeAll;

    private int textIndex;

    public List<Camera> cameras;
    public List<Light> mainLights;
    public List<Light> waterLights;

    public List<Sprite> mageSprites;

    public GameObject mainTextBox;
    public GameObject rightTextBox;
    public GameObject leftTextBox;
    public GameObject allTextBox;

    private Vector3 mainTextOriginalPosition;
    private Vector3 rightTextOriginalPosition;
    private Vector3 leftTextOriginalPosition;
    private Vector3 allTextOriginalPosition;

    private string textbox;

    private GameObject LoadingPanel;

    private List<List<Light>> lightsList;

    private bool isFirstTime = true;

    private GameObject blueMage;
    private GameObject greenMage;
    private GameObject redMage;
    private GameObject orangeMage;
    private GameObject purpleMage;
    private GameObject whiteMage;
    private GameObject yellowMage;

    public Image mageImage;
    public Text mageName;

    void Start()
    {
        blueMage = GameObject.Find("BlueMage");
        whiteMage = GameObject.Find("WhiteMage");
        greenMage = GameObject.Find("GreenMage");
        redMage = GameObject.Find("RedMage");
        purpleMage = GameObject.Find("PurpleMage");
        orangeMage = GameObject.Find("OrangeMage");
        yellowMage = GameObject.Find("YellowMage");
        if (PlayerPrefs.GetInt("StoryStage") == 5)
        {
            isFirstTime = false;
            transform.position = new Vector3(14, 16, 5);
            transform.Rotate(0, 130, 0);
        }
        mainTextOriginalPosition = mainTextBox.GetComponent<RectTransform>().position;
        rightTextOriginalPosition = mainTextBox.GetComponent<RectTransform>().position;
        leftTextOriginalPosition = mainTextBox.GetComponent<RectTransform>().position;
        allTextOriginalPosition = mainTextBox.GetComponent<RectTransform>().position;
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
            SetUpCameras("Water");
        SetUpTextBoxes("main");
        LoadingPanel = GameObject.Find("LoadingPanel");
        textIndex = 0;
        autoTypeMain = FindObjectOfType(typeof(AutoTypeMainBox)) as AutoTypeMainBox;
        autoTypeLeft = FindObjectOfType(typeof(AutoTypeLeftBox)) as AutoTypeLeftBox;
        autoTypeRight = FindObjectOfType(typeof(AutoTypeRightBox)) as AutoTypeRightBox;
        autoTypeAll = FindObjectOfType(typeof(AutoTypeRightBox)) as AutoTypeRightBox;
        DisableMages();
        blueMage.SetActive(false);
        if (isFirstTime)
            autoTypeMain.StartText(TextList[0]);
        else
        {
            blueMage.SetActive(true);
            textbox = "left";
            SetUpTextBoxes("left");
            autoTypeLeft.autoType = true;
            autoTypeLeft.StartText(TextList[0]);
        }
    }

    private void DisableMages()
    {
        redMage.SetActive(false);
        yellowMage.SetActive(false);
        orangeMage.SetActive(false);
        greenMage.SetActive(false);
        purpleMage.SetActive(false);
    }

    private void InstantiateShine(string color)
    {
        GameObject shrine = Instantiate(Resources.Load(@"ShrinesTutorial\Shrine" + color, typeof(GameObject)) as GameObject);
        shrine.transform.position = new Vector3(12, 0, 12);
    }

    private void SetUpLightsList()
    {
        lightsList.Add(mainLights);
        lightsList.Add(waterLights);
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
        else if (cam == "Water")
        {
            cameras[1].enabled = true;
            foreach (Light light in waterLights)
            {
                light.enabled = true;
            }
        }
    }

    private void InitializeTextList()
    {
        if (isFirstTime)
        {
            TextList.Add("In an instant, Targus went from the furious depths of Mount Goragundi to the cool, calming waterfalls of the isle of Lapucha.");
            TextList.Add("The soothing sounds, slight breeze, and cool spray of the waterfalls made Targus relax for just a moment. The relaxation was short lived, as an angry sorcerer awaited him in the floating waters.");
            TextList.Add("It appears my plot armor was strong enough to survive the crash landing!");
            TextList.Add("Your luck runs out there! You will feel the commotion of the entire ocean crashing down upon your shoulders!");
            TextList.Add("More theme-related threats, I see. This is starting to get old. Fariday, where Iver Hagroot?");
            TextList.Add("You’re light years away from facing him! Prepare to be washed away!");
            TextList.Add("");
        }
        else
        {
            TextList.Add("Fariday, are you alright?");
            TextList.Add("Blech... You would think that salt would make water taste better. I'm fine, but I have some terrible news.");
            TextList.Add("Iver Hagroot has taken over your tower and has put up a powerful protective barrier around it!");
            TextList.Add("There must be a way in somehow...");
            TextList.Add("I'm not powerful enough to help you get through that barrier.");

            TextList.Add("Fortunately, you are not alone. Sebastian Meriweather here!");
            TextList.Add("I have a bone to pick with that wannabe Necromancer as well. I, Sir Gilbaard, offer my assistance.");
            TextList.Add("I'd like for the mummies of my oasis to stay where they are. Merwin Etherfrost, here to help.");
            TextList.Add("I, Quin Zoltan, offer the powers of the volcano to help.");
            TextList.Add("It seems that you lot will be requiring my power as well.");

            TextList.Add("Theodore Darden!!!");

            TextList.Add("I got word that Iver Hagroot had returned. I made the journey as quickly as possible.");
            TextList.Add("It's great to see you, Theodore. We must combine forces to break that barrier.");
            TextList.Add("Everyone on three.");
            TextList.Add("1...");
            TextList.Add("2...");
            TextList.Add("3!!!");
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
                SetUpCameras("Water");
                transform.position = new Vector3(15, 16, 9);
                transform.Rotate(0, 130, 0);
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 3)
            {
                blueMage.SetActive(true);
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
                LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
                SetUpTextBoxes("none");
                PlayerPrefs.SetString("Player1Color", "White");
                PlayerPrefs.SetString("Player2Color", "Blue");
                PlayerPrefs.SetString("Difficulty", "Hard");
                PlayerPrefs.SetString("GameType", "Story");
                StartCoroutine(LoadAsync(8));
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
            else if (textIndex == 3)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 4)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 5)
            {
                //green
                greenMage.SetActive(true);
                mageImage.sprite = mageSprites[1];
                mageName.text = "Sebastian";
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 6)
            {
                //purple
                purpleMage.SetActive(true);
                mageImage.sprite = mageSprites[2];
                mageName.text = "Sir Gilbaard";
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 7)
            {
                //yellow
                yellowMage.SetActive(true);
                mageImage.sprite = mageSprites[3];
                mageName.text = "Merwin";
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 8)
            {
                //red
                redMage.SetActive(true);
                mageImage.sprite = mageSprites[4];
                mageName.text = "Quin";
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 9)
            {
                //orange
                orangeMage.SetActive(true);
                mageImage.sprite = mageSprites[5];
                mageName.text = "Theodore";
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 10)
            {
                //all
                textbox = "all";
                SetUpTextBoxes("all");
            }
            else if (textIndex == 11)
            {
                //orange
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if (textIndex == 12)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 17)
            {
                LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
                StartCoroutine(LoadAsync(16));
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
            allTextBox.transform.position = new Vector3(allTextOriginalPosition.x, 1000, 0);
        }
        else if (box == "left")
        {
            mainTextBox.transform.position = new Vector3(mainTextOriginalPosition.x, 1000, 0);
            leftTextBox.transform.position = leftTextOriginalPosition;
            rightTextBox.transform.position = new Vector3(rightTextOriginalPosition.x, 1000, 0);
            allTextBox.transform.position = new Vector3(allTextOriginalPosition.x, 1000, 0);
        }
        else if (box == "right")
        {
            mainTextBox.transform.position = new Vector3(mainTextOriginalPosition.x, 1000, 0);
            leftTextBox.transform.position = new Vector3(leftTextOriginalPosition.x, 1000, 0);
            rightTextBox.transform.position = rightTextOriginalPosition;
            allTextBox.transform.position = new Vector3(allTextOriginalPosition.x, 1000, 0);
        }
        else if (box == "all")
        {
            mainTextBox.transform.position = new Vector3(mainTextOriginalPosition.x, 1000, 0);
            leftTextBox.transform.position = new Vector3(leftTextOriginalPosition.x, 1000, 0);
            rightTextBox.transform.position = new Vector3(rightTextOriginalPosition.x, 1000, 0);
            allTextBox.transform.position = allTextOriginalPosition;
        }
        else if (box == "none")
        {
            mainTextBox.transform.position = new Vector3(mainTextOriginalPosition.x, 1000, 0);
            leftTextBox.transform.position = new Vector3(leftTextOriginalPosition.x, 1000, 0);
            rightTextBox.transform.position = new Vector3(rightTextOriginalPosition.x, 1000, 0);
            allTextBox.transform.position = new Vector3(allTextOriginalPosition.x, 1000, 0);
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
        else //all
        {
            textIndex++;
            SceneLogic();
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