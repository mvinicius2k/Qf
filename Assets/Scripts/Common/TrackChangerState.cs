using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common
{
    public enum TrackChangerState
    {
        /// <summary>
        /// Primeiro movimento
        /// </summary>
        StartRotation,
        /// <summary>
        /// Segundo movimento
        /// </summary>
        Travel,
        /// <summary>
        /// Terceiro movimento
        /// </summary>
        FinalRotation,
        /// <summary>
        /// Completado
        /// </summary>
        Finalized

    }
}
