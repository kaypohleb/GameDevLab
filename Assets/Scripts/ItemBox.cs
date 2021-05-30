using System.Collections;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    Rigidbody2D rigidBody;
    SpringJoint2D springJoint;
    [SerializeField] GameObject consummablePrefab;
    private bool hit =  false;
    Animator ItemBoxAnimator;
    AudioManager audioManager;
    void Start()
    {
        ItemBoxAnimator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        springJoint = GetComponent<SpringJoint2D>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        ItemBoxAnimator.SetBool("Hit", false);
        hit =  false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Other: " + other.gameObject.tag);
        if (other.gameObject.tag == "Player" &&  !hit){
		    hit  =  true;
            audioManager.playBumpSound();
            rigidBody.AddForce(new Vector2(0, rigidBody.mass*50));
            Instantiate(consummablePrefab, new  Vector3(this.transform.position.x, this.transform.position.y  +  1.0f, this.transform.position.z), Quaternion.identity);
            ItemBoxAnimator.SetBool("Hit", true);
            StartCoroutine(DisableHittable());
	    }
    }
    bool ObjectMovedAndStopped(){
        return Mathf.Abs(rigidBody.velocity.magnitude) < 0.01;
    }

    IEnumerator DisableHittable(){
        if (!ObjectMovedAndStopped()){
		yield  return  new  WaitUntil(() =>  ObjectMovedAndStopped());
	    }

        transform.localPosition  =  Vector3.zero;
	    //continues here when the ObjectMovedAndStopped() returns true
	    rigidBody.bodyType  =  RigidbodyType2D.Static; // make the box unaffected by Physics
	    springJoint.enabled  =  false; // disable spring
    }
}
