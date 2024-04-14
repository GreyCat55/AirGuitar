using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Oculus.Interaction.HandGrab;

public class Guitar : MonoBehaviour
{

    public GameObject E_String;
    public GameObject A_String;
    public GameObject D_String;
    public GameObject G_String;
    [SerializeField] private AudioMixerGroup mixer;

    public GameObject connectedDevice;

    void Awake()
    {
        detachMixer();
        gameObject.GetComponent<HandGrabInteractable>().Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void connectDevice(GameObject device)
    {
        if (device)
        {
            connectedDevice = device;
            Debug.Log("Attaching " + device.name);
            if (device.CompareTag("MixerPlug"))
            {
                mixer.audioMixer.SetFloat("distortion", 0.0f);
                attachMixer();
                Debug.Log("Applying Mixer " + mixer.name);
            }
            if (device.CompareTag("PedalPlug"))
            {
                Debug.Log("Checking for amp now...");
                if (device.transform.root.GetComponent<Pedal>().output) {
                    device.transform.root.GetComponent<Pedal>().connectDevice(gameObject);
                    attachMixer();
                    Debug.Log("Mixer fully attached");
                }

            }
        }
    }

    public void disconnectDevice()
    {
        connectedDevice = null;
        detachMixer();
        Debug.Log("No Mixer Applied");
    }

    public void attachMixer()
    {
        E_String.GetComponentInChildren<AudioSource>().outputAudioMixerGroup = mixer;
        A_String.GetComponentInChildren<AudioSource>().outputAudioMixerGroup = mixer;
        D_String.GetComponentInChildren<AudioSource>().outputAudioMixerGroup = mixer;
        G_String.GetComponentInChildren<AudioSource>().outputAudioMixerGroup = mixer;
    }

    public void detachMixer()
    {
        E_String.GetComponentInChildren<AudioSource>().outputAudioMixerGroup = null;
        A_String.GetComponentInChildren<AudioSource>().outputAudioMixerGroup = null;
        D_String.GetComponentInChildren<AudioSource>().outputAudioMixerGroup = null;
        G_String.GetComponentInChildren<AudioSource>().outputAudioMixerGroup = null;
    }
}
