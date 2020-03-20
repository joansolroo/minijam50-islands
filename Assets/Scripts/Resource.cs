using UnityEngine;

public abstract class Resource : MonoBehaviour
{
    public const float Min = 0;

    [SerializeField]
    private float max;

    [SerializeField]
    private float value;

    public float Max => max;

    public float Value => value;

    public void Add(float value)
    {
        this.value += value;
        this.value = Mathf.Min(this.value, Max);
    }

    public void Remove(float value)
    {
        this.value -= value;
        this.value = Mathf.Max(this.value, Min);
    }
}
