using Assets.Scripts.Common;
using Assets.Scripts.Utils;
using Assets.Scripts.World;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class TrackChanger : MonoBehaviour
{
    public const string EndTrackObjName = "End Track";
    public const string StartTrackObjName = "Start Track";






    public Vector2 TravelVector
    {
        get => travelAngleToEnd.ToVector2();
    }
    public Vector3 TravelVector3
    {
        get => travelAngleToEnd.ToVector3(SnapAxis.Y);
    }

    /// <summary>
    /// Rotação do trecho final
    /// </summary>
    public float EndRotation
    {
        get => endTrackObj.transform.eulerAngles.y;
    }

    /// <summary>
    /// Rotação do trecho de inicio
    /// </summary>
    public float StartRotation
    {
        get => startTrackObj.transform.eulerAngles.y;
    }


    public float Y
    {
        get => gameObject.transform.position.y;
    }




    public bool bidirectional = true;
    [Range(0f, 360f)]
    public float travelAngleToEnd = 0f;


    public Direction2D startDirection = Direction2D.Right;
    public Direction2D endDirection = Direction2D.Left;
    public float checkSize = 0.5f;
    public Renderer masterRender;
    public Collider masterCollider;

    [SerializeField]
    private Track endTrack;
    [SerializeField]
    private Track startTrack;


    private GameObject endTrackObj, startTrackObj;
    private BoxCollider endTrackBc, startTrackBc;

    [SerializeField]
    private Vector3 _size = Vector3.one;


    [Header("Gizmos")]
    public float gsize = 0.5f;

    private Vector3? colliderPos;
    private bool colliderResult;
    private bool masterColliderInit;

    public Track GetEndTrack()
    {

        return GetTrack(true);

    }



    public Track GetStartTrack()
    {

        return GetTrack(false);
    }
    public Track GetTrack(bool isTheEndTrack)
    {
        float rotation;
        Vector2 bcAngleVector;
        BoxCollider bc;

        if (isTheEndTrack)
        {
            rotation = -EndRotation;
            bc = endTrackBc;
        }
        else
        {
            rotation = -StartRotation + 180;
            bc = startTrackBc;
        }

        bcAngleVector = (rotation).ToVector2();


        
        var point = bc.bounds.center.ToVector2(SnapAxis.Y);
        float offsetX = ((bc.size.x / 2) + checkSize) * bcAngleVector.y;
        float offsetZ = ((bc.size.x / 2) + checkSize) * bcAngleVector.x;

        point.y += offsetX;
        point.x += offsetZ;
        return new Track(point, rotation);
    }

    private void Awake()
    {

        
    }

    // Start is called before the first frame update
    void Start()
    {

        
        SharedInit();

    }

    

    private void SharedInit(bool renderOnly = true)
    {
        if (masterRender == null)
        {

            if (!TryGetComponent(out masterRender))
            {
                Debug.LogWarning("Não foi possível obter o render do objeto, referencie-o.");
            }
        }

        if (renderOnly)
            return;

        if(masterCollider == null)
        {
            MeshCollider meshcollider;
            if(TryGetComponent(out meshcollider))
            {
                masterCollider = meshcollider;
                masterCollider.tag = Constants.ChangeTrackFloorTag;
                masterColliderInit = true;
            } 
            else
            {
                Debug.LogWarning("Referencie o collider do objeto");
            }
            
        }

        if (endTrackObj == null) // Obtem ou cria os objetos
        {

            var childEnd = gameObject.transform.Find(EndTrackObjName);

            if(childEnd == null)
            {
                endTrackObj = new GameObject(EndTrackObjName);
                endTrackObj.layer = LayerMask.NameToLayer(Constants.TriggersLayer);
                endTrackObj.AddComponent<BoxCollider>();
                endTrackObj.transform.parent = gameObject.transform;
                endTrackObj.transform.localPosition = Vector3.zero;
            }
            else{
                endTrackObj = childEnd.gameObject;
            }

            endTrackBc = endTrackObj.GetComponent<BoxCollider>();

            

        }
        if (startTrackObj == null && bidirectional)
        {
            var childStart = gameObject.transform.Find(StartTrackObjName);

            if (childStart == null)
            {
                startTrackObj = new GameObject(StartTrackObjName);
                
                startTrackObj.layer = LayerMask.NameToLayer(Constants.TriggersLayer);
                startTrackObj.AddComponent<BoxCollider>();
                startTrackObj.transform.parent = gameObject.transform;
                startTrackObj.transform.localPosition = Vector3.zero;
            }
            else
            {
                startTrackObj = childStart.gameObject;
            }

            startTrackBc = startTrackObj.GetComponent<BoxCollider>();
           
            
           

        }


    }

    
    private void OnValidate()
    {
        if (this == null)
            return;
        if (!this.gameObject.activeSelf)
            return;

        SharedInit(false);

        if (!masterColliderInit && masterCollider != null)
        {
            masterCollider.tag = Constants.ChangeTrackFloorTag;
        }

            if (endTrackBc != null)
            {
                //this.endTrackBc.size = _size;
            }
        
        
    }

    

    // Update is called once per frame
    void Update()
    {
        

    }

    public bool IsOnEndTrack(Vector3 position, out RaycastHit info)
    {
        return IsOnTrack(position, true, out info);
    }
    public bool IsOnStartTrack(Vector3 position, out RaycastHit info)
    {
        return IsOnTrack(position, false, out info);
    }


    public bool IsOnTrack(Vector3 position, bool toEndTrack, out RaycastHit info)
    {
        this.colliderPos = position;


        Vector3 travelVector;
        if (toEndTrack)
        {
            travelVector = travelAngleToEnd.ToVector3(SnapAxis.Y);
        }
        else
        {
            travelVector = -travelAngleToEnd.ToVector3(SnapAxis.Y);
        }

        var ray = new Ray(position, travelVector);
        var col = Physics.Raycast(ray, out info, checkSize, LayerMask.GetMask(Constants.TriggersLayer));
        if (col)
        {
            Debug.DrawLine(position, info.point);
            this.colliderResult = true;
            return true;
        } else
        {
            this.colliderResult = false;
            return false;
        }

    }

    /// <summary>
    /// Obtem o ponto mais próximo do trecho final em relação à sua posição <paramref name="currentPosition"/> 
    /// </summary>
    /// 
    public Vector3 EndPositionEqualAt(Vector3 currentPosition)
    {
        currentPosition.z = GetEndTrack().Point.y;
        return currentPosition;
    }

    /// <summary>
    /// Obtem o ponto mais próximo do trecho incial em relação à sua posição <paramref name="currentPosition"/> 
    /// </summary>
    /// 
    public Vector3 StartPositionEqualAt(Vector3 currentPosition)
    {
        currentPosition.z = GetStartTrack().Point.y;
        return currentPosition;
    }

    private void OnDrawGizmos()
    {
        if (masterRender == null)
            return;

        CustomGizmos.DrawPlaneArrow(masterRender.bounds.center, travelAngleToEnd, gsize, this.Y);

        if (!colliderResult)
        {
            if (colliderPos != null)
            {
                var finalPos = colliderPos.Value;
                finalPos.x += TravelVector.x * checkSize;
                finalPos.z += TravelVector.y * checkSize;

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(colliderPos.Value, finalPos);
            }
        }

        if(endTrackObj != null)
        {
            var pos = Vector3.zero;
            var size = endTrackBc.size;

            Gizmos.color = new Color(255, 0, 0, 0.2f);
            Gizmos.DrawSphere(GetEndTrack().Point.ToVector3(SnapAxis.Y, this.Y), 0.05f);
            //size.z *= endTrack.transform.localScale.z; <--- wtf
            //size.x *= endTrack.transform.localScale.x;
            //size.y *= endTrack.transform.localScale.y;


            size.z += checkSize * 2;
            size.x += checkSize * 2;

            var matrixBak = Gizmos.matrix;
            Gizmos.matrix = endTrackObj.transform.localToWorldMatrix;
            
            Gizmos.DrawCube(pos, size);
            Gizmos.DrawWireCube(pos, size);
            Gizmos.color = Color.red;
            CustomGizmos.DrawPlaneArrow(pos, -90f * endDirection.ToInt(), gsize/2f, 0f, false);
            Gizmos.matrix = matrixBak;

        }
        if (startTrackObj != null)
        {
            var pos = Vector3.zero;
            var size = startTrackBc.size;

            Gizmos.color = new Color(0, 255, 0, 0.2f);
            Gizmos.DrawSphere(GetStartTrack().Point.ToVector3(SnapAxis.Y, this.Y), 0.05f);
            //size.z *= startTrack.transform.localScale.z;
            //size.x *= startTrack.transform.localScale.x;
            //size.y *= startTrack.transform.localScale.y;
            size.z += checkSize * 2;
            size.x += checkSize * 2;

            var matrixBak = Gizmos.matrix;
            Gizmos.matrix = startTrackObj.transform.localToWorldMatrix;
            
            Gizmos.DrawCube(pos, size);
            Gizmos.DrawWireCube(pos, size);
            Gizmos.color = Color.green;
            CustomGizmos.DrawPlaneArrow(pos, -90f * endDirection.ToInt(), gsize / 2f, 0f, false);
            Gizmos.matrix = matrixBak;
        }
        

        //


    }

    private void OnDrawGizmosSelected()
    {
        if(endTrackObj != null)
        {
            //Gizmos.DrawRay(EndTrack.Point.ToVector3(SnapAxis.Y, this.Y), Vector3.left);
            
        }

    }


}
