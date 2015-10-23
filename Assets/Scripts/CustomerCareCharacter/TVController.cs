using UnityEngine;
using System.Collections;

public class TVController : MonoBehaviour {

    public GameObject speaker1;
    public GameObject speaker2;
    public GameObject tv;
    public float scale;

    public GameObject spotLight;
    public GameObject speakerLight;
    bool IsDimming;

    public GameObject overheadLight;

    public bool IsScaling = false;

    public Animator anim;

    // Use this for initialization
    void Start () {
        StartCoroutine(Flicker());
        iTween.PunchRotation(overheadLight, iTween.Hash("y", 0.5f, "x", .7f, "z", 0.5f, "easeType", "linear", "loopType", "pingPong", "time", 10f));
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDimming && spotLight.GetComponent<Light>().intensity > 0.1)
        {
            spotLight.GetComponent<Light>().intensity = spotLight.GetComponent<Light>().intensity - 1f;
        }
        else if (!IsDimming && spotLight.GetComponent<Light>().intensity < 3)
        {
            spotLight.GetComponent<Light>().intensity = spotLight.GetComponent<Light>().intensity + 1f;

        }

        if (IsDimming && speakerLight.GetComponent<Light>().intensity > 0.1)
        {
            speakerLight.GetComponent<Light>().intensity = speakerLight.GetComponent<Light>().intensity - 0.25f;
        }
        else if (!IsDimming && speakerLight.GetComponent<Light>().intensity < 1)
        {
            speakerLight.GetComponent<Light>().intensity = speakerLight.GetComponent<Light>().intensity + 0.25f;
        }
    }


    public void SpeakerScale()
    {
        iTween.PunchRotation(speaker1, iTween.Hash("time", 0.75f, "y", scale * 3, "z", scale * 3));
        iTween.PunchRotation(speaker2, iTween.Hash("time", 0.75f, "y", scale * 3, "z", scale * 3));
    }

    IEnumerator Flicker()
    {
        IsDimming = true;
        speaker1.SetActive(false);
        speaker2.SetActive(false);
        tv.SetActive(false);
        overheadLight.SetActive(false);
        yield return new WaitForSeconds(1f);
        IsDimming = false;
        speaker1.SetActive(true);
        speaker2.SetActive(true);
        tv.SetActive(true);
        overheadLight.SetActive(true);
    }
}
