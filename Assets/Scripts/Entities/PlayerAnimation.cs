using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class PlayerAnimation : MonoBehaviour
    {

        public const int AnimLayerMovement = 0;

        public const string VarIdle = "Idle";
        public const string VarRun = "Run";
        public const string VarJump = "Jump";
        public const string VarGrounded = "Grounded";
        public const string VarFall = "Fall";
        public const string VarHit = "HitKind";



        public Animator Animator;

        public Player player;

        

        public void Update()
        {
            Animator.SetBool(VarRun, player.playerMovementController.IsRunning);
            Animator.SetBool(VarIdle, player.playerMovementController.IsIdle);
            Animator.SetBool(VarJump, player.playerMovementController.IsJumping);
            Animator.SetBool(VarGrounded, player.playerMovementController.IsGrounded);
            Animator.SetBool(VarFall, player.playerMovementController.IsFalling);
            Animator.SetInteger(VarHit, (int)player.defense.currentHit);

            if(Animator.GetBool(VarFall) == true)
            {

            }
            

        }
    }
}
