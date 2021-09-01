using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class Timer : MonoBehaviour
    {
        /// <summary>
        /// Em segundos
        /// </summary>
        public float countdown;
        public bool running;
        public bool startFinished;
        private new string name;

        public string Name { get => name; set => name = value; }
        public bool Finished { get => countdown == 0f; }

        

        private void Update()
        {
            if (running)
            {

                if(countdown > 0)
                {
                    countdown -= Time.deltaTime;

                }
                else
                {
                    running = false;
                    countdown = 0f;
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

        public void Restart(float? newCountdown = null)
        {
            countdown = newCountdown ?? countdown;
            Play();
        }

        private void Start()
        {
            running = startFinished;
        }

        public void Stop()
        {
            countdown = 0f;
        }
    }
}
