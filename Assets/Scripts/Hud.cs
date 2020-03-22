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
    public GameObject buttonInteract;
    public GameObject buttonRetreat;
    public GameObject buttonNext;

    [Header("Player")]
    public Boat boat;
    public Text boatText;

    [Header("Resources")]
    public Camp camp;

    public Text foodText;

    public Text waterText;

    public Text woodText;

    private void LateUpdate()
    {
        boatText.text = "Boat:"+(int)(boat.Progress*100)+'%';

        UpdateHelp();
        UpdateDayTime();
        UpdatePlayerResources();
    }

    public void UpdateHelp()
    {
        //buttonRetreat.SetActive(player.)
        buttonInteract.SetActive(player.CanInteract());
        buttonRetreat.SetActive(!player.AtBase());
        buttonNext.SetActive(player.Stamina > 0);
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
        staminaText.text = $"Crew: {player.Stamina}/{player.maxStamina}";
        foodText.text = $"Food: {camp.GetFood()}";
        woodText.text = $"Wood: {camp.GetWood()}";
    }
}
