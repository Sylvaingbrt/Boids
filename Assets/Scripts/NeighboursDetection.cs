
using System.Collections.Generic;
using UnityEngine;

public class NeighboursDetection : MonoBehaviour
{

    private List<Transform> neighbours;

    void Awake()
    {
        neighbours = new List<Transform>();
    }

    private void Start() {
        Vector2 bounds = GlobalVariables.Instance.GetBoundaries();
        for(int i=-1; i<=1;i++){
            for(int j=-1; j<=1;j++){
                CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D>();
                circleCollider.isTrigger = true;
                circleCollider.radius = GlobalVariables.Instance.detectionRadius;
                circleCollider.offset = new Vector2(2*i*bounds.x,2*j*bounds.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        neighbours.Add(other.transform);
    }

    private void OnTriggerExit2D(Collider2D other) {
        neighbours.Remove(other.transform);
    }

    public List<Transform> GetNeighbours(){
        return neighbours;
    }

}
