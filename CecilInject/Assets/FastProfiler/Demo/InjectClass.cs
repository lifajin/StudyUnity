using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[FastProfileClass]
public class InjectClass : MonoBehaviour
{
    public GameObject obj;
    
    [FastProfileMethod]
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError("start set active false");
        gameObject.SetActive(true);
    }

    [Tooltip("添加GameObject")]
    public bool isAddObj= false;
    [Tooltip("销毁所有GO")]
    public bool isRmvObj= false;
    [Tooltip("创建C#类")]
    public bool isAddCObj= false;
    [Tooltip("Set C# Null")]
    public bool isSetNull= false;
    [Tooltip("GC")]
    public bool isGCCall= false;
    
    protected List<GameObject> golist = new List<GameObject>();
    protected List<EmptyClassData> cslist = new List<EmptyClassData>();
    private void Update()
    {
        if (isAddObj)
        {
            var ngo = GameObject.Instantiate(obj);
            golist.Add(ngo);
            isAddObj = false;
        }

        
        if (isRmvObj)
        {
            for (int i = 0; i < golist.Count; i++)
            {
                GameObject.Destroy(golist[i]);
                golist[i] = null;
            }
            
            golist.Clear();
            isRmvObj = false;
        }

        if (isAddCObj)
        {
            Debug.LogError("create data");
            cslist.Add(new EmptyClassData());
            isAddCObj = false;
        }
        
        if (isSetNull)
        {
            for (int i = 0; i < cslist.Count; i++)
            {
                cslist[i] = null;
            }
            
            cslist.Clear();
            isSetNull = false;
        }

        if (isGCCall)
        {
            GC.Collect();
            isGCCall = false;
        }

    }
}
