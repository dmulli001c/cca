using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {

    public GameObject overheadLight;
    public GameObject speakerLight;
    bool IsDimming;

	// Use this for initialization
	void Start () {
        StartCoroutine(Flicker());
	}
	
	// Update is called once per frame
	void Update () {
	    if(IsDimming && overheadLight.GetComponent<Light>().intensity > 0.1)
        {
            overheadLight.GetComponent<Light>().intensity = overheadLight.GetComponent<Light>().intensity - 1f;
        }
        else if (!IsDimming && overheadLight.GetComponent<Light>().intensity < 3)
        {
            overheadLight.GetComponent<Light>().intensity = overheadLight.GetComponent<Light>().intensity + 1f;

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

    IEnumerator Flicker()
    {
        IsDimming = true;
        yield return new WaitForSeconds(1f);
        IsDimming = false;
        yield return new WaitForSeconds(0.1f);

        IsDimming = true;
        yield return new WaitForSeconds(.25f);
        IsDimming = false;
    }
}
