using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickEventManager : MonoBehaviour
{
    Rigidbody2D BrickRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        BrickRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Other: " + other.gameObject.tag);
        if (other.gameObject.tag == "Player"){
            BrickRigidBody.AddForce(new Vector2(0, BrickRigidBody.mass*50));
	    }
        if(other.gameObject.tag == "Enemies"){
            //store eneimies in list to bump
            Debug.Log("Enemy on block");
        }
    }
    bool ObjectMovedAndStopped(){
        return Mathf.Abs(BrickRigidBody.velocity.magnitude) < 0.01;
    }

    IEnumerator DisableHittable(){
        if (!ObjectMovedAndStopped()){
		yield  return  new  WaitUntil(() =>  ObjectMovedAndStopped());
	    }

        transform.localPosition  =  Vector3.zero;

    }


    

}
