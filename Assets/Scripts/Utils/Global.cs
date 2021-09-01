using Assets.Scripts.Res;
using Assets.Scripts.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class Global : MonoBehaviour
    {
        public static Global reference;

        //public StringsResource stringsResource;
        public CheckpointInfo lastCheckpoint;
        public Player player;
        public Soundtrack soundtrack;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if(reference == null)
            {
                reference = this;
            }
            if(lastCheckpoint.location != null)
            {
                Vector3 spawnPos;
                /*if (global.lastCheckpoint.location == null)
                    spawnPos = global.gameObject.transform.position;
                else*/
                spawnPos = Global.reference.lastCheckpoint.location.position;


                player.transform.position = spawnPos;
                //player.playerAnimation.playerTemplate = lastCheckpoint.playerTemplate;
                //player.
                soundtrack.audioSource.Stop();
                soundtrack.AudioNum = lastCheckpoint.soundTrackNum;
                soundtrack.audioSource.Play();
            }
        }

        private void Start()
        {

        }

        



    }
}
