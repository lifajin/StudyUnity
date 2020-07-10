using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [AttributeUsage(AttributeTargets.Method, Inherited = true)]
 public class FastProfileMethodAttribute : Attribute
 {
     public bool SingleClass = true;
     public FastProfileMethodAttribute(bool singleClass = true)
     {
         SingleClass = singleClass;
     }
 }
 
 [AttributeUsage(AttributeTargets.Class, Inherited = true)]
 public class FastProfileClassAttribute : Attribute
 {

 }
