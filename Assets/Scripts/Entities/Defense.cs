using Assets.Scripts.Common;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Presets;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Defense : MonoBehaviour
    {
        [SerializeField]
        private float defaultLife = 500f;
        [SerializeField]
        private float currentLife = 500f;
        [SerializeField]
        private float defaultArmor = 1000f;
        [SerializeField]
        private float currentArmor = 1000f;

        public Player player;
        public DamageKind immunity, weakness;
        public float immunityMult = 1f;
        public float weaknessMult = 1f;
        public EffectSlot effectSlot;
        [HideInInspector] 
        public HitKind currentHit = HitKind.NoHit;
        
        

        private Timer recoverTimer;
        public bool ReadyForAttacks { get => recoverTimer.Finished; }
        private MeshRenderer[] mantle;
        public void DealDamage(float totalDamage, DamageKind dk, HitKind hk = HitKind.Auto, float recoverDelay = 0f, Effect effect = null, bool forceDamage = false)
        {
            

            if (dk == weakness && dk != DamageKind.None)
                totalDamage *= weaknessMult; 
            if (dk == immunity && dk != DamageKind.None)
                totalDamage *= immunityMult;

            currentHit = hk == HitKind.Auto ? GetHitKind(totalDamage) : hk;
            currentArmor -= totalDamage;
            if(currentArmor < 0)
            {
                currentLife -= -currentArmor;
                currentArmor = 0f;
            }

            if(player != null && recoverDelay > 0f)
            {
                recoverTimer.countdown = recoverDelay;
                recoverTimer.Play();
            }

            if(effect != null)
            {
                effectSlot.StartEffect(this, effect);

            }
        }

        

        public HitKind GetHitKind(float totalDamage)
        {
            /*if(totalDamage > currentArmor)
            {

            }*/
            return HitKind.Light;
        }

        public void UpdateMantle()
        {
            mantle = player.playerModel.GetComponentsInChildren<MeshRenderer>();
        }

        private void Awake()
        {
            recoverTimer = gameObject.AddComponent<Timer>();
            recoverTimer.startFinished = true;
            recoverTimer.Name = "Recover Timer";

        }

        private void Start()
        {
            UpdateMantle();
        }

        public void FixedUpdate()
        {
            
        }


    }
}
