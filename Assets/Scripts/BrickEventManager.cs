using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickEventManager : MonoBehaviour
{
    Rigidbody2D BrickRigidBody;
    bool BlockOccupied;
    List<GameObject> ThingsOnBlock;
    // Start is called before the first frame update
    void Start()
    {
        BrickRigidBody = GetComponent<Rigidbody2D>();
        ThingsOnBlock =  new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Other: " + other.gameObject.tag);
        if (other.gameObject.tag == "Player"){
            BrickRigidBody.AddForce(new Vector2(0, BrickRigidBody.mass*50));
	    }else{
            //store gameobject
            ThingsOnBlock.Add(other.gameObject);
        }
        
    }
    private void OnCollisionExit(Collision other) {
        if(other.gameObject.tag != "Player"){
            //store eneimies in list to bump
            Debug.Log("Enemy on block");
            ThingsOnBlock.Remove(other.gameObject);
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
