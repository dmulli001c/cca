/// <copyright>
/// Copyright (c)2015, TopCoder, Inc. All rights reserved
/// </copyright>
/// <version>1.0.0</version>
/// <author>engkfke</author> 
/// <summary>
/// A simple test script that shows how to use the InstantiationManager script to De-serializes json String to GameObject.
/// </summary>
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// A simple test script that shows how to use the InstantiationManager script to De-serializes json String to GameObject.
/// </summary>
public class TestDeserializeJSONObject : MonoBehaviour
{

    /// <summary>
    /// This function shows how to De-serializes json String to GameObject.
    /// </summary>
    void DeserializeJSONObject()
    {
     // Simple JSON string for Stool GameObject

        /*
        {
         "$type": "UnityEngine.GameObject",
         "layer": 9,
         "active": true,
         "isStatic": true,
         "tag": "Untagged",
         "Components": [
          {
             "$type": "UnityEngine.Transform",
             "position": { "$type": "UnityEngine.Vector3", "x": 5.5, "y": 0, "z": -14.25},
             "rotation": { "$type": "UnityEngine.Quaternion", "eulerAngles": {"$i": 462}, "x": 0, "y": 0.4617486, "z": 0, "w": 0.8870109},
             "Scale": { "$type": "UnityEngine.Vector3", "x": 1.25, "y": 1.25, "z": 1.25}
         },
         {
             "$type": "UnityEngine.MeshFilter",
             "mesh": {"$type": "UnityEngine.Mesh",  "PrefabName": "Stool" }
         },
         {
             "$type": "UnityEngine.MeshRenderer",
             "materials": [{ "$type": "UnityEngine.Material", "PrefabName": "StoolMaterial" }]
         }
         ],
         "Children": [
             { "$type": "UnityEngine.Prefabs", "PrefabName": "StoolCollider1" },
             { "$type": "UnityEngine.Prefabs", "PrefabName": "StoolCollider2" },
             { "$type": "UnityEngine.Prefabs", "PrefabName": "StoolCollider3" },
             { "$type": "UnityEngine.Prefabs", "PrefabName": "StoolCollider4" }
         ]
         }
            */
        try
        {
            //1- Create "NewStool" GameObject and attach it the "Environment" GameObject

            GameObject StoolGameObject = new GameObject("NewStool");
            StoolGameObject.layer = 9;
            StoolGameObject.transform.parent = GameObject.Find("Environment").transform;

            //2- Edit the position,rotation and Scale of the transform component

            //"$type": "UnityEngine.Transform",
            //"position": { "$type": "UnityEngine.Vector3", "x": 5.5, "y": 0, "z": -14.25},
            //"rotation": { "$type": "UnityEngine.Quaternion", "eulerAngles": {"$i": 462}, "x": 0, "y": 0.4617486, "z": 0, "w": 0.8870109},
            //"Scale": { "$type": "UnityEngine.Vector3", "x": 1.25, "y": 1.25, "z": 1.25}
            StoolGameObject.transform.position = new Vector3(-6.01f, 0, -14.25f);
            StoolGameObject.transform.rotation = new Quaternion(0, 0.4617486f, 0, 0.8870109f);
            StoolGameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);

            //3- Add new MeshFilter component and attach the "Stool" Mesh Object to it .

            //"$type": "UnityEngine.MeshFilter",
            //"mesh": {"$type": "UnityEngine.Mesh",  "PrefabName": "Stool" }
            StoolGameObject.AddComponent<MeshFilter>().mesh = InstantiationManager.instance.instantiate<Mesh>("Stool");

            //4- Add new MeshRenderer component and attach the "StoolMaterial" Material Object to it.

            // "$type": "UnityEngine.MeshRenderer",  "additionalVertexStreams": null
            Material []materials = new Material[1];
            materials[0] = InstantiationManager.instance.instantiate<Material>("StoolMaterial");
            StoolGameObject.AddComponent<MeshRenderer>().materials = materials;

            //5- Attach the Children Prefabs to the StoolGameObject (NewStool). 

            //{ "$type": "UnityEngine.Prefabs", "PrefabName": "StoolCollider1" },
            //{ "$type": "UnityEngine.Prefabs", "PrefabName": "StoolCollider2" },
            //{ "$type": "UnityEngine.Prefabs", "PrefabName": "StoolCollider3" },
            //{ "$type": "UnityEngine.Prefabs", "PrefabName": "StoolCollider4" }
            InstantiationManager.instantiatePrefabs("StoolCollider1", StoolGameObject);
            InstantiationManager.instantiatePrefabs("StoolCollider2", StoolGameObject);
            InstantiationManager.instantiatePrefabs("StoolCollider3", StoolGameObject);
            InstantiationManager.instantiatePrefabs("StoolCollider4", StoolGameObject);
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }  
    }

    void Start()
    {
        DeserializeJSONObject();
    }

}
