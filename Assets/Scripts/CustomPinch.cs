using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CustomPinch : MonoBehaviour
{
    [SerializeField] private Transform finger;
    [SerializeField] private Transform thumb;
    private SphereCollider fingerCollider, thumbCollider;
    private LineRenderer lr;
    private Vector3 pinchVector;
    private bool pinchedSomething = false;
    private float lastMagnitude = 0.0f;
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        fingerCollider = finger.GetComponent<SphereCollider>();
        thumbCollider = finger.GetComponent<SphereCollider>();

        Debug.Log("Got finger collider with center/rad: " + fingerCollider.radius);
    }

    // Update is called once per frame
    void Update()
    {
        pinchVector = finger.position - thumb.position;
        lr.SetPosition(0, finger.position);
        lr.SetPosition(1, thumb.position);
        lr.SetPosition(2, new Vector3(finger.position.x, thumb.position.y, finger.position.z));

        
        Collider[] fingerTouching = Physics.OverlapSphere(fingerCollider.transform.position, fingerCollider.radius);
        Collider[] thumbTouching = Physics.OverlapSphere(thumbCollider.transform.position, thumbCollider.radius);

        Collider[] thingsPinched = fingerTouching.Intersect<Collider>(thumbTouching).ToArray<Collider>();

        foreach (var collider in thingsPinched)
        {
            if (collider.CompareTag("Knob")) {
                if (!pinchedSomething)
                {
                    pinchedSomething = true;
                    lastMagnitude = pinchVector.magnitude;
                    Debug.Log("You grabbed a knob");
                }
                collider.transform.eulerAngles = new Vector3(collider.transform.eulerAngles.x, pinchVector.magnitude * 2500.0f, collider.transform.eulerAngles.z);
                Debug.Log("Turned " + pinchVector.magnitude * 5000.0f + " degrees");
            }
            else
            {
                pinchedSomething = false;
            }
        }
        
    }

}
