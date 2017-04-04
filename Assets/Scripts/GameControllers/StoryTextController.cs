using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTextController : MonoBehaviour
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

    private string textbox;

    private List<List<Light>> lightsList;

    // Use this for initialization
    void Start ()
    {
        TextList = new List<string>();
        InitializeTextList();
        textbox = "main";
        lightsList = new List<List<Light>>();
        SetUpLightsList();
        SetUpCameras("Main");
        SetUpTextBoxes("main");

        textIndex = 0;
        autoTypeMain = FindObjectOfType(typeof(AutoTypeMainBox)) as AutoTypeMainBox;
        autoTypeLeft = FindObjectOfType(typeof(AutoTypeLeftBox)) as AutoTypeLeftBox;
        autoTypeRight = FindObjectOfType(typeof(AutoTypeRightBox)) as AutoTypeRightBox;
        
        autoTypeMain.StartText(TextList[0]);
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

        switch (cam)
        {
            case "Main":
                cameras[0].enabled = true;
                foreach (Light light in mainLights)
                {
                    light.enabled = true;
                }
                break;
            case "Forest":
                cameras[1].enabled = true;
                foreach (Light light in forestLights)
                {
                    light.enabled = true;
                }
                break;
        }
    }
	
	private void InitializeTextList()
    {
        TextList.Add("In a world far away from our own lies the Kingdom Of Derraveth. This kingdom thrives on the energy given off by ancient magic artifacts which the citizens call shrines. The king of Derraveth employs seven sorcerers who protect the shrines, using magic to keep evil-doers at bay.");
        TextList.Add("The most renowned of these sorcerers is Targus Zweilander. His skills in the ancient art of Virillian sorcerery has made him the stongest and wisest of all sorcerers. He oversees the kingdom from the keep of his tower, a stronghold floating high above the land.");
        TextList.Add("Targus is also the liaision between the king and the sorcerers of the land. Our story begins as Targus is returning to his home after a visit with the king and the royal librarian, Theodore Darden, another sorcerer employed by the king. Targus crosses the great bridge, headed for home.");
        //zoom into map and cut to forest stage
        
        TextList.Add("I always love taking the scenic route back home. I wonder where Sebastian Meriweather is? He should be around here somewhere. Probably tending to his plants.");
        TextList.Add("Oh, there he is! How are you doing Sebastian? It's been a whi--");

        TextList.Add("Your carcass is mine, intruder! I will use you as fertilizer for my precious plants! You are no match for my garden variety of spells and alchemy!");

        TextList.Add("Oh dear, this certainly isn't the welcome I expected. Sebastian, did you get into the yellow mushrooms again? You know we aren't supposed to eat the yellow ones...");

        TextList.Add("You will make a fine meal for my babies. They haven't tasted flesh in a long time!");
        TextList.Add("");
        TextList.Add("");
        TextList.Add("");
        TextList.Add("");
        TextList.Add("");
    }

    private void SceneLogic()
    {
        if (textIndex == 3)
        {
            SetUpTextBoxes("left");
            SetUpCameras("Forest");
            textbox = "left";
            autoTypeLeft.StartText(TextList[textIndex]);
        }
        if (textIndex == 5)
        {
            textbox = "right";
            SetUpTextBoxes("right");
            autoTypeRight.StartText(TextList[textIndex]);
        }
        if (textIndex == 6)
        {
            textbox = "left";
            SetUpTextBoxes("left");
            autoTypeLeft.StartText(TextList[textIndex]);
        }
        if (textIndex == 7)
        {
            textbox = "right";
            SetUpTextBoxes("right");
            autoTypeRight.StartText(TextList[textIndex]);
        }
    }

    private void SetUpTextBoxes(string box)
    {
        if(box == "main")
        {
            mainTextBox.SetActive(true);
            rightTextBox.SetActive(false);
            leftTextBox.SetActive(false);
        }
        else if (box == "left")
        {
            leftTextBox.SetActive(true);
            mainTextBox.SetActive(false);
            rightTextBox.SetActive(false);
            print("none");
        }
        else if (box == "right")
        {
            mainTextBox.SetActive(false);
            rightTextBox.SetActive(true);
            leftTextBox.SetActive(false);
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
                autoTypeMain.autoType = true;
                textIndex++;

                if (textIndex == 3)
                {
                    SceneLogic();
                }
                else
                    autoTypeMain.StartText(TextList[textIndex]);
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

                if (textIndex == 33)
                {
                    SceneLogic();
                }
                else
                    autoTypeRight.StartText(TextList[textIndex]);
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

                if (textIndex == 33)
                {
                    SceneLogic();
                }
                else
                    autoTypeLeft.StartText(TextList[textIndex]);
            }
        }
    }
}
