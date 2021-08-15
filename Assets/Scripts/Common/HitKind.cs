using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common
{
    public enum HitKind
    {
        /// <summary>
        /// Sem hit
        /// </summary>
        NoHit = -1,
        /// <summary>
        /// Não interfere na movimentação do player
        /// </summary>
        Light = 0,
        /// <summary>
        /// Interfere na movimentação por um curto periodo de tempo
        /// </summary>
        Knock = 1,
        /// <summary>
        /// O player é acertado na frente, o jogando para trás
        /// </summary>
        FrontKnockout = 2,
        /// <summary>
        /// O player é acertado nas costas, o jogando para frente
        /// </summary>
        BackKnockout = 3,
        /// <summary>
        /// É acertado com um hit mais forte no ar
        /// </summary>
        OnAir = 4,
        /// <summary>
        /// Depende da quantidade de dano e da armadura
        /// </summary>
        Auto = 100,

        
    }
}
