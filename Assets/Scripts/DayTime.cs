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
    [SerializeField] int day = 0;
    [SerializeField] float currentTime;
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

    private void OnGUI()
    {
        float hours = (currentTime - day) * 24;
        int h = (int)hours;
        int m = (int)((hours - h)*60);
        GUI.Label(new Rect(300, 0, 120, 30), "day:" + (day+1) + " " + (h.ToString("00")+ ":"+m.ToString("00")));
    }
    private void Update()
    {
        float newTime = currentTime + Time.deltaTime / dayDuration;
        if (newTime < time)
        {
            newTime = Mathf.MoveTowards(newTime, time, catchUpSpeed * Time.deltaTime);
        }

        currentTime = newTime;
        day = (int)currentTime;

        if(sky)
            sky.UpdateColor(currentTime % 1);
        if(nightVision && sky)
            nightVision.color = new Color(1, 1, 1, Mathf.Max(0,4*(0.5f-sky.sprite.color.grayscale)));
    }
}
