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

        [SerializeField]
        private float damagePerHit, hitPerSeconds;
        [SerializeField]
        private DamageKind damageKind = DamageKind.None;
        [SerializeField]
        private Material effectMaterial;
        [SerializeField]
        private float duration;
        [SerializeField]
        private ClearEffectRule clearEffectRule = ClearEffectRule.DamagePenalty;

        public float DamagePerHit { get => damagePerHit;}
        public float HitPerSeconds { get => hitPerSeconds;}
        public DamageKind DamageKind { get => damageKind;}
        public Material EffectMaterial { get => effectMaterial; }
        public float Duration { get => duration; }
        public ClearEffectRule ClearEffectRule { get => clearEffectRule; }

        public float GetDamagePerSecond()
        {
            return damagePerHit * hitPerSeconds;
            
        }

        

        public abstract ParticleSystem GetParticules();
    }
}
