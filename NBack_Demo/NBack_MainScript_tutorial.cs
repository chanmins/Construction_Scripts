using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

//08.23 스트립트 수정중
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;


//현재 게임은 1-back
//2-back 추가해야함 -> que부분을 수정하면 될듯


public class NBack_MainScript_tutorial : MonoBehaviour
{
    public Material[] colors;

    public GameObject container;//컨테이너 전체
    public GameObject box;//컨테이너 박스만 -> 색변경시 사용
    public GameObject MainCamera;

    public bool checkButton;
    public bool nbackFlag;
    public bool choice_N;// 메인에서 선택할 n

    public Button buttonT;
    public Button buttonF;
    public Button buttonT_tutorial;
    public Button buttonF_tutorial;


    public float lerpTime;//컨테이너가 움직임을 소요하는 시간.

    public GameObject endPanel;
    public GameObject startPanel;

    public TextMeshProUGUI textAnswer;
    public TextMeshProUGUI textWrong;

    Vector3 upPos;//올렸을때 위치
    Vector3 downPos;//내렸을때 위치

    Queue<int> que = new Queue<int>();

    public int cnt;    //문제수
    public int rightCnt;
    public int wrongCnt;
    int twoBackData;

    // 08.23 스트립트 수정중
    [DllImport("__Internal")]
    private static extern void SendRightcntRatio(float rightcntRatio);

    // 09.05 NBack 튜토리얼 만들기 위해 수정중.
    public Tutorial_tutorial myTutorial;
    public bool isStart = false;
    public TMP_Text curMode;
    bool D1 = false;
    bool D2 = false;


    public Image Collect_image;
    public Image unCollect_image;

    public GameObject MenuPanel;

    void Start()
    {
        startPanel.SetActive(true);
        cnt = 0;
        rightCnt = 0;
        wrongCnt = 0;

        choice_N = false;

        textAnswer.text = "정답: 0";
        textWrong.text = "오답: 0";
        lerpTime = 0.5f;

        upPos = new Vector3(container.transform.position.x, container.transform.position.y + 3.2f, container.transform.position.z);
        downPos = container.transform.position;
    }

    void Update()
    {
        if (myTutorial.currentStep == 1 && !D1)
        {
            D1 = true;
            StartCoroutine(StartContainerMove());
        }
        else if (myTutorial.currentStep == 5)
        {
            myTutorial.next.gameObject.SetActive(false);
            buttonT_tutorial.gameObject.SetActive(true);
            buttonF_tutorial.gameObject.SetActive(true);
        }
        else if (myTutorial.currentStep == 6)
        {
            myTutorial.next.gameObject.SetActive(true);
            buttonT_tutorial.gameObject.SetActive(false);
            buttonF_tutorial.gameObject.SetActive(false);
        }
    }

    public void Test1Back()
    {
        if (!isStart) return;
        curMode.text = "연습모드 : 1_Back";

        startPanel.SetActive(false);
        choice_N = true;
        myTutorial.NextStep();
    }

    public void Test2Back()
    {
        if (!isStart) return;
        curMode.text = "연습모드 : 2_Back";

        startPanel.SetActive(false);
        choice_N = false;
        myTutorial.NextStep();
    }

    IEnumerator MoveContainer()
    {
        if (cnt == 0)
        {
            yield return new WaitForSeconds(2.0f);
        }
        checkButton = false;
        float moveTime = 0.0f;

        RandomColor_Container();

        while (moveTime < lerpTime)
        {
            moveTime += Time.deltaTime;

            container.transform.position = Vector3.Lerp(downPos, upPos, moveTime / lerpTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.7f);

        moveTime = 0.0f;
        while (moveTime < lerpTime)
        {
            moveTime += Time.deltaTime;

            container.transform.position = Vector3.Lerp(upPos, downPos, moveTime / lerpTime);
            yield return null;
        }
        cnt++;
        yield return new WaitForSeconds(1.0f);


        if (cnt < 3)//처음 시작만 3번.
        {
            StartCoroutine(MoveContainer());
        }
        else if (cnt >= 2)
        {
            if (!D2)
            {
                D2 = true;
                myTutorial.HiddenPanel.SetActive(true);
                myTutorial.tutorial_Panel.SetActive(true);
                myTutorial.NextStep();
                myTutorial.next.gameObject.SetActive(true);
            }
            checkButton = true; //초반 3회 운동시 버튼 비활성화  
        }
    }

    public void RandomColor_Container()
    {
        int RandomNum = 0;

        // 튜토리얼 초반에 빨 -> 초 -> 빨을 확정적으로 뽑기 위해서 스크립트 수정.
        if (cnt > 2)
        {
            RandomNum = Random.Range(0, 2);//0 or 1 뽑음
        }
        else
        {
            if (cnt == 0) RandomNum = 0;
            else if (cnt == 1) RandomNum = 1;
            else if (cnt == 2) RandomNum = 0;
        }

        if (RandomNum == 0) box.GetComponent<Renderer>().material = colors[0]; // 빨강
        else box.GetComponent<Renderer>().material = colors[1]; // 초록

        //큐에 랜덤값 저장
        twoBackData = RandomNum;
        que.Enqueue(RandomNum);
        if (choice_N && cnt == 1)   //true => 1back, false => 2back
        {
            print("Queue에 값저장.");
            que.Dequeue();
        }
    }

    /*
        동작 알고리즘.
        큐를 사용해 마지막과 마지막에서 두번째 수를 비교
        첫번째 수는 Dequeue를 사용해서 저장.
        두번째 수는 peek를 사용해서 최근에 큐에 진입한 수를 저장
        비교후 Dequeue 할 시 큐에서 빠져나가게되고
        비교에 맞는 버튼을 눌러 다음 수가 큐에 저장됨.
        반복.
     */
    public void TrueButton()
    {
        if (checkButton)
        {
            bool answer = Compare_que(true);
            if (answer)
            {
                textAnswer.text = "정답: " + (int.Parse(textAnswer.text.Split(':')[1]) + 1);
                print("정답");
                StartCoroutine(onImage(answer));
                rightCnt++;
            }
            else
            {
                textWrong.text = "오답: " + (int.Parse(textWrong.text.Split(':')[1]) + 1);
                print("오답");
                StartCoroutine(onImage(answer));
                wrongCnt++;
            }
            StartCoroutine(MoveContainer());
        }
    }
    public void FalseButton()
    {
        if (checkButton)
        {
            bool answer = Compare_que(false);
            if (answer)
            {
                textAnswer.text = "정답: " + (int.Parse(textAnswer.text.Split(':')[1]) + 1);
                print("정답");
                StartCoroutine(onImage(answer));
                rightCnt++;
            }
            else
            {
                textWrong.text = "오답: " + (int.Parse(textWrong.text.Split(':')[1]) + 1);
                print("오답");
                StartCoroutine(onImage(answer));
                wrongCnt++;
            }
            StartCoroutine(MoveContainer());
        }
    }

    public void SaveNBackData()
    {
        if (cnt == 23)
        {
            endPanel.SetActive(true);

            //08.23 React와 유니티간 통신을 위한 함수 추가
            float percent = (float)rightCnt / 20 * 100;
            SendRightcntRatio(percent);
            buttonT.interactable = false;
            buttonF.interactable = false;


            if (!endPanel.active)
            {
                endPanel.SetActive(false);
                Time.timeScale = 1f;
            }
            Time.timeScale = 0f;
            print("테스트 종료");
        }
    }
    public bool Compare_que(bool choice)
    {
        if (choice_N)//1back 알고리즘
        {
            int num = que.Dequeue(); // 상단 뽑은거
            bool check;

            if (num == que.Peek())
                check = true;
            else
                check = false;

            return check == choice;
        }
        else//2back 알고리즘
        {
            int num = que.Dequeue();
            bool check;

            print(num + " " + twoBackData);

            if (num == twoBackData)
                check = true;
            else
                check = false;


            return check == choice;
        }
    }

    IEnumerator StartContainerMove()
    {
        yield return new WaitForSeconds(3f);
        myTutorial.tutorial_Panel.SetActive(false);
        StartCoroutine(MoveContainer());
    }

    public void ChangeMode()
    {
        // 현재 모드가 1_Back 인 경우 -> 2_Back으로 변경.
        if (choice_N)
        {
            curMode.text = "연습모드 : 2_Back";
            choice_N = false;
        }

        // 현재 모드가 2_Back 인 경우 -> 1_Back으로 변경.
        else if (!choice_N)
        {
            curMode.text = "연습모드 : 1_Back";
            choice_N = true;
        }
    }

    public void tutorial_answer(bool answer)
    {
        string reply = "";
        // 1-Back
        if (choice_N)
        {
            if (answer) reply = "틀렸습니다. ";
            else reply = "맞았습니다. ";
            myTutorial.tutorialSteps[myTutorial.currentStep+1].instruction = reply + "'1-Back' 모드에서는 가장 최근 컨테이너의 색과 1개 이전의 컨테이너 색을 고려하기 때문에 빨강 - 초록으로 두 색이 달라 'X' 버튼이 맞는 답입니다.";
            myTutorial.NextStep();
        }
        // 2-Back
        else
        {
            if (answer) reply = "맞았습니다. ";
            else reply = "틀렸습니다. ";
            myTutorial.tutorialSteps[myTutorial.currentStep+1].instruction = reply + "'2-Back' 모드에서는 가장 최근 컨테이너의 색과 2개 이전의 컨테이너 색을 고려하기 때문에 빨강 - 빨강으로 두 색이 같아 'O' 버튼이 맞는 답입니다.";
            myTutorial.NextStep();
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

    IEnumerator onImage(bool isCollect)
    {
        if(isCollect)
        {
            Collect_image.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            Collect_image.gameObject.SetActive(false);
        }
        else
        {
            unCollect_image.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            unCollect_image.gameObject.SetActive(false);
        }
    }

}