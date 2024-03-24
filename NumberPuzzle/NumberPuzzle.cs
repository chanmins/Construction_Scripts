using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//08.23 스트립트 수정중
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class NumberPuzzle : MonoBehaviour
{
    public Button[] buttons;
    public TextMeshProUGUI[] blocks;

    private List<int> numbers = new List<int>();
    private int count;

    private float endTime;
    public Text timertext;
    public GameObject exitPanel;
    public float timer;

    private const int PuzzleSize = 15;
    private const int LastNumber = PuzzleSize + 1;

    // 09.10 정반응, 오반응 개수를 세기 위한 변수추가.
    [Header("Count Collect / Wrong")]
    public int CollectReaction = 0;
    public int WrongReaction = 0;

    public GameObject MenuPanel;

    //08.23 스트립트 수정중
    [DllImport("__Internal")]
    private static extern void SendEndTime(float endTime);

    [DllImport("__Internal")]
    private static extern void SendCollectReaction(int CollectReaction);

    [DllImport("__Internal")]
    private static extern void SendWrongReaction(int WrongReaction);

    private void Start()
    {
        Time.timeScale = 0;
        count = 1;
        endTime = 0;
        timer = 0;

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
            endTime += Time.deltaTime;
            timertext.text = ((int)endTime / 60 % 60).ToString("00") + " : " + ((int)endTime % 60).ToString("00");
        }
    }

    public void ButtonCheck()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        string buttonText = GameObject.Find(buttonName).GetComponentInChildren<TextMeshProUGUI>().text;

        if (count == int.Parse(buttonText))
        {
            CollectReaction++;
            GameObject buttonObject = GameObject.Find(buttonName);
            GameObject checkMark = GameObject.Find(buttonName + "-");

            count++;
            buttonText = "V";
            buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
            buttonObject.SetActive(false);
        }
        else
        {
            WrongReaction++;
            print("False");
        }

        if (count == LastNumber)
        {
            Time.timeScale = 0f;
            exitPanel.SetActive(true);

            //08.23 스트립트 수정중, 리액트와 통신하기 위해 추가한 코드
            Debug.Log(endTime);
            SendEndTime(endTime);
            SendCollectReaction(CollectReaction);
            SendWrongReaction(WrongReaction);

            if (!exitPanel.active)
            {
                exitPanel.SetActive(false);
                Time.timeScale = 1f;
            }
            print("테스트 종료");
/*            SQLiteDB.Instance.NumberPuzzleDataSave(timer.ToString());*/
        }
    }

    public void GameStart()
    {
        for (int i = 0; i < PuzzleSize; i++)
        {
            buttons[i].gameObject.SetActive(true);
            Time.timeScale = 1;
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

    // 08.26 테스트 종료 후 메인화면으로 돌아가는 요청을 하는 함수를 공통적으로 추가함.
    // 홈버튼을 추가함.
    public void ReturnMainMenu_toreact()
    {
        Application.OpenURL("https://web-template-3prof2llkxuyz4l.sel4.cloudtype.app/dashboard");
    }

    public void PlayTutorial()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Puzzle_temp");
    }

}

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