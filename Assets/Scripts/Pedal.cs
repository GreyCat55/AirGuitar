using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Pedal : MonoBehaviour
{

    public GameObject input;
    public GameObject output;
    [SerializeField] private AudioMixerGroup mixer;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (input && output)
        {
            mixer.audioMixer.SetFloat("distortion", transform.GetChild(0).GetComponent<Dial>().percentage);
        }
        else
        {
            mixer.audioMixer.SetFloat("distortion", 0.0f);
        }
    }

    public void connectDevice(GameObject device)
    {
        input = device;
        if (device.CompareTag("GuitarPlug") && output)
        {
            Debug.Log("Guitar going to pedal");
            if (output.CompareTag("MixerPlug"))
            {
                mixer.audioMixer.SetFloat("distortion", transform.GetChild(0).GetComponent<Dial>().percentage);
                input.transform.root.GetComponent<Guitar>().attachMixer();
            }
        }
        if (device.CompareTag("MixerPlug") && input)
        {
            Debug.Log("Amp going to pedal");
            if (input.CompareTag("GuitarPlug"))
            {
                mixer.audioMixer.SetFloat("distortion", transform.GetChild(0).GetComponent<Dial>().percentage);
                input.transform.root.GetComponent<Guitar>().attachMixer();
            }
        }
    }

    public void disconnectDevice(bool isInput)
    {
        mixer.audioMixer.SetFloat("distortion", 0.0f);
        if (isInput)
        {
            input.transform.root.GetComponent<Guitar>().disconnectDevice();
            input = null;
        }
        else
        {
            input.transform.root.GetComponent<AmpMixer>().disconnectDevice();
            output = null;
        }
    }
}
