using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTextController : MonoBehaviour
{
    private List<string> TextList;
    private AutoType autoType;

    private int textIndex;

	// Use this for initialization
	void Start ()
    {
        textIndex = 0;
		autoType = FindObjectOfType(typeof(AutoType)) as AutoType;
        TextList = new List<string>();
        InitializeTextList();

        autoType.StartText(TextList[0]);
    }
	
	private void InitializeTextList()
    {
        TextList.Add("In a world far away from our own lies the Kingdom Of Derraveth. This kingdom thrives on the energy given off by ancient magic artifacts which the citizens call shrines. The king of Derraveth employs seven sorcerers who protect the shrines, using magic to keep evil-doers at bay.");
        TextList.Add("The most renowned of these sorcerers is Targus Zweilander. His skills in the ancient art of Virillian sorcerery has made him the stongest and wisest of all sorcerers. He oversees the kingdom from the keep of his tower, a stronghold floating high above the land.");
        TextList.Add("");
        TextList.Add("");
        TextList.Add("");
        TextList.Add("");
        TextList.Add("");
        TextList.Add("");
        TextList.Add("");
        TextList.Add("");
        TextList.Add("");
        TextList.Add("");
        TextList.Add("");
    }

    private void OnMouseDown()
    {
        if (autoType.autoType)
            autoType.autoType = false;
        else
        {
            autoType.autoType = true;
            textIndex++;
            autoType.StartText(TextList[textIndex]);
        }
    }
}
