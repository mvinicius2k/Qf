using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public interface IEffect
    {
        
        public ParticleSystem GetParticules();
        public float GetDamagePerSecond();
    }
}
