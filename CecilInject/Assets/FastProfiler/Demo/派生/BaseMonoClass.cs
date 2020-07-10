using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InterfaceTest
{
    void InterfaceCall();
}

public class BaseMonoClass : MonoBehaviour, InterfaceTest
{
    public virtual void InterfaceCall()
    {
        Debug.LogError("BaseClass InterfaceCall");
    }
}
