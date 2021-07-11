using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    class PlayerAnimation
    {

        public const int AnimLayerMovement = 0;

        public const string VarIdle = "Idle";
        public const string VarRun = "Run";
        public const string VarJump = "Jump";
        public const string VarGrounded = "Grounded";
        public const string VarFall = "Fall";
        public const string VarHit = "HitKind";

        

        public Animator Animator { get; private set; }

        private PlayerMovementController movementInstance;
        private AnimatorClipInfo[] currentClipInfo;
        private float currentClipLength;
        public PlayerAnimation(PlayerMovementController movementInstance, Animator animator)
        {
            this.movementInstance = movementInstance;
            Animator = animator;

            currentClipInfo = Animator.GetCurrentAnimatorClipInfo(AnimLayerMovement);
            currentClipLength = currentClipInfo[0].clip.length;
        }

        public void UpdateMotion()
        {
            Animator.SetBool(VarRun, movementInstance.IsRunning);
            Animator.SetBool(VarIdle, movementInstance.IsIdle);
            Animator.SetBool(VarJump, movementInstance.IsJumping);
            Animator.SetBool(VarGrounded, movementInstance.IsGrounded);
            Animator.SetBool(VarFall, movementInstance.IsFalling);
            Animator.SetInteger(VarHit, (int) movementInstance.Hitting);

            if(Animator.GetBool(VarFall) == true)
            {

            }
        }
    }
}
