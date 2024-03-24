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

public class Hammering_MainScript : MonoBehaviour
{
    public Vector3[] nailposition;

    //public LineRenderer line;
    public GameObject hammer;
    public GameObject uiManager;
    public GameObject exitPanel;
    public int cnt;
    Animator hammering;

    // 08.25 hammer 애니메이션 동적으로 바꾸기 위해 코드 수정중.
    // 08.25 line layer 비활성화
    public LayerMask layerMask; // 원하는 레이어 마스크 설정

    private Coroutine currentAttackCoroutine;
    // 현재 실행 중인 코루틴을 저장할 변수. 새롭게 AttackHammer 함수가 실행되면 이전에 실행됬던 AttackHammer 함수를 중지하고
    // 새롭게 코루틴 함수를 실행한다.


    // 09.10 정반응, 오반응 개수를 세기 위한 변수추가.
    [Header("Count Collect / Wrong")]
    public int CollectReaction = 0;
    public int WrongReaction = 0;

    //08.23 스트립트 수정중
    [DllImport("__Internal")]
    private static extern void SendEndTime(float endTime);

    [DllImport("__Internal")]
    private static extern void SendCollectReaction(int CollectReaction);

    [DllImport("__Internal")]
    private static extern void SendWrongReaction(int WrongReaction);

    private void Start()
    {
        nailposition = new Vector3[25];

        /*
            1. 버튼과 line 상호작용 -> 완료
            2. 머터리얼 변경 -> 추후변경
            3. 라인 크기 변경 -> 완료 근데 두꺼우니까 뭔가 정신사나움
            4. nail pushButton 크기 변경 -> 귀찮음 나중에 하자.
            5. 시계추가 - 완료
         */
        //line.startWidth = line.endWidth = 0.25f;    // 라인 크기
        for (int i = 0; i < 25; i++)
        {
            nailposition[i] = GameObject.Find("nail_" + (i + 1)).transform.position;
            nailposition[i].y = 0.7f; //y값 고정
        }

        cnt = 1;
        hammering = hammer.GetComponent<Animator>();
    }

    public void DownNail(GameObject button, GameObject nail)//버튼과 못
    {
        // 정반응 카운트.
        if (cnt.ToString() == button.name && nail != null)
        {
            CollectReaction++;
            // 09.10 추가. 플레이어가 못을 빠르게 누르면 망치가 못 따라오는 것을 방지하기 위해 수정.
            if (currentAttackCoroutine != null) StopCoroutine(currentAttackCoroutine);
            currentAttackCoroutine = StartCoroutine(AttackHammer(nail));

            this.GetComponent<AudioSource>().Play();//망치 소리

            button.GetComponent<RectTransform>().position = new Vector3(button.transform.position.x, button.transform.position.y - 54f, 0);
            nail.transform.Translate(new Vector3(0, -0.1f, 0));
/*            if (cnt >= 1)
            {
                line.SetPosition(cnt - 1, nailposition[cnt - 1]);
                line.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                line.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            }*/

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
                SendCollectReaction(CollectReaction);
                SendWrongReaction(WrongReaction);

                if (!exitPanel.active)
                {
                    exitPanel.SetActive(false);
                    Time.timeScale = 1f;
                }

                print("테스트 종료.");
            }
        }

        // 오반응 카운트.
        else
        {
            WrongReaction++;
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

    // 08.24 테스트 종료 후 메인화면으로 돌아가는 요청을 하는 함수
    public void ReturnMainMenu_toreact()
    {
        Application.OpenURL("https://web-template-3prof2llkxuyz4l.sel4.cloudtype.app/dashboard");
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("Hammering_temp1");
    }

}