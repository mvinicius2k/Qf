using Assets.Scripts.Common;
using Assets.Scripts.Entities;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCombatController : MonoBehaviour
{



    public bool stop;

    public Weapon weapon;
    private bool shooted;
    public Player player;

    public bool Shooted { get => shooted;}

    private void Awake()
    {
       

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton(Constants.InputFire1) && player.weaponSlot.Equip)
        {
            if (weapon.TryShoot())
            {
                player.playerMovementController.StopControls(weapon.stopDuration);
            }
        }
      
    }

    
}
