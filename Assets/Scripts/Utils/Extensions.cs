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
        
        public static void SetSharedMaterial(this MeshRenderer meshR, int index, Material newMaterial)
        {
            var copy = meshR.sharedMaterials;
            copy[index] = newMaterial;
            meshR.sharedMaterials = copy;
        }
        public static void MergeComponents(this GameObject gameObject, params Type[] components)
        {
            foreach (var item in components)
            {

                //Type type = typeof(item);
                if (gameObject.GetComponent(item) == null)
                    gameObject.AddComponent(item);

            }
        }

        [Obsolete]
        public static void MergeComponents(this GameObject gameObject, GameObject other)
        {

            var components = other.GetComponents(typeof(Component)).Select(x => x.GetType());
            
            MergeComponents(gameObject, components.ToArray());
        }

        public static bool InstanceEquals(this GameObject gameObj, GameObject other)
        {
            return gameObj.GetInstanceID() == other.GetInstanceID();
        }

        public static bool IsDestroyed(this GameObject gameObj)
        {
            return gameObj == null && ReferenceEquals(gameObj, null);
        }

       

        public static void DestroyChilds(this GameObject parent, int[] childInstancesId = null, bool immediate = false)
        {
            for (int i = parent.transform.childCount - 1; i >= 0; i--)
            {
                var child = parent.transform.GetChild(i).gameObject;
                var childID = child.GetInstanceID();
                if (childInstancesId == null || childInstancesId.Contains(childID))
                {
                    if (immediate)
                    {
                        GameObject.DestroyImmediate(parent.transform.GetChild(i).gameObject);
                    }
                    else
                    {
                        GameObject.Destroy(parent.transform.GetChild(i).gameObject);

                    }
                }

                
            }
            
        }

        /// <summary>
        /// Anexa um <paramref name="child"/> a ao <paramref name="parent"/> se já não houver outro filho de <paramref name="parent"/> com a mesma tag.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns>Uma instância do objeto que foi anexado, sendo o <paramref name="child"/> ou o objeto que já estava lá</returns>
        public static GameObject AttachChildByTag(this GameObject parent, GameObject child, bool immediate = false, bool samePosition = true)
        {
            var found = parent.FindChildByTag(child.tag);
            GameObject attached;
            if(found != null)
            {
                attached = found;



                if(!found.InstanceEquals(child))
                {
                    if(immediate)
                        GameObject.DestroyImmediate(child);
                    else
                        GameObject.Destroy(child);
                }
                
                //child = null;
            }
            else
            {
                attached = child;
                attached.transform.SetParent(parent.transform);
                attached.transform.position = parent.transform.position;
            }

            
            //attached.transform.parent = parent.transform;
            return attached;
        }

        /// <summary>
        /// Cria um filho se já não tiver um <see cref="GameObject"/> com a mesma <paramref name="tag"/>
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name">Se <see langword="null"/>, <paramref name="tag"/> será o nome</param>
        /// <param name="tag"></param>
        /// <returns>A instância do filho achado ou criado</returns>
        public static GameObject AddChildByTag(this GameObject parent, string tag, string name = null, bool immediateDelete = false)
        {
            
            var child = parent.FindChildByTag(tag);
            if(child == null)
            {
                child = new GameObject(name ?? tag);
                child.tag = tag;
            }

            return parent.AttachChildByTag(child, immediateDelete);
        }

        /// <summary>
        /// Obtém uma lista de filhos com a tag <paramref name="tag"/>>
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static GameObject[] FindChildsByTag(this GameObject parent, string tag)
        {
            return parent.GetComponentsInChildren<GameObject>()
                .Where(c =>  c.CompareTag(tag))
                .ToArray();
            

        }

        /// <summary>
        /// Obtém o primeiro filho com a tag <paramref name="tag"/>>
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static GameObject FindChildByTag(this GameObject parent, string tag)
        {
            var res = parent.GetComponentsInChildren<Transform>()
                .FirstOrDefault(c => c.CompareTag(tag));

            return res != null ? res.gameObject : null;
                
               
            

        }

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
