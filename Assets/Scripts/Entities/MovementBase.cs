using Assets.Scripts.Common;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public abstract class MovementBase : MonoBehaviour
    {
        protected const float JumpPenalty = 2f;
        protected const float HorizontalVelocityPenalty = 5f;

        public GameObject model;

        public Vector2 inputVector;
        public float jumpForce = 3f;
        public float speed = 7f;
        public float defaultJumpTimer = 0.3f; //um décimo do valor de pulo é um bom timer
        public Vector3 feetCheckSize = new Vector3(0.3f, 0.03f, 0.36f);

        //Autos
        public float minFeetCheckSizeOffset = 0.02f;
        public float extendedFeetCheckSizeOffset = 0.1f;

        public float slopeCheckDistance = 0.3f;
        [Range(0f, 90f)]
        public float maxAngleMove = 45f;
        public Transform feetPos;
        
        public LayerMask ground;
        public ParticleSystem dustOnGrounding;

        public Collider feetCollider, bodyCollider;
        public Rigidbody rigidBody;

        [Header("Sound")]
        public AudioSource audioSource;
        public AudioClip[] steps;
        public float stepDelayInterval = 0.3f;
        public AudioClip jumpSound;

        protected Direction2D front = Direction2D.Right;
        protected Vector3 jumpVector, velocityVector;
        protected Vector3 inertia; //Vetor de inercia para usar em pulos e quedas
        protected Slope slope;
        protected PhysicMaterial oldFeetColliderMaterial;
        protected Collider[] whatIsGround;
        protected TrackChangerState trackChangerState;
        protected TrackChanger trackChanger;
        protected Animator anim;
        protected bool oldIsGrounded;
        protected bool isGrounded, isJumping;
        protected float jumpTimer;
        protected bool isOnChangeTrack;
        protected bool isChangingToEndTrack, isChangingToStartTrack;
        protected int trackChangerLerpCount;
        protected bool sliding;
        protected bool autoRun;

        
        protected float stepDeltaTimeCount;
        protected bool Grounding { get => !oldIsGrounded && isGrounded; }
        public bool IsRunning
        {
            get => autoRun || (isGrounded && inputVector.x != 0f);
        }
        public bool IsIdle
        {
            get => IsGrounded && inputVector.x == 0f && !isJumping && !autoRun;
        }
        public bool IsGrounded
        {
            get => isGrounded;
        }
        public bool IsJumping
        {
            get => isJumping;
        }
        public bool IsFalling
        {
            get => !isGrounded && !isJumping && rigidBody.velocity.y < 0f;
        }
        public Vector3 VelocityVector
        {
            get => velocityVector;
        }


        /// <summary>
        /// Restaura todas as possibilidade de movimentação referente a quando o personagem está no chão
        /// </summary>
        public virtual void RestoreMovement()
        {

            //Debug.Log("Restaurando movimento");

            if (oldFeetColliderMaterial != null)
            {
                feetCollider.material = oldFeetColliderMaterial;
                oldFeetColliderMaterial = null;
            }


        }

        protected virtual bool DetectGround()
        {
            return DetectGround(out _);
        }

        protected virtual bool DetectGround(out Collider[] info)
        {
            if (feetCollider == null)
            {
                info = null;
                return false;
            }

            info = Physics.OverlapBox(feetCollider.bounds.center, feetCheckSize / 2, Quaternion.identity, ground.value);
            var collidableGround = false;
            foreach (var i in info)
            {
                if (LayerMask.LayerToName(i.gameObject.layer) == Constants.GroundLayer && i.gameObject.tag != Constants.PlayerTag)
                {
                    collidableGround = true;
                    break;
                }

            }
            //Debug.Log($"{nameof(overlapBoxCount)} = {overlapBoxCount}");
            return collidableGround && !isJumping && slope.angle < maxAngleMove; //Algo colidindo com os pés e o terreno não é tão íngrime
        }

        protected virtual void BackupFeetMaterial()
        {
            if (oldFeetColliderMaterial == null)
            {
                oldFeetColliderMaterial = new PhysicMaterial();
                oldFeetColliderMaterial.dynamicFriction = feetCollider.material.dynamicFriction;
                oldFeetColliderMaterial.staticFriction = feetCollider.material.staticFriction;
                oldFeetColliderMaterial.bounceCombine = feetCollider.material.bounceCombine;
                oldFeetColliderMaterial.frictionCombine = feetCollider.material.frictionCombine;



                feetCollider.material.dynamicFriction = 0f;
                feetCollider.material.staticFriction = 0f;
                feetCollider.material.bounceCombine = PhysicMaterialCombine.Multiply;
                feetCollider.material.frictionCombine = PhysicMaterialCombine.Multiply;
            }
        }

        /// <summary>
        /// Detecta as propriedades do ch�o que colidiu
        /// </summary>
        protected virtual void GetGroundProperties()
        {
            bool isOnChangeTrack = false;


            foreach (var w in whatIsGround)
            {
                switch (w.tag)
                {
                    case Constants.ChangeTrackFloorTag:
                        isOnChangeTrack = true;

                        if (trackChanger == null)
                        {
                            var sucess = w.TryGetComponent(out trackChanger);

                            if (sucess)
                            {
                                isOnChangeTrack = true;
                                Debug.Log("Tem sim" + trackChanger);

                            }
                            
                            else
                            {
                                Debug.LogError($"{w.name} não tem o componente {nameof(TrackChanger)}");
                                isOnChangeTrack = false;
                            }
                        }


                        break;


                }

            }

            //TrackChanger
            this.isOnChangeTrack = isOnChangeTrack;
            if (!isOnChangeTrack && trackChanger != null)
            {
                Debug.Log("Nularia" + trackChanger);
                //trackChanger = null;

            }

            //Pousando
            if (Grounding)
            {
                MakeDust();
                SetFeetBounds(new Vector3(
                            minFeetCheckSizeOffset,
                            extendedFeetCheckSizeOffset,
                            minFeetCheckSizeOffset));
            }

        }

        protected virtual void SetFeetBounds(Vector3 offset)
        {
            if (feetCollider == null)
            {
                Debug.LogWarning($"{nameof(feetCollider)} nulo");
            }

            if (feetPos != null)
            {
                //feetPos.position = feetCollider.bounds.center;
                feetCheckSize = feetCollider.bounds.size + offset;
            }

        }

        protected virtual void MakeDust()
        {
            if (dustOnGrounding == null)
                return;
            foreach (var w in whatIsGround)
            {
                if (Helpers.DustableTags.Contains(w.tag))
                {
                    dustOnGrounding?.Play();
                    return;
                }
            }

        }

        /// <summary>
        /// Checagem de chãos inclinados
        /// </summary>
        protected virtual void SlopeCheck()
        {
            if (isJumping)
            {
                slope.isOnSlope = false;
                return;
            }
            Slope frontSlope = Slope.NoSlope();
            Slope backSlope = Slope.NoSlope();




            var checkPosFront = feetPos.position;
            var checkPosBack = feetPos.position;
            checkPosFront.y += feetCheckSize.y / 2;
            checkPosFront.x += front == Direction2D.Right ? feetCheckSize.x / 2 : feetCheckSize.x / -2;
            checkPosBack.y += feetCheckSize.y / 2;
            checkPosBack.x -= front == Direction2D.Right ? feetCheckSize.x / 2 : feetCheckSize.x / -2;

            RaycastHit raycastFront, raycastBack;
            Ray rayFront = new Ray(checkPosFront, Vector3.down);
            Ray rayBack = new Ray(checkPosBack, Vector3.down);

            bool hitFront = Physics.Raycast(rayFront, out raycastFront, slopeCheckDistance, ground);
            bool hitBack = Physics.Raycast(rayBack, out raycastBack, slopeCheckDistance, ground);

            frontSlope = hitFront ? new Slope(raycastFront) : Slope.NoSlope();
            backSlope = hitBack ? new Slope(raycastBack) : Slope.NoSlope();


            if (hitFront && hitBack) //Considera o ray menos baixo
            {


                if (raycastFront.point.y > raycastBack.point.y)
                {
                    if (frontSlope.angle < maxAngleMove || !isGrounded || isJumping)
                    {
                        slope = frontSlope;
                        Debug.DrawRay(raycastFront.point, raycastFront.normal, Color.red);


                    }
                    else // O que está a frente é muito íngrime  
                    {
                        slope = backSlope;

                        sliding = !isGrounded;

                        Debug.DrawRay(raycastBack.point, raycastBack.normal, Color.red);
                    }
                }
                else
                {
                    if (backSlope.angle < maxAngleMove || !isGrounded || isJumping)
                    {
                        slope = backSlope;
                        Debug.DrawRay(raycastBack.point, raycastBack.normal, Color.red);

                    }
                    else
                    {
                        slope = frontSlope;

                        sliding = !isGrounded;
                        Debug.DrawRay(raycastFront.point, raycastFront.normal, Color.red);
                    }

                }





            }
            else //Considera o que colidir
            {
                if (hitFront)
                {
                    slope = frontSlope;
                    Debug.DrawRay(raycastFront.point, raycastFront.normal, Color.red);
                }
                else
                {
                    slope = backSlope;
                    Debug.DrawRay(raycastBack.point, raycastBack.normal, Color.red);
                }

            }


        }

        protected virtual void Jumping()
        {
            if (jumpTimer > 0)
            {
                velocityVector.Set(0, jumpForce, 0);
                rigidBody.velocity = velocityVector;
                jumpTimer -= Time.deltaTime;
            }
            else
            {
                isJumping = false; //Força de pulo interrompida ao soltar o botão... agora em queda livre
            }
        }

        protected virtual void Jump()
        {
            

            inertia = rigidBody.velocity;
            isJumping = true;
            jumpVector.Set(0, jumpForce, 0);
            rigidBody.velocity = jumpVector;
            jumpTimer = defaultJumpTimer; //iniciando timer

            SetFeetBounds(new Vector3(
                        minFeetCheckSizeOffset,
                        minFeetCheckSizeOffset,
                        minFeetCheckSizeOffset));

            MakeDust();
            if(audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }

        protected virtual void ChangeTrack(bool toEnd)
        {

            var info = new RaycastHit();

            bool isOnTrack;
            int rotateDirection;

            if (toEnd)
            {
                isOnTrack = trackChanger.IsOnEndTrack(feetPos.position, out info);
                rotateDirection = isOnTrack ? 1 : -1;
            }
            else
            {
                if(trackChanger == null)
                {
                    Debug.Log("Nulo");
                    trackChanger.bidirectional = trackChanger.bidirectional;
                }
                isOnTrack = trackChanger.IsOnStartTrack(feetPos.position, out info);
                rotateDirection = isOnTrack ? -1 : 1;
            }

            rotateDirection *= front.ToInt();


            switch (trackChangerState)
            {
                case TrackChangerState.StartRotation:
                    rigidBody.constraints &= ~RigidbodyConstraints.FreezePositionZ; //Liberando movimentação no eixo Z
                    autoRun = true;
                    model.transform.Rotate(new Vector3(0f, trackChanger.travelAngleToEnd * rotateDirection, 0f));

                    trackChangerState = TrackChangerState.Travel;
                    break;

                case TrackChangerState.Travel:

                    if (toEnd)
                    {
                        velocityVector = trackChanger.TravelVector.ToVector3(SnapAxis.Y) * speed;
                    }
                    else
                    {
                        velocityVector = trackChanger.TravelVector.ToVector3(SnapAxis.Y) * -speed;
                    }


                    rigidBody.velocity = velocityVector;

                    if (isOnTrack)
                    {
                        trackChangerState = TrackChangerState.FinalRotation;
                    }
                    break;

                case TrackChangerState.FinalRotation:
                    model.transform.eulerAngles = new Vector3(0f, Mathf.Abs(trackChanger.EndRotation) * trackChanger.startDirection.ToInt(), 0f);
                    this.front = trackChanger.startDirection;


                    velocityVector.Set(0, 0, 0);
                    rigidBody.velocity = velocityVector;



                    trackChangerState = TrackChangerState.Finalized;
                    break;

                case TrackChangerState.Finalized:

                    Vector3 newPosition;

                    autoRun = false;
                    if (toEnd)
                    {
                        newPosition = trackChanger.EndPositionEqualAt(gameObject.transform.position);
                    }
                    else
                    {
                        newPosition = trackChanger.StartPositionEqualAt(gameObject.transform.position);
                    }


                    if (trackChangerLerpCount == 5)
                    {
                        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, newPosition, 0.2f); //Melhorar isso
                        trackChangerLerpCount++;
                    }
                    else
                    {
                        this.isChangingToEndTrack = false;
                        this.isChangingToStartTrack = false;
                    }


                    rigidBody.constraints |= RigidbodyConstraints.FreezePositionZ; //Bloqueando movimentação no eixo Z

                    break;

            }
        }

        public virtual void SetFront(Direction2D front)
        {
            if (this.front != front)
            {
                transform.Rotate(new Vector3(0f, 180f, 0f));
            }
            this.front = front;
        }

        protected virtual void HorizontalMovement()
        {
            //Movimenta��o no eixo x
            //input = GetHorizontal();

            if (isGrounded)
            {

                if (inputVector.x > 0)
                {
                    SetFront(Direction2D.Right);
                }
                else if (inputVector.x < 0)
                {
                    SetFront(Direction2D.Left);
                }
            }




            float horizontalVelocity = inputVector.x * speed;



            if (isGrounded && !slope.isOnSlope && !isJumping)
            {
                velocityVector.Set(horizontalVelocity, 0f, 0f);


            }
            else if (isGrounded && slope.isOnSlope && !isJumping && slope.angle < maxAngleMove)
            {
                velocityVector.Set(horizontalVelocity * -slope.normal.x, horizontalVelocity * -slope.normal.y, 0f);


            }
            else if (!isGrounded && slope.isOnSlope)
            {

                velocityVector.Set(rigidBody.velocity.x, rigidBody.velocity.y, 0f);

            }
            else
            {
                //Debug.Log($"No ar, inercia = {inertia.x}");
                velocityVector.Set(inertia.x / JumpPenalty, rigidBody.velocity.y, 0f);
                rigidBody.AddForce(horizontalVelocity / HorizontalVelocityPenalty, 0f, 0f, ForceMode.VelocityChange);
            }

            rigidBody.velocity = velocityVector;
        }
        /// <summary>
        /// Diz se o ângulo <paramref name="degress"/> é parecido com o ângulo formado pelo vetor de inputs (Horizontal, Vertical)
        /// </summary>
        /// <param name="degress">ângulo em graus</param>
        /// <param name="sensibility">deslocamento do ângulo a considerar</param>
        /// <returns></returns>

        private bool InputDirectionLike(float degress, float sensibility = 0f)
        {


            degress %= 360f;

            if (degress < 0)
            {
                degress = 360f - degress;
            }

            //var inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (45 - sensibility <= degress && degress <= 135f + sensibility) //Para cima
            {
                return inputVector.y == 1;
            }
            else if (135f - sensibility <= degress && degress <= 225f) //Para esquerda
            {
                return inputVector.x == -1;
            }
            else if (225f - sensibility <= degress && degress <= 315f + sensibility) //Para baixo
            {
                return inputVector.y == -1;
            }
            else if (inputVector != Vector2.zero) //Para frente
            {
                return true;
            }
            else //Parado
            {
                return false;
            }

        }
        protected virtual void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            feetCollider = GetComponent<CapsuleCollider>();
            anim = model.GetComponent<Animator>();

            oldFeetColliderMaterial = null;

            //hitting = PlayerHitKind.NoHit;
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {

            inertia = Vector3.zero;

        }

        protected virtual void FixedUpdate()
        {
            SlopeCheck();
            oldIsGrounded = isGrounded;
            isGrounded = DetectGround(out whatIsGround);
            GetGroundProperties();




            if (isChangingToEndTrack || isChangingToStartTrack) //Está trocando de trecho
            {
                ChangeTrack(isChangingToEndTrack);

            }
            else
            {
                HorizontalMovement();
            }

            





        }

        protected virtual void OnDrawGizmos()
        {
            if (feetPos == null)
                return;

            var grounded = DetectGround();
            if (grounded)
            {
                Gizmos.color = Color.blue;
            }
            else
            {
                Gizmos.color = Color.white;
            }

            if (feetCollider != null)
                Gizmos.DrawWireCube(feetCollider.bounds.center, feetCheckSize);

            Gizmos.color = Color.green;

        }


        protected virtual void Update()
        {




            if (!isGrounded)
            {
                BackupFeetMaterial();

            }
            else
            {
                RestoreMovement();
            }

            if (!isGrounded && inertia.x == 0)
            {
                inertia = rigidBody.velocity;
            }

            

            if (trackChanger != null)
            {
                if (!this.isChangingToEndTrack && !this.isChangingToStartTrack && this.isOnChangeTrack) //Irá trocar de trecho
                {



                    if (!trackChanger.IsOnEndTrack(feetPos.position, out _) && InputDirectionLike(trackChanger.travelAngleToEnd))
                    {
                        this.isChangingToEndTrack = true;
                        this.trackChangerState = TrackChangerState.StartRotation;
                    }
                    else if (!trackChanger.IsOnStartTrack(feetPos.position, out _) && InputDirectionLike(trackChanger.travelAngleToEnd + 180f))
                    {
                        this.isChangingToStartTrack = true;
                        this.trackChangerState = TrackChangerState.StartRotation;
                    }

                }
            }





            if (!isChangingToEndTrack) //Não está trocando de trecho
            {
                if (isGrounded && Input.GetButtonDown("Jump")) //Corpo vai pular
                {
                    Jump();

                }

                if (Input.GetButton("Jump") && isJumping) //Acréscimo na força de pulo se botão segurado
                {
                    Jumping();
                }

                if (Input.GetButtonUp("Jump"))
                {
                    isJumping = false;  //Força de pulo interrompida ao soltar o botão... agora em queda livre
                }
            }

            stepDeltaTimeCount += Time.deltaTime;
            if (IsRunning && stepDeltaTimeCount > stepDelayInterval)
            {
                if (audioSource != null && steps.Length != 0)
                {
                    //UnityEngine.Random.InitState(Local);
                    audioSource.PlayOneShot(steps[UnityEngine.Random.Range(0, steps.Length)]);
                }
                stepDeltaTimeCount = 0f;
            }



        }
#if UNITY_EDITOR
        [CustomEditor(typeof(MovementBase), true)]
        private class MovementBaseEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                MovementBase script = (MovementBase)target;
                EditorGUILayout.Space();
                if (GUILayout.Button("Set Min Feet Bounds"))
                {
                    script.SetFeetBounds(new Vector3(
                        script.minFeetCheckSizeOffset,
                        script.minFeetCheckSizeOffset,
                        script.minFeetCheckSizeOffset));
                }
                if (GUILayout.Button("Set Extended Feet Bounds"))
                {
                    script.SetFeetBounds(new Vector3(
                        0f,
                        script.extendedFeetCheckSizeOffset,
                        0f));
                }

            }




        }
#endif
    }


}
