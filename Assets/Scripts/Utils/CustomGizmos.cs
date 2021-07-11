using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    static class CustomGizmos
    {
        public static void DrawPlaneArrow(Vector3 center, float degrees, float gsize, float surfacePos, bool bidirectional = true)
        {
            var axis = SnapAxis.Y;

            var floorPos = center;

            var angle = degrees.ToVector2();
            var angleArrowR = (degrees - 30f).ToVector2();
            var angleArrowL = (degrees + 30f).ToVector2();
            var point0 = angle.ToVector3(axis, surfacePos);
            var point1 = angle.ToVector3(axis, surfacePos);

            var arrow0l = angleArrowL.ToVector3(axis, surfacePos);
            var arrow0r = angleArrowR.ToVector3(axis, surfacePos);
            var arrow1r = angleArrowR.ToVector3(axis, surfacePos);
            var arrow1l = angleArrowL.ToVector3(axis, surfacePos);


            floorPos.y = surfacePos;
            point0.Scale(new Vector3(gsize, 0, gsize));
            point1.Scale(new Vector3(-gsize, 0, -gsize));

            arrow0l.Scale(new Vector3(gsize / 2f, 0, gsize / 2f));
            arrow0r.Scale(new Vector3(gsize / 2f, 0, gsize / 2f));
            arrow1r.Scale(new Vector3(-gsize / 2f, 0, -gsize / 2f));
            arrow1l.Scale(new Vector3(-gsize / 2f, 0, -gsize / 2f));

            point0 += floorPos;
            point1 += floorPos;
            arrow0l += floorPos;
            arrow0r += floorPos;
            arrow1r += floorPos;
            arrow1l += floorPos;

            Gizmos.color = Color.white;
            Gizmos.DrawLine(point0, point1);


            Gizmos.color = Color.red;
            Gizmos.DrawLine(arrow0r, point0);
            Gizmos.DrawLine(arrow0l, point0);

            if (bidirectional)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(arrow1r, point1);
                Gizmos.DrawLine(arrow1l, point1);
            }


        }
    }
}
