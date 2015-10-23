/// <copyright>
/// Copyright (c)2015, TopCoder, Inc. All rights reserved
/// </copyright>
/// <version>1.0.0</version>
/// <author>engkfke</author> 
/// <summary>
/// The following class will make any class that inherits from it a singleton automatically.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// The following class will make any class that inherits from it a singleton automatically.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T tInstance;

    /// <summary>
    /// Returns the instance of this singleton.
    /// </summary>
    public static T instance
    {
        get
        {
            if (tInstance == null)
            {
                tInstance = (T)FindObjectOfType(typeof(T));

                if (tInstance == null)
                {
                    GameObject instantiationManagerGameObject = new GameObject("InstantiationManager");

                    //Makes the object not be destroyed automatically when loading a new scene.
                    DontDestroyOnLoad(instantiationManagerGameObject);

                    tInstance = instantiationManagerGameObject.AddComponent(typeof(T)) as T;
                }
            }

            return tInstance;
        }
    }
}

