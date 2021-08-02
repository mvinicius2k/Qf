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
    protected bool InitializingObjs { get; private set; }
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


    public void InitMetaObjects(bool @override = true, bool calledOnLive = false)
    {
        InitializingObjs = true;
        if (@override)
        {
            CreateMetaObjects(calledOnLive);
        }
        /*
        foreach(var m in MetaReferences.Values.ToArray())
        {
            MetaReferences[m.tag] = gameObject.AttachChildByTag(m, !calledOnLive);
        }*/


    }

    public void RemoveGameObjects(bool calledOnLive)
    {
        var instancesID = MetaReferences.Values.Select(x => x.GetInstanceID()).ToArray();
        gameObject.DestroyChilds(instancesID, !calledOnLive);
        MetaReferences.Clear();
    }

    

    public GameObject RegisterReference(string tag, bool calledOnLive, params Type[] components)
    {

        if (this == null)
            return null;

        if (!MetaReferences.ContainsKey(tag))
        {
            
            MetaReferences[tag] = gameObject.FindChildByTag(tag) ?? new GameObject(tag, components);
            MetaReferences[tag].MergeComponents(components);
            MetaReferences[tag].tag = tag;
        }
        else
        {

            if (MetaReferences[tag].IsDestroyed())
            {
                MetaReferences[tag] = new GameObject(tag, components);
            } 
            else
            {
                MetaReferences[tag].MergeComponents(components);
                

            }

            
        }


        return MetaReferences[tag];

    }

    public void ResetMetaObjects(bool calledOnLive)
    {
        RemoveGameObjects(calledOnLive);
        InitMetaObjects(true);
    }

    private void Start()
    {

        InitMetaObjects();
    }
    
    [ExecuteInEditMode]
    protected void OnValidate()
    {
#if UNITY_EDITOR
        // Para tirar aquele flood de sendmessage
        if (this == null)
            return;
        if (!this.gameObject.activeSelf)
            return;


        
        if (!Initialized && !InitializingObjs)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {

                InitMetaObjects(true, false);
                Initialized = true;
            };
        }

#endif
    }

    public void CreateMetaObjects(bool calledOnLive)
    {
        instance.MetaObjects(calledOnLive);
    }

    public abstract void MetaObjects(bool calledOnLive);

    
}
