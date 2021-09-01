using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class PlayerStats : EntityStats
    {
        public const float LifePerCell = 100f;

        public Player player;
        

        

        public GUIStats GUIStats;

        /// <summary>
        /// Atualiza os stats da GUI
        /// </summary>
        public override void UpdateStats()
        {
            if(GUIStats != null)
            {
                GUIStats.UpdateCells(this);
            }
        }

        private void Start()
        {
            UpdateStats();
        }
    }
}
