using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePileTemplate : MonoBehaviour
{
    public ResourceType type;
    public int stackIndex;
    public Vector3 initPosition;

    public float cooldown = 0f;
    public float t = 0f;
    public Vector3 displacementTarget;
    public float targetAngle = 0f;

    public AnimationCurve speedAmplitude;
    public AnimationCurve curve;

    public bool destroy = false;
    public bool appear = true;
    public Sprite[] animations;
    public SpriteRenderer sr;
    private int animationIndex;
    private float animationTime;
    public float perFrameTime;
    private Vector3 velocity;
    private Vector3 lastParentPosition;

    void Start()
    {
        cooldown = Random.Range(0.5f, 1f);
        initPosition = transform.localPosition;
        t = Random.Range(0f, 1f);
        velocity = Vector3.zero;

        if(!sr)
            sr = GetComponent<SpriteRenderer>();
        animationTime = Random.Range(-perFrameTime, 0);
        animationIndex = animations.Length - 1;

        if(appear)
        {
            sr.sprite = animations[animationIndex];
        }
        lastParentPosition = transform.parent.position;
    }
    
    void Update()
    {
        if(appear)
        {
            if (animationTime >= perFrameTime)
            {
                animationTime -= perFrameTime;
                animationIndex--;
                if (animationIndex == 0)
                {
                    animationTime = Random.Range(-perFrameTime, 0);
                    appear = false;
                }
                sr.sprite = animations[animationIndex];
            }
            animationTime += Time.deltaTime;
        }

        if (destroy)
        {
            velocity += -9.81f * Time.deltaTime * Vector3.up;
            transform.localPosition += velocity * Time.deltaTime;
            
            if (animationTime >= perFrameTime)
            {
                animationTime -= perFrameTime;
                animationIndex++;
                if (animationIndex >= animations.Length)
                    Destroy(gameObject);
                else
                    sr.sprite = animations[animationIndex];
            }
            animationTime += Time.deltaTime;
        }
        else
        {
            Vector3 parentSpeed = Vector3.zero;
            if(transform.parent)
                parentSpeed= transform.parent.position - lastParentPosition;
            float speed = 0.2f * parentSpeed.x;
            float amplitude = speedAmplitude.Evaluate(Mathf.Abs(speed));

            t += amplitude * Time.deltaTime;
            if (t >= 1f)
                t = 0f;

            Vector3 newPosition = new Vector3(initPosition.x - 0.1f * Mathf.Pow(2, initPosition.y) * speed, initPosition.y * (1f + 0.1f * amplitude * (1f - 2f * curve.Evaluate(t))), initPosition.z);
            velocity = newPosition - transform.localPosition;
            transform.localPosition = newPosition;
        }

        if (transform.parent)
            lastParentPosition = transform.parent.position;
    }

    public void Destroy()
    {
        destroy = true;
        transform.parent = null;
        animationIndex = 0;
    }
}
