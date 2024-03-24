using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//08.23 스트립트 수정중
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class TrafficLight_move : MonoBehaviour
{
    public Transform target;//종료 후 트럭이 도착할 위치.

    public GameObject gate;//게이트
    public GameObject truck;
    public GameObject[] wheels;
    public GameObject exitPanel;
    public GameObject TrafficLight_RadiosObj;

    public bool flag;//게임 종료시 flag 변환.
    private bool wasFlag;
    private bool wheel_flag;
    private float speed;
    private Animator gateAnimator;
    public GameObject MenuPanel;

    [DllImport("__Internal")]
    private static extern void SendAverageTime(float averageTime);

    [DllImport("__Internal")]
    private static extern void SendWrongReaction(int WrongReaction);

    void Start()
    {
        speed = 0.04f;
        flag = false;
        wasFlag = false;
        wheel_flag = false;
        gateAnimator = gate.GetComponent<Animator>();
    }

    // 휠 회전이 물리에 따라 달라져 Update보다 FixedUpdate 선호
    void FixedUpdate()
    {
        if (flag && !wasFlag)
        {
            wasFlag = true;
            StartCoroutine(ContentsClear());
        }
        
        if (wheel_flag)
            Spin();
    }


    IEnumerator ContentsClear()
    {
        gateAnimator.enabled = true;    //게이트 열림.
        yield return new WaitForSeconds(2.0f);
        //차 출발.
        wheel_flag = true;

        Vector3 a = truck.transform.position;
        Vector3 b = target.position;

        while (Vector3.Distance(a, b) > 0.01f)
        {
            a = truck.transform.position;
            truck.transform.position = Vector3.MoveTowards(a, b, speed);
            yield return null;
        }

        yield return new WaitForSeconds(3.0f);

        if (wheel_flag)
        {
            exitPanel.SetActive(true);

            float[] timeData = TrafficLight_RadiosObj.GetComponent<TrafficLight_Radios>().timeData;
            int WrongReaction = TrafficLight_RadiosObj.GetComponent<TrafficLight_Radios>().WrongReaction;
            float averageTime = (timeData[2] + timeData[1] + timeData[0]) / 3;


            Debug.Log(WrongReaction);
            Debug.Log(averageTime);

            SendAverageTime(averageTime);
            SendWrongReaction(WrongReaction);

            Time.timeScale = 1f;
        }


    }
    public void Spin()
    {
        foreach(GameObject wheel in wheels)
        {
            wheel.transform.Rotate(Time.deltaTime * 200f, 0f, 0f);  // 바귀 회전
        }
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("TrafficLight_Tutorial");
    }

    // 08.26 테스트 종료 후 메인화면으로 돌아가는 요청을 하는 함수
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
