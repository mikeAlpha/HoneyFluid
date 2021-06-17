using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private static Dictionary<string, Delegate> mGlobalEvents = new Dictionary<string, Delegate>();

    private static void RegisterEvent(string eventname , Delegate callback)
    {
        Delegate pCallback;
        if(mGlobalEvents.TryGetValue(eventname , out pCallback))
        {
            mGlobalEvents[eventname] = Delegate.Combine(pCallback, callback);
        }
        else
        {
            mGlobalEvents.Add(eventname, callback);
        }
    }

    private static void UnregisterEvent(string eventname, Delegate callback)
    {
        Delegate pCallback;
        if (mGlobalEvents.TryGetValue(eventname, out pCallback))
        {
            mGlobalEvents[eventname] = Delegate.Remove(pCallback, callback);
        }
    }

    public static void RegisterEvent(string eventname, Action callback)
    {
        RegisterEvent(eventname, (Delegate)callback);
    }

    public static void UnregisterEvent(string eventname, Action callback)
    {
        UnregisterEvent(eventname, (Delegate)callback);
    }

    public static void RegisterEvent<T>(string eventname, Action<T> callback)
    {
        RegisterEvent(eventname, (Delegate)callback);
    }

    public static void UnregisterEvent<T>(string eventname, Action<T> callback)
    {
        UnregisterEvent(eventname, (Delegate)callback);
    }

    public static void ExecuteEvent(string eventname)
    {
        Action callback = GetDelegate(eventname) as Action;
        if(callback != null)
        {
            callback();
        }
    }

    public static void ExecuteEvent<T>(string eventname , T param)
    {
        Action<T> callback = GetDelegate(eventname) as Action<T>;
        if (callback != null)
        {
            callback(param);
        }
    }

    private static Delegate GetDelegate(string eventname)
    {
        Delegate callback;
        if(mGlobalEvents.TryGetValue(eventname , out callback))
        {
            return callback;
        }
        return null;
    }
}
