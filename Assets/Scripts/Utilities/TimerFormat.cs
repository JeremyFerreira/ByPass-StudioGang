using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerFormat
{
    public static string FormatTime(float time)
    {
        int intTime = (int)time;
        int Minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = time * 100;
        fraction = (fraction % 100);
        string timeText = String.Format("{0:00}:{1:00}:{2:00}", Minutes, seconds, fraction);
        return timeText;
    }
}
