using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class Constants
    {


        public const string TriggersLayer = "Triggers";

        //Tags
        public const string UntaggedTag = "Untagged";
        public const string ChangeTrackFloorTag = "ChangeTrackFloor";
        public const string ActionTriggerTag = "ActionTrigger";
        public const string UnderwaterGroundTag = "UnderwaterGround";
        public const string MantleTag = "Mantle";

        public const string MaterialsPath = "";
        public const string GroundLayer = "Ground";


        
        public const string PlayerTag = "Player";

        public const string MainCameraTag = "MainCamera";
        public const string ContainerTag = "Container";
        public const string EnergyTag = "Energy";

        //Paths
        public const string NoMaterialPath = "Materials/NoMaterial.mat";

        public const string InputContextAction = "Context Action";

        public const int CountGroundHitKind = 6;
    }
}
