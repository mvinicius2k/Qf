using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common
{
    public interface IInitializable
    {
        /// <summary>
        /// Responsável por criar os meta objetos. É chamada caso queira recriar os objetos.
        /// </summary>
        public void CreateMetaObjects();

        /// <summary>
        /// Inicializa e linka os meta objetos que script usa. É chamado <see cref="CreateMetaObjects"/> para recriar meta objetos que foram destruídos.
        /// </summary>
        public void InitMetaObjects(bool forceRecreate);
        /// <summary>
        /// Exclui todos os meta objetos que o script usa e executa o <see cref="InitMetaObjects"/>
        /// </summary>
        public void ResetMetaObjects();

       
    }
}
