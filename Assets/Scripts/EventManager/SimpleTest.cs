using UnityEngine;
using System.Collections;
using UnityEngine.CustomEvents;
using System;

public class SimpleTest : MonoBehaviour {

    /// <summary>
    /// custom Message CustomEventArgs
    /// </summary>
    public class MessageEventArgs : CustomEventArgs
    {
        public String message;
    }

    void Start()
    {
        // Test to register event using its type.
        EventManager.register<SimpleTest>(ListenerFunction1);

        //Invoke after 3 seconds
        InvokeRepeating("InvokeFunction", 3, 0);
	}

    void InvokeFunction()
    {
        MessageEventArgs messageEventArgs = new MessageEventArgs();

        // Test to invoke event instantly.
        EventManager.Trigger<SimpleTest>(messageEventArgs);
        Debug.Log(messageEventArgs.message);
    }

    void ListenerFunction1(CustomEventArgs eventArgs)
    {
        MessageEventArgs messageEventArgs = eventArgs as MessageEventArgs;
        messageEventArgs.message= "Returned Text from ListenerFunction1.";
	}
}
