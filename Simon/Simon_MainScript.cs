using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//08.23 스트립트 수정중
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class Simon_MainScript : MonoBehaviour
{
    public GameObject[] lightColor = new GameObject[4]; //빨노초파
    public GameObject exitPanel;

    public bool compareflag;
    public bool upflag;
    public TextMeshProUGUI roundText;

    List<int> question = new List<int>();

    int problemCount;
    int answerCount;
    int ListCount;
    int listIndex;
    int round;

    public bool answerflag;
    public GameObject MenuPanel;
    bool problemflag;

    [DllImport("__Internal")]
    private static extern void SendPassedRoundNum(int hitNum);

    void Start()
    {
        Time.timeScale = 0f;

        question.Add(Random.Range(0, 4));
        question.Add(Random.Range(0, 4));

        round = 1;
        answerCount = 0;

        answerflag = false;
        upflag = false;
        problemflag = true;
        compareflag = true;

        ListCount = 3;
    }

    void Update()
    {

        if (problemflag && !upflag)
        {
            StartCoroutine(CreateProblem());
        }
    }

    public void startTest()
    {
        Time.timeScale = 1f;
    }

    public void Push_Button()//누른 버튼의 색갈 오브젝트 생성.
    {
        if (answerflag)
        {
            GameObject Color_btt = EventSystem.current.currentSelectedGameObject;   //내가 누른 버튼오브젝트
            int color_index;

            switch (Color_btt.name)
            {
                case "Red":
                    color_index = 0;
                    StartCoroutine(OnLight(color_index));
                    CompareAnswer(color_index);
                    break;
                case "Yellow":
                    color_index = 1;
                    StartCoroutine(OnLight(color_index));
                    CompareAnswer(color_index);
                    break;
                case "Green":
                    color_index = 2;
                    StartCoroutine(OnLight(color_index));
                    CompareAnswer(color_index);
                    break;
                case "Blue":
                    color_index = 3;
                    StartCoroutine(OnLight(color_index));
                    CompareAnswer(color_index);
                    break;
            }

            listIndex++;
            answerCount++;//버튼을 누를때마다 카운트 증가.

            if (!compareflag)//중간에 문제가 틀렸다면
            {
                Time.timeScale = 0f;
                exitPanel.SetActive(true);

                //08.23 스트립트 수정중, 리액트와 통신하기 위해 추가한 코드
                SendPassedRoundNum(round);

                if (!exitPanel.active)
                {
                    exitPanel.SetActive(false);
                    Time.timeScale = 1f;
                }
/*                SQLiteDB.Instance.SimonDataSave((round-1).ToString());*/
            }

            if (problemCount == answerCount)// 버튼을 누른수와 문제수가 일치 할 경우 부분초기화
            {
                answerCount = 0;
                problemflag = true;
                answerflag = false;
                if (compareflag)
                {
                    round++;
                    roundText.text = "Round :" + round.ToString().PadLeft(6,' ');
                }
                upflag = false;
            }
        }
    }

    IEnumerator OnLight(int index)//신호등 index에 따라 불키고 끄기
    {
        lightColor[index].SetActive(true);

        yield return new WaitForSeconds(0.7f);

        lightColor[index].SetActive(false);
    }

    IEnumerator CreateProblem() // 문제생성.
    {
        problemflag = false;

        yield return new WaitForSeconds(2.5f);// 시작전 1초 delay

        //리스트에 있는거 부터 다시 보여줌
        for(int i = 0; i < question.Count; i++)
        {
            StartCoroutine(OnLight(question[i]));
            yield return new WaitForSeconds(0.9f);
        }
        
        int plus = Random.Range(0, 4);
        question.Add(plus);
        StartCoroutine(OnLight(plus));

        problemCount = question.Count;

        answerflag = true;
        listIndex = 0;
        /*
            첫 3개 최대한 중복 안나오게 고쳐야함.
         */
    }

    public void CompareAnswer(int index)//내가 누른 버튼과 답이 맞는지 확인.
    {

        if (question[listIndex] == index)
        {
            print("마자");
        }
        else
        {
            print("틀림");
            compareflag = false;
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
        SceneManager.LoadScene("Simon_Tut");
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
