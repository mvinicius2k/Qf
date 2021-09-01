using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public enum PlayerTemplate
    {
        Default = 0,
        Gun = 1
    }

    public class PlayerAnimation : MonoBehaviour
    {

        public const int AnimLayerMovement = 0;

        public const string VarIdle = "Idle";
        public const string VarRun = "Run";
        public const string VarJump = "Jump";
        public const string VarGrounded = "Grounded";
        public const string VarFall = "Fall";
        public const string VarHit = "HitKind";
        public const string VarTemplate = "Template";
        public const string VarShooted = "Shooted";
        public const string VarDeath = "Death";



        public Animator Animator;

        public Player player;

        public PlayerTemplate playerTemplate;

        public void Die()
        {
            Animator.SetTrigger(VarDeath);
        }

        public void Update()
        {

            Animator.SetFloat(VarTemplate, Convert.ToSingle((int) playerTemplate));
            Animator.SetBool(VarRun, player.playerMovementController.IsRunning);
            Animator.SetBool(VarIdle, player.playerMovementController.IsIdle);
            Animator.SetBool(VarJump, player.playerMovementController.IsJumping);
            Animator.SetBool(VarGrounded, player.playerMovementController.IsGrounded);
            Animator.SetBool(VarFall, player.playerMovementController.IsFalling);
            Animator.SetInteger(VarHit, (int)player.defense.currentHit);
            Animator.SetBool(VarShooted, player.playerCombatController.Shooted);
            
            

        }

        
    }
}
