using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GUIBlackScreen : MonoBehaviour
    {

        public Image screen;
        private bool dark, changing;
        private float targetAlpha;
        private float speed;
        public AudioClip whooshSfx;
        public AudioSource sfx;
        public bool Dark { get => dark;}
        public bool Dark1 { get => dark; set => dark = value; }
        public bool Changing { get => changing; set => changing = value; }
        public float TargetAlpha { get => targetAlpha; set => targetAlpha = value; }
        public float FadeDuration { get => speed; set => speed = value; }


        public void PlayShooshSfx(float delay)
        {

            StartCoroutine(enumerator());
            IEnumerator enumerator()
            {
                yield return new WaitForSeconds(delay);
                sfx.PlayOneShot(whooshSfx, 0.8f);

            }
        }
        public void ToDark(float speed = 3f)
        {
            
            dark = true;
            changing = true;
            targetAlpha = 1f;
            this.speed = speed;
        }
        public void LeaveDark(float speed = 5f)
        {
            if (changing)
            {
                Debug.Log($"{screen.name} já está em fade");

            }
            dark = false;
            changing = true;
            targetAlpha = 0f;
            this.speed = speed;

        }

        public void Save(float speed = 3f)
        {
            screen.color = new Color(1f, 1f, 1f, 0f);
            
            ToDark(speed);
        }

        public void Update()
        {

            if (changing && screen != null)
            {

                Color curColor = screen.color;

                if(Mathf.Abs(curColor.a - targetAlpha) > 0.0001f)
                {
                    curColor.a = Mathf.Lerp(curColor.a, targetAlpha, (speed) * Time.deltaTime);
                    screen.color = curColor;

                }
                else
                {
                    changing = false;
                }
            }


        }

        


    }
}
