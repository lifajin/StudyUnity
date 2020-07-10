using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EvtManager
{
    public static List<System.Action> allListen = new List<Action>();

    public static void AddListen(System.Action action)
    {
        allListen.Add(action);
    }
    
    public static void RmvListen(System.Action action)
    {
        allListen.Remove(action);
    }
}
