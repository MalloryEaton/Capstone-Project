using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerTutorialScript : MonoBehaviour
{
    public GameObject TutorialPanel;
    public GameObject BlackPanel;
    public List<GameObject> TutorialSlides;
    public GameObject BackButton;
    private TutorialLogic TL;

    public int slideIndex;

    void Awake()
    {
        TL = FindObjectOfType(typeof(TutorialLogic)) as TutorialLogic;
        slideIndex = 0;
        TutorialSlides[0].SetActive(true);
        BackButton.SetActive(true);
        for (int i = 1; i < TutorialSlides.Count; i++)
        {
            TutorialSlides[i].SetActive(false);
        }
    }

    public void nextSlide()
    {
        if (slideIndex == 3) //one before empty
        {
            TL.SetupText();

            TutorialPanel.SetActive(false);
        }

        if (slideIndex < TutorialSlides.Count-1)
        {
            TutorialSlides[slideIndex].SetActive(false);
            TutorialSlides[slideIndex + 1].SetActive(true);
            slideIndex++;
        }
        else
        {
            TutorialSlides[slideIndex].SetActive(false);
        }
    }

    public void backSlide()
    {
        if(slideIndex > 0)
        {
            TutorialSlides[slideIndex].SetActive(false);
            TutorialSlides[slideIndex - 1].SetActive(true);
            slideIndex--;
        }
        else
        {
            BackButton.SetActive(false);
        }
    }

}
