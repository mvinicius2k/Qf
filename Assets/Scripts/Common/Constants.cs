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

        public const string UntaggedTag = "Untagged";
        public const string ChangeTrackFloorTag = "ChangeTrackFloor";
        public const string ActionTriggerTag = "ActionTrigger";

        public const string MaterialsPath = "";
        public const string GroundLayer = "Ground";


        public static string PlayerFeetMaterial
        {
            get => Path.Combine(MaterialsPath, @"/PlayerFoot");
        }
        public static string PlayerBodyMaterial
        {
            get => Path.Combine(MaterialsPath, @"/PlayerBody");
        }
        public const string MainCameraTag = "MainCamera";
        public const string ContainerTag = "Container";
        public const string EnergyTag = "Energy";

        public static Material GreenLedMat = Resources.Load<Material>(Path.Combine(MaterialsPath, @"/Floor"));
        public static Material RedLedMat = Resources.Load<Material>(Path.Combine(MaterialsPath, @"/red led"));

        public const string InputContextAction = "Context Action";
    }
}
