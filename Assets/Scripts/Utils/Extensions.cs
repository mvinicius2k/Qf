using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class Extensions
    {
        public static int ToInt(this Direction2D direction)
        {
            return (int)direction;
        }

        public static float ToRadians(this float degrees)
        {
            return Mathf.PI * degrees / 180f;
        }

        


        /// <summary>
        /// Converte o ângulo em um vetor com ângulo equivalente a <paramref name="degrees"/>°
        /// </summary>
        /// <param name="degrees">Graus</param>
        /// <returns></returns>
        public static Vector2 ToVector2(this float degrees)
        {
            degrees = degrees.ToRadians();
            return new Vector2(Mathf.Cos(degrees), Mathf.Sin(degrees)).normalized;
        }

        /// <summary>
        /// Converte um ângulo em Vector3
        /// </summary>
        /// <param name="degrees">Graus</param>
        /// <param name="exclude">Eixo a desprezar</param>
        /// <returns></returns>
        public static Vector3 ToVector3(this float degrees, SnapAxis exclude = SnapAxis.Z, float defaultValue = 0f)
        {
            return degrees.ToVector2().ToVector3(exclude, defaultValue).normalized;
        }

        /// <summary>
        /// Converte para Vector3, incluindo um eixo e deslocando os valores da tupla para a direita.
        /// </summary>
        /// <param name="include">Eixo a incluir</param>
        /// <param name="value">Valor a incluir</param>
        /// <exception cref="ArgumentException">Se não for um ângulo x,y ou z</exception>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector2 v, SnapAxis include = SnapAxis.Z, float value = 0)
        {
            switch (include)
            {
                case SnapAxis.X:
                    return new Vector3(value, v.x, v.y);
                case SnapAxis.Y:
                    return new Vector3(v.x, value, v.y);
                case SnapAxis.Z:
                    return new Vector3(v.x, v.y, value);
                default:
                    throw new ArgumentException($"eixo {include.ToString()} é invalido");
            }
        }

        /// <summary>
        /// Converte para Vector2, desprezando um eixo e deslocando os valores da tupla para a esquerda.
        /// </summary>
        /// <param name="exclude">Eixo a desprezar</param>
        /// <exception cref="ArgumentException">Se não for um ângulo x,y ou z</exception>
        /// <returns></returns>
        public static Vector2 ToVector2(this Vector3 v, SnapAxis exclude = SnapAxis.Z)
        {
            switch (exclude)
            {
                case SnapAxis.X:
                    return new Vector2(v.y, v.z);
                case SnapAxis.Y:
                    return new Vector2(v.x, v.z);
                case SnapAxis.Z:
                    return new Vector2(v.x, v.y);
                default:
                    throw new ArgumentException($"eixo {exclude.ToString()} é invalido");
            }
        }

        public static void Rotate(this Transform obj, Vector3 eulers, Transform exception = null)
        {
            var oldPos = exception?.position ?? Vector3.zero;
            obj.Rotate(eulers);

            if (exception != null)
            {
                exception.Rotate(-eulers);
                exception.position = oldPos;
            }


        }


    }


}
