using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour
{
    public float percentage;
    private LineRenderer tether;

    void Awake()
    {
        tether = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        tether.SetPosition(0,transform.position);
        percentage = transform.localEulerAngles.y / 360.0f;
    }
}
