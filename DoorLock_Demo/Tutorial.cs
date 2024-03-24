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
    public TextMeshProUGUI instructionText;
    public Button next;
    public Button startTutorial;
    public Button endTutorial;

    public List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    public GameObject[] numbers;
    [SerializeField]
    private int currentStep;

    private bool isTyping;

    public RectTransform tutorialPanelRectTransform;
    public Vector2[] panelPositions;
    private int currentPosIndex;
    private int targetPosIndex;

    public GameObject doorLockScript;
    private DoorLock_Truck_Demo doorLock;

    private void Start()
    {
        doorLock = doorLockScript.GetComponent<DoorLock_Truck_Demo>();
        currentPosIndex = 0;
        targetPosIndex = 0;
        isTyping = false;
        currentStep = 0;
        endTutorial.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        startTutorial.gameObject.SetActive(true);

        foreach (GameObject gameobject in numbers)
        {
            gameobject.SetActive(false);
        }
    }

    public void StartTutorial()
    {
        if (isTyping) return;

        if (currentStep == 0)
            next.gameObject.SetActive(false);

        ShowStep(currentStep);
        startTutorial.gameObject.SetActive(false);
        StartCoroutine(TypeText(tutorialSteps[currentStep].instruction));
        Invoke("NextStep", 7.0f);
        Invoke("Move", 3.0f);
        MoveTutorialPanel();
    }

    private void Move()
    {
        doorLock.interactFlag = true;
    }

    public void NextStep()
    {
        if (isTyping) return;

        currentStep++;
        
        if (currentStep < tutorialSteps.Count)
        {
            if (currentStep == 1)
            {
                doorLock.interactFlag = false;
                next.gameObject.SetActive(false);
                numbers[0].SetActive(true);
            }

            ShowStep(currentStep);
            StartCoroutine(TypeText(tutorialSteps[currentStep].instruction));

            targetPosIndex = (targetPosIndex + 1) % panelPositions.Length;

            MoveTutorialPanel();
        }

        // 마지막 step에 도착하면 다음버튼 비활성화
        if (currentStep == tutorialSteps.Count - 1)
        {
            endTutorial.gameObject.SetActive(true);
        }
    }

    private void ShowStep(int stepIndex)
    {
        instructionText.text = tutorialSteps[stepIndex].instruction;
    }

    public void EndTutorial()
    {
        SceneManager.LoadScene("DoorLock");
    }

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

    private void MoveTutorialPanel()
    {
        currentPosIndex = targetPosIndex;
        tutorialPanelRectTransform.anchoredPosition = panelPositions[currentPosIndex];
    }
}
