using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public abstract class Weapon : MonoBehaviour
    {
        public WeaponKind kind;
        public float stopDuration = 0.5f;

        public abstract bool TryShoot();
    }
}
