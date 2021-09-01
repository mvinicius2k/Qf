using Assets.Scripts.Entities;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class EffectSlot : MonoBehaviour, IEffectable
    {
        public List<Transform> particuluesPoints = new List<Transform>();
        public List<Vector3> forceScalePoints = new List<Vector3>();
        
        private Defense defense;

        private bool started, finished;

        private MeshRenderer[] body;
        private Material[][] oldMaterials;
        private Timer durationTimer;
        private Effect currentEffect; // É uma instância, se alterar valores, irá alterar o efeito que a entidade causa para todas as  outras entidades

        public Effect CurrentEffect { get => currentEffect; }
        public bool Started { get => started; }
        public bool Finished { get => finished; }

        

        public void Clear(ClearEffectRule forceRule = ClearEffectRule.Auto)
        {
            if (CurrentEffect == null)
                return;

            ClearEffectRule rule = forceRule != ClearEffectRule.Auto ? forceRule : CurrentEffect.ClearEffectRule;
            if (rule == ClearEffectRule.Auto)
                rule = ClearEffectRule.Nothing;

            switch (rule)
            {
                case ClearEffectRule.Clear:
                    durationTimer.Stop();
                    break;
                case ClearEffectRule.DamagePenalty:
                    defense.DealDamage(
                        Mathf.Floor(durationTimer.countdown * CurrentEffect.HitPerSeconds * CurrentEffect.DamagePerHit),
                        CurrentEffect.DamageKind,
                        PlayerHitKind.NoHit,
                        0f,
                        null,
                        true);
                    durationTimer.Stop();
                    break;
                default:
                    break;
            }


        }

        public virtual void StartEffect(Defense defense, Effect effect, MeshRenderer[] body = null)
        {
            started = true;
            this.defense = defense;
            this.currentEffect = effect;
            /*
            if (body != null)
            {
                oldMaterials = new Material[body.Length][];
                for (int i = 0; i < body.Length; i++)
                {
                    oldMaterials[i] = body[i].sharedMaterials.ToArray();
                    body[i].sharedMaterials = body[i].sharedMaterials.Concat(new Material[] { effectMaterial }).ToArray();
                }
                this.body = body;

            }*/

            durationTimer.Restart(effect.Duration);
            var effectParticules = effect.GetParticules();

            if(effectParticules != null)
            {
                for (int i = 0; i < particuluesPoints.Count; i++)
                {
                    var particules = Instantiate(effectParticules);
                    particules.transform.parent = particuluesPoints[i];
                    particules.transform.localPosition = Vector3.zero;
                    particules.transform.localScale = Vector3.one;
                    particules.transform.localEulerAngles = Vector3.zero;
                    //if(forceScalePoints.Count > 0)
                    //    particules.transform.localScale = forceScalePoints[i % forceScalePoints.Count];

                }

            }

            if(defense != null)
                StartCoroutine(DealDamageTimes(effect));

            Debug.Log("Efeito iniciado");
        }
        public virtual void StopEffect()
        {
            started = true;
            finished = true;
            this.currentEffect = null;

            if (body != null)
            {
                //effectMaterial = MaterialNoEffect;
                for (int i = 0; i < body.Length; i++)
                {
                    body[i].sharedMaterials = oldMaterials[i].ToArray();

                }

            }


            for (int i = 0; i < particuluesPoints.Count; i++)
            {
                particuluesPoints[i].gameObject.DestroyChilds();
            }

            Debug.Log("Efeito finalizado");

            oldMaterials = null;
            body = null;

        }




        private IEnumerator DealDamageTimes(Effect effect, PlayerHitKind hk = PlayerHitKind.NoHit)
        {

            Debug.Log("->");
            if (effect.HitPerSeconds > 0f)
            {
                while (!durationTimer.Finished)
                {
                    
                    yield return new WaitForSeconds(1f / effect.HitPerSeconds);
                    Debug.Log("*");
                    defense.DealDamage(effect.DamagePerHit, effect.DamageKind, hk, 0f, null, true);
                }
            }
            StopEffect();
        }

        private void Awake()
        {
            durationTimer = gameObject.AddComponent<Timer>();
            durationTimer.Name = "Effect Timer";
        }
    }
}
