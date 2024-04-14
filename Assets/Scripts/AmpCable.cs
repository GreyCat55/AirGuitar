using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpCable : MonoBehaviour
{
    public Transform NorthJack;
    public Transform SouthJack;

    public GameObject northIO;
    public GameObject southIO;

    // Start is called before the first frame update
    void Awake()
    {
        northIO = NorthJack.GetComponent<jack>().getIODevice();
        southIO = SouthJack.GetComponent<jack>().getIODevice();
    }

    // Update is called once per frame
    void Update()
    {
        northIO = NorthJack.GetComponent<jack>().getIODevice();
        southIO = SouthJack.GetComponent<jack>().getIODevice();
    }
}
