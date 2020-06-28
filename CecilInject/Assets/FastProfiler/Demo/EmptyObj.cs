using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FastProfileClassAttribute]
public class EmptyObj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    ~EmptyObj()
    {
        
    }
}

[FastProfileClassAttribute]
public class EmptyClassData
{
    public int age;

    ~EmptyClassData()
    {
        
    }
}
