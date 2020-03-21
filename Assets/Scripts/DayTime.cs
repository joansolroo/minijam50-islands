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
    [SerializeField] bool autoIncrement = false;

    [SerializeField] float morningTime = 0.3f;
    [SerializeField] float nightTime = 0.9f;
    public void StartDay()
    {

    }
    public void GoToNight()
    {
        time = day + nightTime;
    }
    public void GoToNextMorning()
    {
        if (time < (day + 0.3f))
        {
            time = morningTime;
        }
        else
        {
            time = day + 1+ morningTime;
        }
    }

    private void Update()
    {
        float newTime = currentTotalTime ;
        if (autoIncrement)
        {
            newTime += Time.deltaTime / dayDuration;
        }
        if (newTime < time)
        {
            newTime = Mathf.MoveTowards(newTime, time, catchUpSpeed * Time.deltaTime);
        }

        currentTotalTime = newTime;
        day = (int)currentTotalTime;

        if(sky)
            sky.UpdateColor(currentTotalTime % 1);
        if(nightVision && sky)
            nightVision.color = new Color(1, 1, 1, Mathf.Max(0,2*(0.5f-sky.sprite.color.grayscale)));
    }
}
