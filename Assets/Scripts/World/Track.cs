using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.World
{
   
    [System.Serializable]
    public class Track
    {






        public float m { get; private set; }
        public float b { get; private set; }

        public Vector2 Point { get; private set; }
        public float Angle { get; private set; }

        public Track()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="angle"></param>
        public Track(Vector2 point, float angle)
        {

            this.Point = point;
            this.Angle = angle;
            

            this.m = Mathf.Tan(angle.ToRadians());
            this.b = -((m * point.x) - point.y);

           
            

        }
        public override bool Equals(object obj)
        {
            return obj is Track track &&
                   m == track.m &&
                   b == track.b;
        }

        public override int GetHashCode()
        {
            int hashCode = -634995522;
            hashCode = hashCode * -1521134295 + m.GetHashCode();
            hashCode = hashCode * -1521134295 + b.GetHashCode();
            hashCode = hashCode * -1521134295 + Point.GetHashCode();
            hashCode = hashCode * -1521134295 + Angle.GetHashCode();
            return hashCode;
        }
    }
}
