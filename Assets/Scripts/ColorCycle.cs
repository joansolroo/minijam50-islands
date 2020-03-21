using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCycle : MonoBehaviour
{
    [SerializeField] public SpriteRenderer sprite;
    [SerializeField] Gradient gradient;
    [SerializeField] float duration = 60;
    [SerializeField] [Range(0,1)]float time;
    [SerializeField] bool animate = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (animate) {
            time += Time.deltaTime / duration;
            if (time > 1f)
            {
                time %= 1f;
            }
            UpdateColor(time);
        }
       
    }
    public void UpdateColor(float _time)
    {
        time = _time %= 1f;
        sprite.color = gradient.Evaluate(time);
    }
}
