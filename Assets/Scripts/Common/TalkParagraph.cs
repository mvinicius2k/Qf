using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common
{
    
    public class TalkParagraph
    {
        public TextMotion[] paragraph;

        public TalkParagraph(TextMotion[] paragraph)
        {
            this.paragraph = paragraph;
        }

        public TalkParagraph()
        {
        }
    }
}
