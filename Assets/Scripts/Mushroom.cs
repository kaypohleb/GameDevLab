using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public  enum PowerType{
	    jump =  0,
        grow =  1
}
[System.Serializable]
public class PowerUp{
    public Sprite sprite;
    public PowerType powerType;
    public PowerUp(Sprite s, PowerType p){
        this.sprite  =  s;
		this.powerType  =  p;
	}
}

public class Mushroom : MonoBehaviour
{
    Rigidbody2D mushroomBody;
    Transform feetPos;
    [SerializeField] LayerMask whatisGround;
    float moveInput;
    [SerializeField] bool randomDirection;
    [SerializeField] float MAXSPEED = 6f;
    [SerializeField] bool isGrounded = false;
    [SerializeField] float checkRadius = 0.1f;
    public PowerUp powerUp;
    public float stuckLife = 0.1f;
    private float stuckTimer = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        switch(gameObject.tag){
            case "Grow":
                powerUp = new PowerUp(GetComponent<SpriteRenderer>().sprite, PowerType.grow);
                break;
            case "JumpBoost":
                powerUp = new PowerUp(GetComponent<SpriteRenderer>().sprite, PowerType.jump);
                break;
        }
        randomDirection = Random.value > 0.5f;
        mushroomBody = GetComponent<Rigidbody2D>(); 
        feetPos = gameObject.transform.GetChild(0);
        
        stuckTimer = stuckLife;
        //mushroomBody.AddForce(Vector2.up * MAXSPEED,  ForceMode2D.Impulse);
    } 
    private void Update() {
        if(mushroomBody.velocity.x < 0.01f && mushroomBody.velocity.x > -0.01f && isGrounded){
            if(stuckTimer>0){
                //Debug.Log("Stuck");
                stuckTimer -= Time.fixedDeltaTime;
            }else{
                stuckTimer = stuckLife;
                randomDirection = !randomDirection;
            }
        }
        else{
            stuckTimer = stuckLife;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatisGround);
        if(!isGrounded){
            mushroomBody.constraints = RigidbodyConstraints2D.FreezePositionX;
            transform.rotation = Quaternion.Euler(transform.rotation.x,transform.eulerAngles.y,transform.rotation.z);
        }
        if(isGrounded && mushroomBody.velocity.magnitude <MAXSPEED){
            mushroomBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y,transform.rotation.z);
            if(randomDirection){
                mushroomBody.AddForce(new Vector2(-1*MAXSPEED,0));
            }else{
                mushroomBody.AddForce(new Vector2(1*MAXSPEED,0));
            }
        }
        
    }
}
