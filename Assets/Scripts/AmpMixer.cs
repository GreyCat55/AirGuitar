using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AmpMixer : MonoBehaviour
{

    public bool powerOn = false;
    public GameObject[] mixerGadgets;
    
    public AudioMixer ampMixer;
    private float gain;
    private float centerFreq;

    public GameObject connectedDevice;

    void Awake()
    {
        /*
         Notes for audio mixer stuff:
        - Bass encapsulates low frequencies, treble = high, middle = self-explanatory
        - Increase bass = lower center frequency
        - Increase Treble = raise center frequency
        - Increase Mid = bring center frequency closer to 600Hz, the middle of the supported equalize range
         */
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.GetComponent<AudioSource>().volume = mixerGadgets[2].GetComponent<Dial>().percentage / 10.0f;

        if (powerOn)
        {
            ampMixer.SetFloat("volume", (mixerGadgets[2].GetComponent<Dial>().percentage * 100.0f) - 100.0f);
        }
        else
        {
            ampMixer.SetFloat("volume", -80.0f);
        }

        //adjust equalizer values
        gain = 0.5f + mixerGadgets[3].GetComponent<Dial>().percentage + mixerGadgets[4].GetComponent<Dial>().percentage + mixerGadgets[5].GetComponent<Dial>().percentage;

        //precalculate effects of bass and treble
        centerFreq = 500.0f - (mixerGadgets[3].GetComponent<Dial>().percentage * 375.0f) + (mixerGadgets[5].GetComponent<Dial>().percentage * 375.0f);

        //factor in middle frequencies
        if (centerFreq > 600.0f)
        {
            centerFreq -= (mixerGadgets[4].GetComponent<Dial>().percentage * 375.0f);
        }
        else
        {
            centerFreq += (mixerGadgets[4].GetComponent<Dial>().percentage * 375.0f);
        }

        //set exposed equalizer parameters
        ampMixer.SetFloat("gain", gain);
        ampMixer.SetFloat("center", centerFreq);
    }

    void TogglePower()
    {
        //set ON/OFF state
        powerOn = !powerOn;

        //CLICK!
        mixerGadgets[0].GetComponent<AudioSource>().Play();

        //Turn led on
        mixerGadgets[1].SetActive(!mixerGadgets[1].activeSelf);

        //Toggles the amp static
        if (transform.parent.GetComponent<AudioSource>().isPlaying)
        {
            transform.parent.GetComponent<AudioSource>().Stop();
        }
        else
        {
            transform.parent.GetComponent<AudioSource>().Play();
        }

        if (powerOn)
        {
            mixerGadgets[0].transform.localEulerAngles = new Vector3(10.0f, 0.0f, 0.0f);
            ampMixer.SetFloat("volume", (mixerGadgets[2].GetComponent<Dial>().percentage * 100.0f) - 100.0f);
        }
        else
        {
            mixerGadgets[0].transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            ampMixer.SetFloat("volume", -80.0f);
        }
        
    }

    public void connectDevice(GameObject device)
    {
        if (device)
        {
            connectedDevice = device;
            Debug.Log("Attaching " + device.transform.root.name);

            if (device.CompareTag("PedalPlug"))
            {
                device.transform.root.GetComponent<Pedal>().output = gameObject;
                if (device.transform.root.GetComponent<Pedal>().input)
                {
                    Debug.Log("Distortion successfully applied to guitar");
                    device.transform.root.GetComponent<Pedal>().connectDevice(transform.GetChild(0).GetChild(0).gameObject);
                    device.transform.root.GetComponent<Pedal>().input.transform.root.GetComponent<Guitar>().attachMixer();
                }
            }

            if (device.CompareTag("GuitarPlug"))
            {
                Debug.Log("Amp directly plugged into guitar");
                device.transform.root.GetComponent<Guitar>().connectDevice(transform.GetChild(0).GetChild(0).gameObject);
            }
        }
    }

    public void disconnectDevice()
    {
        if (connectedDevice)
        {

        
        if (connectedDevice.CompareTag("GuitarPlug"))
        {
            connectedDevice.transform.root.GetComponent<Guitar>().disconnectDevice();
        }
        if (connectedDevice.CompareTag("PedalPlug"))
        {
            if (connectedDevice.name.Equals("Input"))
            {
                connectedDevice.transform.root.GetComponent<Pedal>().disconnectDevice(true);
            }
            if (connectedDevice.name.Equals("Output"))
            {
                connectedDevice.transform.root.GetComponent<Pedal>().disconnectDevice(false);
            }
        }
        }
    }
}
