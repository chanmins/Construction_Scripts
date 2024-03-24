using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class DoorLock_Truck_Demo : MonoBehaviour
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

    public int roundCount, correctCount;
    bool flag, correct_flag;
    public bool interactFlag;

    Vector3 truckPos;

    public GameObject tutorialScript;
    public GameObject MenuPanel;
    private Tutorial tutorial;

    private void Start()
    {
        tutorial = tutorialScript.GetComponent<Tutorial>();
        interactFlag = false;
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

    // 912로 숫자 고정
    public void Random_Problem(int round)// 트럭에 랜덤으로 숫자 넣기
    {
        problem.text = "912";
    }

    public void MoveTruck()
    {
        if (truck.transform.position.z < 5 && !flag && interactFlag)// 시작 부분.
        {
            StartCoroutine(stopTruck());
            Spin();
            truck.transform.Translate(new Vector3(0, 0, Time.deltaTime * 20f));//트럭 이동 
        }
        else if(flag && truck.transform.position.z >= 5)
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

            if(KeyPad.name == "9")
            {
                tutorial.numbers[0].SetActive(false);
                tutorial.numbers[1].SetActive(true);
            }
            else if(KeyPad.name == "1")
            {
                tutorial.numbers[1].SetActive(false);
                tutorial.numbers[2].SetActive(true);
            }
            else if (KeyPad.name == "2")
                tutorial.numbers[2].SetActive(false);

            correctCount++;
        }
    }

    public void Delete()
    {
        tutorial.numbers[0].SetActive(true);
        correctCount = 0;
        answer.text = "";
    }

    public void check_answer()
    {
        if (correct_flag) // 트럭이 종료위치에 도달했을경우 corret_flage = true
        {
            if (problem.text.ToString().Equals(answer.text.ToString()))//확인
            {
                StartCoroutine(ShowMessage("정답입니다!", 1.0f));
                tutorial.NextStep();
                foreach (GameObject gameobject in tutorial.numbers)
                {
                    gameobject.SetActive(false);
                }
            }
            else
            {
                StartCoroutine(ShowMessage("다시 입력해주세요!", 1.0f));
                tutorial.numbers[0].SetActive(true);
                tutorial.numbers[1].SetActive(false);
                tutorial.numbers[2].SetActive(false);
            }
            
        Random_Problem(roundCount);
        flag = false;
        }
    }

    private IEnumerator ShowMessage(string message, float duration)
    {
        answer.text = message;
        yield return new WaitForSeconds(duration);
        Delete();
    }

    public void ReturnMainMenu_toreact()
    {
        Application.OpenURL("https://web-template-3prof2llkxuyz4l.sel4.cloudtype.app/dashboard");
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
