using UnityEngine;
using System.Collections;
using UnityEngine.CustomEvents;
using System;

public class UnitTestScript : MonoBehaviour {


    /// <summary>
    /// custom Message CustomEventArgs
    /// </summary>
    public class MessageEventArgs : CustomEventArgs
    {
        public String message;
    }

    void Start()
    {
        // Test to unRegister Listener before register its event type or name.
        try
        {
            EventManager.unRegister<UnitTestScript>(ListenerFunction1);
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }

        // Test to invoke event before register its event type or name.
        try
        {
            MessageEventArgs messageEventArgs = new MessageEventArgs();
            EventManager.Trigger<UnitTestScript>(messageEventArgs);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        // Test to register event using its type.
        EventManager.register<UnitTestScript>(ListenerFunction1);

        // Test to register event using its type.
        EventManager.register(typeof(UnitTestScript), ListenerFunction2);

        // Test to register event using its name.
        EventManager.register("UnitTestScript", ListenerFunction3);

        //Invoke after 3 seconds
        InvokeRepeating("InvokeFunction", 3, 0);

        //Invoke after 3 seconds
		InvokeRepeating("unRegisterFunctions", 5, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void unRegisterFunctions()
	{
		EventManager.unRegister<UnitTestScript>(ListenerFunction1);
		EventManager.unRegister<UnitTestScript>(ListenerFunction2);
        EventManager.unRegister<UnitTestScript>(ListenerFunction3);

        // Test to invoke event after un-register all its listeners.
        try
        {
            MessageEventArgs messageEventArgs = new MessageEventArgs();
            EventManager.Trigger("UnitTestScript", messageEventArgs);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
	}

    void InvokeFunction()
    {
        MessageEventArgs messageEventArgs = new MessageEventArgs();

        // Test to invoke event instantly.
        EventManager.Trigger<UnitTestScript>(messageEventArgs);
        Debug.Log(messageEventArgs.message);

        // Test to invoke event Asynchronous (please note we can use it if we do not need output from the listeners).
        EventManager.TriggerAsync<UnitTestScript>(messageEventArgs);
    }

    void ListenerFunction1(CustomEventArgs eventArgs)
    {
        MessageEventArgs messageEventArgs = eventArgs as MessageEventArgs;
        Debug.Log("Enter ListenerFunction1.");
        messageEventArgs.message= "Returned Text from ListenerFunction1.";
	}
    void ListenerFunction2(CustomEventArgs eventArgs)
    {
        MessageEventArgs messageEventArgs = eventArgs as MessageEventArgs;
        Debug.Log("Enter ListenerFunction2.");
        messageEventArgs.message = "Returned Text from ListenerFunction2.";
	}
    void ListenerFunction3(CustomEventArgs eventArgs)
    {
        MessageEventArgs messageEventArgs = eventArgs as MessageEventArgs;
        Debug.Log("Enter ListenerFunction3.");
        messageEventArgs.message = "Returned Text from ListenerFunction3.";
	}
}
