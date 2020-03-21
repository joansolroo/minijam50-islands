using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTime : MonoBehaviour
{
    [SerializeField] ColorCycle sky;
    [SerializeField] SpriteRenderer nightVision;
    [SerializeField] bool play;
   
    [SerializeField] float dayDuration = 60;
    [SerializeField] float catchUpSpeed = 0.5f;
    [Header("Status")]
    [SerializeField] public int day = 0;
    [SerializeField] public float currentTotalTime;
    [SerializeField] float time;
    public void StartDay()
    {

    }
    public void GoToNight()
    {
        time = day + 0.8f;
    }
    public void GoToNextMorning()
    {
        if (time < (day + 0.3f))
        {
            time = 0.3f;
        }
        else
        {
            time = day + 1.3f;
        }
    }

    private void Update()
    {
        float newTime = currentTotalTime + Time.deltaTime / dayDuration;
        if (newTime < time)
        {
            newTime = Mathf.MoveTowards(newTime, time, catchUpSpeed * Time.deltaTime);
        }

        currentTotalTime = newTime;
        day = (int)currentTotalTime;

        sky.UpdateColor(currentTotalTime % 1);
        nightVision.color = new Color(1, 1, 1, Mathf.Max(0,4*(0.5f-sky.sprite.color.grayscale)));
    }
}
