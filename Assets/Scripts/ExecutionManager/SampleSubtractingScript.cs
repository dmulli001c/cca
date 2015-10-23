/// <copyright>
/// Copyright (c)2015, TopCoder, Inc. All rights reserved
/// </copyright>
/// <version>1.0.0</version>
/// <author>engkfke</author> 
/// <summary>
/// Sample test Script, to test Subtracting two numbers
/// </summary>
using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using UnityEngine.CustomEvents;

/// <summary>
/// Sample test Script, to test Subtracting two numbers
/// </summary>
class SampleSubtractingScript : MonoBehaviour
{
    /// <summary>
    /// Subtract function.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <returns></returns>
    public int Subtract(int x, int y)
    {
        int result = 0;

        Debug.Log("Subtract : Start Subtracting " + x + "-" + y);

        // just sleep one second to make it like a real function
        Thread.Sleep(1000);

        result = x - y;

        ResultEventArgs args = new ResultEventArgs();

        args.result = result;

        EventManager.Trigger("OnCompleteExecution", args);

        return result;
    }
}
