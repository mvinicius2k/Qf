using Assets.Scripts.Entities;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.World
{
    public enum WorldColor
    {
        Normal, Dark, Red
    }

    public class FirstApparenceCryWolf : MonoBehaviour
    {
        private Color ambientColor;
        public Color darkAmbient;
        public Color redAmbient;
        public Light[] lightsToChange;
        private Color[] lightsToChangeColorBak;
        [ColorUsage(true, true)]
        public Color newColorForLights;
        public Material[] materialsToChange;
        public Color newColorForMaterials;
        [ColorUsage(true, true)]
        public Color newEmissionColorForMaterials;
        private Color[] colorForMaterialsBak, emissionColorForMaterialsBak;
        public CryWolf cryWolf;
        public GateDoor gateDoorCryWolf,gateDoorPlayer;
        public GateDoor[] gateDoorsToOpen, gateDoorsToClose;
        public CMCameraController cmCameraController;

        public WorldColor worldColor;
        private float speed;
        private bool backUpOk;

        [Header("Sound")]
        public Soundtrack soundtrack;
        public AudioSource audioSource;
        public AudioClip heavyShutdown, cryWolfBram;
        public void TurnAmbientDark(float speed, float delay = 0f)
        {
            StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(delay);
                ambientColor = RenderSettings.ambientLight;
                worldColor = WorldColor.Dark;
                this.speed = speed;

            }
        }

        public void TurnAmbientRed(float speed, float delay = 0f)
        {
            StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(delay);
                //ambientColor = RenderSettings.ambientLight;
                worldColor = WorldColor.Red;
                this.speed = speed;

            }
        }

        public void RestoreAmbient(float speed, float delay = 0f)
        {

            StartCoroutine(Coroutine());
            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(delay);
                worldColor = WorldColor.Normal;
                this.speed = speed;
            }
        }

        public void ChangeLightsAndMaterials(float delay = 0f)
        {
            StartCoroutine(Coroutine());
            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(delay);

                if(!backUpOk)
                    BackupLightsAndMaterials();

                for (int i = 0; i < materialsToChange.Length; i++)
                {
                    

                    materialsToChange[i].SetColor("_Color", newColorForMaterials);
                    materialsToChange[i].SetColor("_EmissionColor", newEmissionColorForMaterials);
                }

                for (int i = 0; i < lightsToChange.Length; i++)
                {
                    lightsToChange[i].color = newColorForLights;
                }
            }


        }

        private void BackupLightsAndMaterials()
        {
            colorForMaterialsBak = new Color[materialsToChange.Length];
            emissionColorForMaterialsBak = new Color[materialsToChange.Length];
            for (int i = 0; i < materialsToChange.Length; i++)
            {
                colorForMaterialsBak[i] = materialsToChange[i].GetColor("_Color");
                emissionColorForMaterialsBak[i] = materialsToChange[i].GetColor("_EmissionColor");
            }
            lightsToChangeColorBak = new Color[lightsToChange.Length];
            for (int i = 0; i < lightsToChange.Length; i++)
            {
                lightsToChangeColorBak[i] = lightsToChange[i].color;
            }
            backUpOk = true;
        }
        public void OffLights(float delay = 0f)
        {

            StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(delay);
                if (!backUpOk)
                    BackupLightsAndMaterials();

                for (int i = 0; i < lightsToChange.Length; i++)
                {
                    lightsToChange[i].color = Color.black;
                }
                for (int i = 0; i < materialsToChange.Length; i++)
                {
                    materialsToChange[i].SetColor("_Color", Color.black);
                    materialsToChange[i].SetColor("_EmissionColor", Color.black);
                }

            }
        }

        public void RestoreLightsAndMaterials(float delay = 0f)
        {
            StartCoroutine(Coroutine());
            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(delay);

                RecoverMaterials();

                for (int i = 0; i < lightsToChange.Length; i++)
                {
                    lightsToChange[i].color = lightsToChangeColorBak[i];
                }
                backUpOk = false;
            }
        }

        private void RecoverMaterials()
        {
            if (materialsToChange == null || colorForMaterialsBak == null || materialsToChange.Length != colorForMaterialsBak.Length)
                return;

            for (int i = 0; i < materialsToChange.Length; i++)
            {
                materialsToChange[i].SetColor("_Color", colorForMaterialsBak[i]);
                materialsToChange[i].SetColor("_EmissionColor", emissionColorForMaterialsBak[i]);
            }
        }

        public void StartScene(float delay = 0f)
        {

            StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(delay);
                foreach (var item in gateDoorsToClose)
                {
                    item.Openned = false;
                }

                foreach(var item in gateDoorsToOpen)
                {
                    item.Openned = true;
                }
                gateDoorCryWolf.ForceOpen();
                yield return new WaitForSeconds(5);

                TurnAmbientDark(100f);
                OffLights();
                soundtrack.audioSource.Stop();
                audioSource.PlayOneShot(heavyShutdown);

                yield return new WaitForSeconds(heavyShutdown.length);

                //audioSource.PlayOneShot(breathing);
                //yield return new WaitForSeconds(breathing.length / 2);

                cryWolf.animation.AwakeCryWolf();
                audioSource.PlayOneShot(cryWolfBram);
                yield return new WaitForSeconds(6);

                cryWolf.movement.inputVector = new Vector2(1f, 0f);
                yield return new WaitForSeconds(4f);

                cryWolf.gameObject.SetActive(false);
                ChangeLightsAndMaterials();
                gateDoorPlayer.ForceOpen();
                soundtrack.AudioNum++;
                soundtrack.StartSoundtrack();
                cmCameraController.SetAutoDollyEnabled(true);
                cmCameraController.SetDeadZoneWidth(1f);
                TurnAmbientRed(100f);


            }
        }

        public void OnApplicationQuit()
        {
            Debug.Log("Recuperandom materiais");
            RecoverMaterials();
        }

        public void OnDestroy()
        {
            Debug.Log("Recuperandom materiais");
            RecoverMaterials();
        }

        public void Update()
        {
            if (worldColor == WorldColor.Dark && RenderSettings.ambientLight != darkAmbient)
                RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, darkAmbient, speed * Time.deltaTime);
            else if (worldColor == WorldColor.Normal && RenderSettings.ambientLight != ambientColor)
            {
                RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, ambientColor, speed * Time.deltaTime);
                Debug.Log(RenderSettings.ambientLight);

            }
            else if (worldColor == WorldColor.Red && RenderSettings.ambientLight != redAmbient)
            {
                RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, redAmbient, speed * Time.deltaTime);
                Debug.Log(RenderSettings.ambientLight);

            }

        }



        public void Awake()
        {
            ambientColor = RenderSettings.ambientLight;

        }

       
    }
}
