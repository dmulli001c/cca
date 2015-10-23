using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Reflection;


namespace UnityEngine.CustomEvents
{
    public class CustomEventArgs
    {
        public String eventName;
    }
    public delegate void EventDelegate(CustomEventArgs eventArgs);

    /// <summary>
    /// The event handler class
    /// </summary>
    public class EventElement
    {
        protected event EventDelegate eventDelegate;

        /// <summary>
        /// Triggers the event to all listeners, it will Invoke this event instantly.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">Invoke failed due to no listeners</exception>
        public void Trigger(CustomEventArgs eventArgs)
        {            
            if (eventDelegate != null)
            {
                eventDelegate.Invoke(eventArgs);
            }
            else
            {
                Debug.Log("Invoke failed due to no listeners");
                throw new Exception("Invoke failed due to no listeners");
            }
        }

        /// <summary>
        /// Add a listener to this event that will receive any events of the supplied event name.
        /// </summary>
        /// <param name="eventDelegate">The event delegate.</param>
        /// <exception cref="ArgumentNullException">eventDelegate is null.</exception>
        public void addListener(EventDelegate eventDelegate)
        {
            if (eventDelegate == null)
            {
                Debug.Log("eventDelegate is null.");
                throw new ArgumentNullException("eventDelegate is null.");
            }

            if (this.eventDelegate != null)
            {
                foreach (Delegate existingHandler in this.eventDelegate.GetInvocationList())
                {
                    if (existingHandler == eventDelegate)
                    {
                        Debug.Log("The Listener is already in list for this event.");
                        return;
                    }
                }
            }
            this.eventDelegate += eventDelegate;
        }

        /// <summary>
        /// Removes the listener from the current event.
        /// </summary>
        /// <param name="eventDelegate">The event delegate.</param>
        /// <exception cref="ArgumentNullException">eventDelegate is null.</exception>
        /// <exception cref="Exception">Remove Listener failed due to no listeners</exception>
        public void removeListener(EventDelegate eventDelegate)
        {
            if (eventDelegate == null)
            {
                Debug.Log("eventDelegate is null.");
                throw new ArgumentNullException("eventDelegate is null.");
            }

            if (this.eventDelegate == null)
            {
                Debug.Log("Remove Listener failed due to no listeners");
                throw new Exception("Remove Listener failed due to no listeners");
            }
            this.eventDelegate -= eventDelegate;
        }
    }

    /// <summary>
    /// An event manager is generally a singleton that triggers events from anywhere in a game.
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        private Dictionary<string, EventElement> eventDictionary = new Dictionary<string, EventElement>();
        private static EventManager eventManager = null;

        /// <summary>
        /// container for the Event arguments.
        /// </summary>
        class EventItem
        {
            public CustomEventArgs eventArgs;
            public string eventName;

            /// <summary>
            /// Initializes a new instance of the EventItem class.
            /// </summary>
            /// <param name="eventName">Name of the event.</param>
            /// <param name="eventArgs">The <see cref="CustomEventArgs"/> instance containing the event data.</param>
            public EventItem(string eventName, CustomEventArgs eventArgs)
            {
                eventArgs.eventName = eventName;
                this.eventName = eventName;
                this.eventArgs = eventArgs;
            }
        }
        private Queue eventQueue = new Queue();

        /// Every update cycle the queue is processed, if the queue processing is limited, 
        /// a maximum processing time per update can be set after which the events will have to be processed next update loop.
        public bool limitQueueProcesing = false;
        public float queueProcessTime = 0.0f;

        /// <summary>
        /// The public reference to this singleton, which creates a singleton instance if none exists.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static EventManager instance
        {
            get
            {
                if (eventManager == null)
                {
                    eventManager = (EventManager)(new GameObject("EventManager")).AddComponent(typeof(EventManager));
                }
                return eventManager;
            }
        }

        /// <summary>
        /// Registers listener to the specified event type.
        /// </summary>
        /// <typeparam name="TYPE">The event type.</typeparam>
        /// <param name="listener">The listener.</param>
        public static void register<TYPE>(EventDelegate listener)
        {
            register(typeof(TYPE).Name, listener);
        }

        /// <summary>
        /// Registers listener to the specified event type.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="eventDelegate">The event delegate.</param>
        /// <exception cref="ArgumentNullException">eventType is null.</exception>
        public static void register(Type eventType, EventDelegate eventDelegate)
        {
            if (eventType == null)
            {
                Debug.Log("eventType is null.");
                throw new ArgumentNullException("eventType is null.");
            }
            register(eventType.Name, eventDelegate);
        }

        /// <summary>
        /// Registers listener to the specified event name.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="listener">The listener.</param>
        /// <exception cref="ArgumentNullException">
        /// listener is null.
        /// or
        /// eventName is null or empty.
        /// </exception>
        public static void register(String eventName, EventDelegate listener)
        {
            if (listener == null)
            {
                Debug.Log("listener is null.");
                throw new ArgumentNullException("listener is null.");
            }

            if (String.IsNullOrEmpty(eventName))
            {
                Debug.LogWarning("eventName is null or empty.");
                throw new ArgumentNullException("eventName is null or empty.");
            }
            EventElement eventElement = null;

            if (instance.eventDictionary.TryGetValue(eventName, out eventElement))
            {
                eventElement.addListener(listener);
            }
            else
            {
                eventElement = new EventElement();
                eventElement.addListener(listener);
                instance.eventDictionary.Add(eventName, eventElement);
            }
        }

        /// <summary>
        /// UnRegisters listener from the specified event type.
        /// </summary>
        /// <typeparam name="TYPE">The event type.</typeparam>
        /// <param name="listener">The listener.</param>
        public static void unRegister<TYPE>(EventDelegate listener)
        {
            unRegister(typeof(TYPE).Name, listener);
        }

        /// <summary>
        /// UnRegisters listener from the specified event type.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="eventDelegate">The event delegate.</param>
        /// <exception cref="ArgumentNullException">eventType is null.</exception>
        public static void unRegister(Type eventType, EventDelegate eventDelegate)
        {
            if (eventType == null)
            {
                Debug.Log("eventType is null.");
                throw new ArgumentNullException("eventType is null.");
            }
            unRegister(eventType.Name, eventDelegate);
        }

        /// <summary>
        /// UnRegisters listener from the specified event name.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="listener">The listener.</param>
        /// <exception cref="ArgumentNullException">
        /// listener is null.
        /// or
        /// eventName is null or empty.
        /// </exception>
        public static void unRegister(string eventName, EventDelegate listener)
        {
            if (listener == null)
            {
                Debug.Log("listener is null.");
                throw new ArgumentNullException("listener is null.");
            }

            if (String.IsNullOrEmpty(eventName))
            {
                Debug.LogWarning("eventName is null or empty.");
                throw new ArgumentNullException("eventName is null or empty.");
            }

            EventElement thisEvent = null;

            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.removeListener(listener);
            }
            else
            {
                Debug.Log("unRegister Listener failed due to no Registered type for it.");
            }
        }

        /// <summary>
        /// Triggers the specified event name to its all listeners, it will Invoke this event instantly.
        /// </summary>
        /// <typeparam name="TYPE">The event type.</typeparam>
        /// <returns></returns>
        public static void Trigger<TYPE>(CustomEventArgs eventArgs)
        {
            Trigger(typeof(TYPE).Name, eventArgs);
        }

        /// <summary>
        /// Triggers the specified event name to its all listeners, it will Invoke this event instantly.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">eventType is null.</exception>
        public static void Trigger(Type eventType, CustomEventArgs eventArgs)
        {
            if (eventType == null)
            {
                Debug.Log("eventType is null.");
                throw new ArgumentNullException("eventType is null.");
            }
            Trigger(eventType.Name, eventArgs);
        }

        /// <summary>
        ///  Triggers the specified event name to its all listeners, it will Invoke this event instantly.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">eventName is null or empty.</exception>
        /// <exception cref="Exception">Invoke Listener failed due to no Registered type for it.</exception>
        public static void Trigger(string eventName, CustomEventArgs eventArgs)
        {
            if (String.IsNullOrEmpty(eventName))
            {
                Debug.LogWarning("eventName is null or empty.");
                throw new ArgumentNullException("eventName is null or empty.");
            }

            if (eventArgs == null)
            {
                Debug.LogWarning("eventArgs is null.");
                throw new ArgumentNullException("eventArgs is null.");
            }
            eventArgs.eventName = eventName;
            EventElement thisEvent = null;

            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Trigger(eventArgs);
            }
            else
            {
                Debug.Log("Invoke Listener failed due to no Registered type for it.");
                throw new Exception("Invoke Listener failed due to no Registered type for it.");
            }
        }

        /// <summary>
        /// Triggers the specified event type to its all listeners asynchronous.
        /// An event manager centric game will have a lot of events controlling every aspect of the game. 
        /// So being able to queue events for the next frame (using TriggerAsync) will ensure not too many events will fire at once.
        /// It will prevent the game from advancing forward too quickly (event trigger chains) and will also help with the frame rate.
        /// </summary>
        /// <typeparam name="TYPE">Type of the event.</typeparam>
        /// <param name="eventArgs">The <see cref="CustomEventArgs"/> instance containing the event data.</param>
        public static void TriggerAsync<TYPE>(CustomEventArgs eventArgs)
        {
            TriggerAsync(typeof(TYPE).Name, eventArgs);
        }

        /// <summary>
        /// Triggers the specified event type to its all listeners asynchronous.
        /// An event manager centric game will have a lot of events controlling every aspect of the game. 
        /// So being able to queue events for the next frame (using TriggerAsync) will ensure not too many events will fire at once.
        /// It will prevent the game from advancing forward too quickly (event trigger chains) and will also help with the frame rate.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="eventArgs">The <see cref="CustomEventArgs"/> instance containing the event data.</param>
        /// <exception cref="ArgumentNullException">eventType is null.</exception>
        public static void TriggerAsync(Type eventType, CustomEventArgs eventArgs)
        {
            if (eventType == null)
            {
                Debug.Log("eventType is null.");
                throw new ArgumentNullException("eventType is null.");
            }
            TriggerAsync(eventType.Name, eventArgs);
        }
        
        /// <summary>
        /// Triggers the specified event name to its all listeners asynchronous.       
        /// An event manager centric game will have a lot of events controlling every aspect of the game. 
        /// So being able to queue events for the next frame (using TriggerAsync) will ensure not too many events will fire at once.
        /// It will prevent the game from advancing forward too quickly (event trigger chains) and will also help with the frame rate.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="eventArgs">The <see cref="CustomEventArgs"/> instance containing the event data.</param>
        /// <exception cref="ArgumentNullException">eventName is null or empty.</exception>
        /// <exception cref="Exception">Invoke Listener failed due to no listeners for it.</exception>
        public static void TriggerAsync(string eventName, CustomEventArgs eventArgs)
        {
            if (String.IsNullOrEmpty(eventName))
            {
                Debug.LogWarning("eventName is null or empty.");
                throw new ArgumentNullException("eventName is null or empty.");
            }

            if (!instance.eventDictionary.ContainsKey(eventName))
            {
                Debug.Log("Invoke Listener failed due to no listeners for it.");
                throw new Exception("Invoke Listener failed due to no listeners for it.");
            }

            instance.eventQueue.Enqueue(new EventItem(eventName, eventArgs));
        }

        /// <summary>
        /// Every update cycle the queue is processed, if the queue processing is limited,
        /// a maximum processing time per update can be set after which the events will have
        /// to be processed next update loop.
        /// An event manager centric game will have a lot of events controlling every aspect of the game. 
        /// So being able to queue events for the next frame (using TriggerAsync) will ensure not too many events will fire at once.
        /// It will prevent the game from advancing forward too quickly (event trigger chains) and will also help with the frame rate.
        /// </summary>
        void Update()
        {
            float timer = 0.0f;

            while (eventQueue.Count > 0)
            {
                if (limitQueueProcesing)
                {
                    if (timer > queueProcessTime)
                        return;
                }

                EventItem eventItem = eventQueue.Dequeue() as EventItem;
                EventManager.Trigger(eventItem.eventName, eventItem.eventArgs);

                if (limitQueueProcesing)
                    timer += Time.deltaTime;
            }
        }

        /// <summary>
        /// Stop listening to events and clean the eventQueue when we destroy this object.
        /// </summary>
        public void OnDestroy()
        {
            eventQueue.Clear();
            eventDictionary.Clear();
            eventManager = null;
        }
    }
}