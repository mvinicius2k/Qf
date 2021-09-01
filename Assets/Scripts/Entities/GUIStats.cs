using Assets.Scripts.Common;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Entities
{


    public class GUIStats : MonoBehaviour
    {


        public RawImage life, energy, armor, lifeOff, energyOff;
        
        public float lifeCellSize = 100f;
        public float energyCellSize = 100f;

        public PlayerStats testPlayerStats;
        /// <summary>
        /// Atualiza uma das barras
        /// </summary>
        /// <param name="cell">Imagem da celula</param>
        /// <param name="cellSize">Quanto cada cálula equivale a uma certa quantia</param>
        /// <param name="defaultPlayerStat">Máximo de vida default</param>
        private void GerateCells(RawImage cell, float cellSize,float defaultPlayerStat, float defaultSizeX = 0f)
        {
            
            
            cell.transform.localScale = Vector3.one;
            var cellCount = defaultPlayerStat / cellSize;


            if(defaultSizeX == 0f)
                defaultSizeX = cell.rectTransform.sizeDelta.x / cell.uvRect.width;

            cell.uvRect = new Rect(Vector2.zero, new Vector2(cellCount, 1f));
            cell.rectTransform.sizeDelta = new Vector2(defaultSizeX * cellCount, cell.rectTransform.sizeDelta.y);
            
            
        }

        /// <summary>
        /// Usar para testar a GUI, só executa em edit mode
        /// </summary>
        [ExecuteInEditMode]
        public void TestUpdateCells()
        {
            UpdateCells(testPlayerStats);
        }

        /// <summary>
        /// Atualiza a barra de stats na tela
        /// </summary>
        /// <param name="ps"></param>
        public void UpdateCells(PlayerStats ps)
        {
            GerateCells(lifeOff, lifeCellSize, ps.defaultLife);
            GerateCells(life, lifeCellSize, ps.currentLife, lifeOff.rectTransform.sizeDelta.x / lifeOff.uvRect.width);
            GerateCells(energyOff, energyCellSize, ps.defaultEnergy);
            GerateCells(energy, energyCellSize, ps.currentEnergy, energyOff.rectTransform.sizeDelta.x / energyOff.uvRect.width);
            GerateCells(armor, lifeCellSize * (ps.defaultArmor / ps.defaultLife), ps.currentArmor, lifeOff.rectTransform.sizeDelta.x / lifeOff.uvRect.width);



        }


        private void Awake()
        {
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(GUIStats))]
        private class GUIStatsEditor : Editor
        {

            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                GUIStats script = (GUIStats)target;
                EditorGUILayout.Space();
                if (GUILayout.Button(nameof(script.UpdateCells)))
                {
                    script.UpdateCells(null);
                }



            }


            /*
            [SerializeField]
            private float cellOffset = 16f;
            [SerializeField]
            private RawImage startHealthCellSample, startEnergyCellSample;
            private Stack<KeyValuePair<GameObject, RawImage>> healthCells = new Stack<KeyValuePair<GameObject, RawImage>>(),
                                    energyCells = new Stack<KeyValuePair<GameObject, RawImage>>(),
                                    ;
            private int _GUIHealthCount, _GUIEnergyCount;
            public float lifePerCell = 100f,
                         energyPerCell = 100f;

            public Stack<KeyValuePair<GameObject, RawImage>> HealthCells { get => healthCells; }
            public Stack<KeyValuePair<GameObject, RawImage>> EnergyCells { get => energyCells;}
            public int GUIHealthCount { get => _GUIHealthCount; }
            public int GUIEnergyCount { get => _GUIEnergyCount; }

            public void SetGUIHealthCount(float life)
            {

                var count = Mathf.FloorToInt(life / lifePerCell);
                var over =  life - count * lifePerCell;
                over /= lifePerCell;

                _GUIHealthCount = count;


                if (over > 0f)
                    _GUIHealthCount++;
                else
                    over = 1f;

                UpdateCells(healthCells, startHealthCellSample);
                var newColor = healthCells.Peek().Value.color;
                newColor.a = over;
                healthCells.Peek().Value.color = newColor;
            }




            public void UpdateHealthGUI()
            {
                UpdateCells(healthCells, startHealthCellSample);

            }

            public void UpdateEnergyGUI()
            {
                UpdateCells(energyCells, startEnergyCellSample);
            }

            private void UpdateCells(Stack<KeyValuePair<GameObject, RawImage>> cells, RawImage sample)
            {
                if (_GUIEnergyCount < 0f || _GUIHealthCount < 0f)
                {
                    Debug.LogError("Valores negativos não sao suportados");
                    return;
                }
                while (_GUIHealthCount != cells.Count)
                {
                    if (_GUIHealthCount > cells.Count)
                    {
                        GameObject newCell;
                        if (cells.Count == 0)
                            newCell = Instantiate(sample.gameObject);
                        else
                            newCell = Instantiate(cells.Peek().Key);

                        var rect = newCell.GetComponent<RectTransform>();
                        var image = newCell.GetComponent<RawImage>();
                        image.enabled = true;
                        newCell.transform.SetParent(sample.transform);

                        var lastPosX = cells.Count > 0 ? cells.Peek().Key.transform.localPosition.x : -cellOffset;

                        rect.localPosition = Vector3.zero;
                        rect.localScale = Vector3.one;
                        rect.localPosition = new Vector3(lastPosX + cellOffset, rect.localPosition.y, rect.localPosition.z);
                        newCell.name = cells.Count.ToString();
                        cells.Push(new KeyValuePair<GameObject, RawImage>(newCell, image));

                    }
                    else if (_GUIHealthCount < healthCells.Count)
                    {
                        var obj = cells.Pop();

                        if(Application.isEditor)
                            DestroyImmediate(obj.Key);
                        else
                            Destroy(obj.Key);






                    }

                }
            }

            public void Start()
            {
                startHealthCellSample.GetComponent<RawImage>().enabled = false;
                startEnergyCellSample.GetComponent<RawImage>().enabled = false;
            }
        }


        [CustomEditor(typeof(GUIStats))]
        class GUIStatsEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                GUIStats script = (GUIStats)target;
                EditorGUILayout.Space();
                if (GUILayout.Button(nameof(script.UpdateHealthGUI)))
                {
                    script.UpdateHealthGUI();
                }
                if (GUILayout.Button(nameof(script.UpdateEnergyGUI)))
                {
                    script.UpdateEnergyGUI();
                }
                if (GUILayout.Button("ClearHealthCells"))
                {
                    while(script.HealthCells.Count > 0)
                        GameObject.DestroyImmediate(script.HealthCells.Pop().Key);
                }


            }
            */

        }
#endif
    }
}
