using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    [Serializable]
    public class Timer : MonoBehaviour
    {
        /// <summary>
        /// Em segundos
        /// </summary>
        public float countdown;
        public bool running;
        public bool startFinished;


        public float CurrentCountdown { private set; get; }



        private void Update()
        {
            if (running)
            {

                if(CurrentCountdown > 0)
                {
                    CurrentCountdown -= Time.deltaTime;

                }
                else
                {
                    running = false;
                    CurrentCountdown = 00f;
                }
            }
            
        }

        public void Pause()
        {
            running = false;
        }

        public void Play()
        {
            running = true;
        }

        public void Restart()
        {
            CurrentCountdown = countdown;
            running = true;
        }

        private void Start()
        {
            CurrentCountdown = 0f;
            running = false;
        }
    }
}
