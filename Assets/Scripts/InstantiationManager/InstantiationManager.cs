/// <copyright>
/// Copyright (c)2015, TopCoder, Inc. All rights reserved
/// </copyright>
/// <version>1.0.0</version>
/// <author>engkfke</author> 
/// <summary>
/// A C# script that takes an Object's name, type and an existing game object or scene object's name as argument,
/// the C# script instantiates new object and then attaches it to the specified existing game object/scene object.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Reflection;
using System.Linq;

/// <summary>
/// An instantiation manager is generally a singleton that takes an Object's name/type and an existing game
/// object or scene object's name as argument. the instantiation manager instantiates new object and then attaches it to
/// the specified existing game object/scene object.
/// </summary>
public class InstantiationManager : Singleton<InstantiationManager>
{
    /// <summary>
    /// The assetReferences Dictionary is used to cash the asset references to speed up the  instantiation
    /// because the Resources.LoadAll or Resources.Load functions are slow and we can't use it many times.
    /// </summary>
    Dictionary<Type, Dictionary<string, List<UnityEngine.Object>>> assetReferences = new Dictionary<Type, Dictionary<string, List<UnityEngine.Object>>>();

    /// <summary>
    /// Gets the unity object reference by its name and type, first it checks the cashed objects 
    /// for the requested object's type if the object there it will return it.
    /// if it's not there, it will load all objects in the Resources folder which have the same object's type, 
    /// then cash them in the assetReferences Dictionary for improving performance, because the Resources.LoadAll or
    /// Resources.Load functions are slow and we can't use it many times.
    /// </summary>
    /// <typeparam name="TYPE">The type of the type.</typeparam>
    /// <param name="objectName">Name of the object.</param>
    /// <returns>The object reference</returns>
    /// <exception cref="ArgumentNullException">object's name is null or empty.</exception>
    /// <exception cref="Exception">object reference is not found.</exception>
    TYPE GetUnityObjectReference<TYPE>(String objectName) where TYPE : UnityEngine.Object
    {
        Dictionary<string, List<UnityEngine.Object>> nameLookup = null;
        List<UnityEngine.Object> references = null;

        if (String.IsNullOrEmpty(objectName))
        {
            Debug.LogWarning("object's name is null or empty.");
            throw new ArgumentNullException("object's name is null or empty.");
        }

        //first it checks the cashed objects for the requested object's type
        if (!assetReferences.TryGetValue(typeof(TYPE), out nameLookup))
        {
            // if it's not there, it will load all objects in the Resources folder which have the same object's type, 
            // then cash them in the assetReferences Dictionary for improving performance, because the Resources.LoadAll or
            // Resources.Load functions are slow and we can't use it many times.
            nameLookup = new Dictionary<string, List<UnityEngine.Object>>();

            var objectsOfType = Resources.LoadAll<TYPE>("");

            foreach (var reference in objectsOfType)
            {
                if (!nameLookup.ContainsKey(reference.name))
                    nameLookup[reference.name] = new List<UnityEngine.Object>();

                nameLookup[reference.name].Add(reference);
            }
            assetReferences[typeof(TYPE)] = nameLookup;
        }

        if (!nameLookup.TryGetValue(objectName, out references))
        {
            Debug.LogWarning("object's name : " + objectName + ", type" + typeof(TYPE).Name + " is not found.");
            throw new Exception("object's name : " + objectName + ", type" + typeof(TYPE).Name + " is not found.");
        }

        if (references.Count > 1)
            Debug.LogWarning("There are more than one item with the same name : " + objectName + " with type" + typeof(TYPE).Name);

        return references[0] as TYPE;
    }

    /// <summary>
    /// Instantiates the specified object with name and type.
    /// </summary>
    /// <typeparam name="TYPE">The type of the type.</typeparam>
    /// <param name="objectName">Name of the object.</param>
    /// <returns>The instantiated object</returns>
    /// <exception cref="ArgumentNullException">object's name is null or empty.</exception>
    public TYPE instantiate<TYPE>(String objectName) where TYPE : UnityEngine.Object
    {
        if (String.IsNullOrEmpty(objectName))
        {
            Debug.LogWarning("object's name is null or empty.");
            throw new ArgumentNullException("object's name is null or empty.");
        }
        return Instantiate(instance.GetUnityObjectReference<TYPE>(objectName));
    }

    /// <summary>
    /// Instantiates the specified object with name and type.
    /// </summary>
    /// <typeparam name="TYPE">The type of the asset reference.</typeparam>
    /// <param name="objectName">Name of the object.</param>
    /// <param name="position">The position.</param>
    /// <param name="rotation">The rotation.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">object's name is null or empty.</exception>
    /// <exception cref="System.ArgumentNullException">objectName is null or empty.</exception>
    public TYPE instantiate<TYPE>(String objectName, Vector3 position, Quaternion rotation) where TYPE : UnityEngine.Object
    {
        if (String.IsNullOrEmpty(objectName))
        {
            Debug.LogWarning("object's name is null or empty.");
            throw new ArgumentNullException("object's name is null or empty.");
        }
        return Instantiate(instance.GetUnityObjectReference<TYPE>(objectName), position, rotation) as TYPE;
    }

    /// <summary>
    /// Instantiates the specified Prefabs object with name.
    /// </summary>
    /// <param name="objectName">Name of the object.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Scene Object's Name is null or empty.</exception>
    /// <exception cref="System.ArgumentNullException">sceneObjectsName is null or empty.</exception>
    /// <exception cref="System.Exception">Scene object :  + sceneObjectsName +  Not Found</exception>
    public static GameObject instantiatePrefabs(String objectName)
    {
        if (String.IsNullOrEmpty(objectName))
        {
            Debug.LogWarning("Scene Object's Name is null or empty.");
            throw new ArgumentNullException("Scene Object's Name is null or empty.");
        }
        return instance.instantiate<GameObject>(objectName);
    }

    /// <summary>
    /// Instantiates the specified Prefabs object with name.
    /// </summary>
    /// <param name="objectName">Name of the object.</param>
    /// <param name="position">The position.</param>
    /// <param name="rotation">The rotation.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Scene Object's Name is null or empty.</exception>
    /// <exception cref="System.ArgumentNullException">sceneObjectsName is null or empty.</exception>
    /// <exception cref="System.Exception">Scene object :  + sceneObjectsName +  Not Found</exception>
    public static GameObject instantiatePrefabs(String objectName, Vector3 position, Quaternion rotation)
    {
        if (String.IsNullOrEmpty(objectName))
        {
            Debug.LogWarning("Scene Object's Name is null or empty.");
            throw new ArgumentNullException("Scene Object's Name is null or empty.");
        }
        return instance.instantiate<GameObject>(objectName, position, rotation);
    }

    /// <summary>
    /// Instantiates the specified Prefabs object with name, then attaches it to the input sceneObject.
    /// </summary>
    /// <param name="objectName">Name of the object.</param>
    /// <param name="sceneObjectsName">Name of the scene objects.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Scene Object's Name is null or empty.</exception>
    /// <exception cref="Exception">Scene object :  + sceneObjectsName +  Not Found</exception>
    /// <exception cref="System.ArgumentNullException">sceneObjectsName is null or empty.</exception>
    /// <exception cref="System.Exception">Scene object :  + sceneObjectsName +  Not Found</exception>
    public static GameObject instantiatePrefabs(String objectName, String sceneObjectsName)
    {
        GameObject sceneObject = null;

        if (String.IsNullOrEmpty(objectName))
        {
            Debug.LogWarning("Object's Name is null or empty.");
            throw new ArgumentNullException("Object's Name is null or empty.");
        }

        if (String.IsNullOrEmpty(sceneObjectsName))
        {
            Debug.LogWarning("Scene Object's Name is null or empty.");
            throw new ArgumentNullException("Scene Object's Name is null or empty.");
        }
        sceneObject = GameObject.Find(sceneObjectsName);

        if (sceneObject == null)
        {
            Debug.LogWarning("Scene object : " + sceneObjectsName + " Not Found");
            throw new Exception("Scene object : " + sceneObjectsName + " Not Found");
        }
        return instantiatePrefabs(objectName, sceneObject);
    }

    /// <summary>
    /// Instantiates the specified Prefabs object with name, then attaches it to the input sceneObject.
    /// </summary>
    /// <param name="objectName">Name of the object.</param>
    /// <param name="sceneObjectsName">Name of the scene objects.</param>
    /// <param name="position">The position.</param>
    /// <param name="rotation">The rotation.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Scene Object's Name is null or empty.</exception>
    /// <exception cref="Exception">Scene object :  + sceneObjectsName +  Not Found</exception>
    /// <exception cref="System.ArgumentNullException">sceneObjectsName is null or empty.</exception>
    /// <exception cref="System.Exception">Scene object :  + sceneObjectsName +  Not Found</exception>
    public static GameObject instantiatePrefabs(String objectName, String sceneObjectsName, Vector3 position, Quaternion rotation)
    {
        GameObject sceneObject = null;

        if (String.IsNullOrEmpty(objectName))
        {
            Debug.LogWarning("Object's Name is null or empty.");
            throw new ArgumentNullException("Object's Name is null or empty.");
        }

        if (String.IsNullOrEmpty(sceneObjectsName))
        {
            Debug.LogWarning("Scene Object's Name is null or empty.");
            throw new ArgumentNullException("Scene Object's Name is null or empty.");
        }
        sceneObject = GameObject.Find(sceneObjectsName);

        if (sceneObject == null)
        {
            Debug.LogWarning("Scene object : " + sceneObjectsName + " Not Found");
            throw new Exception("Scene object : " + sceneObjectsName + " Not Found");
        }
       return instantiatePrefabs(objectName, sceneObject, position, rotation);
    }

    /// <summary>
    /// Instantiates the specified Prefabs object with name, then attaches it to the input sceneObject.
    /// </summary>
    /// <param name="objectName">Name of the object.</param>
    /// <param name="sceneGameObject">The existing game object.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">existingGameObject is null.</exception>
    /// <exception cref="System.ArgumentNullException">existingGameObject is null.</exception>
    /// <exception cref="System.Exception">Object :  + objectName +  Not Found</exception>
    public static GameObject instantiatePrefabs(String objectName, GameObject sceneGameObject)
    {
        GameObject instantiatedGameObject = null;

        if (String.IsNullOrEmpty(objectName))
        {
            Debug.LogWarning("Object's Name is null or empty.");
            throw new ArgumentNullException("Object's Name is null or empty.");
        }

        if (sceneGameObject == null)
        {
            Debug.LogWarning("Scene Object is null.");
            throw new ArgumentNullException("Scene Object is null.");
        }

        instantiatedGameObject = instance.instantiate<GameObject>(objectName);
        instantiatedGameObject.transform.parent = sceneGameObject.transform;
        Debug.Log("GameObject" + objectName + " Attached To " + sceneGameObject.name);
        return instantiatedGameObject;
    }

    /// <summary>
    /// Instantiates the specified Prefabs object with name, then attaches it to the input sceneObject.
    /// </summary>
    /// <param name="objectName">Name of the object.</param>
    /// <param name="sceneGameObject">The existing game object.</param>
    /// <param name="position">The position.</param>
    /// <param name="rotation">The rotation.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">existingGameObject is null.</exception>
    /// <exception cref="System.ArgumentNullException">existingGameObject is null.</exception>
    /// <exception cref="System.Exception">Object :  + objectName +  Not Found</exception>
    public static GameObject instantiatePrefabs(String objectName, GameObject sceneGameObject, Vector3 position, Quaternion rotation)
    {
        GameObject instantiatedGameObject = null;

        if (String.IsNullOrEmpty(objectName))
        {
            Debug.LogWarning("Object's Name is null or empty.");
            throw new ArgumentNullException("Object's Name is null or empty.");
        }

        if (sceneGameObject == null)
        {
            Debug.LogWarning("Scene Object is null.");
            throw new ArgumentNullException("Scene Object is null.");
        }
        instantiatedGameObject = instance.instantiate<GameObject>(objectName, position, rotation);
        instantiatedGameObject.transform.parent = sceneGameObject.transform;
        Debug.Log("GameObject" + objectName + " Attached To " + sceneGameObject.name);
        return instantiatedGameObject;
    }

    /// <summary>
    /// Clears Unused Assets.
    /// </summary>
    public void Clear()
    {
        assetReferences.Clear();
        Resources.UnloadUnusedAssets();
    }
}