using UnityEngine;
using System.Collections;
using UnityEngine.CustomEvents;

public class TestGameObjectScript : MonoBehaviour { 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public long Add(long x, long y)
    {
        Debug.Log("Add : "+x+" + "+y);
        long result = x + y;

        ResultEventArgs args = new ResultEventArgs();

        args.result = result;

        EventManager.Trigger("onExecute", args);

        return result;
    }

	public long subtract(long x, long y){
		Debug.Log("Subtract : "+x+" - "+y);
		long result = x - y;
		
		ResultEventArgs args = new ResultEventArgs();
		
		args.result = result;
		
		EventManager.Trigger("onExecute", args);
		
		return result;
	}
}
