using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common
{
    public class Constants
    {


        public const string MovimentationLayer = "Movimentation";

        public const string ChangeTrackFloorTag = "ChangeTrackFloor";

        public const string MaterialsPath = "Assets/Materials/";
        public const string GroundLayer = "Ground";


        public static string PlayerFeetMaterial
        {
            get => Path.Combine(MaterialsPath, @"/PlayerFoot");
        }
        public static string PlayerBodyMaterial
        {
            get => Path.Combine(MaterialsPath, @"/PlayerBody");
        }
        
    }
}
