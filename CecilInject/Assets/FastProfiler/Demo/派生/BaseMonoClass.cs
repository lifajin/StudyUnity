using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InterfaceTest
{
    [FastProfileMethod]
    void InterfaceCall();
}

[FastProfileClass]
public class BaseMonoClass : MonoBehaviour, InterfaceTest
{
    [FastProfileMethod]
    public virtual void InterfaceCall()
    {
        Debug.LogError("BaseClass InterfaceCall");
    }
}
