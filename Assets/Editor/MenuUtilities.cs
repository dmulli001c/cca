using UnityEngine;
using UnityEditor;

public class MenuUtilities : MonoBehaviour
{
    [MenuItem("Utils/Clear PlayerPrefs", false, 0)]
    static void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs cleared.");
    }


    [MenuItem("Utils/Open Folder/Data Path...", false, 20)]
    static void OpenDataPath()
    {
        EditorUtility.RevealInFinder(Application.dataPath);
    }


    [MenuItem("Utils/Open Folder/Persistent Data Path...", false, 21)]
    static void OpenPersistentDataPath()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }


    [MenuItem("Utils/Open Folder/Temporary Cache Path...", false, 22)]
    static void OpenTemporaryCachePath()
    {
        EditorUtility.RevealInFinder(Application.temporaryCachePath);
    }


    [MenuItem("Utils/Open Folder/Streaming Assets Path...", false, 23)]
    static void OpenStreamingAssetsPath()
    {
        EditorUtility.RevealInFinder(Application.streamingAssetsPath);
    }
}
