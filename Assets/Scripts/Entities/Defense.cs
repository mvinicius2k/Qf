using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Defense : MonoBehaviour
    {
        public float defaultLife = 50f;
        public float currentLife = 50f;
        public float defaultArmor = 100f;
        public float currentArmor = 100f;

        public Effect immunity;
        
    }
}
