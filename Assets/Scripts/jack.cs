using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jack : MonoBehaviour
{

    private GameObject ioDevice;
    private Vector3 coordinatePositionOffset;
    private Vector3 coordinateAngleOffset;
    private bool grabbed = false;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SnapToPlug();
    }
    void SnapToPlug()
    {
        if (ioDevice)
        {
            transform.position = ioDevice.transform.position + coordinatePositionOffset;
            transform.eulerAngles = ioDevice.transform.parent.parent.eulerAngles + coordinateAngleOffset;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Plug") && !ioDevice)
        {
            ioDevice = other.gameObject;
            Debug.Log(gameObject.name + " plugged into " + other.transform.root.name);
        }

        if (other.CompareTag("MixerPlug"))
        {
            coordinatePositionOffset = new Vector3(0.0f, 0.025f, 0.0f);
            coordinateAngleOffset = new Vector3(90.0f, 00.0f, 0.0f);
            if (getOppositeCableConnection())
            {
                if (getOppositeCableConnection().transform.root.GetComponent<Guitar>())
                {
                    getOppositeCableConnection().transform.root.GetComponent<Guitar>().connectDevice(ioDevice);
                }
                else if (getOppositeCableConnection().transform.root.GetComponent<Pedal>())
                {
                    getOppositeCableConnection().transform.root.GetComponent<Pedal>().connectDevice(ioDevice);
                }
            }
        }

        if (other.CompareTag("GuitarPlug"))
        {
            coordinatePositionOffset = new Vector3(0.01f, -0.00025f, 0.00f);
            coordinateAngleOffset = new Vector3(0.0f, 15.0f, -90.0f);
            if (getOppositeCableConnection())
            {
                other.transform.root.GetComponent<Guitar>().connectDevice(getOppositeCableConnection());
                if (getOppositeCableConnection().transform.parent.parent.GetComponent<AmpMixer>())
                {
                    getOppositeCableConnection().transform.parent.parent.GetComponent<AmpMixer>().connectDevice(ioDevice);
                }
                else if (getOppositeCableConnection().transform.root.GetComponent<Pedal>())
                {
                    getOppositeCableConnection().transform.root.GetComponent<Pedal>().connectDevice(ioDevice);
                }
            }
        }
        if (other.CompareTag("PedalPlug"))
        {
            if (other.transform.parent.name.Equals("Output"))
            {
                coordinatePositionOffset = new Vector3(0.01f, -0.00025f, 0.00f);
                coordinateAngleOffset = new Vector3(90.0f, 0.0f, -90.0f);
            }
            else
            {
                coordinatePositionOffset = new Vector3(-0.01f, -0.00025f, 0.00f);
                coordinateAngleOffset = new Vector3(90.0f, 0.0f, 90.0f);
            }

            if (getOppositeCableConnection())
            {
                Debug.Log("This is where the headache happens..." + getOppositeCableConnection().transform.parent.parent.name);
                if (getOppositeCableConnection().transform.parent.parent.GetComponent<AmpMixer>())
                {
                    getOppositeCableConnection().transform.parent.parent.GetComponent<AmpMixer>().connectDevice(ioDevice);
                }
                else if (getOppositeCableConnection().transform.root.GetComponent<Guitar>())
                {
                    getOppositeCableConnection().transform.root.GetComponent<Guitar>().connectDevice(ioDevice);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Plug") && ioDevice && grabbed)
        {
            ioDevice = null;
            Debug.Log(gameObject.name + " unplugged from " + other.transform.root.name);
            
            if (other.transform.root.GetComponent<Guitar>())
            {
                other.transform.root.GetComponent<Guitar>().disconnectDevice();
            }
            if (other.transform.parent.parent.GetComponent<AmpMixer>())
            {
                other.transform.parent.parent.GetComponent<AmpMixer>().disconnectDevice();
            }
        }
    }

    private void detectGrabState()
    {
        grabbed = !grabbed;
    }

    public GameObject getIODevice()
    {
        return ioDevice;
    }

    private GameObject getOppositeCableConnection()
    {
        if (gameObject.name.Equals("JackNorth"))
        {
            return transform.root.GetComponent<AmpCable>().southIO;
        }
        else
        {
            return transform.root.GetComponent<AmpCable>().northIO;
        }
    }
}
