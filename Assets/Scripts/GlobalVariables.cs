using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{

    public static GlobalVariables Instance {get; private set;}

    [SerializeField] private Vector2 boundaries;
    private Vector2 blPoint;
    private Vector2 tlPoint;
    private Vector2 trPoint;
    private Vector2 brPoint;

    [SerializeField] private int numberOfEntities;
    [SerializeField] private Transform pfEntities;


    //FISHES VARIABLES
    [Range(-1f,1f)]
    public float fovThreshold = -0.4f;

    [Range(0,5f)]
    public float sepatationFactor = 1f;

    [Range(0,5f)]
    public float alignementFactor = 1f;

    [Range(0,5f)]
    public float cohesionFactor = 1f;
    public float detectionRadius = 2f;


    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numberOfEntities; i++) {
            var entity = Instantiate(pfEntities, new Vector2(Random.Range(-boundaries.x,boundaries.x), Random.Range(-boundaries.y,boundaries.y)),Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI() {
        if (Instance != null) {
            blPoint = RectTransformUtility.WorldToScreenPoint(Camera.main,new Vector2(-boundaries.x,-boundaries.y));
            tlPoint = RectTransformUtility.WorldToScreenPoint(Camera.main,new Vector2(-boundaries.x,boundaries.y));
            trPoint = RectTransformUtility.WorldToScreenPoint(Camera.main,new Vector2(boundaries.x,boundaries.y));
            brPoint = RectTransformUtility.WorldToScreenPoint(Camera.main,new Vector2(boundaries.x,-boundaries.y));
            //Debug.Log("Draw");
            Drawing.DrawLine(blPoint, tlPoint,Color.white,2f,false);
            Drawing.DrawLine(tlPoint, trPoint,Color.white,2f,false);
            Drawing.DrawLine(trPoint, brPoint,Color.white,2f,false);
            Drawing.DrawLine(brPoint, blPoint,Color.white,2f,false);
        }
    }

    public Vector2 GetBoundaries(){
        return boundaries;
    }

    public void ChangeValue(SliderValueChanged.Type type, float value)
    {
        switch (type) {
            case SliderValueChanged.Type.fov:
                fovThreshold = value;
                break;
            case SliderValueChanged.Type.separation:
                sepatationFactor = value;
                break;
            case SliderValueChanged.Type.alignement:
                alignementFactor = value;
                break;
            case SliderValueChanged.Type.cohesion:
                cohesionFactor = value;
                break;
            case SliderValueChanged.Type.detection:
                detectionRadius = value;
                break;
            default:
                Debug.Log($"Unknown type: {type}");
                break;
        }
    }

    public float GetValue(SliderValueChanged.Type type)
    {
        switch (type) {
            case SliderValueChanged.Type.fov:
                return fovThreshold;
            case SliderValueChanged.Type.separation:
                return sepatationFactor;
            case SliderValueChanged.Type.alignement:
                return alignementFactor;
            case SliderValueChanged.Type.cohesion:
                return cohesionFactor;
            case SliderValueChanged.Type.detection:
                return detectionRadius;
            default:
                Debug.Log($"Unknown type: {type}");
                break;
        }
        return 0;
    }
}
