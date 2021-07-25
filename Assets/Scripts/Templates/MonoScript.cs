using Assets.Scripts.Common;
using Assets.Scripts.Templates;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class MonoScript : MonoBehaviour, IInitializable, IMonoScript
{

    protected bool Initialized { get; private set; }
    protected Dictionary<string, GameObject> MetaReferences {get; private set; }
    protected IMonoScript instance;

    public MonoScript()
    {
        this.instance = this;
        MetaReferences = new Dictionary<string, GameObject>();
    }



    public void ClearDestroyed()
    {
        var keys = MetaReferences.Keys.ToArray();

        for (int i = keys.Length - 1; i >= 0; i--)
        {
            var reference = MetaReferences[keys[i]];

            if (reference.IsDestroyed())
            { 
                MetaReferences.Remove(keys[i]);
            }
        }
    }


    public void InitMetaObjects(bool forceRecreate = false)
    {
        if (forceRecreate)
        {
            CreateMetaObjects();
        }
        

        foreach(var m in MetaReferences.Values)
        {
            gameObject.AddChildByTag(m.tag);
        }


    }

    public void RemoveGameObjects()
    {
        var instancesID = MetaReferences.Values.Select(x => x.GetInstanceID());
        gameObject.DestroyChilds(instancesID);
    }

    public GameObject RegisterReference(string tag, params Type[] components)
    {
        return RegisterReference(new GameObject(tag, components), tag);
    }

    public GameObject RegisterReference(GameObject gameObj, string tag = null)
    {

        
        gameObj.tag = tag ?? gameObj.tag ?? Constants.UntaggedTag;
        tag = gameObj.tag;

        if (!MetaReferences.ContainsKey(gameObj.tag))
        {
            MetaReferences[gameObj.tag] = gameObj;
        }
        else
        {
            DestroyImmediate(gameObj);
        }


        return MetaReferences[tag];

    }

    public void ResetMetaObjects()
    {
        RemoveGameObjects();
        InitMetaObjects();
    }

    void Start()
    {
        InitMetaObjects();
        Debug.Log("Start");
    }
    

    protected void OnValidate()
    {
        Debug.Log("Validade");
        if (!Initialized)
        {
            InitMetaObjects(true);
            Initialized = true;
        }

        
    }

    public void CreateMetaObjects()
    {
        instance.make();
    }

    public abstract void make();
}
