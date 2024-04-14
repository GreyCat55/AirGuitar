using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    public Transform clipboard;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PickUpToWrite()
    {
        transform.parent = null;
    }

    void ReturnToClipboard()
    {
        transform.parent = clipboard;
        transform.localPosition = new Vector3(0.0f,0.2f,-0.32f);
        transform.localEulerAngles = new Vector3(0.0f, 90.0f, 180.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.parent = null;
    }
}
