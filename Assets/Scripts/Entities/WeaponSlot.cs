using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Entities
{


    public class WeaponSlot : MonoBehaviour
    {
        public Player player;
        public GameObject weaponObj;
        //private Weapon currentWeapon;
        public Vector3 baseRotation, basePosition, baseScale;
        public Transform weaponPos;

        private bool equip = false;

        public bool Equip { get => equip;}

        public void ToggleEquip(bool equip)
        {

            weaponObj.SetActive(true);
            
            
            /*currentWeapon.transform.position = basePosition;
            currentWeapon.transform.eulerAngles = baseRotation;
            currentWeapon.transform.localScale = baseScale;
            */
            this.equip = equip;

            if (Equip)
            {
                weaponObj.gameObject.SetActive(true);
                player.playerAnimation.playerTemplate = PlayerTemplate.Gun;

            }
            else
            {
                weaponObj.gameObject.SetActive(false);
            }
        }
#if UNITY_EDITOR
        [CustomEditor(typeof(WeaponSlot))]
        private class WeaponSlotEditor : Editor
        {


            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                WeaponSlot script = (WeaponSlot)target;
                EditorGUILayout.Space();
                if (GUILayout.Button(nameof(script.ToggleEquip) + " On"))
                {
                    script.ToggleEquip(true);
                }
                if (GUILayout.Button(nameof(script.ToggleEquip) + "Off"))
                {
                    script.ToggleEquip(false);
                }









            }
        }
#endif
    }
}
