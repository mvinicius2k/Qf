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
    public class PlasmaGun : Weapon
    {

        public AudioClip[] shoots;
        public AudioSource audioSource;

        public Attack shootAtack;

        public Collider shootArea;

        public float intervalDuration = 1f;
        private float intervalDurationCount = 0f;
        public LayerMask hits;
        public ParticleSystem sparks, lightShoot;
        public Light bright;
        public PlayerAnimation playerAnimation;
        public Vision vision;
        

        public override bool TryShoot()
        {
            if (intervalDurationCount > 0f)
                return false;

            sparks.Play();
            lightShoot.Play();
            audioSource.PlayOneShot(shoots[UnityEngine.Random.Range(0, shoots.Length)], 0.5f);
            foreach (var item in vision.Captured.Values)
            {
                var comp = item.GetComponent<Defense>();
                if (comp)
                {
                    comp.DealDamage(shootAtack.TotalDamage, shootAtack.damageKind, shootAtack.hitkind, 0f, null, false);

                }
            } 

            bright.enabled = true;
            intervalDurationCount = intervalDuration;
            StartCoroutine(OffLight(0.1f));
            return true;
            IEnumerator OffLight(float delay)
            {
                yield return new WaitForSeconds(delay);
                bright.enabled = false;
            }
           
        }

        private void Update()
        {
            if (intervalDurationCount > 0)
                intervalDurationCount -= Time.deltaTime;
            else
                intervalDurationCount = 0f;

            
        }

        private void OnDrawGizmosSelected()
        {
            
        }
    }
}
