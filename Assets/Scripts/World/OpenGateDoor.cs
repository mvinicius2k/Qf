using Assets.Scripts.Common;
using Assets.Scripts.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGateDoor : MonoScript
{

    public Collider trigger;
    
    public override void MetaObjects(bool calledOnLive)
    {
        var gameObj = RegisterReference(Constants.ActionTriggerTag, calledOnLive, typeof(SphereCollider));
        gameObj.layer = LayerMask.NameToLayer(Constants.TriggersLayer);
        gameObj.transform.position = gameObject.transform.position;
        
        trigger = gameObj.GetComponent<SphereCollider>();
        trigger.isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Alguém entrou");
        //
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
