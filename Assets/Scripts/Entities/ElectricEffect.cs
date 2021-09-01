using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class ElectricEffect : Effect
    {
        public ParticleSystem particules;

        public ElectricEffect()
        {

        }

        public override ParticleSystem GetParticules()
        {
            return particules;
        }
    }
}
