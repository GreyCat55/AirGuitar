using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerometer : MonoBehaviour
{

    private Vector3 instantVelocity;
    private Vector3 lastPosition;
    private float elapsedTime;

    // Start is called before the first frame update
    void Awake()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.01f)
        {
            SetVelocity();
            elapsedTime = 0.0f;
        }
    }

    void SetVelocity()
    {
        instantVelocity = transform.position - lastPosition;
        lastPosition = transform.position;
    }

    public Vector3 GetVelocity()
    {
        return instantVelocity;
    }
}
