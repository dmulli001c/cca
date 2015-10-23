/// <author>
/// engkfke
/// </author>
/// <copyright>
/// Copyright (c)2015, TopCoder, Inc. All rights reserved
/// </copyright>
/// <version>1.0.0</version>
/// <summary>
/// Sample test Script, to test adding two numbers
/// </summary>
using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using UnityEngine.CustomEvents;

/// <summary>
/// Sample test Script, to test adding two numbers
/// </summary>
class SampleAddingScript : MonoBehaviour
{
    /// <summary>
    /// Slow add function.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="z">The z.</param>
    /// <returns></returns>
    public int Add(int x,int y)
    {
        int result = 0;

        Debug.Log("add : Start adding "+x+"+"+y);

        // just sleep one second to make it like a real function
        Thread.Sleep(1000);

        result = x + y;

        ResultEventArgs args = new ResultEventArgs();

        args.result = result;

        EventManager.Trigger("OnCompleteExecution", args);

        return result;
    }
}