using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;
using System;
using TMPro;
using HighlightingSystem;

public class Hammering_UIManager_Tutorial : MonoBehaviour
{
    public bool timerflag;
    public float time;
    public TMP_Text timertext;

    // 08.25 못의 숫자를 랜덤하게 바꾸기 위해 추가함.
    // buttons들은 피셔-에이트 알고리즘을 통해 O(25)의 시간복잡도 만큼 랜덤으로 배치가 될 것임. 
    public GameObject buttons;

    // 09.10 스크립트 추가
    public HighlightingRenderer HR;
    public GameObject MenuPanel;

    void Start()
    {
        buttons.SetActive(false);
        time = 0;
        timerflag = false;
    }

    private void Update()
    {
        if (timerflag)
        {
            timertext.gameObject.SetActive(true);
            time += Time.deltaTime;
            timertext.text = ((int)time / 60 % 60).ToString("00") + " : " + ((int)time % 60).ToString("00");
        }
    }

    public void SkipTutorial()
    {
        SceneManager.LoadScene("Hammering");
    }

    public void GameStart()
    {
        timerflag = true; 
        buttons.SetActive(true);
        HR.enabled = true;
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
