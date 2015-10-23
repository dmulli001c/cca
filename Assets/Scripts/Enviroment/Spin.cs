using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
			//JGC Rotate for demo 
			transform.Rotate(0,260*Time.deltaTime,0);
	}
	
	
}
