using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class CryWolf : MonoBehaviour
    {
        public CryWolfMovement movement;
        public new CryWolfAnimation animation;
        public Defense defense;
        public CryWolfStats stats;
        public GameObject model;
        private Player player;

        private bool toClear;

        public void Clear(float delay)
        {
            StartCoroutine(Enumerator());
            IEnumerator Enumerator()
            {
                yield return new WaitForSeconds(delay);
                this.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if(stats != null)
            {

                if (stats.IsDead && !toClear)
                {
                    toClear = true;
                    Clear(3f);

                }

            }
            else
            {


            }

            if(player != null)
            {
                var target = player.playerModel.transform.position;

                if(target.x > model.transform.position.x)
                {
                    movement.inputVector = new Vector3(0.5f, 0f);
                }
                else if(target.x < model.transform.position.x)
                {
                    movement.inputVector = new Vector3(-0.5f, 0f);

                }
            }

            

        }

        public void AwakeWolf(Player target)
        {
            this.player = target;
            animation.paused = false;


        }
    }
}
