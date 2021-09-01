using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Res
{
    class StringsResource : MonoBehaviour
    {
        private static StringResourceNode[] allLeafs;
        //
        public StringResourceNode stringResourceTree;
        public static StringResourceNode[] AllLeafs { get 
            {
                Debug.Log("Verificando folhas");
                return allLeafs; 
            } 
        }

        //


        public void Awake()
        {
            UpdateAllLeafs();
        }
        public void Load()
        {
            
            if (File.Exists(Constants.StringsPtbrPath))
            {
                var jsonStr = File.ReadAllText(Constants.StringsPtbrPath);
                stringResourceTree = JsonUtility.FromJson<StringResourceNode>(jsonStr);
                UpdateAllLeafs();
            }
            else
            {
                Debug.LogWarning($"Json de strings não encontrado em {Constants.StringsPtbrPath}");
            }
        }

        public void OpenFolder()
        {
#if UNITY_EDITOR_WIN
            System.Diagnostics.Process.Start(Application.persistentDataPath);
#endif
        }
        public void Save()
        {

            var node = JsonUtility.ToJson(stringResourceTree);

            if(!File.Exists(Constants.StringsPtbrPath))
                Directory.CreateDirectory(Path.GetDirectoryName(Constants.StringsPtbrPath));

            File.WriteAllText(Constants.StringsPtbrPath, node, Encoding.UTF8);

            UpdateAllLeafs();
#if UNITY_EDITOR
            WriteClass();
#endif
            Debug.Log($"{name} {nameof(StringsResource)} salvo com sucesso");
        }

        private void WriteClass()
        {
            

            var code = @"
namespace Assets.Scripts.Res
{
    public class Id
    {
";
            var vars = new string[allLeafs.Length];
            for (int i = 0; i < allLeafs.Length; i++)
            {
                vars[i] = $"        public const string {allLeafs[i].key} = @\u0022{allLeafs[i].value.Replace("\"", "\u0022").Replace("\\","\\\\")}\u0022;";
            }
            code += String.Join("\r\n", vars);
            code += @"
    }
}
";

            var pa = Application.dataPath;
            if (!File.Exists(Constants.StringsPtbrClassPath))
                Directory.CreateDirectory(Path.GetDirectoryName(Constants.StringsPtbrClassPath));

            File.WriteAllText(Constants.StringsPtbrClassPath, code, Encoding.UTF8);

        }

        private void OnValidate()
        {
            //UpdateAllLeafs();
        }

        private void UpdateAllLeafs()
        {
            allLeafs = stringResourceTree.GetAllLeafs(new LinkedList<StringResourceNode>()).ToArray();
        }
    }
}
