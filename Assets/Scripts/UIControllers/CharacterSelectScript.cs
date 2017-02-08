using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectScript : MonoBehaviour {

    public GameObject BlackMage;
    public GameObject BlueMage;
    public GameObject OrangeMage;
    public GameObject GreenMage;
    public GameObject YellowMage;
    public GameObject RedMage;
    public GameObject WhiteMage;
    public GameObject PurpleMage;

    static public bool isCharacterSelected;

    static public CharacterSelectScript characterSelectScript;

    private void Start()
    {
        BlackMage.SetActive(false);
        BlueMage.SetActive(false);
        OrangeMage.SetActive(false);
        GreenMage.SetActive(false);
        YellowMage.SetActive(false);
        RedMage.SetActive(false);
        WhiteMage.SetActive(false);
        PurpleMage.SetActive(false);
        isCharacterSelected = false;
        characterSelectScript = this;
    }

    public void ResetMages(bool active)
    {
        BlackMage.SetActive(active);
        BlueMage.SetActive(active);
        OrangeMage.SetActive(active);
        GreenMage.SetActive(active);
        YellowMage.SetActive(active);
        RedMage.SetActive(active);
        WhiteMage.SetActive(active);
        PurpleMage.SetActive(active);
    }

    public void CharacterSelected (string selection)
    {
        isCharacterSelected = true;

        if (selection == "black")
        {
            BlackMage.SetActive(true);
            BlueMage.SetActive(false);
            OrangeMage.SetActive(false);
            GreenMage.SetActive(false);
            YellowMage.SetActive(false);
            RedMage.SetActive(false);
            WhiteMage.SetActive(false);
            PurpleMage.SetActive(false);

        }
        else if (selection == "blue")
        {
            BlackMage.SetActive(false);
            BlueMage.SetActive(true);
            OrangeMage.SetActive(false);
            GreenMage.SetActive(false);
            YellowMage.SetActive(false);
            RedMage.SetActive(false);
            WhiteMage.SetActive(false);
            PurpleMage.SetActive(false);
        }
        else if (selection == "orange")
        {
            BlackMage.SetActive(false);
            BlueMage.SetActive(false);
            OrangeMage.SetActive(true);
            GreenMage.SetActive(false);
            YellowMage.SetActive(false);
            RedMage.SetActive(false);
            WhiteMage.SetActive(false);
            PurpleMage.SetActive(false);
        }
        else if (selection == "green")
        {
            BlackMage.SetActive(false);
            BlueMage.SetActive(false);
            OrangeMage.SetActive(false);
            GreenMage.SetActive(true);
            YellowMage.SetActive(false);
            RedMage.SetActive(false);
            WhiteMage.SetActive(false);
            PurpleMage.SetActive(false);
        }
        else if (selection == "yellow")
        {
            BlackMage.SetActive(false);
            BlueMage.SetActive(false);
            OrangeMage.SetActive(false);
            GreenMage.SetActive(false);
            YellowMage.SetActive(true);
            RedMage.SetActive(false);
            WhiteMage.SetActive(false);
            PurpleMage.SetActive(false);
        }
        else if (selection == "red")
        {
            BlackMage.SetActive(false);
            BlueMage.SetActive(false);
            OrangeMage.SetActive(false);
            GreenMage.SetActive(false);
            YellowMage.SetActive(false);
            RedMage.SetActive(true);
            WhiteMage.SetActive(false);
            PurpleMage.SetActive(false);
        }
        else if (selection == "white")
        {
            BlackMage.SetActive(false);
            BlueMage.SetActive(false);
            OrangeMage.SetActive(false);
            GreenMage.SetActive(false);
            YellowMage.SetActive(false);
            RedMage.SetActive(false);
            WhiteMage.SetActive(true);
            PurpleMage.SetActive(false);
        }
        else if (selection == "purple")
        {
            BlackMage.SetActive(false);
            BlueMage.SetActive(false);
            OrangeMage.SetActive(false);
            GreenMage.SetActive(false);
            YellowMage.SetActive(false);
            RedMage.SetActive(false);
            WhiteMage.SetActive(false);
            PurpleMage.SetActive(true);
        }
    }

    public void CharacterHover(string selection)
    {
        if (!isCharacterSelected)
        {


            if (selection == "black")
            {
                BlackMage.SetActive(true);
                //BlueMage.SetActive(false);
                //OrangeMage.SetActive(false);
                //GreenMage.SetActive(false);
                //YellowMage.SetActive(false);
                //RedMage.SetActive(false);
                //WhiteMage.SetActive(false);
                //PurpleMage.SetActive(false);
            }
            else if (selection == "blue")
            {
                //BlackMage.SetActive(false);
                BlueMage.SetActive(true);
                //OrangeMage.SetActive(false);
                //GreenMage.SetActive(false);
                //YellowMage.SetActive(false);
                //RedMage.SetActive(false);
                //WhiteMage.SetActive(false);
                //PurpleMage.SetActive(false);
            }
            else if (selection == "orange")
            {
                //BlackMage.SetActive(false);
                //BlueMage.SetActive(false);
                OrangeMage.SetActive(true);
                //GreenMage.SetActive(false);
                //YellowMage.SetActive(false);
                //RedMage.SetActive(false);
                //WhiteMage.SetActive(false);
                //PurpleMage.SetActive(false);
            }
            else if (selection == "green")
            {
                //BlackMage.SetActive(false);
                //BlueMage.SetActive(false);
                //OrangeMage.SetActive(false);
                GreenMage.SetActive(true);
                //YellowMage.SetActive(false);
                //RedMage.SetActive(false);
                //WhiteMage.SetActive(false);
                //PurpleMage.SetActive(false);
            }
            else if (selection == "yellow")
            {
                //BlackMage.SetActive(false);
                //BlueMage.SetActive(false);
                //OrangeMage.SetActive(false);
                //GreenMage.SetActive(false);
                YellowMage.SetActive(true);
                //RedMage.SetActive(false);
                //WhiteMage.SetActive(false);
                //PurpleMage.SetActive(false);
            }
            else if (selection == "red")
            {
                //BlackMage.SetActive(false);
                //BlueMage.SetActive(false);
                //OrangeMage.SetActive(false);
                //GreenMage.SetActive(false);
                //YellowMage.SetActive(false);
                RedMage.SetActive(true);
                //WhiteMage.SetActive(false);
                //PurpleMage.SetActive(false);
            }
            else if (selection == "white")
            {
                //BlackMage.SetActive(false);
                //BlueMage.SetActive(false);
                //OrangeMage.SetActive(false);
                //GreenMage.SetActive(false);
                //YellowMage.SetActive(false);
                //RedMage.SetActive(false);
                WhiteMage.SetActive(true);
                //PurpleMage.SetActive(false);
            }
            else if (selection == "purple")
            {
                //BlackMage.SetActive(false);
                //BlueMage.SetActive(false);
                //OrangeMage.SetActive(false);
                //GreenMage.SetActive(false);
                //YellowMage.SetActive(false);
                //RedMage.SetActive(false);
                //WhiteMage.SetActive(false);
                PurpleMage.SetActive(true);
            }
        }
    }

    public void CharacterExit(string selection)
    {
        if (!isCharacterSelected)
        {
            if (selection == "black")
            {
                BlackMage.SetActive(false);
            }
            else if (selection == "blue")
            {
                BlueMage.SetActive(false);
            }
            else if (selection == "orange")
            {
                OrangeMage.SetActive(false);
            }
            else if (selection == "green")
            {
                GreenMage.SetActive(false);
            }
            else if (selection == "yellow")
            {
                YellowMage.SetActive(false);
            }
            else if (selection == "red")
            {
                RedMage.SetActive(false);
            }
            else if (selection == "white")
            {
                WhiteMage.SetActive(false);
            }
            else if (selection == "purple")
            {
                PurpleMage.SetActive(false);
            }
        }
    }

}
