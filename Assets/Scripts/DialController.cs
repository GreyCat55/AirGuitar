using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialController : MonoBehaviour
{
    public GameObject controller;
    public GameObject control;
    private float firstGrabAngle = 0.0f;
    private float deltaRotation = 0.0f;
    private bool rotating = false;


    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (/*selected pinched*/true && control)
        {
            rotating = true;
            firstGrabAngle = controller.transform.eulerAngles.y;
            deltaRotation = 0.0f;


        }
        if (/*unselected pinch*/true || control == null)
        {
            rotating = false;
        }
        if (rotating)
        {
            deltaRotation = controller.transform.eulerAngles.y - firstGrabAngle;
            control.gameObject.transform.localEulerAngles = new Vector3(control.gameObject.transform.localEulerAngles.x, control.gameObject.transform.localEulerAngles.y + deltaRotation, control.gameObject.transform.localEulerAngles.z);
            if (Mathf.Abs(deltaRotation) > 1.5f) {
                control.GetComponentInChildren<AudioSource>().Play();
            }
            firstGrabAngle = controller.transform.eulerAngles.y;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Dial"))
        {
            Debug.Log("You can grab the dial");
            control = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
      if (other.gameObject.CompareTag("Dial"))
       {
            Debug.Log("You left the dial");
            control = null;
       }
    }
}
