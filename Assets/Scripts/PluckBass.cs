using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class PluckBass : MonoBehaviour
{
    public GameObject E_String;
    public GameObject A_String;
    public GameObject D_String;
    public GameObject G_String;

    private Accelerometer accelerometer;

    public GameObject pluckPose;
    private bool plucking = false;

    static Dictionary<string,float> openPitches = new Dictionary<string, float>();
    static Dictionary<string, GameObject> notesFret = new Dictionary<string, GameObject>();

    private enum SemitonesBetweenStrings{ E_String = 0, A_String = 1, D_String = 2, G_String = 3};

    private bool fretting = false;

    static bool tuned;
    public static bool realisticFretting = true;


    void Awake()
    {

        accelerometer = gameObject.GetComponent<Accelerometer>();

        pluckPose = GameObject.Find("PluckPoseRight");
        Debug.Log("Checking Poses: " + pluckPose.GetComponent<SelectorUnityEventWrapper>().isActiveAndEnabled);

        //Lowers the E string tuning down a half step (Eb)
        //E_String.GetComponentInChildren<AudioSource>().pitch *= Mathf.Pow(1.05946f, -1);
        if (!tuned) { 
            A_String.GetComponentInChildren<AudioSource>().pitch *= Mathf.Pow(1.05946f, 5);
            D_String.GetComponentInChildren<AudioSource>().pitch *= Mathf.Pow(1.05946f, 10);
            G_String.GetComponentInChildren<AudioSource>().pitch *= Mathf.Pow(1.05946f, 15);

            openPitches.Add("E_String", E_String.GetComponentInChildren<AudioSource>().pitch);
            openPitches.Add("A_String", A_String.GetComponentInChildren<AudioSource>().pitch);
            openPitches.Add("D_String", D_String.GetComponentInChildren<AudioSource>().pitch);
            openPitches.Add("G_String", G_String.GetComponentInChildren<AudioSource>().pitch);

            notesFret.Add("E_String", null);
            notesFret.Add("A_String", null);
            notesFret.Add("D_String", null);
            notesFret.Add("G_String", null);

            tuned = true;
            Debug.Log("Bass is tuned");
            Debug.Log(openPitches.Keys.Count);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        E_String.GetComponent<MeshRenderer>().enabled = true;
        A_String.GetComponent<MeshRenderer>().enabled = true;
        D_String.GetComponent<MeshRenderer>().enabled = true;
        G_String.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("String") && plucking)
        {
            Debug.Log("You poked " + other.gameObject.name);
            other.gameObject.GetComponentInChildren<AudioSource>().volume = accelerometer.GetVelocity().magnitude * 500.0f;
            other.gameObject.GetComponentInChildren<AudioSource>().Play();
        }
        if (other.gameObject.CompareTag("Fret"))
        {
            if (realisticFretting) {
                Debug.Log("You fret " + System.Int32.Parse(other.gameObject.name.Substring(4)));
                FretNote(other.transform.parent.gameObject, System.Int32.Parse(other.gameObject.name.Substring(4)));
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                int fretNumber = System.Int32.Parse(other.gameObject.name.Substring(4));
                MeshRenderer fretIndicator = other.gameObject.GetComponent<MeshRenderer>();
                fretIndicator.enabled = !fretIndicator.enabled;
                Debug.Log("You fret " + other.transform.parent.gameObject.name + " at " + fretNumber);
                FretNote(other.transform.parent.gameObject, fretNumber);
                foreach (MeshRenderer m in other.transform.parent.GetComponentsInChildren<MeshRenderer>())
                {
                        m.enabled = false;
                }
                notesFret[other.transform.parent.gameObject.name] = other.gameObject;
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Fret") && realisticFretting)
        {
            StartCoroutine(OpenNote(other.transform));
        }
    }

    void FretNote(GameObject stringToFret, int fret)
    {
        if (realisticFretting) {
            if (!fretting)
            {
                fretting = true;
                stringToFret.GetComponentInChildren<AudioSource>().pitch *= Mathf.Pow(1.05946f, fret);
            }
        }
        else
        {
            stringToFret.GetComponentInChildren<AudioSource>().pitch = openPitches[stringToFret.gameObject.name.Substring(0, 8)];
            stringToFret.GetComponentInChildren<AudioSource>().pitch *= Mathf.Pow(1.05946f, fret);
        }
    }

    IEnumerator OpenNote(Transform fretToOpen)
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("You reopened " + fretToOpen.transform.parent.gameObject.name.Substring(0, 8));
        fretToOpen.transform.parent.gameObject.GetComponentInChildren<AudioSource>().pitch = openPitches[fretToOpen.transform.parent.gameObject.name.Substring(0, 8)];
        fretting = false;
        fretToOpen.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    void PluckingPose()
    {
        plucking = true;
    }

    void RestingPose()
    {
        plucking = false;
    }
}
