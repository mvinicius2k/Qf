using Assets.Scripts.Common;
using Assets.Scripts.Entities;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace Assets.Scripts.Entities
{

    public class Attack : MonoBehaviour, IAttackable
    {
        public Effect effect;
        public float damage;
        public float damageMult = 1;
        public bool loop;

        public PlayerHitKind hitkind;
        public DamageKind damageKind;
        public float delayBetweenAttacks;
        public float delayAttackRecover;

        protected List<Defense> toAtack;
        protected Timer delayBetweenAttacksTimer;


        /// <summary>
        /// Dano total
        /// </summary>
        public float TotalDamage { get => damage * damageMult; }
        

        public Collider areaAttack;

        

        public virtual void Hit(Defense defense)
        {
            if(defense.ReadyForAttacks)
                defense.DealDamage(TotalDamage, damageKind, hitkind, delayAttackRecover, effect);


        }


        private void OnTriggerEnter(Collider other)
        {
            if (Helpers.AttackableTags.Contains(other.gameObject.tag))
            {
                
                for (int i = 0; i < toAtack.Count; i++)
                {
                    if (toAtack[i].gameObject.InstanceEquals(gameObject))
                    {
                        return;
                    }

                }

                var defense = other.gameObject.GetComponent<Defense>();
                if (defense != null)
                    toAtack.Add(defense);
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (Helpers.AttackableTags.Contains(other.gameObject.tag))
            {
                var defense = other.gameObject.GetComponent<Defense>();
                if(defense != null)
                    toAtack.Remove(other.gameObject.GetComponent<Defense>());
            }
        }

        private void Update()
        {
            if (loop && delayBetweenAttacksTimer != null)
            {
                if(delayBetweenAttacksTimer.Finished)
                {
                    for (int i = 0; i < toAtack.Count; i++)
                    {
                        Hit(toAtack[i]);
                    }

                    delayBetweenAttacksTimer.Restart();
                }
                

            }

        }

        private void Awake()
        {
            toAtack = new List<Defense>();
            delayBetweenAttacksTimer = gameObject.AddComponent<Timer>();
            delayBetweenAttacksTimer.countdown = delayBetweenAttacks;
            delayBetweenAttacksTimer.Name = "DelayBetweenAttacksTimer";
            if(areaAttack != null)
                areaAttack.isTrigger = true;
        }


    }

}