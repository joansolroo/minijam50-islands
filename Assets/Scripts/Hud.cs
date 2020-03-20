using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [Header("PlayerResources")]
    public PlayerResources playerResources;

    public Text foodText;

    public Text waterText;

    public Text woodText;

    private void LateUpdate()
    {
        UpdatePlayerResources();
    }

    private void UpdatePlayerResources()
    {
        foodText.text = $"Food: {playerResources.Food.Value}";
        waterText.text = $"Water: {playerResources.Water.Value}";
        woodText.text = $"Wood: {playerResources.Wood.Value}";
    }
}
