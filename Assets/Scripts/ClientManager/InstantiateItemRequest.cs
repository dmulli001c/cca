/// <copyright>
/// Copyright (c)2015, TopCoder, Inc. All rights reserved
/// </copyright>
/// <version>1.0.0</version>
/// <author>engkfke</author> 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Reflection;
using System.Linq;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using UnityEngine.UI;
using UnityEngine.CustomEvents;

/// <summary>
/// container for the Instantiate Item request, create a specific object.
/// </summary>
public class InstantiateItemRequest : IRequest
{
    /// <summary>
    /// The object name
    /// </summary>
    public String objectName;

    /// <summary>
    /// The scene game object
    /// </summary>
    public String sceneGameObject;

    public InstantiateItemRequest()
    {
        this.objectName = "";
        this.sceneGameObject = "";
    }

    public InstantiateItemRequest(String objectName, String sceneGameObject)
    {
        this.objectName = objectName;
        this.sceneGameObject = sceneGameObject;
    }
    /// <summary>
    /// Processes this instance.
    /// </summary>
    /// <returns></returns>
    IResponse IRequest.Process()
    {
        Debug.Log("Process InstantiateItemRequest.");
        InstantiationManager.instantiatePrefabs(objectName, sceneGameObject);
        return null;
    }
}