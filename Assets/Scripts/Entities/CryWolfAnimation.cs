using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public enum CryWolfAnimationTemplate
    {
        Default,
        FirstApparence
    }

    public class CryWolfAnimation : MonoBehaviour
    {
        public const string TagFirstApparece = "First Apparence";

        private bool atacked;

        public void attackAnim()
        {
            animator.SetBool(VarAttack, true);
            var infos = animator.GetCurrentAnimatorClipInfo(0);
            if (!atacked)
            {
                for (int i = 0; i < infos.Length; i++)
                {
                    if (infos[i].clip.name == "rig|ATK 2r" && !paused)
                    {
                        atacked = true;
                        StartCoroutine(enumerator());
                        IEnumerator enumerator()
                        {
                            cryWolf.movement.stop = true;
                            yield return new WaitForSeconds(infos[i].clip.length);
                            atacked = false;
                            cryWolf.movement.stop = false;
                        }
                    }
                }
            }
        }

        public const string VarStartWalk = "startWalk";
        public const string VarWalk = "Walk";
        public const string VarAttack = "Attack";
        public const string VarHit = "Hit";
        public const string VarDie = "Death";
        public const string VarIdle = "Idle";
        private bool awaked = false;

        public Animator animator;

        public CryWolf cryWolf;

        public bool paused = false;
        private bool dead = false;

        public CryWolfAnimationTemplate cryWolfAnimationTemplate;
        private float oldAnimatorSpeed;

        private void Update()
        {
            
            if (paused)
            {
                animator.speed = 0f;
            }
            else
            {
                animator.speed = 1f;
            }


            if (CryWolfAnimationTemplate.FirstApparence == cryWolfAnimationTemplate)
            {
                
                animator.SetBool(VarStartWalk, cryWolf.movement.IsRunning);

            }
            else
            {

                var infos = animator.GetCurrentAnimatorClipInfo(0);

                if (!awaked)
                {
                    for (int i = 0; i < infos.Length; i++)
                    {
                        if (infos[i].clip.name == "rig|DAMAGE Fast Stagger" && !paused)
                        {
                            awaked = true;
                            StartCoroutine(enumerator());
                            IEnumerator enumerator()
                            {
                                cryWolf.movement.stop = true;
                                cryWolf.movement.inputVector = Vector2.zero;
                                yield return new WaitForSeconds(infos[i].clip.length);
                                cryWolf.movement.stop = false;
                            }
                        }
                    } 
                }



                if(dead)
                    animator.SetBool(VarDie, false);
                if (cryWolf.stats.IsDead && !dead)
                {
                    animator.SetBool(VarDie, cryWolf.stats.IsDead);
                    dead = true;

                }

                

                
                if(cryWolf.defense.currentHit != Common.PlayerHitKind.NoHit)
                {
                    animator.SetBool(VarHit, true);
                    cryWolf.defense.currentHit = Common.PlayerHitKind.NoHit;
                    cryWolf.movement.stop = true;

                    StartCoroutine(enumerator());
                    IEnumerator enumerator()
                    {
                        yield return new WaitForSeconds(1f);
                        cryWolf.movement.stop = false;
                    }
                }
                else
                {
                    animator.SetBool(VarHit, false);
                }
                animator.SetBool(VarWalk, cryWolf.movement.IsRunning);
                animator.SetBool(VarIdle, cryWolf.movement.IsIdle);

            }

        }

        public void AwakeCryWolf(float delay = 0f)
        {

            StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(delay);
                cryWolf.movement.inputVector = Vector2.zero;
                paused = false;
                animator.speed = 1f;

               

            }
        }

        public void MoveCryWolf(float delay = 0f)
        {

            StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(delay);
                cryWolf.movement.inputVector = new Vector2(1f, 0f);
            }
        }
    }
}
