using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.CustomEvents;
using CrazyMinnow.SALSA;
using System;
using System.Net;
using System.IO;
using Mp3Sharp;

public class FuseCharacterController : MonoBehaviour {

    public GameObject targetTracking;

    public Salsa3D salsa;
    public RandomEyes3D randomEyes;

    // Use this for initialization
    IEnumerator Start()
    {
        salsa = gameObject.GetComponent<Salsa3D>();
        randomEyes = gameObject.GetComponent<RandomEyes3D>();
        yield return new WaitForEndOfFrame();
        randomEyes.lookTarget = targetTracking;
        gameObject.GetComponent<Animator>().speed = 0.25f;
    }
}
