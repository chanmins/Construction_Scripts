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


public class NBack_MainScript : MonoBehaviour
{
    public Material[] colors;

    public GameObject container;//컨테이너 전체
    public GameObject box;//컨테이너 박스만 -> 색변경시 사용
    
    public bool checkButton;
    public bool nbackFlag;
    public bool choice_N;// 메인에서 선택할 n

    public Button buttonR;
    public Button buttonF;
    
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

    public Image Collect_image;
    public Image unCollect_image;
    public GameObject MenuPanel;

    // 08.23 스트립트 수정중
    [DllImport("__Internal")]
    private static extern void SendRightcntRatio(float rightcntRatio);

    void Start()
    {
        startPanel.SetActive(true);
        cnt = 0;
        rightCnt = 0;
        wrongCnt = 0;

        Time.timeScale = 0f;
        choice_N = false;

        textAnswer.text = "정답: 0";
        textWrong.text = "오답: 0";
        lerpTime = 0.5f;

        upPos = new Vector3(container.transform.position.x, container.transform.position.y + 3.2f, container.transform.position.z);
        downPos = container.transform.position;
        StartCoroutine(MoveContainer());
    }

    public void Test1Back()
    {
        Time.timeScale = 1f;
        startPanel.SetActive(false);
        choice_N = true;
    }

    public void Test2Back()
    {
        Time.timeScale = 1f;
        startPanel.SetActive(false);
        choice_N = false;
    }

    IEnumerator MoveContainer()
    {
        if(cnt == 0)
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

        //print("내려가여");
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
        else if(cnt >= 2)
        {
            print("cnt  " + cnt);
            checkButton = true; //초반 3회 운동시 버튼 비활성화  
        }


    }
 
    public void RandomColor_Container()
    {
        int RandomNum = Random.Range(0, 2);//0 or 1 뽑음

        if(RandomNum == 0)//빨강
            box.GetComponent<Renderer>().material = colors[0];
        else//초록
            box.GetComponent<Renderer>().material = colors[1];

        //큐에 랜덤값 저장
        twoBackData = RandomNum;
        que.Enqueue(RandomNum);
        if (choice_N && cnt == 1)   //true => 1back, false => 2back
        {
            print("asd");
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

            if(rightCnt + wrongCnt <= 19)
            {
                StartCoroutine(MoveContainer());
            }
        }

        SaveNBackData();
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
            if (rightCnt + wrongCnt <= 19)
            {
                StartCoroutine(MoveContainer());
            }
        }

        SaveNBackData();
    }

    public void SaveNBackData()
    {
        if (rightCnt + wrongCnt == 20)
        {
            endPanel.SetActive(true);

            //08.23 React와 유니티간 통신을 위한 함수 추가
            float percent = (float)rightCnt / 20 * 100;
            SendRightcntRatio(percent);
            buttonR.interactable = false;
            buttonF.interactable = false;


            if (!endPanel.activeSelf)
            {
                endPanel.SetActive(false);
                Time.timeScale = 1f;
            }
            Time.timeScale = 0f;
/*            SQLiteDB.Instance.NBackDataSave(percent.ToString());*/
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

    // 08.26 테스트 종료 후 메인화면으로 돌아가는 요청을 하는 함수를 공통적으로 추가함.
    public void ReturnMainMenu_toreact()
    {
        Application.OpenURL("https://web-template-3prof2llkxuyz4l.sel4.cloudtype.app/dashboard");
    }

    public void PlayTutorial()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("NBack_temp");
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
        if (isCollect)
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