using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Simon_Demo : MonoBehaviour
{
    public List<Image> lightImages = new List<Image>(4);
    public List<Button> buttons = new List<Button>(4);
    public List<GameObject> spheres = new List<GameObject>(4);

    public GameObject timer;

    public TextMeshProUGUI roundText;
    public TextMeshProUGUI roundText2;

    private bool isShowingImages;
    public bool flag;

    [SerializeField]
    private int clickCount;

    public GameObject imageFader;
    public GameObject MenuPanel;
    private ImageFader imageFaderScript;

    [SerializeField]
    private int currentButtonIndex;

    [SerializeField]
    private int nextButtonIndex;

    void Start()
    {
        isShowingImages = true;
        imageFaderScript = imageFader.GetComponent<ImageFader>();
        roundText.text = "Round :      1";
        roundText2.text = "Round :      1";
        clickCount = 0;
        flag = false;
        currentButtonIndex = 0;
        nextButtonIndex = 0;

        foreach(Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    // 사용자가 버튼을 클릭했을 때 호출되는 함수
    public void OnButtonClick(string buttonName)
    {
        if (!isShowingImages)
        {
            // 버튼 이름과 일치하는 lightImage를 표시
            ShowLightImage(buttonName);

            // 클릭 횟수 증가
            clickCount++;


            // 다음 버튼을 활성화
            if (clickCount < 4)
            {
                currentButtonIndex = nextButtonIndex;
                nextButtonIndex++;

                // 다음 버튼을 활성화
                if (nextButtonIndex < 4)
                {
                    buttons[currentButtonIndex].gameObject.SetActive(false);
                    buttons[nextButtonIndex].gameObject.SetActive(true);

                    // Sphere도 순서대로 활성화
                    spheres[currentButtonIndex].SetActive(false);
                    spheres[nextButtonIndex].SetActive(true);
                }
                if (nextButtonIndex == 3)
                    spheres[currentButtonIndex + 1].SetActive(false);                    
            }           
            else if (clickCount == 4) // 4번 누르면 버튼 비활성화
            {
                foreach (Button button in buttons)
                {
                    button.gameObject.SetActive(false);
                }
                roundText.text = "Round :       2";
                roundText2.text = "Round :       2";
                flag = true;
            }
        }
    }

    // 버튼에 이미지 할당
    void AssignImageToButton(Image buttonImage, Sprite imageToAssign)
    {
        buttonImage.sprite = imageToAssign;
    }

    // 빛 이미지를 순차적으로 표시
    public IEnumerator ShowLightImagesSequentially()
    {
        var wfs = new WaitForSeconds(1.0f);

        yield return new WaitForSeconds(2.0f);

        foreach (Image lightImage in lightImages)
        {
            // 이미지를 활성화하여 표시
            lightImage.gameObject.SetActive(true);
            yield return wfs; // 2초 동안 이미지 표시

            // 이미지를 비활성화
            lightImage.gameObject.SetActive(false);
            yield return wfs; // 2초 동안 이미지 비표시
        }

        // 모든 빛 이미지를 표시한 후에 버튼을 표시하도록 설정
        ShowButtonImages();
        flag = true;
        imageFaderScript.fadePlaying = true;
    }

    // 버튼 이미지를 표시하고 이미지 표시 중 상태를 변경
    void ShowButtonImages()
    {
        foreach (Image lightImage in lightImages)
        {
            lightImage.gameObject.SetActive(false); // 빛 이미지 숨김
        }

        buttons[0].gameObject.SetActive(true);
        spheres[0].gameObject.SetActive(true);
        isShowingImages = false;
    }

    // 버튼 이름과 일치하는 lightImage를 표시
    void ShowLightImage(string buttonName)
    {
        foreach (Image lightImage in lightImages)
        {
            // 버튼 이름과 일치하는 lightImage를 찾음
            if (lightImage.name == buttonName)
            {
                // 버튼 이름과 일치하는 lightImage를 1초 동안 표시
                lightImage.gameObject.SetActive(true);
                StartCoroutine(HideLightImageAfterDelay(lightImage));
            }
        }
    }

    // 이미지를 일정 시간 후에 비활성화
    public IEnumerator HideLightImageAfterDelay(Image lightImage)
    {
        yield return new WaitForSeconds(1.0f); // 1초 동안 기다림
        lightImage.gameObject.SetActive(false); // 이미지를 비활성화
    }

    public void MenuPanelOnAndOff()
    {
        if (MenuPanel.activeSelf == false)
        {
            MenuPanel.SetActive(true);
        }
        else MenuPanel.SetActive(false);
    }

    public void ReturnMainMenu_toreact()
    {
        Application.OpenURL("https://web-template-3prof2llkxuyz4l.sel4.cloudtype.app/dashboard");
    }
}
