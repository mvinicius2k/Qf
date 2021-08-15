using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class Mantle : MonoScript
    {
        public Transform position;

        private GameObject mantleObj;
        

        public override void MetaObjects(bool calledOnLive)
        {
            mantleObj = RegisterReference(Constants.MantleTag, calledOnLive, typeof(MeshFilter));
            if(position != null)
                mantleObj.transform.position = position.transform.position;

        }
    }
}
