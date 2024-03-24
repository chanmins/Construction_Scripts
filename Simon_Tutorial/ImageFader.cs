using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    public float minAlpha;
    public float maxAlpha;
    public float fadeDuration;

    public Image[] imageList;
    private float timer;
    private bool fadeIn;
    public bool fadePlaying;

    void Start()
    {
        fadePlaying = false;
        minAlpha = 0.2f;
        maxAlpha = 0.7f;
        fadeDuration = 1;
        fadeIn = true;
        timer = fadeDuration;
    }

    void FixedUpdate()
    {
        if (fadePlaying)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                fadeIn = !fadeIn;

                timer = fadeDuration;
            }

            float alpha = Mathf.Lerp(minAlpha, maxAlpha, timer / fadeDuration);

            foreach (var image in imageList)
            {
                if (fadeIn)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                }
                else
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 1f - alpha);
                }
            }

        }
    }
}
