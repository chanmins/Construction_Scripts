using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//08.23 스트립트 수정중
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class TrafficLight_move_Tut : MonoBehaviour
{
    public Transform target;//종료 후 트럭이 도착할 위치.

    public GameObject gate;//게이트
    public GameObject truck;
    public GameObject[] wheels;
    public GameObject TrafficLight_RadiosObj;

    public bool flag;//게임 종료시 flag 변환.
    private bool wasFlag;
    private bool wheel_flag;
    private float speed;
    private Animator gateAnimator;

    [DllImport("__Internal")]
    private static extern void SendAverageTime(float averageTime);

    public GameObject tutorial;
    private Tutorial tutorialScript;

    void Start()
    {
        tutorialScript = tutorial.GetComponent<Tutorial>();
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

        yield return new WaitForSeconds(2.0f);

        if (wheel_flag)
        {
            float[] timeData = TrafficLight_RadiosObj.GetComponent<TrafficLight_Radios_Tut>().timeData;
            Time.timeScale = 1f;
        }
        tutorialScript.endTutorial.gameObject.SetActive(true);
    }

    public void Spin()
    {
        foreach(GameObject wheel in wheels)
        {
            wheel.transform.Rotate(Time.deltaTime * 200f, 0f, 0f);  // 바귀 회전
        }
    }
}
