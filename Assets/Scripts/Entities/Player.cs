using Assets.Scripts.Entities;
using Assets.Scripts.UI;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public PlayerMovementController playerMovementController;
    public PlayerCombatController playerCombatController;
    public PlayerAnimation playerAnimation;
    public Attack attack;
    public Defense defense;
    public PlayerStats playerStats;
    public GameObject playerModel;
    //public Global global;
    public GUIBlackScreen GUIBlackScreen;
    public WeaponSlot weaponSlot;

    bool deading;

    public void Respawn(float delayToRespawn)
    {

        if (deading)
            return;

        deading = true;
        DisableAllControls();
        
        GUIBlackScreen.ToDark(delayToRespawn);
        GUIBlackScreen.PlayShooshSfx(0f);
        playerAnimation.Die();
        StartCoroutine(enumerator());
        IEnumerator enumerator()
        {
            yield return new WaitForSeconds(delayToRespawn);

            

            Vector3 spawnPos;
            /*if (global.lastCheckpoint.location == null)
                spawnPos = global.gameObject.transform.position;
            else*/
                spawnPos = Global.reference.lastCheckpoint.location.position;

            gameObject.transform.position = spawnPos;
            while (GUIBlackScreen.Changing)
                yield return new WaitForSeconds(0.2f);
            GUIBlackScreen.LeaveDark();

            EnableAllControls();
            deading = false;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
    public void FastRespawn(Attack penalty, ClearEffectRule clearEffectRule, float delayToRespawn = 2f, bool disableAllControls = true)
    {
        if (disableAllControls)
            DisableAllControls();
        if(penalty != null)
            penalty.Hit(defense);
        defense.effectSlot.Clear(clearEffectRule);

        Vector3 spawnPos;
        /*if (Global.reference.lastCheckpoint.location == null)
            spawnPos = Global.gameObject.transform.position;
        else*/
            spawnPos = Global.reference.lastCheckpoint.location.position;

        GUIBlackScreen.ToDark(delayToRespawn);

        StartCoroutine(Coroutine());

        
        
        
        IEnumerator Coroutine()
        {
            yield return new WaitForSeconds(delayToRespawn);

            gameObject.transform.position = spawnPos;
            while(GUIBlackScreen.Changing)
                yield return new WaitForSeconds(0.2f);
            GUIBlackScreen.LeaveDark();
            if(disableAllControls)
                EnableAllControls();
        }

    }

    public void DisableAllControls()
    {
        playerCombatController.stop = true;
        playerMovementController.stopConstrols = true;
        playerMovementController.inputVector = Vector2.zero;
    }
    public void EnableAllControls()
    {
        playerCombatController.stop = false;
        playerMovementController.stopConstrols = false;
    }

    public void Update()
    {
        if (playerStats.IsDead)
        {

            Respawn(3f);
            

        }
    }



    // Start is called before the first frame update

}
