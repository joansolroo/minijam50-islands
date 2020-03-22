using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectTransform : MonoBehaviour
{
    [SerializeField] float posterization = 16;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 position = transform.parent.position;
        position.x = (int)((position.x) * posterization) / posterization;
        position.y = (int)((position.y) * posterization) / posterization;
        this.transform.position = position;     
    }
}
