using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    [Serializable]
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

        public void DealDamage(float totalDamage, DamageKind dk)
        {
            if (dk == immunity)
                totalDamage *= immunityMult;

            currentArmor -= totalDamage;
            if(currentArmor < 0)
            {
                currentLife -= -currentArmor;
                currentArmor = 0f;
            }
        }

        public DamageKind immunity;
        public float immunityMult = 1f;
        
    }
}
