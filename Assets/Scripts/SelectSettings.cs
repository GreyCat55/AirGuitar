using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class SelectSettings : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] string setting;
    private bool state = false;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PenTip")){
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
            state = !state;
            gameObject.SendMessage(setting);
        }
    }

    private void makeGuitarGrabbable()
    {
        if (state)
        {
            target.GetComponent<HandGrabInteractable>().Enable();
        }
        else
        {
            target.GetComponent<HandGrabInteractable>().Disable();
        }
    }

    private void tapToFretStrings()
    {
        PluckBass.realisticFretting = !PluckBass.realisticFretting;
    }

    private void showIndicators()
    {

    }
}
