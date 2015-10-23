using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

/// <summary>
/// Test class to test the JsonSerializeScript.getCatalog and JsonSerializeScript.getCatalogItem functions
/// </summary>
public class TestJsonSerialize : MonoBehaviour
{
    /// <summary>
    /// Test function to test the JsonSerializeScript.getCatalog
    /// </summary>
    void testGetCatalog()
    {
        try
        {
            JsonSerializeScript jsonSerializeScript = new JsonSerializeScript();
            String JosnText = jsonSerializeScript.getCatalog();

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"TestGetCatalogResult.txt"))
            {
                file.Write(JosnText);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    /// <summary>
    /// Test function to test the JsonSerializeScript.getCatalogItem
    /// </summary>
    void testgetCatalogItem()
    {
        try
        {
            JsonSerializeScript jsonSerializeScript = new JsonSerializeScript();
            String JosnText = jsonSerializeScript.getCatalogItem("GameObject");

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"TestgetCatalogItemResult.txt"))
            {
                file.Write(JosnText);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

	void Start () {
        testGetCatalog();
        testgetCatalogItem();
	}
}