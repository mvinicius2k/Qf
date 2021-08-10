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

        public HitKind hitkind;
        public DamageKind damageKind;
        public Timer delayBetweenAttacks;

        private List<Defense> toAtack;


        /// <summary>
        /// Dano total
        /// </summary>
        public float TotalDamage { get => damage * damageMult; }

        public Collider areaAttack;

        public void Hit(Defense defense)
        {
            defense.DealDamage(TotalDamage, damageKind);


        }


        private void OnTriggerEnter(Collider other)
        {
            if (Helpers.AttackableTags.Contains(other.gameObject.tag))
            {
                for (int i = 0; i < toAtack.Count; i++)
                {
                    if (!toAtack[i].gameObject.InstanceEquals(gameObject))
                    {
                        toAtack.Add(other.gameObject.GetComponent<Defense>());
                    }

                }
            }

        }

        private void Update()
        {
            if (loop && delayBetweenAttacks != null)
            {
                if(delayBetweenAttacks.CurrentCountdown == 0f)
                {
                    for (int i = 0; i < toAtack.Count; i++)
                    {
                        Hit(toAtack[i]);
                    }

                    delayBetweenAttacks.Restart();
                }
                

            }

        }

        
    }

}