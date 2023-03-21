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
        public const string AutoActionTriggerTag = "AutoActionTrigger";
        public const string HUDCellTag = "HUDCell";
        public const string EnemyTag = "Enemy";

        public const string MaterialsPath = "";
        public const string GroundLayer = "Ground";


        
        public const string PlayerTag = "Player";

        public const string MainCameraTag = "MainCamera";
        public const string ContainerTag = "Container";
        public const string EnergyTag = "Energy";

        //Paths
        public const string ResStringsPath = "Strings/Main";
        public const string ResNoMaterialPath = "Materials/NoMaterial";

        public static string StringsPtbrPath = Path.Combine(Application.persistentDataPath, "Strings/pt-BR.json");
        public static string StringsPtbrClassPath = Application.dataPath + "/Scripts/Res/Id.cs";

        public const string InputContextAction = "Context Action";
        public const string InputFire1 = "Fire1";
        public const string InputHideCharacter = "Hide Character";

        public const int CountGroundHitKind = 6;
    }
}
