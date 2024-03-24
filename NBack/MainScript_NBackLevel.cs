/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainScript_NBackLevel : MonoBehaviour
{
    public bool level;
    public GameObject LevelManagement;


    public void choice_Nback()
    {
        string button = EventSystem.current.currentSelectedGameObject.name;//내가 누른 버튼오브젝트
        if (button == "1-back")
        {
            level = true;
*//*            DontDestroyOnLoad(LevelManagement);*//*
        }
        else if(button == "2-back")
        {
            level = false;
*//*            DontDestroyOnLoad(LevelManagement);*//*
        }
*//*        SceneManager.LoadScene("Nback");*//*
    }

}
*/