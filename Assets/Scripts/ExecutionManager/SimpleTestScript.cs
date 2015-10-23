/// <copyright>
/// Copyright (c)2015, TopCoder, Inc. All rights reserved
/// </copyright>
/// <version>1.0.0</version>
/// <author>engkfke</author> 
/// <summary>
/// A test script that tests the ExecutionManager script.
/// </summary>
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using fastJSON;
using UnityEngine.CustomEvents;

/// <summary>
/// A simple test script that tests the ExecutionManager script.
/// </summary>
public class SimpleTestScript : MonoBehaviour
{
    public GameObject AddingGameObject;

    public GameObject SubtractingGameObject;

    /// <summary>
    /// Tests Subtracting.
    /// </summary>
    void testSubtracting()
    {
        try
        {
            Debug.Log("Start Subtracting 15 +3");

            ExecutionManager.executeAsync(SubtractingGameObject, typeof(SampleSubtractingScript), "Subtract", 15, 3);

            Debug.Log("End Subtracting");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    /// <summary>
    /// Tests Adding.
    /// </summary>
    void testAdding()
    {
        try
        {
            Debug.Log("Start Adding 1+2");

            ExecutionManager.executeAsync(AddingGameObject, typeof(SampleAddingScript), "Add", 1, 2);

            Debug.Log("End Adding");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    void Start()
    {
        EventManager.register("OnCompleteExecution", OnCompleteExecution);
        testSubtracting();
        testAdding();
    }

    public void OnCompleteExecution(CustomEventArgs eventArgs)
    {
        ResultEventArgs resultEventArgs = eventArgs as ResultEventArgs;
        Debug.Log("OnCompleteExecution : result " + resultEventArgs.result);
    }
}
