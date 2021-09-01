using Assets.Scripts.Entities;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.World
{
    [Serializable]
    public struct CheckpointInfo
    {
        public Transform location;
        public int soundTrackNum;
        public PlayerTemplate playerTemplate;


    }

    public class Checkpoint : MonoBehaviour
    {
        public Global globalObject;
        public CheckpointInfo checkpointInfo;

        public void Register()
        {
            Global.reference.lastCheckpoint = checkpointInfo;
        }


    }
}
