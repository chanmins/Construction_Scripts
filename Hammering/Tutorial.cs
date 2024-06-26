using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TutorialStep
{
    public Image image;
    public string instruction;
}

public class Tutorial : MonoBehaviour
{
    // 09.04 튜토리얼 플레이를 만들기 위해 수정중.
    public GameObject Hammering_main;
    Tutorial myTutorial;

    public GameObject tutorial_ImagePanel;
    public GameObject tutorial_Panel;
    public TextMeshProUGUI instructionText;
    public Button next;
    public Button previous;
    public Button startTutorial;
    public Button endTutorial;

    public List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    private int currentStep;

    private bool isTyping;

    private void Start()
    {
        isTyping = false;
        currentStep = 0;
        previous.gameObject.SetActive(false);
        endTutorial.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        startTutorial.gameObject.SetActive(true);

        // 09.04 튜토리얼 플레이를 만들기 위해 수정중.
        if(Hammering_main != null)
        {
            myTutorial = Hammering_main.GetComponent<Tutorial>();
        }
    }

    public void StartTutorial()
    {
        if (isTyping) return;

        ShowStep(currentStep);
        startTutorial.gameObject.SetActive(false);
        next.gameObject.SetActive(true);
        StartCoroutine(TypeText(tutorialSteps[currentStep].instruction));
    }

    public void NextStep()
    {
        if (isTyping) return;


        currentStep++;

        if (currentStep < tutorialSteps.Count)
        {
            ShowStep(currentStep);
            previous.gameObject.SetActive(true);
            StartCoroutine(TypeText(tutorialSteps[currentStep].instruction));
        }
        else
        {
            EndTutorial();
        }

        // 마지막 step에 도착하면 다음버튼 비활성화
        if (currentStep == tutorialSteps.Count - 1)
        {
            next.gameObject.SetActive(false);
            endTutorial.gameObject.SetActive(true);
        }
    }

    public void PreviousStep()
    {
        if (isTyping) return;

        if (currentStep > 0)
        {
            currentStep--;
            ShowStep(currentStep);
            next.gameObject.SetActive(true);
            StartCoroutine(TypeText(tutorialSteps[currentStep].instruction));
        }

        // 맨 처음 step에 도착하면 이전버튼 비활성화 
        if (currentStep == 0)
        {
            previous.gameObject.SetActive(false);
        }

        if (currentStep != tutorialSteps.Count - 1)
        {
            endTutorial.gameObject.SetActive(false);
            next.gameObject.SetActive(true);
        }
    }


    private void ShowStep(int stepIndex)
    {
        for (int i = 0; i < tutorialSteps.Count; i++)
        {
            if (i == stepIndex)
            {
                tutorialSteps[i].image.gameObject.SetActive(true); // Activate the current image
            }
            else
            {
                tutorialSteps[i].image.gameObject.SetActive(false); // Deactivate other images
            }
        }

        instructionText.text = tutorialSteps[stepIndex].instruction;
    }

    public void EndTutorial()
    {
        SceneManager.LoadScene("Hammering");
    }

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        instructionText.text = "";

        foreach(char letter in textToType)
        {
            instructionText.text += letter;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false; 
    }
}
