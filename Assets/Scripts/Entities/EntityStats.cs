using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public abstract class EntityStats : MonoBehaviour
    {
        public float defaultLife = 500f;
        public float currentLife = 500f;
        public float defaultArmor = 1000f;
        public float currentArmor = 1000f;
        public float defaultEnergy = 100f;
        public float currentEnergy = 100f;

        public abstract void UpdateStats();

        public bool IsDead { get => currentLife <= 0f; }
    }
}
