using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common
{
    [Serializable]
    public struct TextMotion
    {
        [TextArea]
        public string text;
        //public float? speed;
        public float preDelay, afterDelay;

        
    }
}
