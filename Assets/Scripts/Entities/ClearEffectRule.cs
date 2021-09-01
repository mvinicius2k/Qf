using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entities
{
    public enum ClearEffectRule
    {

        /// <summary>
        /// Depende das regras do efeito atual
        /// </summary>
        Auto,
        /// <summary>
        /// Limpa sem nenhuma penalidade
        /// </summary>
        Clear,
        /// <summary>
        /// Executa todo o dano que o efeito causaria
        /// </summary>
        DamagePenalty,
        /// <summary>
        /// Não aplica nenhuma ação
        /// </summary>
        Nothing,


    }
}
