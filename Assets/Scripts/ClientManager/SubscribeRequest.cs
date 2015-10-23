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
/// container for the Subscribe request, This should add an event to listen for and send the events out to the network as JSON.
/// </summary>
public class SubscribeRequest : IRequest
{
    public String eventName;

    public SubscribeRequest()
    {
        eventName = "";
    }

    public SubscribeRequest(String eventName)
    {
        this.eventName = eventName;
    }

    IResponse IRequest.Process()
    {
        Debug.Log("Process SubscribeRequest.");
        EventManager.register(eventName, ClientManager.instance.OnEvents);
        return null;
    }
}

/// <summary>
/// container for the Subscribe response.
/// </summary>
public class SubscribeResponse : IResponse
{
    public String eventName;
    public CustomEventArgs eventArgs;

    public SubscribeResponse(CustomEventArgs eventArgs)
    {
        eventName = eventArgs.eventName;
        this.eventArgs = eventArgs;
    }
}