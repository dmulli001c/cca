using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// JsonSerializeScript object is exposes a getCatalog() method and a getCatalogItem() method that
/// return JSON formatted serialization of Unity objects. 
/// The returned JSON contains publicly available attributes and methods on the Unity objects.
/// </summary>
public class JsonSerializeScript{
    /// <summary>
    /// collect up all the objects in the scene, hierarchically,
    /// and introspect their features as json.
    /// </summary>
    /// <returns>return JSON as result</returns>
    public String getCatalog()
    {
        Scene scene = new Scene();
        scene.parentGameObjects = getParentGameObjects();
        return fastJSON.JSON.ToJSON(scene);;
    }

    /// <summary>
    /// returns a specific catalog item, based upon it's position in the catalog tree.
    /// </summary>
    /// <param name="pathToItem"></param>
    /// <returns>return JSON as result</returns>
    public String getCatalogItem(String pathToItem)
    {
        String JosnText = "";
        GameObject gameObject = GameObject.Find(pathToItem); 

        if (gameObject != null)
        {
            JosnText = fastJSON.JSON.ToJSON(gameObject);
        }
        return JosnText;
    }

    /// <summary>
    /// collect up all the parent objects in the scene.
    /// </summary>
    /// <returns>return list of the parent objects</returns>
    private List<GameObject> getParentGameObjects()
    {
        Transform[] sceneTransforms = UnityEngine.Object.FindObjectsOfType<Transform>();
        List<Transform> parentTransforms = new List<Transform>();
        List<GameObject> parentGameObjects = new List<GameObject>();
        
        foreach (Transform transform in sceneTransforms)
            parentTransforms.Add(transform);

        foreach (Transform transform in sceneTransforms)
            removeChildrenTransforms(transform, parentTransforms);

        foreach (Transform transform in parentTransforms)
        {
            // skip to Serialize the gameobject that attach the  TestJsonSerialize 
            if (transform.gameObject.GetComponent<TestJsonSerialize>() == null)
                parentGameObjects.Add(transform.gameObject);
        }

        return parentGameObjects;
    }

    /// <summary>
    /// remove all the child objects from the input list
    /// </summary>
    /// <param name="parentTransform"></param>
    /// <param name="parentTransforms"></param>
    private void removeChildrenTransforms(Transform parentTransform, List<Transform> parentTransforms)
    {
        foreach (Transform childTransform in parentTransform.transform)
        {
            parentTransforms.Remove(childTransform);
            removeChildrenTransforms(childTransform, parentTransforms);
        }
    }
}