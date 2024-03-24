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

    public GameObject trafficLightRadiosTut;
    private TrafficLight_Radios_Tut trafficLightRadiosScript;
    public GameObject imageFader;

    private void Start()
    {
        trafficLightRadiosScript = trafficLightRadiosTut.GetComponent<TrafficLight_Radios_Tut>();
        isTyping = false;
        currentStep = 0;
        endTutorial.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        startTutorial.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (trafficLightRadiosScript.flag)
        {
            // flag�� true�� ���� nextStep ȣ��
            NextStep();
        }
    }

    // Ʃ�丮�� ����
    public void StartTutorial()
    {
        if (isTyping) return;
        
        // START��ư Ȱ��ȭ
        trafficLightRadiosScript.startButton.gameObject.SetActive(true);
        trafficLightRadiosScript.startEffect.SetActive(true);

        // ���� INSTRUCTION ������
        ShowStep(currentStep);

        // �ʹ� instruction ��Ȱ��ȭ
        startTutorial.gameObject.SetActive(false);
        
        // instruction Ÿ���� ����
        StartCoroutine(TypeText(tutorialSteps[currentStep].instruction));
    }

    // ���� ��ư
    public void NextStep()
    {
        if (isTyping) return;

        currentStep++;

        if (currentStep >= 1)
        {
            // ������ư Ȱ��ȭ�ϰ�  ��Ȱ��ȭ
            trafficLightRadiosScript.flag = false;
            next.gameObject.SetActive(true);
        }

        if (currentStep == 2)
        {
            trafficLightRadiosScript.yellowButton.gameObject.SetActive(true);
            trafficLightRadiosScript.yellowEffect.SetActive(true);
            next.gameObject.SetActive(false);
        }

        // ������ step�� �����ϸ� Ʈ�� �����̱�
        if (currentStep == tutorialSteps.Count - 1)
        {
            next.gameObject.SetActive(false);
            trafficLightRadiosScript.tutorialPanel.gameObject.SetActive(false);
            trafficLightRadiosScript.moveTruck = true;
            trafficLightRadiosScript.timerEffect.SetActive(false);
        }

        if (currentStep < tutorialSteps.Count)
        {
            ShowStep(currentStep);
            StartCoroutine(TypeText(tutorialSteps[currentStep].instruction));
        }
    }

    private void ShowStep(int stepIndex)
    {
        instructionText.text = tutorialSteps[stepIndex].instruction;
    }

    // Ʃ�丮�� ����
    public void EndTutorial()
    {
        SceneManager.LoadScene("TrafficLight");
    }

    public void ReturnMainMenu_toreact()
    {
        Application.OpenURL("https://web-template-3prof2llkxuyz4l.sel4.cloudtype.app/dashboard");
    }

    // instruction �ؽ�Ʈ �Է�
    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        instructionText.text = "";

        foreach (char letter in textToType)
        {
            instructionText.text += letter;
            yield return new WaitForSeconds(0.03f);            
        }

        isTyping = false;
    }
}