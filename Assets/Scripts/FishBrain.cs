using System.Collections.Generic;
using UnityEngine;

public class FishBrain : MonoBehaviour
{

    private NeighboursDetection neighboursDetection;
    private FishMovement fishMovement;
    private InBoundManager inBoundManager;
    private List<Transform> neighboursToConsider;
    public bool showNeighbours = false;

    private Vector3 debugSteerCohesion;
    private Vector3 debugSteerAlignement;
    private Vector3 debugSteerSeparation;
    private Vector3 debugMovement;
    [SerializeField] private float sepatationFactorBase = 0.05f;
    [SerializeField] private float alignementFactorBase = 0.03f;
    [SerializeField] private float cohesionFactorBase = 0.07f;
    

    // Start is called before the first frame update
    void Start()
    {
        neighboursDetection = GetComponentInChildren<NeighboursDetection>();
        fishMovement = GetComponent<FishMovement>();
        inBoundManager = GetComponent<InBoundManager>();
        neighboursToConsider = new List<Transform>();
        float angle = Random.Range(0,360f);
        Vector2 movement = new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
        fishMovement.SetMouvement(movement);
    }

    // Update is called once per frame
    void Update()
    {
        CheckNeighbours(neighboursDetection.GetNeighbours());
        debugMovement = fishMovement.GetMovement();
        debugSteerSeparation = SteerSeparation();
        debugSteerAlignement = SteerAlignement();
        debugSteerCohesion = SteerCohesion();
        Vector3 target =    (   debugMovement
                                +sepatationFactorBase*GlobalVariables.Instance.sepatationFactor*debugSteerSeparation
                                +alignementFactorBase*GlobalVariables.Instance.alignementFactor*debugSteerAlignement
                                +cohesionFactorBase*GlobalVariables.Instance.cohesionFactor*debugSteerCohesion
                            ).normalized;
        fishMovement.SetMouvement(target);
    }

    private void OnGUI() {
        if(showNeighbours){
            Vector3 myPos = RectTransformUtility.WorldToScreenPoint(Camera.main,transform.position);
            myPos.y=Screen.height-myPos.y;
            if(neighboursToConsider.Count>0){
                foreach(Transform t in neighboursToConsider){
                    Vector2 otherPos = RectTransformUtility.WorldToScreenPoint(Camera.main,t.position);
                    otherPos.y=Screen.height-otherPos.y;
                    Drawing.DrawLine(myPos, otherPos,Color.green,2f,false);
                }
            }
            Vector3 cDebugMovement = RectTransformUtility.WorldToScreenPoint(Camera.main,transform.position+debugMovement);
            cDebugMovement.y=Screen.height-cDebugMovement.y;
            Vector3 cDebugSteerSeparation = RectTransformUtility.WorldToScreenPoint(Camera.main,transform.position+debugSteerSeparation);
            cDebugSteerSeparation.y=Screen.height-cDebugSteerSeparation.y;
            Vector3 cDebugSteerAlignement = RectTransformUtility.WorldToScreenPoint(Camera.main,transform.position+debugSteerAlignement);
            cDebugSteerAlignement.y=Screen.height-cDebugSteerAlignement.y;
            Vector3 cDebugSteerCohesion = RectTransformUtility.WorldToScreenPoint(Camera.main,transform.position+debugSteerCohesion);
            cDebugSteerCohesion.y=Screen.height-cDebugSteerCohesion.y;
            Drawing.DrawLine(myPos, cDebugMovement,Color.blue,2f,false);
            Drawing.DrawLine(myPos, cDebugSteerSeparation,Color.red,2f,false);
            Drawing.DrawLine(myPos, cDebugSteerAlignement,Color.magenta,2f,false);
            Drawing.DrawLine(myPos, cDebugSteerCohesion,Color.white,2f,false);
        }  
    }


    private void CheckNeighbours(List<Transform> neighbours){
        neighboursToConsider.Clear();
        Vector2 myDir = fishMovement.GetMovement().normalized;
        Vector2 otherDir;
        foreach(Transform t in neighbours){
            Vector3 tpos = PositionWrappedCorrection(t.position);
            otherDir = (tpos - transform.position).normalized;
            if(Vector3.Dot(myDir,otherDir)>GlobalVariables.Instance.fovThreshold){
                neighboursToConsider.Add(t);
            }
        }
    }

    private Vector3 SteerSeparation(){
        Vector3 steerDirection = Vector3.zero;
        foreach(Transform t in neighboursToConsider){
            Vector3 tpos = PositionWrappedCorrection(t.position);
            if((tpos - transform.position).sqrMagnitude!=0){
                float ratio = 1/(tpos - transform.position).sqrMagnitude;
                steerDirection -= (tpos - transform.position)*ratio;
            }
        }
        return steerDirection;
    }

    private Vector3 SteerAlignement(){
        Vector3 steerDirection = Vector3.zero;
        foreach(Transform t in neighboursToConsider){
            steerDirection += t.GetComponentInParent<FishMovement>().GetMovement()*1/(float)neighboursToConsider.Count;
        }
        return steerDirection;
    }

    private Vector3 SteerCohesion(){
        Vector3 steerDirection = Vector3.zero;
        foreach(Transform t in neighboursToConsider){
            Vector3 tpos = PositionWrappedCorrection(t.position);
            steerDirection += tpos;
        }
        if(neighboursToConsider.Count>0){
            steerDirection *= 1/(float)neighboursToConsider.Count;
            steerDirection-=transform.position;
        }
        return steerDirection;
    }

    private Vector3 PositionWrappedCorrection(Vector3 pos){
        //Check if neighbours is not wrapped at the other side of the map, and correct the position if so.
        Vector3 tpos =pos;
        if((pos - transform.position).sqrMagnitude>GlobalVariables.Instance.detectionRadius*GlobalVariables.Instance.detectionRadius){
            Vector2 bounds = GlobalVariables.Instance.GetBoundaries();
            if(tpos.x-transform.position.x>bounds.x){
                tpos.x-=2*bounds.x;
            }
            else if(tpos.x-transform.position.x<-bounds.x){
                tpos.x+=2*bounds.x;
            }

            if(tpos.y-transform.position.y>bounds.y){
                tpos.y-=2*bounds.y;
            }
            else if(tpos.y-transform.position.y<-bounds.y){
                tpos.y+=2*bounds.y;
            }
        }
        return tpos;
    }
}
