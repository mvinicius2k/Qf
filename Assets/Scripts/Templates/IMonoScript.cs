using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Templates
{
    public interface IMonoScript
    {
        /// <summary>
        /// Chamado para carregar os objetos
        /// </summary>
        public void MetaObjects(bool calledOnLive);
    }
}
