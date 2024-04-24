using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBoundManager : MonoBehaviour
{

    private void LateUpdate() {
        Vector2 position = transform.position;
        Vector2 boundaries = GlobalVariables.Instance.GetBoundaries();

        if(position.x<boundaries.x && position.x>-boundaries.x && position.y<boundaries.y && position.y>-boundaries.y){
            //Nothing to do
        }
        else{            
            if(position.x>boundaries.x){
                position.x -= 2*boundaries.x;
            }
            else if(position.x<-boundaries.x){
                position.x += 2*boundaries.x;
            }
            if(position.y>boundaries.y){
                position.y -= 2*boundaries.y;
            }
            else if(position.y<-boundaries.y){
                position.y += 2*boundaries.y;
            }
            transform.position=position;
        }
    }
}
