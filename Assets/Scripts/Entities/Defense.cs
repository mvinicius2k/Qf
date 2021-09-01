using Assets.Scripts.Common;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Defense : MonoBehaviour
    {
        

        public EntityStats stats;
        public DamageKind immunity, weakness;
        public float immunityMult = 1f;
        public float weaknessMult = 1f;
        public EffectSlot effectSlot;
        [HideInInspector] 
        public PlayerHitKind currentHit = PlayerHitKind.NoHit;
        
        
        

        private Timer recoverTimer;
        public bool ReadyForAttacks { get => recoverTimer.Finished; }
        private MeshRenderer[] mantle;


        

        public void DealDamage(float totalDamage, DamageKind dk, PlayerHitKind hk = PlayerHitKind.Auto, float recoverDelay = 0f, Effect effect = null, bool forceDamage = false)
        {
            if (stats.IsDead)
                return;

            if (dk == weakness && dk != DamageKind.None)
                totalDamage *= weaknessMult; 
            if (dk == immunity && dk != DamageKind.None)
                totalDamage *= immunityMult;

            currentHit = hk == PlayerHitKind.Auto ? GetHitKind(totalDamage) : hk;
            stats.currentArmor -= totalDamage;
            if(stats.currentArmor < 0)
            {
                stats.currentLife -= -stats.currentArmor;
                stats.currentArmor = 0f;
            }

            if(stats != null && recoverDelay > 0f)
            {
                recoverTimer.countdown = recoverDelay;
                recoverTimer.Play();
            }

            if(effect != null)
            {
                effectSlot.StartEffect(this, effect);

            }

            stats.UpdateStats();
        }

        

        public PlayerHitKind GetHitKind(float totalDamage)
        {
            /*if(totalDamage > currentArmor)
            {

            }*/
            return PlayerHitKind.Light;
        }

       

        private void Awake()
        {
            recoverTimer = gameObject.AddComponent<Timer>();
            recoverTimer.startFinished = true;
            recoverTimer.Name = "Recover Timer";

        }

        private void Start()
        {
            //UpdateMantle();
        }

        public void FixedUpdate()
        {
            
        }


    }
}
