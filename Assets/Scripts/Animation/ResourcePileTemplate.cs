using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePileTemplate : MonoBehaviour
{
    public Rigidbody2D playerBody;

    public int stackIndex;
    public Vector3 initPosition;

    public float cooldown = 0f;
    public float t = 0f;
    public Vector3 displacementTarget;
    public float targetAngle = 0f;

    public AnimationCurve speedAmplitude;
    public AnimationCurve curve;

    void Start()
    {
        cooldown = Random.Range(0.5f, 1f);
        initPosition = transform.localPosition;
        t = Random.Range(0f, 1f);
    }
    
    void Update()
    {
        float speed = 0.2f * playerBody.velocity.x;
        float amplitude = speedAmplitude.Evaluate(Mathf.Abs(speed));

        t += amplitude * Time.deltaTime;
        if (t >= 1f)
            t = 0f;

        transform.localPosition = new Vector3(initPosition.x - 0.1f * Mathf.Pow(2, initPosition.y) * speed, initPosition.y * (1f + 0.1f * amplitude * (1f - 2f * curve.Evaluate(t))), initPosition.z);
    }
}
