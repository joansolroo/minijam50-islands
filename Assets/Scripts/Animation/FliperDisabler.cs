using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FliperDisabler : MonoBehaviour
{
    private SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void LateUpdate()
    {
        if (sr)
            sr.enabled = !sr.flipX;
    }
}
