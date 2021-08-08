using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : MonoBehaviour
{
    public void AllColliders(bool enable)
    {
        foreach(var c in gameObject.GetComponentsInChildren<Collider>())
        {
            c.enabled = enable;
        }
    }

    public void DisableAllColliders()
    {
        AllColliders(false);
    }
    public void EnableAllColliders()
    {
        AllColliders(true);
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
