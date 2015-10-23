using UnityEngine;
using System.Collections;
using PowerUI;

public class TVScreen : MonoBehaviour {
	
	/// <summary>Is input enabled for WorldUI's?</summary>
	public bool InputEnabled;
	/// <summary>A File containing the html/css/nitro for the screen.</summary>
	public TextAsset HtmlFile;

		
	// Use this for initialization
	void Start () {
		
		// Next, generate a new UI:
		WorldUI ui=new WorldUI("TVScreenContent");
		
		// It's representing a TV, so lets use some standard TV screen dimensions:
		ui.SetDimensions(2176, 1224);
		
		// But we need to define the resolution - that's how many pixels per world unit.
		// This of course entirely depends on the model and usage.
		ui.SetResolution(540, 612);
		
		// As this example only uses a WorldUI, we'll set the filter mode to bilinear so it looks smoother.
		ui.TextFilterMode=FilterMode.Bilinear;
		
		// Give it some content:
		if(HtmlFile!=null){
			ui.document.innerHTML=HtmlFile.text;
		}
		
		// Parent it to the TV:
		ui.transform.parent=transform;
        //ui.FaceCamera();
		
		// Let's move it around a little too:
		ui.transform.localPosition= new Vector3 (0.05f,0f,0.044f);

        // And spin it around - the TV mesh is facing the other way:
        ui.transform.localEulerAngles = new Vector3 ( 0f, 270f, 270f);

		if(InputEnabled){
			// World UI's should accept input:
			UI.WorldInputMode=InputMode.Screen;
		}
		
	}
	
}
