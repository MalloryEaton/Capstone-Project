using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryTextController1 : MonoBehaviour
{
    private List<string> TextList;
    private AutoTypeMainBox autoTypeMain;
    private AutoTypeLeftBox autoTypeLeft;
    private AutoTypeRightBox autoTypeRight;

    private int textIndex;

    public List<Camera> cameras;
    public List<Light> mainLights;
    public List<Light> forestLights;

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

    private GameObject greenMage;

    // Use this for initialization
    void Start ()
    {
        greenMage = GameObject.Find("GreenMage");
        if (PlayerPrefs.GetInt("StoryStage") == 1)
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
        if(isFirstTime)
            InstantiateShine("Black");
        else
            InstantiateShine("White");
        textbox = "main";
        lightsList = new List<List<Light>>();
        SetUpLightsList();
        if(isFirstTime)
            SetUpCameras("Main");
        else
            SetUpCameras("Forest");
        SetUpTextBoxes("main");
        LoadingPanel = GameObject.Find("LoadingPanel");
        textIndex = 0;
        autoTypeMain = FindObjectOfType(typeof(AutoTypeMainBox)) as AutoTypeMainBox;
        autoTypeLeft = FindObjectOfType(typeof(AutoTypeLeftBox)) as AutoTypeLeftBox;
        autoTypeRight = FindObjectOfType(typeof(AutoTypeRightBox)) as AutoTypeRightBox;
        greenMage.SetActive(false);
        if (isFirstTime)
            autoTypeMain.StartText(TextList[0]);
        else
        {
            GameObject mage = Instantiate(Resources.Load(@"MagesForBoard\GreenMage", typeof(GameObject)) as GameObject);
            mage.transform.position = new Vector3(4, 0, -4);
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
        lightsList.Add(forestLights);
    }

    private void SetUpCameras(string cam)
    {
        foreach (List<Light> l in lightsList)
        {
            foreach(Light light in l)
            {
                light.enabled = false;
            }
        }

        foreach (Camera c in cameras)
        {
            c.enabled = false;
        }

        if(cam == "Main")
        {
            cameras[0].enabled = true;
            foreach (Light light in mainLights)
            {
                light.enabled = true;
            }
        }
        else if (cam == "Forest")
        {
            cameras[1].enabled = true;
            foreach (Light light in forestLights)
            {
                light.enabled = true;
            }
        }
    }
	
	private void InitializeTextList()
    {
        if (isFirstTime)
        {
            TextList.Add("In a world far away from our own lies the Kingdom Of Derraveth. This kingdom thrives on the energy given off by ancient magic artifacts which the citizens call shrines. The king of Derraveth employs seven sorcerers who protect the shrines, using magic to keep evil-doers at bay.");
            TextList.Add("The most renowned of these sorcerers is Targus Zweilander. His skills in the ancient art of Virillian sorcerery has made him the stongest and wisest of all sorcerers. He oversees the kingdom from the keep of his tower, a stronghold floating high above the land.");
            TextList.Add("Targus is returning from a visit with Theodore Darden, a sorcerer employed as the royal librarian and the liaison between the king and the sorcerers of the land. On his way home, Targus stops in the forest to visit a friend.");
            TextList.Add("I always love taking the scenic route back home. I wonder where Sebastian Meriweather is? He should be around here somewhere. Probably tending to his plants.");
            TextList.Add("Oh, there he is! How are you doing Sebastian? It's been a whi--");
            TextList.Add("Your carcass is mine, intruder! I will use you as fertilizer for my precious plants! You are no match for my garden variety of spells and alchemy!");
            TextList.Add("Oh dear, this certainly isn't the welcome I expected. Sebastian, did you get into the yellow mushrooms again? You know we aren't supposed to eat the yellow ones...");
            TextList.Add("You will make a fine meal for my children. They haven't tasted flesh in a long time!");
            TextList.Add("");
        }
        else
        {
            TextList.Add("Owwwww..... Targus, is that you? What just happened?");
            TextList.Add("Sebastian! It seems you have come to your senses. Have you been eating anything different lately? Perhaps something of the mushroom variety?");
            TextList.Add("Uhhhh... my head. I think... maybe... No, I honestly don't remember anything. I wish I could be of more help!");
            TextList.Add("Hmmmmm.... very strange. Well, I'm glad you're feeling better now. However, I must be going. I want to check up with the other sorcerers on my way home.");
            TextList.Add("Say hello to everyone for me. Oh, and let me know if you ever need some healthy additions to your diet! Maybe a nice mushroom salad?");
            TextList.Add("Thanks, Sebastian. I'll... I'll get back to you on that. *Mumbles* I don't want to give up my pastries. That would be pure pandemonium... *Mumbles*");
            TextList.Add("");
        }
    }

    private void SceneLogic()
    {
        if(isFirstTime)
        {
            if (textIndex == 3)
            {
                SetUpCameras("Forest");
                transform.position = new Vector3(14, 16, 5);
                transform.Rotate(0, 130, 0);
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if (textIndex == 5)
            {
                greenMage.SetActive(true);
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
                LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
                SetUpTextBoxes("none");
                PlayerPrefs.SetString("Player1Color", "White");
                PlayerPrefs.SetString("Player2Color", "Green");
                PlayerPrefs.SetString("Difficulty", "Easy");
                StartCoroutine(LoadAsync(4));
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
        else
        {
            if(textIndex == 1)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if(textIndex == 2)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if(textIndex == 3)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
                autoTypeLeft.StartText(TextList[textIndex]);
            }
            else if(textIndex == 4)
            {
                textbox = "right";
                SetUpTextBoxes("right");
                autoTypeRight.autoType = true;
                autoTypeRight.StartText(TextList[textIndex]);
            }
            else if(textIndex == 5)
            {
                textbox = "left";
                SetUpTextBoxes("left");
                autoTypeLeft.autoType = true;
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
        if(textbox == "main")
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