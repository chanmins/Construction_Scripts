using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using TMPro;

// 3회 완료 시 바리게이트 오픈&화물차 이동

public class TrafficLight_Radios : MonoBehaviour
{
    public bool click;
    public bool lightFlag;

    public GameObject trafficLight;
    public GameObject move_Script;

    public Vector3[] trafficLightPos = new Vector3[3];

    public TextMeshProUGUI TimeText;

    GameObject nowLight; // 현재 불이 들어온 신호등

    private int num;//신호등 인덱스, 랜덤값.

    // 08.24 private static int -> public int 로 수정.
    public int Count;//전체 문제수

    private float clickTime; //Start버튼을 누른 시간.
    private float reactionTime; //신호등이 켜지고 Player가 정답을 맞추는대까지 걸린 시간.
    public float[] timeData;

    bool reactionFlag;

    // 09.20 정반응, 오반응 개수를 세기 위한 변수추가.
    [Header("Count Collect / Wrong")]
    public int CollectReaction = 0;
    public int WrongReaction = 0;

    private void Start()//각 변수 초기화
    {
        for(int i = 0; i < 3; i++)
        {
            trafficLightPos[i] = trafficLight.transform.GetChild(i).transform.position;
        }
        reactionTime = 0f;
        lightFlag = true;
        reactionFlag = false;
        clickTime = 0f;
        Count = 3;
        timeData = new float[Count];

    }
    private void Update()
    {
        if (click)
        {
            clickTime += Time.deltaTime;
            if (clickTime >= 2 && lightFlag) Onlight();

        }
        else clickTime = 0;

        if (reactionFlag)
        {
            TimeText.text = "시간 : " + reactionTime.ToString("F3");
            ReactionTime();
        }
    }

    // 신호등 불 키는 메소드
    public void Onlight()
    {
        int randPos = Random.Range(0, 100) % 3;
        lightFlag = false;
        num = Random.Range(0, 9) % 3;// 0~8사이 랜덤값.
       
        nowLight = trafficLight.transform.GetChild(num).gameObject;
        nowLight.transform.position = trafficLightPos[randPos];
        nowLight.SetActive(true);
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
    }

    public void ReactionTime()
    {
        reactionTime += Time.deltaTime;
    }
    public float AverTime(float t1, float t2, float t3)
    {
        return (t1 + t2 + t3)/3;
    }


    public void CheckLight()
    {
        if (!lightFlag)
        {
            string button_color = EventSystem.current.currentSelectedGameObject.name;//내가 누른 무전기의 버튼의 색깔
            print("내가 누른색 : " + button_color);
            if (button_color == nowLight.name)
            {
                print("같음");
                Count--; // 정답일시 전체 게임 카운트를 감소시킴.

                reactionFlag = false;
                nowLight.SetActive(false);

                timeData[Count] = reactionTime;
                TimeText.text = "시간 : " + reactionTime.ToString("F3"); //내가 올바른 버튼을 누를때까지 걸린 시간.

                reactionTime = 0f;
                lightFlag = true;

                if(Count >= 0) CollectReaction++;

            }
            else
            {
                print("다름");
                if(Count >= 0) WrongReaction++;
            }
        }

        if (Count == 0)
        {
            move_Script.GetComponent<TrafficLight_move>().flag = true;
            lightFlag = false;
/*            SQLiteDB.Instance.TrafficLightDataSave(AverTime(timeData[2], timeData[1], timeData[0]).ToString("F3"));*/

        }
    }

}

