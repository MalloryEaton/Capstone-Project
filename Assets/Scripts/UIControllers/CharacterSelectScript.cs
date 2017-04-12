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
    public Text PageHeader;

    static public bool isCharacterSelected;
    static public string currentPlayerColor;

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
        BioMenu.GetComponent<Animator>().SetBool("isDisplayed", false);
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
        BioMenu.GetComponent<Animator>().SetBool("isDisplayed", true);
        print("character selected");

        PlayerPrefs.SetString(currentPlayerColor, selection);
        
        //PlayerPrefs.SetString("PlayerColor", selection);

        print(currentPlayerColor);

        if (selection == "Black")
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
        else if (selection == "Blue")
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
            CharacterBio.text = "Fariday Fink is famous for his ability to think fast on his feet. Though his cunning is great, he prefers a more relaxing life among the soothing waters of Lapucha, the floating island know for its waterfalls.";
        }
        else if (selection == "Orange")
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
        else if (selection == "Green")
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
        else if (selection == "Yellow")
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
        else if (selection == "Red")
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
            CharacterBio.text = "Quin Zoltan is rumored to have been born out of the fires of Mount Goragundi. Naturally, he learned to harness fire from a young age. So young, in fact, that travelers would come from lands far away just to witness the spectacle.";
        }
        else if (selection == "White")
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
            CharacterBio.text = "As a child, Targus Zweilander was given a prophecy that he was destined to face a great trial in his lifetime. This knowledge drove him to attain greatness, and he is renowned for his mastery of ancient Virillian sorcery. He is the only known living sorcerer to have uncovered its secrets.";
        }
        else if (selection == "Purple")
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
            CharacterBio.text = "Sir Gilbaard was born into royalty. He was set to become the ruler of Remmelford, that is until his facination with necromancy drove him from a life of luxury to a life as an undertaker, tending to the dead.";
        }
    }

    public void DeselectCharacter()
    {
        PlayerPrefs.SetString("PlayerColor", "");
        PlayerPrefs.SetString("Player1Color", "");
        PlayerPrefs.SetString("Player2Color", "");

        isCharacterSelected = false;
        ResetMages(false);
        BioMenu.GetComponent<Animator>().SetBool("isDisplayed", false);
    }

    public void CharacterHover(string selection)
    {
        if (!isCharacterSelected)
        {
            string player1Color = PlayerPrefs.GetString("Player1Color");
            if (selection == "Black" && selection != player1Color)
            {
                BlackMage.SetActive(true);
            }
            else if (selection == "Blue" && selection != player1Color)
            {
                BlueMage.SetActive(true);
            }
            else if (selection == "Orange" && selection != player1Color)
            {
                OrangeMage.SetActive(true);
            }
            else if (selection == "Green" && selection != player1Color)
            {
                GreenMage.SetActive(true);
            }
            else if (selection == "Yellow" && selection != player1Color)
            {
                YellowMage.SetActive(true);
            }
            else if (selection == "Red" && selection != player1Color)
            {
                RedMage.SetActive(true);
            }
            else if (selection == "White" && selection != player1Color)
            {
                WhiteMage.SetActive(true);
            }
            else if (selection == "Purple" && selection != player1Color)
            {
                PurpleMage.SetActive(true);
            }
        }
    }

    public void CharacterExit(string selection)
    {
        if (!isCharacterSelected)
        {
            if (selection == "Black")
            {
                BlackMage.SetActive(false);
            }
            else if (selection == "Blue")
            {
                BlueMage.SetActive(false);
            }
            else if (selection == "Orange")
            {
                OrangeMage.SetActive(false);
            }
            else if (selection == "Green")
            {
                GreenMage.SetActive(false);
            }
            else if (selection == "Yellow")
            {
                YellowMage.SetActive(false);
            }
            else if (selection == "Red")
            {
                RedMage.SetActive(false);
            }
            else if (selection == "White")
            {
                WhiteMage.SetActive(false);
            }
            else if (selection == "Purple")
            {
                PurpleMage.SetActive(false);
            }
        }
    }

}
