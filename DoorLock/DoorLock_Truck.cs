using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//08.23 스트립트 수정중
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class DoorLock_Truck : MonoBehaviour
{
    public GameObject truck;
    public GameObject wheel1;
    public GameObject wheel2;
    public GameObject wheel3;
    public GameObject wheel4;
    public GameObject wheel5;
    public GameObject wheel6;

    public TextMeshProUGUI problem;
    public TextMeshProUGUI answer;

    public GameObject exitPanel;

    public int roundCount, correctCount;
    public GameObject MenuPanel;
    bool flag, correct_flag;

    Vector3 truckPos;

    //08.23 스트립트 수정중
    [DllImport("__Internal")]
    private static extern void SendPassedRoundNum(int hitNum);

    private void Start()
    {
        Time.timeScale = 0f;
        flag = false;
        correct_flag = false;
        roundCount = 3; //시작글자
        correctCount = 0;
        truckPos = new Vector3(0f, 0.5f, -25f);
        Random_Problem(roundCount);

    }


    private void Update()
    {
        MoveTruck();
        
    }

    public void Test_Start()
    {
        Time.timeScale = 1f;
    }

    public void Random_Problem(int round)// 트럭에 랜덤으로 숫자 넣기
    {
        for(int i = 0; i < round; i++)
        {
            int num = Random.Range(0, 10);
            problem.text += num.ToString();
        }        
    }
    

    
    public void MoveTruck()
    {

        if (truck.transform.position.z < 5 && !flag)// 시작 부분.
        {
            StartCoroutine(stopTruck());
            Spin();
            truck.transform.Translate(new Vector3(0, 0, Time.deltaTime * 20f));//트럭 이동 
        }else if(flag && truck.transform.position.z >= 5)
        {
            Spin();
            truck.transform.Translate(new Vector3(0, 0, Time.deltaTime * 20f));//트럭 이동 
        }
        
        if(truck.transform.position.z >= 32)
        {
            truck.SetActive(false);
            truck.transform.position = truckPos;
            correct_flag = true;
        }
    }

    IEnumerator stopTruck()
    {
        yield return new WaitForSeconds(2.4f); // 실제로 트럭이 멈추는 시간.
        flag = true;
    }

    public void Spin()
    {
        foreach (GameObject wheel in new[] { wheel1, wheel2, wheel3, wheel4, wheel5, wheel6 })
        {
            wheel.transform.rotation *= Quaternion.Euler(Time.deltaTime * 200f, 0f, 0f);
        }
    }

    public void KeyPad()
    {
        if (correct_flag)
        {
            GameObject KeyPad = EventSystem.current.currentSelectedGameObject;//내가 누른 버튼오브젝트
            string btt_name = KeyPad.name;
            answer.text += btt_name;
            correctCount++;
        }


    }

    public void Delete()
    {
        correctCount = 0;
        answer.text = "";
    }

    public void check_answer()
    {

        if (correct_flag) // 트럭이 종료위치에 도달했을경우 corret_flage = true
        {
            if (correctCount == roundCount)
            {

                if (problem.text.ToString().Equals(answer.text.ToString()))//확인
                {
                    print("맞");
                    roundCount++;

                }
                else
                {
                    print("틀");
                    Time.timeScale = 0f;
                    exitPanel.SetActive(true);

                    //08.23 스트립트 수정중, 리액트와 통신하기 위해 추가한 코드
                    SendPassedRoundNum(roundCount);

                    if (!exitPanel.active)
                    {
                        exitPanel.SetActive(false);
                        Time.timeScale = 1f;
                    }
/*                    SQLiteDB.Instance.DoorLockDataSave((roundCount - 1).ToString());*/
                    
                }
                
            }
            else
            {
                print("틀");
                Time.timeScale = 0f;
                exitPanel.SetActive(true);

                //08.23 스트립트 수정중, 리액트와 통신하기 위해 추가한 코드
                SendPassedRoundNum(roundCount);

                if (!exitPanel.active)
                {
                    exitPanel.SetActive(false);
                    Time.timeScale = 1f;
                }
/*                SQLiteDB.Instance.DoorLockDataSave((roundCount - 1).ToString());*/
            }
            correctCount = 0;
            problem.text = "";
            answer.text = "";
            Random_Problem(roundCount);
            flag = false;
            correct_flag = false;

            truck.SetActive(true);
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
        SceneManager.LoadScene("DoorLock_Tutorial");
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
