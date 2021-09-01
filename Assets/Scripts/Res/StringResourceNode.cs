using Assets.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Res
{
    [Serializable]
    public class StringResourceNode
    {
        [SerializeField]
        public string key = null;
        [SerializeField] [TextArea]
        public string value = null;
        [SerializeField]
        public StringResourceNode[] childs = null;

        public LinkedList<StringResourceNode> GetAllLeafs(LinkedList<StringResourceNode> list)
        {
            if (value != null && (childs == null || childs.Length == 0))
            {
                list.AddLast(this);
            }
            for (int i = 0; i < childs.Length; i++)
            {
                list.Concat(childs[i].GetAllLeafs(list));
            }

            return list;


        }
    }

    
}
