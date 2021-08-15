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

        public bool Started { get => started; }
        public bool Finished { get => finished; }

        public virtual void StartEffect(Defense defense, Effect effect, MeshRenderer[] body = null)
        {
            started = true;
            this.defense = defense;

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

            durationTimer.Restart(effect.duration);
            var effectParticules = effect.GetParticulesSystem();

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




        private IEnumerator DealDamageTimes(Effect effect, HitKind hk = HitKind.NoHit)
        {

            Debug.Log("->");
            if (effect.hitPerSeconds > 0f)
            {
                while (!durationTimer.Finished)
                {
                    
                    yield return new WaitForSeconds(1f / effect.hitPerSeconds);
                    Debug.Log("*");
                    defense.DealDamage(effect.damagePerHit, effect.damageKind, hk, 0f, null, true);
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
