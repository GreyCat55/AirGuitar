using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{

    [SerializeField] ParticleSystem smoke;
    private bool state = false;

    // Update is called once per frame
    void Update()
    {

    }

    void TogglePower()
    {
        state = !state;
        gameObject.GetComponent<AudioSource>().Play();
        if (state)
        {
            transform.localEulerAngles = new Vector3(0.0f,22.0f,0.0f);
            smoke.Play();
        }
        else
        {
            transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            smoke.Stop();
        }
    }
}
