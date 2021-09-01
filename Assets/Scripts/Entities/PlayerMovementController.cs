using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Assets.Scripts.Common;
using Assets.Scripts.Entities;
using Assets.Scripts.World;
using Assets.Scripts.Utils;
using System.IO;


public class PlayerMovementController : MovementBase
{

    

   
    public bool dynamicMove = false;
    public bool stopConstrols;

    
    public Player player;

    

    

    
    private PlayerHitKind hitting;
    //private PlayerAnimation playerAnimation;
    
   
    private bool doubleXInput;



















    protected override void HorizontalMovement()
    {
        inputVector.x = GetHorizontal();
        base.HorizontalMovement();
    }
    
    public void StopControls(float duration)
    {
        StartCoroutine(Enumerator());
        IEnumerator Enumerator()
        {
            stopConstrols = true;
            inputVector = Vector2.zero;
            yield return new WaitForSeconds(duration);
            stopConstrols = false;
        }
    }

    private float GetHorizontal()
    {
        if (stopConstrols)
            return 0f;
        var pcPlataform = new RuntimePlatform[] { 
            
            RuntimePlatform.WindowsEditor,
            RuntimePlatform.OSXEditor,
            RuntimePlatform.LinuxEditor,
            RuntimePlatform.LinuxPlayer,
            RuntimePlatform.WebGLPlayer,
            RuntimePlatform.WindowsPlayer,
        
        };

        if(pcPlataform.Contains(Application.platform) && dynamicMove)
        {
            bool isLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            bool isRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

            if(isLeft && isRight && doubleXInput)
            {
                return inputVector.x;
                ;
            }

            if (isLeft && inputVector.x == 1f)
            {
                doubleXInput = true;
                return -1f;
            }

            if (isRight && inputVector.x == -1f)
            {
                doubleXInput = true;
                return 1f;
            }
            doubleXInput = false;
            return -Convert.ToSingle(isLeft) + Convert.ToSingle(isRight);






        } else
        {
            return Input.GetAxis("Horizontal");
        }
    }

   

    protected override void Jump()
    {
        if (stopConstrols)
            return;
        base.Jump();
        
    }



    override protected void Awake()
    {
        base.Awake();
        
        
        hitting = PlayerHitKind.NoHit;
    }

    private void Start()
    {
     


    }

    protected override void Update()
    {
        if(!stopConstrols)
            inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        base.Update();
    }












}
