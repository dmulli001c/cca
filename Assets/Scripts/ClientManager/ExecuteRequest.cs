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
/// container for the execute request, This should fire off a function.
/// </summary>
public class ExecuteRequest : IRequest
{
    public String objectName;
    public String objectType;
    public String functionName;
    public object[] functionParameters;

    public ExecuteRequest()
    {
        objectName = "";
        objectType = "";
        functionName = "";
        functionParameters = null;
    }

    public ExecuteRequest(String objectName, String objectType, String functionName, params object[] parameters)
    {
        this.objectName = objectName;
        this.objectType = objectType;
        this.functionName = functionName;
        this.functionParameters = parameters;
    }
    IResponse IRequest.Process()
    {
        Debug.Log("Process ExecuteRequest.");
        ExecutionManager.executeAsync(objectName, Type.GetType(objectType), functionName, functionParameters);
        return null;
    }
}