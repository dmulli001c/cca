using UnityEngine;
using System.Collections;

public class OpenURL : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Application.OpenURL(Application.persistentDataPath);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
