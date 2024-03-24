using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using TMPro;

// 3회 완료 시 바리게이트 오픈&화물차 이동

public class TrafficLight_Radios_Tut : MonoBehaviour
{
    private bool click;
    private bool lightFlag;
    private bool reactionFlag;

    public bool flag;
    public bool moveTruck;
    public bool showStartButton;

    public GameObject trafficLight;
    public GameObject move_Script;
    public GameObject tutorialPanel;

    public Vector3[] trafficLightPos = new Vector3[3];

    public TextMeshProUGUI TimeText;

    public int Count;   //전체 문제수

    private float clickTime; //Start버튼을 누른 시간.
    private float reactionTime; //신호등이 켜지고 Player가 정답을 맞추는대까지 걸린 시간.
    public float[] timeData;

    public EventTrigger startButton;
    public GameObject startEffect;
    public GameObject yellowEffect;
    public GameObject timerEffect;

    public Button yellowButton;
    public TextMeshProUGUI timer;

    public Image trafficLight_OffImage;
    public Image trafficLight_OnImage;
    public GameObject MenuPanel;
    private void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            trafficLightPos[i] = trafficLight.transform.GetChild(i).transform.position;
        }
        startButton.gameObject.SetActive(false);
        startEffect.SetActive(false);
        yellowEffect.SetActive(false);
        timerEffect.SetActive(false);

        flag = false;
        moveTruck = false;
        reactionFlag = false;

        trafficLight_OffImage.gameObject.SetActive(false);
        trafficLight_OnImage.gameObject.SetActive(false);

        lightFlag = true;

        reactionTime = 0f;
        clickTime = 0f;
        Count = 1;

        timeData = new float[Count];
    }

    private void Update()
    {
        if (click)
        {
            clickTime += Time.deltaTime;
            if (clickTime >= 2 && lightFlag)
            {
                Onlight();
                flag = true;

                if (flag)
                {
                    timer.gameObject.SetActive(true);
                    lightFlag = false;
                }
            }
        }
        else clickTime = 0;

        if (reactionFlag)
        {
            TimeText.text = "시간 : " + reactionTime.ToString("F3");
            ReactionTime();
        }

        if (Count == 0 && moveTruck)
        {
            move_Script.GetComponent<TrafficLight_move_Tut>().flag = true;
            lightFlag = false;
        }
    }

    // 신호등 불 키는 메소드
    public void Onlight()
    {
        trafficLight_OffImage.gameObject.SetActive(false);
        trafficLight_OnImage.gameObject.SetActive(true);
        reactionFlag = true;
    }


    // 누를 시 실행
    public void ButtonDown()
    {
        click = true;
    }

    // 버튼 떼는 순간 실행 
    public void ButtonUp()
    {
        click = false;
        flag = false;
        startButton.gameObject.SetActive(false);
        startEffect.SetActive(false);
    }

    public void ReactionTime()
    {
        reactionTime += Time.deltaTime;
    }

    public void CheckLight()
    {        
        if (!lightFlag)
        {
            string buttonName = EventSystem.current.currentSelectedGameObject.name;
            string yellowButtonName = yellowButton.gameObject.name;

            if (buttonName == yellowButtonName)
            {
                Count--; // 정답일시 전체 게임 카운트를 감소시킴.

                reactionFlag = false;

                timeData[Count] = reactionTime;
                TimeText.text = "시간 : " + reactionTime.ToString("F3"); //내가 올바른 버튼을 누를때까지 걸린 시간.

                yellowButton.gameObject.SetActive(false);
                trafficLight_OnImage.gameObject.SetActive(false);
                yellowEffect.SetActive(false);
                timerEffect.SetActive(true);

                reactionTime = 0f;
                lightFlag = true;
                flag = true;
            }
        }
    }
    public void MenuPanelOnAndOff()
    {
        if (MenuPanel.activeSelf == false)
        {
            MenuPanel.SetActive(true);
        }
        else MenuPanel.SetActive(false);
    }

}