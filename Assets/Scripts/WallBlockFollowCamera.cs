using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBlockFollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject virtualcam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(virtualcam.transform.position.x-6.9f, 0, 0);
    }
}
