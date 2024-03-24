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
            // flag가 true일 때만 nextStep 호출
            NextStep();
        }
    }

    // 튜토리얼 시작
    public void StartTutorial()
    {
        if (isTyping) return;
        
        // START버튼 활성화
        trafficLightRadiosScript.startButton.gameObject.SetActive(true);
        trafficLightRadiosScript.startEffect.SetActive(true);

        // 현재 INSTRUCTION 보여줌
        ShowStep(currentStep);

        // 초반 instruction 비활성화
        startTutorial.gameObject.SetActive(false);
        
        // instruction 타이핑 시작
        StartCoroutine(TypeText(tutorialSteps[currentStep].instruction));
    }

    // 다음 버튼
    public void NextStep()
    {
        if (isTyping) return;

        currentStep++;

        if (currentStep >= 1)
        {
            // 다음버튼 활성화하고  비활성화
            trafficLightRadiosScript.flag = false;
            next.gameObject.SetActive(true);
        }

        if (currentStep == 2)
        {
            trafficLightRadiosScript.yellowButton.gameObject.SetActive(true);
            trafficLightRadiosScript.yellowEffect.SetActive(true);
            next.gameObject.SetActive(false);
        }

        // 마지막 step에 도착하면 트럭 움직이기
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

    // 튜토리얼 종료
    public void EndTutorial()
    {
        SceneManager.LoadScene("TrafficLight");
    }

    public void ReturnMainMenu_toreact()
    {
        Application.OpenURL("https://web-template-3prof2llkxuyz4l.sel4.cloudtype.app/dashboard");
    }

    // instruction 텍스트 입력
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