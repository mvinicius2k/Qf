using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class SwitchButton : MonoBehaviour
    {
        public Animator anim;
        public const string VarOpen = "open";
        public ParticleSystem highlight, action;
        public bool openned = false;

        public void Start()
        {
            anim.SetBool(VarOpen, openned);
        }



        public void Open(float delay = 0f, bool hideHighlightOnFirstTrigger = false)
        {
            openned = true;

            if (hideHighlightOnFirstTrigger && highlight != null)
                highlight.Stop();
            if (action != null)
                action.Play();
            StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(delay);
                anim.SetBool(VarOpen, openned);
            }
        }

        public void Close(float delay = 0f, bool hideHighlight = false)
        {
            openned = false;
            if (hideHighlight && highlight != null)
                highlight.Stop();
            if (action != null)
                action.Play();
            StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(delay);
                anim.SetBool(VarOpen, openned);
            }
        }


    }
}
