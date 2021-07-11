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


[RequireComponent(typeof(PlayerCombatController))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{

    public GameObject rootPlayer;
    

    public bool freezeRotations = true;
    public float jumpForce = 3f;
    public float speed = 7f;
    public float defaultJumpTimer = 0.3f; //um décimo do valor de pulo é um bom timer
    public Vector3 feetCheckSize = new Vector3(0.3f, 0.03f, 0.36f);
    public float slopeCheckDistance = 0.3f;
    [Range(0f, 90f)]
    public float maxAngleMove = 45f;
    public bool dynamicMove = false;

    public Transform feetPos;
    public LayerMask ground;
    private Collider feetCollider, bodyCollider;

    private const float JumpPenalty = 2f;
    private const float HorizontalVelocityPenalty = 5f;

    private Direction2D front = Direction2D.Right;
    private Vector3 jumpVector, velocityVector; 
    private Vector3 inertia; //Vetor de inercia para usar em pulos e quedas
    private Slope slope;
    private Rigidbody rb;
    private PhysicMaterial oldFeetColliderMaterial;
    private Collider[] whatIsGround;
    private Animator anim;

    private bool isGrounded, isJumping;
    private float xInput;
    private bool autoRun;
    private bool doubleXInput;
    private float jumpTimer;
    private bool isOnChangeTrack;
    private bool isChangingToEndTrack, isChangingToStartTrack;
    private int trackChangerLerpCount;
    private TrackChangerState trackChangerState;
    private TrackChanger trackChanger;
    private PlayerAnimation playerAnimation;
    private HitKind hitting;
    private bool sliding;

    public bool IsRunning
    {
        get => autoRun || (isGrounded && xInput != 0f);
    }
    public bool IsIdle
    {
        get => IsGrounded && xInput == 0f && !isJumping && !autoRun;
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
        get => !isGrounded && !isJumping && rb.velocity.y < 0f;
    }
    public Vector3 VelocityVector 
    { 
        get => velocityVector;
    }
    public HitKind Hitting
    {
        get => hitting;
    }

    

    public void SetFront(Direction2D front)
    {
        if(this.front != front)
        {
            rootPlayer.transform.Rotate(new Vector3(0f, 180f, 0f));
        }
        this.front = front;
    }

    private void IsCollinding(Collision collision)
    {
        //Debug.Log($"{gameObject.tag} colidindo com {collision.gameObject.tag}");
    }

    /// <summary>
    /// Restaura todas as possibilidade de movimentação referente a quando o personagem está no chão
    /// </summary>
    public void RestoreMovement()
    {

        //Debug.Log("Restaurando movimento");

        if(oldFeetColliderMaterial != null)
        {
            feetCollider.material = oldFeetColliderMaterial;
            oldFeetColliderMaterial = null;
        }

        RestoreJump();
        
    }    

    private bool DetectGround()
    {
        return DetectGround(out _);
    }

    private bool DetectGround(out Collider[] info)
    {
        if(feetPos == null)
        {
            info = null;
            return false;
        }
            
        info = Physics.OverlapBox(feetPos.position, feetCheckSize / 2, Quaternion.identity, ground.value);
        var collidableGround = false;
        foreach(var i in info)
        {
            if (LayerMask.LayerToName(i.gameObject.layer) == Constants.GroundLayer)
            {
                collidableGround = true;
                break;
            }
                
        }
        //Debug.Log($"{nameof(overlapBoxCount)} = {overlapBoxCount}");
        return collidableGround && !isJumping && slope.angle < maxAngleMove; //Algo colidindo com os pés e o terreno não é tão íngrime
    }

    

    private void ChangeTrack(bool toEnd)
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
            isOnTrack = trackChanger.IsOnStartTrack(feetPos.position, out info);
            rotateDirection = isOnTrack ? -1 : 1;
        }

        rotateDirection *= front.ToInt();


        switch (trackChangerState)
        {
            case TrackChangerState.StartRotation:

                autoRun = true;
                rootPlayer.transform.Rotate(new Vector3(0f, trackChanger.travelAngleToEnd * rotateDirection, 0f));

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
                

                rb.velocity = velocityVector;

                if (isOnTrack)
                {
                    trackChangerState = TrackChangerState.FinalRotation;
                }
                break;

            case TrackChangerState.FinalRotation:
                rootPlayer.transform.eulerAngles = new Vector3(0f, Mathf.Abs(trackChanger.EndRotation) * trackChanger.startDirection.ToInt(), 0f);
                this.front = trackChanger.startDirection;


                velocityVector.Set(0, 0, 0);
                rb.velocity = velocityVector;

                

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

                
                if(trackChangerLerpCount == 5)
                { 
                    gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, newPosition, 0.2f); //Melhorar isso
                    trackChangerLerpCount++;
                } else
                {
                    this.isChangingToEndTrack = false;
                    this.isChangingToStartTrack = false;
                }

               
                
                break;

        }
    }

    private void BackupFeetMaterial()
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
    private void GetGroundProperties()
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
                        var sucess = w.TryGetComponent<TrackChanger>(out trackChanger);

                        if (sucess)
                            isOnChangeTrack = true;
                        else
                        {
                            Debug.LogError($"{w.name} não tem o componente {nameof(TrackChanger)}");
                            isOnChangeTrack = false;
                        }
                    }
                    

                    break;
            }

        }

        this.isOnChangeTrack = isOnChangeTrack;
        if (!isOnChangeTrack && trackChanger != null)
        {
            trackChanger = null;
        }
    }

  

    
    private void HorizontalMovement()
    {
        //Movimenta��o no eixo x
        xInput = GetHorizontal();

        if (isGrounded)
        {

            if(xInput > 0)
            {
                SetFront(Direction2D.Right);
            } 
            else if(xInput < 0)
            {
                SetFront(Direction2D.Left);
            }
        }
        



        float horizontalVelocity = xInput * speed;

        

        if (isGrounded && !slope.isOnSlope && !isJumping)
        {
            velocityVector.Set(horizontalVelocity, 0f, 0f);
            
            
        }
        else if (isGrounded && slope.isOnSlope && !isJumping && slope.angle < maxAngleMove)
        {
            velocityVector.Set(horizontalVelocity * -slope.normal.x, horizontalVelocity * -slope.normal.y, 0f);
            

        }
        else if(!isGrounded && slope.isOnSlope)
        {

            velocityVector.Set(rb.velocity.x, rb.velocity.y, 0f);

        }
        else
        {
            Debug.Log($"No ar, inercia = {inertia.x}");
            velocityVector.Set(inertia.x/JumpPenalty, rb.velocity.y, 0f);
            rb.AddForce(horizontalVelocity / HorizontalVelocityPenalty, 0f, 0f, ForceMode.VelocityChange);
        }

        rb.velocity = velocityVector;
    }

    private float GetHorizontal()
    {

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
                return xInput;
            }

            if (isLeft && xInput == 1f)
            {
                doubleXInput = true;
                return -1f;
            }

            if (isRight && xInput == -1f)
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

    public void SetFeetBounds()
    {
        if(feetCollider == null)
        {
            feetCollider = GetComponent<BoxCollider>();
        }

        if(feetPos != null)
        {
            feetPos.position = feetCollider.bounds.center;
            feetCheckSize = feetCollider.bounds.size + new Vector3(0.02f, 0.02f, 0.02f);
        }
        
        
    }
    
    /// <summary>
    /// Checagem de chãos inclinados
    /// </summary>
    private void SlopeCheck()
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
            slope = backSlope;
            Debug.DrawRay(raycastBack.point, raycastBack.normal, Color.red);

            if (raycastFront.point.y > raycastBack.point.y)
            {
                if(frontSlope.angle < maxAngleMove || !isGrounded || isJumping)
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
                if(backSlope.angle < maxAngleMove || !isGrounded || isJumping)
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


    
    private void RestoreJump()
    {
        
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

        var inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(45 - sensibility <= degress && degress <= 135f + sensibility) //Para cima
        {
            return inputVector.y == 1;
        }
        else if (135f - sensibility <= degress && degress <= 225f) //Para esquerda
        {
            return inputVector.x == -1;
        }
        else if(225f - sensibility <= degress && degress <= 315f + sensibility) //Para baixo
        {
            return inputVector.y == -1;
        }
        else if(inputVector != Vector2.zero) //Para frente
        {
            return true;
        }
        else //Parado
        {
            return false; 
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        IsCollinding(collision);

        inertia = Vector3.zero;

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        feetCollider = GetComponent<BoxCollider>();
        anim = rootPlayer.GetComponent<Animator>();
        playerAnimation = new PlayerAnimation(this, anim);

        oldFeetColliderMaterial = null;

        hitting = HitKind.NoHit;
    }

    private void Start()
    {
        if (freezeRotations)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        


    }
    private void Update()
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
            inertia = rb.velocity;
        }


        if(trackChanger != null)
        {
            if (! this.isChangingToEndTrack && ! this.isChangingToStartTrack && this.isOnChangeTrack) //Irá trocar de trecho
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
                inertia = rb.velocity;
                isJumping = true;
                jumpVector.Set(0, jumpForce, 0);
                rb.velocity = jumpVector;
                jumpTimer = defaultJumpTimer; //iniciando timer



            }

            if (Input.GetButton("Jump") && isJumping) //Acréscimo na força de pulo se botão segurado
            {
                if (jumpTimer > 0)
                {
                    velocityVector.Set(0, jumpForce, 0);
                    rb.velocity = velocityVector;
                    jumpTimer -= Time.deltaTime;
                }
                else
                {
                    isJumping = false; //Força de pulo interrompida ao soltar o botão... agora em queda livre
                }
            }

            if (Input.GetButtonUp("Jump"))
            {
                isJumping = false;  //Força de pulo interrompida ao soltar o botão... agora em queda livre
            }
        }

        playerAnimation.UpdateMotion();



    }

    private void FixedUpdate()
    {
        SlopeCheck();
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

    private void OnValidate()
    {

    }

    private void OnDrawGizmos()
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
        Gizmos.DrawWireCube(feetPos.position, feetCheckSize);

        Gizmos.color = Color.green;
        
    }
}
