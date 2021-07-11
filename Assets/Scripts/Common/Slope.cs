using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common
{
    [Serializable]
    public struct Slope
    {
        public bool isOnSlope;
        public Vector2 normal;
        public float angle;
        

        public Slope(bool isOnSlope, Vector2 normal, float angle)
        {
            this.isOnSlope = isOnSlope;
            this.normal = normal;
            this.angle = angle;
        }

        public Slope(RaycastHit raycastHit)
        {
            this.normal = Vector2.Perpendicular(raycastHit.normal).normalized;
            this.angle = Vector2.Angle(raycastHit.normal, Vector2.up);
            this.isOnSlope = this.angle != 0f;
        }

        public static Slope NoSlope()
        {
            return new Slope(false, new Vector2(), 90f);
        }
    }
}
