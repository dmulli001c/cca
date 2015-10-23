/// <author>
/// engkfke
/// </author>
/// <copyright>
/// Copyright (c)2015, TopCoder, Inc. All rights reserved
/// </copyright>
/// <version>1.0.0</version>
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
/// A test script that shows how to pub/sub requests to ClientManager object.
/// </summary>
public class TestServer : MonoBehaviour
{
    private MqttClient client;
    public String clientId = "123456";
    public String ServerId = "654321";

    public String hostName = "m11.cloudmqtt.com";
    public String userName = "maqidntx";
    public String password = "s8_UOmKFmn7x";
    public int iPort = 16151;

    String server_requestSubscribeTopic;
    String server_responseSubscribeTopic;

    String client_requestPublishTopic;
    String client_responsePublishTopic;

    bool isConnected = false;

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
            server_requestSubscribeTopic = "control/" + ServerId + "/request";
            server_responseSubscribeTopic = "control/" + ServerId + "/response";

            client_requestPublishTopic = "control/" + clientId + "/request";
            client_responsePublishTopic = "control/" + clientId + "/response";

            byte ret = client.Connect(ServerId, userName, password);

            Debug.Log("Client Connect return : " + ret);
            Debug.Log("Client connected with user " + userName);

            client.Subscribe(new string[] { server_requestSubscribeTopic, server_responseSubscribeTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
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
        if (e.Topic == server_requestSubscribeTopic)
        {
            String strMsg = System.Text.Encoding.UTF8.GetString(e.Message);
            Debug.Log("Server : Received Request: " + strMsg);
        }
        else if (e.Topic == server_responseSubscribeTopic)
        {
            String strMsg = System.Text.Encoding.UTF8.GetString(e.Message);
            Debug.Log("Server : Received Response: " + strMsg);
        }
        else
        {
            Debug.Log("Server : Received Error Topic");
        }
    }

    void Start()
    {
        connect();
    }

    public void btnGetCatalog()
    {
        Debug.Log("Server : btnPress ");
        GetCatalogRequest getCatalogRequest = new GetCatalogRequest();
        String jsonStringRequest = fastJSON.JSON.ToJSON(getCatalogRequest);

        client.Publish(client_requestPublishTopic, System.Text.Encoding.UTF8.GetBytes(jsonStringRequest), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
    }

    public void btnGetCatalogItem()
    {
        Debug.Log("Server : btnPress ");
        String objectName = "TestGameObject";
        GetCatalogItemRequest getCatalogRequest = new GetCatalogItemRequest(objectName);
        String jsonStringRequest = fastJSON.JSON.ToJSON(getCatalogRequest);

        client.Publish(client_requestPublishTopic, System.Text.Encoding.UTF8.GetBytes(jsonStringRequest), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
    }

    public void btnInstantiateItem()
    {
        Debug.Log("Server : btnPress ");
        String objectName = "ZomBear";
        String sceneGameObject = "TestGameObject";
        InstantiateItemRequest getCatalogRequest = new InstantiateItemRequest(objectName, sceneGameObject);
        String jsonStringRequest = fastJSON.JSON.ToJSON(getCatalogRequest);

        client.Publish(client_requestPublishTopic, System.Text.Encoding.UTF8.GetBytes(jsonStringRequest), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
    }

    public void btnSubscribe()
    {
        Debug.Log("Server : btnPress ");
        String eventName = "onExecute";
        SubscribeRequest getCatalogRequest = new SubscribeRequest(eventName);
        String jsonStringRequest = fastJSON.JSON.ToJSON(getCatalogRequest);

        client.Publish(client_requestPublishTopic, System.Text.Encoding.UTF8.GetBytes(jsonStringRequest), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
    }

    public void btnExecute()
    {
        Debug.Log("Server : btnPress ");
        String objectName = "TestGameObject";
        String objectType = "TestGameObjectScript";
        String functionName = "Add";
        ExecuteRequest getCatalogRequest = new ExecuteRequest(objectName, objectType, functionName, (Int32)4, (Int32)3);
        String jsonStringRequest = fastJSON.JSON.ToJSON(getCatalogRequest);

        client.Publish(client_requestPublishTopic, System.Text.Encoding.UTF8.GetBytes(jsonStringRequest), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
    }
    public void btnDisconnect()
    {
        client.Unsubscribe(new string[] { server_requestSubscribeTopic, server_responseSubscribeTopic });
        client.Disconnect();
    }
}