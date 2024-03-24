using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using System;

//08.23 스트립트 수정중
using System.Runtime.InteropServices;
using HighlightingSystem;

public class Hammering_MainScript_Tutorial : MonoBehaviour
{
    public Vector3[] nailposition;

    //public LineRenderer line;
    public GameObject hammer;
    public GameObject uiManager;
    public GameObject exitPanel;
    public int cnt;
    Animator hammering;

    // 09.04 스크립트 수정중
    public Tutorial_Tutorial myTutorial;
    public HighlightingRenderer HR;
    bool D1 = false;
    bool D2 = false;
    bool D3 = false;

    public LayerMask layerMask; // 원하는 레이어 마스크 설정

    private Coroutine currentAttackCoroutine; 
    // 현재 실행 중인 코루틴을 저장할 변수. 새롭게 AttackHammer 함수가 실행되면 이전에 실행됬던 AttackHammer 함수를 중지하고
    // 새롭게 코루틴 함수를 실행한다.


    //08.23 스트립트 수정중
    [DllImport("__Internal")]
    private static extern void SendEndTime(float endTime);

    private void Awake()
    {
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }

    private void Start()
    {
        nailposition = new Vector3[25];
        for (int i = 0; i < 25; i++)
        {
            nailposition[i] = GameObject.Find("nail_" + (i + 1)).transform.position;
            nailposition[i].y = 0.7f; //y값 고정
        }

        cnt = 1;
        hammering = hammer.GetComponent<Animator>();
    }

    private void Update()
    {
        if(cnt == 2 && !D1)
        {
            D1 = true;
            myTutorial.NextStep();
            StartCoroutine(TurnOffPanel());
        }
        else if (cnt == 6 && !D2)
        {
            myTutorial.tutorial_Panel.SetActive(true);
            D2 = true;
            HR.enabled = false;

            myTutorial.NextStep();
            StartCoroutine(TurnOffPanel());
        }
        else if(cnt == 11 && !D3)
        {
            myTutorial.tutorial_Panel.SetActive(true);
            D3 = true;
            myTutorial.NextStep();
            myTutorial.next.enabled = true;
            myTutorial.HiddenPanel.SetActive(true);
        }
    }

    public void DownNail(GameObject button, GameObject nail)//버튼과 못
    {
        if (cnt.ToString() == button.name && nail != null)
        {   
            // 09.10 추가. 플레이어가 못을 빠르게 누르면 망치가 못 따라오는 것을 방지하기 위해 수정.
            if (currentAttackCoroutine != null) StopCoroutine(currentAttackCoroutine);
            currentAttackCoroutine = StartCoroutine(AttackHammer(nail));

            this.GetComponent<AudioSource>().Play();//망치 소리

            button.GetComponent<RectTransform>().position = new Vector3(button.transform.position.x, button.transform.position.y - 54f, 0);
            nail.transform.Translate(new Vector3(0, -0.1f, 0));

            cnt++;
            if (cnt == 26)//종료부분
            {
                // 리액트와 통신하기 위해 추가한 코드 08.22
                float endTime = uiManager.GetComponent<Hammering_UIManager>().time;

                uiManager.GetComponent<Hammering_UIManager>().timerflag = false;
                exitPanel.SetActive(true);

                // 리액트와 통신하기 위해 추가한 코드 08.22
                Debug.Log(endTime);
                SendEndTime(endTime);

                if (!exitPanel.active)
                {
                    exitPanel.SetActive(false);
                    Time.timeScale = 1f;
                }

                print("테스트 종료.");
            }
        }
    }
    IEnumerator AttackHammer(GameObject nail)
    {
        hammer.SetActive(true);
        hammer.transform.position = new Vector3(
                nail.transform.position.x - 0.185f,
                nail.transform.position.y + 0.3f,
                nail.transform.position.z - 0.55f);

        hammering.SetBool("isStarted", true);
        hammering.Play("Hammer");

        yield return new WaitForSeconds(0.5f);
        hammer.SetActive(false);
    }
    public void buttonP()
    {
        // 버튼 누르면 비활성화시킴
        GameObject nail_btt = EventSystem.current.currentSelectedGameObject;//내가 누른 버튼 오브젝트

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        GameObject hitObject = null; // 현재 내가 누른 곳 밑에 있는 못 오브젝트
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            hitObject = hit.collider.gameObject;
        }

        DownNail(nail_btt, hitObject);
    }

    IEnumerator TurnOffPanel()
    {
        yield return new WaitForSeconds(3f);
        myTutorial.tutorial_Panel.SetActive(false);
    }
}