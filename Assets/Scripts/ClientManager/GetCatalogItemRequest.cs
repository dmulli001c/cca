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
/// container for the GetCatalog Item request, get a specific object in the scene.
/// </summary>
public class GetCatalogItemRequest : IRequest
{
    /// <summary>
    /// The object name
    /// </summary>
    public String objectName;

    public GetCatalogItemRequest()
    {
        objectName = "";
    }

    public GetCatalogItemRequest(string objectName)
    {
        this.objectName = objectName;
    }

    /// <summary>
    /// Processes this instance.
    /// </summary>
    /// <returns></returns>
    IResponse IRequest.Process()
    {
        Debug.Log("Process getCatalogItemRequest.");
        JsonSerializeScript jsonSerializeScript = new JsonSerializeScript();
        String jsonString = jsonSerializeScript.getCatalogItem(objectName);
        return new GetCatalogItemResponse(jsonString);
    }
}

/// <summary>
/// container for the GetCatalog Item response
/// </summary>
public class GetCatalogItemResponse : IResponse
{
    /// <summary>
    /// The json string
    /// </summary>
    public String jsonString;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCatalogItemResponse"/> class.
    /// </summary>
    /// <param name="jsonString">The json string.</param>
    public GetCatalogItemResponse(string jsonString)
    {
        this.jsonString = jsonString;
    }
}