using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[FastProfileClass]
public class InjectClass : MonoBehaviour
{
    public GameObject obj;
    
    //[FastProfileMethod]
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError("start set active false");
        gameObject.SetActive(true);
    }

    [Range(0,4)]
    public int op = 0;
    
    protected List<GameObject> golist = new List<GameObject>();
    protected List<EmptyClassData> cslist = new List<EmptyClassData>();
    private void Update()
    {
        if (1 == op)
        {
            var ngo = GameObject.Instantiate(obj);
            golist.Add(ngo);
        }

        
        if (2 == op)
        {
            for (int i = 0; i < golist.Count; i++)
            {
                GameObject.Destroy(golist[i]);
                golist[i] = null;
            }
            
            golist.Clear();
        }

        if (3 == op)
        {
            Debug.LogError("create data");
            cslist.Add(new EmptyClassData());
        }
        
        if (4 == op)
        {
            for (int i = 0; i < cslist.Count; i++)
            {
                cslist[i] = null;
            }
            
            cslist.Clear();
        }

        if (5 == op)
        {
            GC.Collect();
        }
        
        op = 0;
    }
}
