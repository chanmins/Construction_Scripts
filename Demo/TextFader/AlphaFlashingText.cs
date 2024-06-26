﻿// Copyright (c) 2016 Unity Technologies. MIT license - license_unity.txt
// #NVJOB Alpha Flashing UI. MIT license - license_nvjob.txt
// #NVJOB Nicholas Veselov - https://nvjob.github.io
// #NVJOB Alpha Flashing UI v2.3 - https://nvjob.github.io/unity/alpha-flashing-text


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[AddComponentMenu("#NVJOB/Tools/Alpha Flashing UI/Text")]


public class AlphaFlashingText : MonoBehaviour
{

    [Header("Settings")]
    public List<TextAlpha> textList = new List<TextAlpha>();

    //---------------------------------

    Color[] currentColor;
    TextMeshProUGUI[] thisText;

    float[] chAlpha;
    int textCount;

    void Awake()
    {
        //---------------------------------

        textCount = textList.Count;

        //---------------------------------

        currentColor = new Color[textCount];
        thisText = new TextMeshProUGUI[textCount];
        chAlpha = new float[textCount];

        //---------------------------------

        for (int num = 0; num < textCount; num++)
        {
            thisText[num] = textList[num].text.GetComponent<TextMeshProUGUI>();
            currentColor[num] = thisText[num].color;
        }


        //---------------------------------
    }

    void LateUpdate()
    {
        //---------------------------------

        for (int num = 0; num < textCount; num++)
        {
            chAlpha[num] = textList[num].minAlpha + Mathf.PingPong(Time.time / textList[num].timerAlpha, textList[num].maxAlpha - textList[num].minAlpha);
            thisText[num].color = new Color(currentColor[num].r, currentColor[num].g, currentColor[num].b, Mathf.Clamp(chAlpha[num], 0.0f, 1.0f));
        }

        //---------------------------------
    }
    
}

[System.Serializable]

public class TextAlpha
{
    public TextMeshProUGUI text;
    public Image image;
    public float minAlpha = 0.1f;
    public float maxAlpha = 0.9f;
    public float timerAlpha = 1.0f;
}
