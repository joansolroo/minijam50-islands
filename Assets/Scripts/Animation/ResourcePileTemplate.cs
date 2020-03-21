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

    void Start()
    {
        cooldown = Random.Range(0.5f, 1f);
        initPosition = transform.localPosition;
    }
    
    void Update()
    {
        float speed = 0.2f * playerBody.velocity.x;
        float amplitude = speedAmplitude.Evaluate(Mathf.Abs(speed));

        t += amplitude * Time.deltaTime;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, initPosition + 0.3f * amplitude * displacementTarget, 0.03f * amplitude * Time.deltaTime);
        transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles, amplitude * new Vector3(0, 0, targetAngle), 50f * amplitude * Time.deltaTime);

        if(t >= cooldown)
        {
            t = 0f;
            displacementTarget = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            targetAngle = 0; // Random.Range(-20f, 20f);
            cooldown = Random.Range(0.5f, 1f);
        }
    }
}
