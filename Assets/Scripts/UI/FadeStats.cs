using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public struct FadeStats
    {
        public float duration;
        public Color color;

        public FadeStats(float duration, Color color)
        {
            this.duration = duration;
            this.color = color;
        }
    }
}
