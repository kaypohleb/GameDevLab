using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] Transform cam;
    public float relativeMove = 0.3f;
    public float offset = 0;
    public bool backwards = true;

    // Update is called once per frame
    void Update()
    {
        if(backwards){
             transform.position = new Vector2((cam.position.x * -relativeMove) + offset, transform.position.y);
        }else{
             transform.position = new Vector2((cam.position.x * relativeMove) + offset, transform.position.y);
        }
       
    }
}
