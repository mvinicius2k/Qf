using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    class CryWolfCombat : Attack
    {

        public CryWolf cryWolf;
        public Vision vision;
        private float atackDelayCount;
        public override void Hit(Defense defense)
        {
            if (cryWolf.stats.IsDead)
                return;
            //base.Hit(defense);
            bool hasAtack = false;
            foreach (var item in vision.Captured.Values)
            {
                var com = item.GetComponent<Defense>();
                if(com != null)
                {
                    if (com.ReadyForAttacks)
                        com.DealDamage(TotalDamage, damageKind, hitkind, delayAttackRecover, effect);

                    hasAtack = true;
                }
            }
            if (hasAtack)
            {
                cryWolf.animation.attackAnim();
                delayBetweenAttacksTimer.Restart(delayBetweenAttacks);
                cryWolf.movement.inputVector = Vector2.zero;
            }

        }

        private void Update()
        {

            if(delayBetweenAttacksTimer.Finished)
                Hit(null);

            if (!delayBetweenAttacksTimer.Finished && !delayBetweenAttacksTimer.running)
                delayBetweenAttacksTimer.Play();
        }

        private void Start()
        {
            delayBetweenAttacksTimer.Restart(delayBetweenAttacks);
        }
    }
}
