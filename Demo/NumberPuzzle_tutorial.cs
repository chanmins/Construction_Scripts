using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//08.23 스트립트 수정중
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.Collections;
using HighlightingSystem;

public class NumberPuzzle_tutorial : MonoBehaviour
{
    public Button[] buttons;
    public TextMeshProUGUI[] blocks;

    private List<int> numbers = new List<int>();
    [SerializeField]private int count;

    private float time;
    public Text timertext;
    public GameObject exitPanel;
    public float timer;

    private const int PuzzleSize = 15;
    private const int LastNumber = PuzzleSize + 1;

    // 09.04 스크립트 수정중
    public Tutorial_tutorial myTutorial;
    bool D1 = false;
    bool D2 = false;

    // 09.18 글씨 강조를 위한 머테리얼 추가.
    [Header("HighlightTMP")]
    public TextMeshProUGUI[] tmpArray;
    public GameObject[] ContainerHighLighters;
    public Material originMaterial;
    public Material changeMaterial;
    public int numOrder = 0;

    public GameObject MenuPanel;

    //08.23 스트립트 수정중
    [DllImport("__Internal")]
    private static extern void SendEndTime(float endTime);


    private void Start()
    {
        Time.timeScale = 0;
        count = 1;
        time = 0;
        timer = 0;

        // 튜토리얼에서는 번호의 위치를 일정하게 만듬.
        /*
        for (int i = 0; i < PuzzleSize; i++)
        {
            numbers.Add(i + 1);
        }

        for (int i = 0; i < PuzzleSize; i++)
        {
            int k = Random.Range(0, numbers.Count);
            blocks[i].text = numbers[k].ToString();
            numbers.RemoveAt(k);
        }
        */

        for (int i = 0; i < PuzzleSize; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            timer += Time.deltaTime;
            float timer2 = Mathf.Floor(timer);
            time += Time.deltaTime;
            timertext.text = ((int)time / 60 % 60).ToString("00") + " : " + ((int)time % 60).ToString("00");
        }

        if (count == 2 && !D1)
        {
            D1 = true;
            myTutorial.tutorial_Panel.SetActive(true);
            myTutorial.NextStep();
            StartCoroutine(TurnOffPanel());
        }
        else if (count == 16 && !D2)
        {
            D2 = true;
            myTutorial.tutorial_Panel.SetActive(true);
            myTutorial.NextStep();
            myTutorial.HiddenPanel.SetActive(true);
        }
        if (count <= 15)
        {
            tmpArray[count - 1].fontMaterial = changeMaterial;
        }
    }

    public void ReturnMainMenu_toreact()
    {
        Application.OpenURL("https://web-template-3prof2llkxuyz4l.sel4.cloudtype.app/dashboard");
    }

    public void ButtonCheck()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        string buttonText = GameObject.Find(buttonName).GetComponentInChildren<TextMeshProUGUI>().text;

        if (count == int.Parse(buttonText))
        {
            GameObject buttonObject = GameObject.Find(buttonName);
            GameObject checkMark = GameObject.Find(buttonName + "-");

            count++;
            buttonText = "V";
            buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
            buttonObject.SetActive(false);
            ContainerHighLighters[numOrder].GetComponent<Highlighter>().enabled = false;
            numOrder++;
            if(numOrder <= 14)
            {
                ContainerHighLighters[numOrder].GetComponent<Highlighter>().enabled = true;
            }
        }
        else
        {
            print("False");
        }
    }

    public void GameStart()
    {
        for (int i = 0; i < PuzzleSize; i++)
        {
            buttons[i].gameObject.SetActive(true);
            Time.timeScale = 1;
        }
        ContainerHighLighters[0].GetComponent<Highlighter>().enabled = true;
    }

    public void MenuPanelOnAndOff()
    {
        if (MenuPanel.activeSelf == false)
        {
            MenuPanel.SetActive(true);
        }
        else MenuPanel.SetActive(false);
    }

    IEnumerator TurnOffPanel()
    {
        yield return new WaitForSeconds(3f);
        myTutorial.tutorial_Panel.SetActive(false);
    }

    
}

// 2023.08 에 수정한 원본 함수 코드
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;



public class NumberPuzzle : MonoBehaviour
{
    public Button[] button;
    public TextMeshProUGUI[] block;

    List<int> number = new List<int>();
    public int count;
    
    float time;
    public Text timertext;
    public GameObject exitPanel;
    public float timer;

    void Start()
    {
        button = new Button[15];
        block = new TextMeshProUGUI[15];
        count = 1;
        time = 0;
        timer = 0;

        for (int i = 0; i < 15; i++)
        {
            number.Add(i+1);
        }
        print(number[11]);
        for (int i = 0; i < 15; i++)
        {
            int k = Random.Range(0, number.Count);
            block[i].GetComponent<TextMeshProUGUI>().text = number[k].ToString();
            number.RemoveAt(k);
        }

        for(int i = 0; i < 15; i++)
        {
            button[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        //타이머 코드 통일
        timer += Time.deltaTime;
        float timer2 = Mathf.Floor(timer);
        time += Time.deltaTime;
        timertext.text = ((int)time / 60 % 60).ToString("00") + " : " + ((int)time % 60).ToString("00");
    }


    public void ButtonCheck()
    {

        string ButtonName = EventSystem.current.currentSelectedGameObject.name;
        string but = GameObject.Find(ButtonName).GetComponentInChildren<TextMeshProUGUI>().text;


        if (count == int.Parse(but))
        {

            GameObject g = GameObject.Find(ButtonName + "-");

            print("Correct");
            count++;
            GameObject.Find(ButtonName).GetComponentInChildren<TextMeshProUGUI>().text = "V";
            GameObject.Find(ButtonName).SetActive(false);
        }
        else
        {
            print("False");
        }

        if(count == 16)
        {
            Time.timeScale = 0f;
            exitPanel.SetActive(true);
            if (!exitPanel.active)
            {
                exitPanel.SetActive(false);
                Time.timeScale = 1f;
            }
            print("asd");
            SQLiteDB.Instance.NumberPuzzleDataSave(timer.ToString());
        }
    }

    public void gameStart()
    {
        for (int i = 0; i < 15; i++)
        {
            button[i].gameObject.SetActive(true);
        }
    }
}
*/