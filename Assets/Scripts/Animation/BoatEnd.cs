using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEnd : MonoBehaviour
{
    public Vector3 captainSlot;
    public List<Vector3> slots;
    public float speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += speed * Time.deltaTime * Vector3.right;
    }
}
