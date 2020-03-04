﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    int frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;
    float updateRate = 4.0f;  // 4 updates per sec.

    void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0f / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }

        fps = Mathf.RoundToInt(fps);
        text.text ="Fps: " + fps.ToString();
    }

    public void ShowOrHide()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
