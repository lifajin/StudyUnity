using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMonoClass : BaseMonoClass
{
    private void Start()
    {

    }

    [FastProfileMethod]
    public override void InterfaceCall()
    {
        Debug.LogError("ChildMonoClass InterfaceCall");
        base.InterfaceCall();
    }
}
