/// <copyright>
/// Copyright (c)2015, TopCoder, Inc. All rights reserved
/// </copyright>
/// <version>1.0.0</version>
/// <author>engkfke</author> 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Reflection;
using System.Linq;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using UnityEngine.UI;
using UnityEngine.CustomEvents;

/// <summary>
/// Provides client connections for MQTT network services.
/// This will be the basis for passing JSON in and out of the network connection. 
/// </summary>
public class ClientManager : Singleton<ClientManager>
{
    private MqttClient client;
    public String clientId = "123456";
    public String ServerId = "654321";

    public String hostName = "m11.cloudmqtt.com";
    public String userName = "maqidntx";
    public String password = "s8_UOmKFmn7x";
    public int iPort = 16151;

    String requestSubscribeTopic; 
    String responseSubscribeTopic;

    String requestPublishTopic;
    String responsePublishTopic;
    
    bool isConnected = false;

    /// <summary>
    /// container for the requests arguments.
    /// </summary>
    class RequestItem
    {
        public String jsonString;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestItem"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="jsonString">The json string.</param>
        public RequestItem(String jsonString)
        {
            this.jsonString = jsonString;
        }
    }

    /// <summary>
    /// The limit queue processing
    /// Every update cycle the queue is processed, if the queue processing is limited,
    /// a maximum processing time per update can be set after which the Execute requests will have to be processed next update loop.
    /// </summary>
    public bool limitQueueProcessing = false;

    /// <summary>
    /// The queue process time
    /// </summary>
    public float queueProcessTime = 0.0f;

    /// <summary>
    /// The requests queue
    /// </summary>
    private Queue requestsQueue = new Queue();

    /// <summary>
    /// Connects this instance.
    /// </summary>
    void connect()
    {
        try
        {
            Debug.Log("Client connecting to " + hostName + " with user " + userName);
            client = new MqttClient(hostName, iPort, false, null);

            // register to message received 
            client.MqttMsgPublishReceived += MqttMsgPublishReceived;

            //clientId = Guid.NewGuid().ToString();
            requestSubscribeTopic = "control/" + clientId + "/request";
            responseSubscribeTopic = "control/" + clientId + "/response";

            requestPublishTopic = "control/" + ServerId + "/request";
            responsePublishTopic = "control/" + ServerId + "/response";

           byte ret = client.Connect(clientId, userName, password);

           if (ret != MqttMsgConnack.CONN_ACCEPTED)
           {
               Debug.Log("Client Connect return : " + ret);
               throw new Exception("Exception connecting to the broker return :" + ret);
           }
           Debug.Log("Client connected with user " + userName);

            client.Subscribe(new string[] { requestSubscribeTopic, responseSubscribeTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            isConnected = true;
        }
        catch (Exception ex)
        {
            Debug.Log("Client Exception connected : " + ex.Message);
        }
    }

    /// <summary>
    /// event for PUBLISH message received.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="MqttMsgPublishEventArgs"/> instance containing the event data.</param>
    void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        if (e.Topic == requestSubscribeTopic)
        {
            String strMsg = System.Text.Encoding.UTF8.GetString(e.Message);
            Debug.Log("Client : Received Request: " + strMsg);

            RequestItem item = new RequestItem(strMsg);
            instance.requestsQueue.Enqueue(item);
        }
        else if (e.Topic == responseSubscribeTopic)
        {
            String strMsg = System.Text.Encoding.UTF8.GetString(e.Message);
            Debug.Log("Client : Received Response: " + strMsg);
        }
        else
        {
            Debug.Log("Client : Received Error Topic");
        }
    }

    void Start()
    {
        ClientManager.instance.connect();
    }

    /// <summary>
    /// Every update cycle the queue is processed, if the queue processing is limited,
    /// a maximum processing time per update can be set after which the Execute requests will have
    /// to be processed next update loop.
    /// An Execution manager centric game will have a lot of Execute requests controlling every aspect of the game. 
    /// So being able to queue Execute requests for the next frame (using TriggerAsync) will ensure not too many Execute requests will fire at once.
    /// It will prevent the game from advancing forward too quickly (Execution trigger chains) and will also help with the frame rate.
    /// </summary>
    void Update()
    {
        float timer = 0.0f;

        while (instance.requestsQueue.Count > 0)
        {
            // Every update cycle the queue is processed, if the queue processing is limited,
            // a maximum processing time per update can be set after which the Execute requests will have
            // to be processed next update loop.
            if (limitQueueProcessing)
            {
                if (timer > queueProcessTime)
                    return;
            }

            RequestItem requestItem = instance.requestsQueue.Dequeue() as RequestItem;
            Process(requestItem);

            if (limitQueueProcessing)
                timer += Time.deltaTime;
        }
    }

    /// <summary>
    /// Processes the specified request item.
    /// </summary>
    /// <param name="requestItem">The request item.</param>
    private void Process(RequestItem requestItem)
    {
        IRequest obj = null;

        try
        {
            obj = fastJSON.JSON.ToObject(requestItem.jsonString) as IRequest;
            IResponse response = obj.Process();

            if (response != null)
                SendResponse(response);
        }
        catch (Exception ex)
        {
            Debug.Log("Client Exception Process : " + ex.Message);
        }
    }

    /// <summary>
    /// Sends the response.
    /// </summary>
    /// <param name="response">The response.</param>
    private void SendResponse(IResponse response)
    {
        try
        {
            String jsonStringResponce = fastJSON.JSON.ToJSON(response);
            client.Publish(responsePublishTopic, System.Text.Encoding.UTF8.GetBytes(jsonStringResponce), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("Client Process SendResponse."); 
        }
        catch (Exception ex)
        {
            Debug.Log("Client Exception SendResponse : " + ex.Message);
        }
    }

    /// <summary>
    /// Raises the <see cref="E:Events" /> event.
    /// </summary>
    /// <param name="eventArgs">The <see cref="CustomEventArgs"/> instance containing the event data.</param>
    public void OnEvents(CustomEventArgs eventArgs)
    {
        SendResponse(new SubscribeResponse(eventArgs));
    }
}