using Assets.Scripts.Common;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.World
{
    

    public class Talk : MonoBehaviour
    {
        public GUITalk GUITalk;

        [TextArea]
        public string[] paragraph;

        public UltEvents.UltEvent whenFinished;

        private Regex regexNumber0 = new Regex(@"(\A\[\d*\])");
        private Regex regexNumber1 = new Regex(@"(\[\d*\]\Z)");
        private Regex regexAllNumber = new Regex(@"(\[\d*\])");
        private TalkParagraph[] talkParagraph;
        private LifeState lifeState = LifeState.NotStarted;

        public LifeState LifeState { get => lifeState; }

        private IEnumerator TurnFinished()
        {
            lifeState = LifeState.Finished;
            if (whenFinished != null)
                whenFinished.Invoke();
            yield return null;
        }

        public void ActionDelayed(float delay)
        {
            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(delay);
                this.Action();
            }

            StartCoroutine(Coroutine());
        }

        



        public void Action()
        {
            if (lifeState != LifeState.NotStarted)
                return;


            if (GUITalk != null && talkParagraph != null)
            {

                lifeState = LifeState.Started;
                StartCoroutine(GUITalk.StartTalk(talkParagraph, "Desconhecido", TurnFinished()));
                

            }
        }

        



        private TalkParagraph[] ProcessParagraphs()
        {
            TalkParagraph[] talkParagraph = new TalkParagraph[paragraph.Length];
            for (int i = 0; i < paragraph.Length; i++)
            {
                var split = paragraph[i].Split(' ');

                talkParagraph[i] = new TalkParagraph();

                var words = new TextMotion[split.Length];


                for (int j = 0; j < split.Length; j++)
                {
                    string s = split[j];
                    var res0 = regexNumber0.Match(s);
                    var res1 = regexNumber1.Match(s);
                    var value0 = res0.Success ? res0.Value : "[0]";
                    var value1 = res1.Success ? res1.Value : "[0]";


                    words[j].text = regexAllNumber.Replace(s, "") + " ";
                    try
                    {
                        value0 = value0.Substring(1, value0.Length - 2);
                        value1 = value1.Substring(1, value1.Length - 2);
                        words[j].preDelay = Convert.ToSingle(value0) / 1000f;
                        words[j].afterDelay = Convert.ToSingle(value1) / 1000f;

                    }
                    catch (FormatException e)
                    {

                        Debug.LogError($"Erro ao processar conversa: \n{e.StackTrace}");

                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex.StackTrace);
                    }




                }

                talkParagraph[i].paragraph = words;

            }

            return talkParagraph;
        }

        private void Awake()
        {
            talkParagraph = ProcessParagraphs();
        }

        private void FixedUpdate()
        {
           
        }

    }
}
