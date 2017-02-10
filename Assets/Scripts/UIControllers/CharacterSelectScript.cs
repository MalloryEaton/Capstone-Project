using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSelectScript : MonoBehaviour {

    public GameObject BlackMage;
    public GameObject BlueMage;
    public GameObject OrangeMage;
    public GameObject GreenMage;
    public GameObject YellowMage;
    public GameObject RedMage;
    public GameObject WhiteMage;
    public GameObject PurpleMage;
    public GameObject BioMenu;
    public Text CharacterName;
    public Text CharacterBio;

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
        BioMenu.SetActive(false);
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
        BioMenu.SetActive(true);

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
            CharacterName.text = "Iver Hagroot";
            CharacterBio.text = "Iver is something of a mystery. Legend has it that he was born in the Mangleroot swamps. Whatever the case may be, he is thirsty for power and will stop at nothing to get it.";

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
            CharacterName.text = "Fariday Fink";
            CharacterBio.text = "Fariday Fink is infamous for his cunning. Able to think fast on his feet, he is almost always one step ahead. Almost.";
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
            CharacterName.text = "Theodore Darden";
            CharacterBio.text = "If there ever was a bookworm, Theodore Darden would be it. Ever since he was a child, he had his nose stuck in one piece of literature or another. His skill as a sorcerer is a reflection of his inquisitive mind. If only the same could be said of his social skill...";
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
            CharacterName.text = "Sebastian Meriweather";
            CharacterBio.text = "Hailing from the eastern druidic colonies, Sebastian has a knack for alchemy. His deep alchemical knowledge gave way to the development of many popular potions and concoctions used by many sorcerers today.";
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
            CharacterName.text = "Merwin Etherfrost";
            CharacterBio.text = "Merwin Etherfrost spent his youth in training to become a cleric in the Order of the Sun. However, after a fallout with his mentor, he left his humble life to pursue the sorcerous arts.";
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
            CharacterName.text = "Quin Zoltan";
            CharacterBio.text = "Quin Zoltan was born in a small village at the base of the great volcano Mount Goragundi. Naturally, he learned to harness fire from a young age. So young, in fact, that travelers would come from lands far away just to witness the spectacle themselves.";
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
            CharacterName.text = "Targus Zweilander";
            CharacterBio.text = "Targus Zweilander was destined to face a great trial in his lifetime. This burden drove him to greatness, and he is renowned for his mastery of ancient Virillian sorcery. He is the only known living sorcerer to have uncovered its secrets.";
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
            CharacterName.text = "Sir Gilbaard";
            CharacterBio.text = "Sir Gilbaard was born into royalty. He was set to become the ruler of Remmelford until it was overthrown by the roaming barbarian clans. He now wanders the colonies, determined to avenge his fallen kingdom.";
        }
    }

    public void DeselectCharacter()
    {
        isCharacterSelected = false;
        ResetMages(false);
        BioMenu.SetActive(false);
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
