using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;
using System;
using TMPro;

public class Hammering_UIManager : MonoBehaviour
{
    bool setting_flag;
    public bool timerflag;
    public GameObject pannel;
    public float time;
    public TMP_Text timertext;

    // 08.25 못의 숫자를 랜덤하게 바꾸기 위해 추가함.
    // buttons들은 피셔-에이트 알고리즘을 통해 O(25)의 시간복잡도 만큼 랜덤으로 배치가 될 것임. 
    public GameObject buttons;
    public Vector3[] buttonPosition;
    public GameObject MenuPanel;

    private void Awake()
    {
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }
    void Start()
    {
        buttons.SetActive(false);
        setting_flag = true;
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

    public void OpenSettingPanel()
    {
        if (setting_flag)
        {
            pannel.SetActive(true);
            Time.timeScale = 0;
            setting_flag = false;
        }
        else
        {
            pannel.SetActive(false);
            Time.timeScale = 1;
            setting_flag = true;
        }
    }

    public void BackToGame()
    {
        pannel.SetActive(false);
        Time.timeScale = 1;
        setting_flag = true;
    }

    public void StartTimer()
    {
        timerflag = true;
        buttons.SetActive(true);
        Shuffle(buttonPosition);

        for (int i = 0; i < 25; i++)
        {
            buttons.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = buttonPosition[i];
        }
    }

    private void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        int last = n - 2;

        for (int i = 0; i <= last; i++)
        {
            int r = UnityEngine.Random.Range(i, n); // [i, n - 1]
            Swap(i, r);
        }

        // Local Method
        void Swap(int idxA, int idxB)
        {
            T temp = array[idxA];
            array[idxA] = array[idxB];
            array[idxB] = temp;
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

}
