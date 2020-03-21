using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [Header("Time")]
    public DayTime time;
    public Text timeText;
    [Header("Player")]
    public PlayerController player;
    public Text staminaText;

    [Header("Player")]
    public Boat boat;
    public Text boatText;

    [Header("PlayerResources")]
    public PlayerResources playerResources;

    public Text foodText;

    public Text waterText;

    public Text woodText;

    private void LateUpdate()
    {
        boatText.text = "Boat:"+(int)(boat.progress*100)+'%';

        UpdateDayTime();
        UpdatePlayerResources();
    }
    private void UpdateDayTime()
    {
        int day = time.day;
        float hours = (time.currentTotalTime - day) * 24;
        int h = (int)hours;
        int m = (int)((hours - h) * 4)*15;
        timeText.text = $"Day: {day}/30\n"+h.ToString("00")+':'+m.ToString("00");
    }
    private void UpdatePlayerResources()
    {
        staminaText.text = $"Stamina: {playerResources.People}";
        foodText.text = $"Food: {playerResources.Food}";
        woodText.text = $"Wood: {playerResources.Wood}";
    }
}
