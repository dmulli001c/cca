/// <copyright>
/// Copyright (c)2015, TopCoder, Inc. All rights reserved
/// </copyright>
/// <version>1.0.0</version>
/// <author>engkfke</author> 
/// <summary>
///  ExecutionManager takes an existing Object and function name as argument, executes the function on the specific object asynchronously. 
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Reflection;
using System.Linq;

/// <summary>
/// ExecutionManager takes an existing Object and function name as argument,
/// executes the function on the specific object asynchronously. 
/// </summary>
public class ExecutionManager : MonoBehaviour
{
    /// <summary>
    /// An delegate to the Invoke method.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns></returns>
    public delegate object InvokeDelegate(object obj, object[] parameters);

    /// <summary>
    /// The execution manager
    /// </summary>
    private static ExecutionManager executionManager = null;

    /// <summary>
    /// container for the execute request arguments.
    /// </summary>
    class ExecutionItem
    {
        public UnityEngine.Object existingObject;
        public String functionName;
        public object[] parameters;

        /// <summary>
        /// Initializes a new instance of the ExecutionItem class.
        /// </summary>
        /// <param name="existingObject">The existing object.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="parameters">An array of objects to pass as parameters to the given method. This can be null if no arguments are needed.</param>
        public ExecutionItem(UnityEngine.Object existingObject, String functionName, object[] parameters)
        {
            this.functionName = functionName;
            this.existingObject = existingObject;
            this.parameters = parameters;
        }
    }

    /// <summary>
    /// The limit queue processing
    /// Every update cycle the queue is processed, if the queue processing is limited,
    /// a maximum processing time per update can be set after which the Execute requests will have to be processed next update loop.
    /// </summary>
    public bool limitQueueProcessing = false;

    /// <summary>
    /// The queue process time
    /// </summary>
    public float queueProcessTime = 0.0f;

    /// <summary>
    /// The execution items queue
    /// </summary>
    private Queue executionItemsQueue = new Queue();

    /// <summary>
    /// The public reference to this singleton, which creates a singleton instance if none exists.
    /// </summary>
    /// <value>
    /// The instance.
    /// </value>
    public static ExecutionManager instance
    {
        get
        {
            if (executionManager == null)
            {
                executionManager = (ExecutionManager)(new GameObject("ExecutionManager")).AddComponent(typeof(ExecutionManager));
            }
            return executionManager;
        }
    }

    /// <summary>
    /// Finds the method Info.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Method  + functionName +  not found</exception>
    static MethodInfo FindMethod(Type type, string functionName, object[] parameters)
    {
        // the Method may overloaded. As far as I know there's no other way 
        // to get the MethodInfo instance, we have to
        // search for it in all the type methods
        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

        foreach (MethodInfo method in methods)
        {
            if (method.Name == functionName)
            {
                // create the generic method
                ParameterInfo[] methodParameters = method.GetParameters();

                // compare the method parameters
                if (parameters.Length == methodParameters.Length)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (parameters[i].GetType() != methodParameters[i].ParameterType)
                        {
                            continue; // this is not the method we're looking for
                        }
                    }

                    // if we're here, we got the right method
                    return method;
                }
            }
        }
        Debug.Log("Method " + functionName + " not found");
        throw new InvalidOperationException("Method " + functionName + " not found");
    }

    /// <summary>
    /// Executes the functionName on the specified existing object.
    /// </summary>
    /// <param name="existingObject">The existing object.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <param name="parameters">An array of objects to pass as parameters to the given method. This can be null if no arguments are needed.</param>
    /// <exception cref="ArgumentNullException">
    /// existingObject is null.
    /// or
    /// the functionName is null or empty.
    /// </exception>
    static void execute(UnityEngine.Object existingObject, String functionName, params object[] parameters)
    {
        MethodInfo method = null;

        if (existingObject == null)
        {
            Debug.Log("existingObject is null.");
            throw new ArgumentNullException("existingObject is null.");
        }
        method = FindMethod(existingObject.GetType(), functionName, parameters);

        // Executes the specified function synchronously with the specified parameters, on the specified existingObject.
        method.Invoke(existingObject, parameters);
    }

    /// <summary>
    /// Executes the functionName on the specified existing GameObject asynchronously.
    /// </summary>
    /// <param name="existingObject">The existing object.</param>
    /// <param name="type">The type.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <param name="parameters">An array of objects to pass as parameters to the given method. This can be null if no arguments are needed.</param>
    /// <exception cref="ArgumentNullException">
    /// existingObject is null.
    /// or
    /// type is null.
    /// or
    /// the functionName is null or empty.
    /// or
    /// There is no Component with type: + type.ToString() + attached to the existingObject.
    /// </exception>
    public static void executeAsync(UnityEngine.GameObject existingObject, Type type, String functionName, params object[] parameters)
    {
        if (existingObject == null)
        {
            Debug.Log("existingObject is null.");
            throw new ArgumentNullException("existingObject is null.");
        }

        if (type == null)
        {
            Debug.Log("type is null.");
            throw new ArgumentNullException("type is null.");
        }

        if (String.IsNullOrEmpty(functionName))
        {
            Debug.Log("the functionName is null or empty.");
            throw new ArgumentNullException("the functionName is null or empty.");
        }
        Component[] Components = existingObject.GetComponents(type);

        if (Components == null || Components.Length == 0)
        {
            Debug.Log("There is no Component with type:" + type.ToString() + " attached to the existingObject.");
            throw new ArgumentNullException("There is no Component with type:" + type.ToString() + "attached to the existingObject.");
        }

        // we can have more than one script with same type attached to the GameObject (e.g. weapons the character may have two weapons).
        foreach (MonoBehaviour obj in Components)
        {
            ExecutionItem executionItem = new ExecutionItem(obj, functionName, parameters);
            instance.executionItemsQueue.Enqueue(executionItem);
        }
    }

    /// <summary>
    /// Executes the functionName on the specified existing GameObject asynchronously.
    /// </summary>
    /// <param name="existingObjectName">Name of the existing object.</param>
    /// <param name="type">The type.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <param name="parameters">An array of objects to pass as parameters to the given method. This can be null if no arguments are needed.</param>
    /// <exception cref="ArgumentNullException">existingObject is null.
    /// or
    /// type is null.
    /// or
    /// the functionName is null or empty.
    /// or
    /// There is no Component with type: + type.ToString() + attached to the existingObject.</exception>
    public static void executeAsync(String existingObjectName, Type type, String functionName, params object[] parameters)
    {
        GameObject existingObject = GameObject.Find(existingObjectName);

        if (existingObject == null)
        {
            Debug.LogError("The gameObject does not exists.");
            throw new ArgumentNullException("The gameObject does not exists.");
        }
        executeAsync(existingObject, type, functionName, parameters);
    }

    /// <summary>
    /// Every update cycle the queue is processed, if the queue processing is limited,
    /// a maximum processing time per update can be set after which the Execute requests will have
    /// to be processed next update loop.
    /// An Execution manager centric game will have a lot of Execute requests controlling every aspect of the game. 
    /// So being able to queue Execute requests for the next frame (using TriggerAsync) will ensure not too many Execute requests will fire at once.
    /// It will prevent the game from advancing forward too quickly (Execution trigger chains) and will also help with the frame rate.
    /// </summary>
    void Update()
    {
        float timer = 0.0f;

        while (instance.executionItemsQueue.Count > 0)
        {
            // Every update cycle the queue is processed, if the queue processing is limited,
            // a maximum processing time per update can be set after which the Execute requests will have
            // to be processed next update loop.
            if (limitQueueProcessing)
            {
                if (timer > queueProcessTime)
                    return;
            }

            ExecutionItem executionItem = instance.executionItemsQueue.Dequeue() as ExecutionItem;
            execute(executionItem.existingObject, executionItem.functionName, executionItem.parameters);

            if (limitQueueProcessing)
                timer += Time.deltaTime;
        }
    }
}
