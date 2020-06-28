using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastProfiler
{
     [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method )]
     public class FastProfileMethodAttribute : Attribute
     {

     }
     
     [AttributeUsage(AttributeTargets.Class)]
     public class FastProfileClassAttribute : Attribute
     {

     }
     
}