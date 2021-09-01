using Assets.Scripts.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.World { 

    public class Soundtrack : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioTrack[] audioTracks;

        [SerializeField]
        private int trackNum;
        public bool AutoNext = true;

        private bool finalized;

        public bool startPlay;

        public bool Finalized { get => finalized;}
        public int AudioNum { get => trackNum % audioTracks.Length; set => trackNum = value % audioTracks.Length; }

        public void StartSoundtrack()
        {
            if(audioTracks[AudioNum].start == null)
            {
                NextToLoop();
                return;
            }
            audioSource.clip = audioTracks[AudioNum].start;
            audioSource.loop = false;
            audioSource.Play();
            StartCoroutine(Next(audioSource.clip.length));
            IEnumerator Next(float delay)
            {
                yield return new WaitForSeconds(delay);
                NextToLoop();
            }
        }

        public void NextToLoop()
        {
            if(audioTracks[trackNum].loop != null)
            {
                audioSource.clip = audioTracks[trackNum].loop;
                audioSource.loop = true;
                audioSource.Play();

            }
            else
            {
                finalized = true;
                if (AutoNext)
                {
                    AudioNum++;
                    StartSoundtrack();

                }
            }
        }

        private void FixedUpdate()
        {
            
        }

        private void Start()
        {
            if (startPlay == true)
            {
                StartSoundtrack();
            }
        }


    }
}
