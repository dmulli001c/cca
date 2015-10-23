using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class UnityClient : MonoBehaviour
{
    ClientConnector clientConnector = null;
	JsonSerializeScript jsonSerializeScript = null;
	Queue<string> cmdQueue = null;

    public void callback(string str)
    {
        Debug.Log(str);
		str=str.Trim ();
		cmdQueue.Enqueue (str);
    }
    DateTime cueentTime;

    bool isConnected = false;

    void connect()
    {
        clientConnector = new ClientConnector();
        clientConnector.init(10000, "127.0.0.1");
        clientConnector.SetReceiveBufferSize(10000);
        clientConnector.setCallackPointer(new ClientConnector.callbackDelegate(this.callback));
        cueentTime = DateTime.Now;

        Debug.Log("Connected.");
        isConnected = true;
    }
	// Use this for initialization
	void Start () {
        connect();
		jsonSerializeScript=new JsonSerializeScript();
		cmdQueue=new Queue<string>();
	}
	
	// Update is called once per frame
	void Update () {

        if (!isConnected)
            connect();
        else if((DateTime.Now - cueentTime).Seconds > 5)
        {
			if(cmdQueue.Count>0){

				String str=cmdQueue.Dequeue();

				if (str.Equals("{ cmd: 'getCatalog' }")) {
					Debug.Log("fetching Catalog");
					clientConnector.sendRawMessage(jsonSerializeScript.getCatalog());
				}
			}

            cueentTime = DateTime.Now;
            clientConnector.sendRawMessage(cueentTime.ToString());
            Debug.Log("Heartbeat message : " + cueentTime.ToString());


        }
	}
}
