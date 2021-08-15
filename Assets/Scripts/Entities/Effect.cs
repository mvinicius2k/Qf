using Assets.Scripts.Common;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public abstract class Effect : MonoBehaviour, IEffect
    {

        public float damagePerHit, hitPerSeconds;
        public DamageKind damageKind;
        public Material effectMaterial;
        public float duration;

        
        
        public float GetDamagePerSecond()
        {
            return damagePerHit * hitPerSeconds;
        }

        public abstract ParticleSystem GetParticulesSystem();
    }
}
