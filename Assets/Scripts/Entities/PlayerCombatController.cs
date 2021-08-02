using Assets.Scripts.Common;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCombatController : MonoBehaviour
{
    public const uint JumpCost = 100;

    public float defaultHealth = 1000;
    public float defaultEnergy = 1000;
    public float currentEnergy = 1000;
    public float currentHealth = 1000;

    private bool completed;
    private PlayerMovementController pMovement;
    public RectTransform energyHUD;

    private void Awake()
    {
        pMovement = GetComponent< PlayerMovementController>();
        /*energyHUD = gameObject.FindChildByTag(Constants.MainCameraTag)
            .FindChildByTag(Constants.ContainerTag)
            .FindChildByTag(Constants.EnergyTag)
            .GetComponent<RectTransform>();*/

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pMovement != null)
        {
            if (pMovement.IsJumping && !completed)
            {
                currentEnergy -= JumpCost;
                
                var cost = ((JumpCost / defaultEnergy)) * energyHUD.rect.width;
                energyHUD.localPosition = new Vector3(energyHUD.localPosition.x - cost, energyHUD.localPosition.y, energyHUD.localPosition.z);
                completed = true;
            }
            else if(pMovement.IsGrounded)
            {
                completed = false;
            }

        }
    }
}
