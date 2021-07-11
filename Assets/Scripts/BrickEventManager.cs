using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickEventManager : MonoBehaviour
{
    public GameConstants gameConstants;
    Rigidbody2D BrickRigidBody;
    bool BlockOccupied;
    List<GameObject> ThingsOnBlock;
    [SerializeField] GameObject prefab;
    BoxCollider2D brickCollider;
    SpriteRenderer brickSprite;
    [SerializeField] GameObject TopCollider;
    public GameEvent OnBreakBlock;
    public GameEvent OnBumpBlock;
    // Start is called before the first frame update
    void Start()
    {
        BrickRigidBody = GetComponent<Rigidbody2D>();
        brickCollider = TopCollider.GetComponent<BoxCollider2D>();
        brickSprite = TopCollider.GetComponent<SpriteRenderer>();
        ThingsOnBlock =  new List<GameObject>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Other: " + other.gameObject.tag);
        //Debug.Log(broken);
        if (other.gameObject.tag == "Player"){
            OnBumpBlock.Raise();
            //Debug.Log("hit: " + other.gameObject.tag);
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
    
    public void onKill(){
        OnBreakBlock.Raise();
        for (int x =  0; x<gameConstants.spawnNumberOfDebris; x++){
			Instantiate(prefab, transform.position, Quaternion.identity);
		}
        Destroy(gameObject.transform.parent.gameObject);
    }
}
