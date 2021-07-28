using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Templates
{
    public interface IInitializable
    {
        /// <summary>
        /// Responsável por alocar criar os meta objetos
        /// </summary>
        public void CreateMetaObjects(bool calledOnLive);

        /// <summary>
        /// Inicializa e linka os meta objetos que script usa. É chamado <see cref="CreateMetaObjects"/> para recriar meta objetos que foram destruídos e reiniciar os valores.
        ///
        /// </summary>
        /// <paramref name="override"/> Atualiza o objeto com quaisquer novas instruções
        public void InitMetaObjects(bool @override, bool  calledOnLive);
        /// <summary>
        /// Exclui todos os meta objetos que o script usa e executa o <see cref="InitMetaObjects"/>
        /// </summary>
        public void ResetMetaObjects(bool calledOnLive);

        /// <summary>
        /// Destroi os meta objetos
        /// </summary>
        /// <param name="immediate"></param>
        public void RemoveGameObjects(bool calledOnLive);



    }
}
