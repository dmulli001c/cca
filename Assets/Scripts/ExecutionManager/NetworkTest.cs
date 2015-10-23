/// <copyright>
/// Copyright (c)2015, TopCoder, Inc. All rights reserved
/// </copyright>
/// <version>1.0.0</version>
/// <author>engkfke</author> 
/// <summary>
/// Tests executing the functions when receiving commands in the form of JSON objects sent over the network.
/// </summary>
/// 
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.CustomEvents;

/// <summary>
/// Tests executing the functions when receiving commands in the form of JSON objects sent over the network.
/// which it "additional beneficial feature"
/// </summary>
public class NetworkTest : MonoBehaviour
{
    /// <summary>
    /// The client connector
    /// </summary>
    ClientConnector clientConnector = null;

    /// <summary>
    /// The current time
    /// </summary>
    DateTime currentTime;

    /// <summary>
    /// The is connected
    /// </summary>
    bool isConnected = false;

    /// <summary>
    /// The commands queue
    /// </summary>
    Queue<string> commandsQueue = new Queue<string>();

    /// <summary>
    /// Callbacks the receive.
    /// </summary>
    /// <param name="jsonString">The json string.</param>
    public void callbackReceive(string jsonString)
    {
        jsonString = jsonString.Trim();
        Debug.Log("Received Command : " + jsonString);
        commandsQueue.Enqueue(jsonString);
    }

    /// <summary>
    /// Connects to the server.
    /// </summary>
    void connect()
    {
        try
        {
            clientConnector = new ClientConnector();
            clientConnector.init(10000, "127.0.0.1");
            clientConnector.SetReceiveBufferSize(10000);
            clientConnector.setCallackPointer(new ClientConnector.callbackDelegate(this.callbackReceive));
            currentTime = DateTime.Now;
            isConnected = true;
            Debug.Log("Connected.");
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
    }

	// Use this for initialization
	void Start ()
    {
        fastJSON.JSON.Parameters.UsingGlobalTypes = false;
        fastJSON.JSON.Parameters.UseExtensions = false;
        connect();
        EventManager.register("OnCompleteExecution", OnCompleteExecution);
	}

	// Update is called once per frame
    void Update()
    {
        // try to connect every 5 seconds
        if ((DateTime.Now - currentTime).Seconds > 5)
        {
            if (!isConnected)
                connect();
            currentTime = DateTime.Now;
        }

        while (isConnected && commandsQueue.Count > 0)
        {
            String jsonString = commandsQueue.Dequeue();

            ExecuteCmdData cmd = DeSerializeJson(jsonString);

            if (cmd != null)
            {
                Debug.Log("executing Received Command.");
                executeCommand(cmd);
            }
            else
            {
                Debug.Log("the received command is not executeCmd.");
            }
        }
    }

    /// <summary>
    /// De-serialize execute command.
    /// </summary>
    /// <param name="jsonString">The json string.</param>
    /// <returns></returns>
    public static ExecuteCmdData DeSerializeJson(String jsonString)
    {
        ExecuteCmdData obj = null;

        try
        {
            obj = fastJSON.JSON.ToObject<ExecuteCmdData>(jsonString);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        return obj;
    }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="cmd">The command.</param>
    public void executeCommand(ExecuteCmdData cmd)
    {
        GameObject gameObj = GameObject.Find(cmd.objectName);

        if (gameObj == null)
        {
            ResponseCmdData responseCmdData = new ResponseCmdData();
            responseCmdData.cmd = "response";
            responseCmdData.status = "error";
            responseCmdData.message = "The gameObject does not exists";
            responseCmdData.result = 0;

            Debug.Log("Error : The gameObject does not exists.");

            // convert the object to json string
            String jsonStringResponce = fastJSON.JSON.ToJSON(responseCmdData);

            clientConnector.sendRawMessage(jsonStringResponce);
        }

        try
        {
            ExecutionManager.executeAsync(gameObj, Type.GetType(cmd.type), cmd.functionName, cmd.parameters[0], cmd.parameters[1]);
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);

            ResponseCmdData responseCmdData = new ResponseCmdData();
            responseCmdData.cmd = "response";
            responseCmdData.status = "exception";
            responseCmdData.message = ex.Message;
            responseCmdData.result = 0;

            // convert the object to json string
            String jsonStringResponce = fastJSON.JSON.ToJSON(responseCmdData);

            clientConnector.sendRawMessage(jsonStringResponce);
        }
    }

    /// <summary>
    /// Raises the <see cref="E:CompleteExecution" /> event.
    /// </summary>
    /// <param name="eventArgs">The <see cref="CustomEventArgs"/> instance containing the event data.</param>
    public void OnCompleteExecution(CustomEventArgs eventArgs)
    {
        ResponseCmdData responseCmdData = new ResponseCmdData();
        responseCmdData.cmd = "response";
        responseCmdData.status = "success";
        responseCmdData.message = "";

        ResultEventArgs resultEventArgs = eventArgs as ResultEventArgs;

        responseCmdData.result = resultEventArgs.result;

        Debug.Log("OnCompleteExecution : result " + resultEventArgs.result);

        // convert the object to json string
        String jsonStringResponce = fastJSON.JSON.ToJSON(responseCmdData);

        clientConnector.sendRawMessage(jsonStringResponce);
    }
}

/// <summary>
/// container for the execute arguments.
/// </summary>
public class ExecuteCmdData
{
    public String cmd;
    public String objectName;
    public String type;
    public int[] parameters;
    public String functionName;
}

/// <summary>
/// container for the response arguments.status 
/// </summary>
public class ResponseCmdData
{
    public String cmd;
    public String status;   //success,error,wronging
    public long result;      // calculation result
    public String message;  // error message
}

/// <summary>
/// custom Message CustomEventArgs
/// </summary>
public class ResultEventArgs : CustomEventArgs
{
    public long result;
}