using Assets.Scripts.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GUIBase : MonoBehaviour, IGUIDisposable
    {
        public Canvas mainCanvas;


        public bool showing;



        public virtual void HideUI()
        {
            mainCanvas.enabled = false;
            showing = false;
            Debug.Log($"${gameObject.name}.showing={showing}");
        }

        public void OnValidate()
        {
            if (mainCanvas != null && isActiveAndEnabled)
            {
                StartCoroutine(UpdateVisibility());

            }
        }
        public virtual void ShowUI()
        {
            mainCanvas.enabled = true;
            showing = true;
            Debug.Log($"${gameObject.name}.showing={showing}");
        }
        private IEnumerator UpdateVisibility()
        {
            yield return new WaitForEndOfFrame();
            if (showing && !mainCanvas.enabled)
                ShowUI();
            else if (mainCanvas.enabled)
                HideUI();
        }

        
    }
}