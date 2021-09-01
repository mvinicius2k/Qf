using Assets.Scripts.Common;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GUITalk : GUIBase
    {

        public float defaultLetterDelay = 0.08f;
        public float paragraphDelay = 3f;
        public AudioSource speechAudioSource, letterAudioSource;
        public AudioClip startClip, loopClip, endClip;
        public AudioClip[] lettersClip;

        
        private const string symbolWho = "- ";

        

        [SerializeField]
        private Text text, who;

        public void StartAudioLoopV(float delay = 0f, float duration = float.MaxValue)
        {
            IEnumerator StartAudioLoop(float delay = 0f, float duration = float.MaxValue)
            {
                yield return new WaitForSecondsRealtime(delay);
                if (loopClip != null && speechAudioSource != null)
                {
                    speechAudioSource.clip = loopClip;
                    speechAudioSource.loop = true;
                    speechAudioSource.Play();

                    if (duration != float.MaxValue)
                    {
                        yield return new WaitForSecondsRealtime(duration);
                        speechAudioSource.loop = false;
                        speechAudioSource.Stop();

                    }
                }
                yield return null;
            }

            StartCoroutine(StartAudioLoop(delay, duration));
        }

        
        
       
        

        public override void ShowUI()
        {
            base.ShowUI();

            if(speechAudioSource != null)
            {
                speechAudioSource.clip = startClip;
                speechAudioSource.Play();

                StartAudioLoopV(startClip.length);
                
            }
        }

        public override void HideUI()
        {
            base.HideUI();
            if(speechAudioSource != null)
            {
                speechAudioSource.loop = false;
                speechAudioSource.clip = endClip;
                speechAudioSource.Play();
            }
            
        }

        public IEnumerator StartTalk(TalkParagraph[] talkLines, string whoText, IEnumerator callback = null, bool autoInvokeGUI = true, float preDelayTalk = 0.5f, float afterDelayTalk = 2f)
        {


            if (who != null)
                who.text = symbolWho + whoText;

            if (autoInvokeGUI)
            {
                yield return new WaitForSeconds(preDelayTalk);
                ShowUI();
            }
            

            if (text != null)
            {
                
                for (int i = 0; i < talkLines.Length; i++)
                {
                    Random.InitState((int)System.DateTime.Now.Ticks);
                    text.text = "";
                    for (int j = 0; j < talkLines[i].paragraph.Length; j++)
                    {
                        var word = talkLines[i].paragraph[j];

                        yield return new WaitForSeconds(word.preDelay);
                        for (int k = 0; k < word.text.Length; k++)
                        {

                            if(lettersClip.Length > 0 && letterAudioSource != null)
                            {
                                letterAudioSource.clip = lettersClip[Random.Range(0, lettersClip.Length - 1)];
                                letterAudioSource.Play();
                            }

                            text.text += word.text[k];
                            yield return new WaitForSeconds(defaultLetterDelay);
                        }
                        yield return new WaitForSeconds(word.afterDelay);
                    }
                    yield return new WaitForSeconds(paragraphDelay);

                } 
            }

            if (autoInvokeGUI)
            {
                yield return new WaitForSeconds(afterDelayTalk);
                this.HideUI();
            }

            if(callback != null)
                StartCoroutine(callback);
        }




        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}