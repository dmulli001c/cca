using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;

public class mqttTest : MonoBehaviour {
	private MqttClient client;
	public String strDeviceId="123456";
	public String strHost="m11.cloudmqtt.com";
	public String strUser="maqidntx";
	public String strPass="s8_UOmKFmn7x";
	public int iPort=16151;
	private GameObject obj;
	private Text t;
	private String strMsg="";
	
	// Use this for initialization
	void Start () {
		obj=GameObject.Find ("Title");
		t=obj.GetComponents<Text>()[0];
		Debug.Log (t.text);

		// create client instance 
		//mqtt://maqidntx:s8_UOmKFmn7x@m11.cloudmqtt.com:16151
		//client = new MqttClient(IPAddress.Parse("127.0.0.1"),1883 , false , null ); 
		Debug.Log ("connecting to " + strHost);
		client = new MqttClient(strHost,iPort , false , null ); 
		
		// register to message received 
		client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; 
		
		string clientId = Guid.NewGuid().ToString(); 
		Debug.Log ("connecting with user " + strUser);
		client.Connect(clientId,strUser,strPass); 
		
		// subscribe to the topic "/home/temperature" with QoS 2 

		client.Subscribe(new string[] { strDeviceId }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); 

		GameObject btnObj = GameObject.Find ("MQTTBtn");
		Button btn=btnObj.GetComponent<Button> ();

		btn.onClick.AddListener (delegate {
			String str="Button pressed inside "+DateTime.Now;
			Debug.Log("Btn Clicked! publishing msg: "+str);
			sendMsg(str);
		});
			
	}

	public void sendMsg(String strPubMsg){
		client.Publish(strDeviceId, System.Text.Encoding.UTF8.GetBytes(strPubMsg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
	}

	void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
	{ 
		strMsg = System.Text.Encoding.UTF8.GetString (e.Message);
		Debug.Log("Received: " + strMsg  );
	} 
/*
	void OnGUI(){

		if ( GUI.Button (new Rect (20, 40, 80, 20), "Level 1")) {
			Debug.Log("sending...");
			client.Publish(strDeviceId, System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
			Debug.Log("sent");
		}
	}

*/
	// Update is called once per frame
	void Update () {
		t.text = strMsg;
		/*
		if (Input.GetButtonDown ("MQTTBtn")) {
			client.Publish(strDeviceId, System.Text.Encoding.UTF8.GetBytes("Button pressed inside Unity3D!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
		}*/
	}
}
