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
/// container for the GetCatalog request, gets everything in the scene..
/// </summary>
public class GetCatalogRequest : IRequest
{
    /// <summary>
    /// Processes this instance.
    /// </summary>
    /// <returns></returns>
    IResponse IRequest.Process()
    {
        Debug.Log("Process GetCatalogRequest."); 
        JsonSerializeScript jsonSerializeScript = new JsonSerializeScript();
        String jsonString = jsonSerializeScript.getCatalog();
        return new GetCatalogResponse(jsonString);
    }
}

/// <summary>
/// container for the GetCatalog response
/// </summary>
public class GetCatalogResponse : IResponse
{
    /// <summary>
    /// The json string
    /// </summary>
    public String jsonString;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCatalogResponse"/> class.
    /// </summary>
    /// <param name="jsonString">The json string.</param>
    public GetCatalogResponse(string jsonString)
    {
        this.jsonString = jsonString;
    }
}