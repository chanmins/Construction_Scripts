using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TutorialStep
{
    public string instruction;
}

public class Tutorial : MonoBehaviour
{
    public GameObject tutorial_Panel;
    public TextMeshProUGUI instructionText;

    public Button next;
    public Button startTutorial;
    public Button endTutorial;

    public List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    private int currentStep;

    private bool isTyping;

    public GameObject simon_Demo;
    private Simon_Demo simonScript;

    private void Start()
    {
        simonScript = simon_Demo.GetComponent<Simon_Demo>();
        isTyping = false;
        currentStep = 0;
        endTutorial.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        startTutorial.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (simonScript.flag)
        {
            NextStep();
            simonScript.flag = false;
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
            StartCoroutine(TypeText(tutorialSteps[currentStep].instruction));
        }
        else
        {
            EndTutorial();
        }

        if(currentStep == 1)
        {
            StartCoroutine(simonScript.ShowLightImagesSequentially());
            next.gameObject.SetActive(false);
        }

        if (currentStep >= 3)
        {
            simonScript.timer.gameObject.SetActive(true);
            next.gameObject.SetActive(true);
        }

        // 마지막 step에 도착하면 다음버튼 비활성화
        if (currentStep == tutorialSteps.Count - 1)
        {
            next.gameObject.SetActive(false);
            simonScript.timer.gameObject.SetActive(false);
            endTutorial.gameObject.SetActive(true);
        }
    }

    private void ShowStep(int stepIndex)
    {
        instructionText.text = tutorialSteps[stepIndex].instruction;
    }

    public void EndTutorial()
    {
        SceneManager.LoadScene("Simon");
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
