using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    Rigidbody2D mushroomBody;
    Transform feetPos;
    [SerializeField] LayerMask whatisGround;
    float moveInput;
    public bool randomDirection;
    public float MAXSPEED = 6f;
    public bool isGrounded = false;
    public float checkRadius = 0.5f;
    bool hasLanded;
    // Start is called before the first frame update
    void Start()
    {
        hasLanded = false;
        randomDirection = Random.value > 0.5f;
        mushroomBody = GetComponent<Rigidbody2D>(); 
        feetPos = gameObject.transform.GetChild(0);
        //mushroomBody.AddForce(Vector2.up * MAXSPEED,  ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatisGround);
        if(!isGrounded && hasLanded){
            mushroomBody.constraints = RigidbodyConstraints2D.FreezePositionX;
            transform.rotation = Quaternion.Euler(transform.rotation.x,transform.eulerAngles.y,transform.rotation.z);
        }
        if(isGrounded && mushroomBody.velocity.magnitude <MAXSPEED){
            mushroomBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y,transform.rotation.z);
            if(randomDirection){
                mushroomBody.MovePosition(mushroomBody.position + new Vector2(-1*MAXSPEED,0) * Time.fixedDeltaTime);
            }else{
                mushroomBody.MovePosition(mushroomBody.position + new Vector2(1*MAXSPEED,0) * Time.fixedDeltaTime);
            }
        }    
    }
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Mushroom:" + other.gameObject.name);
        if(other.gameObject.tag=="Foreground" && isGrounded){
            randomDirection = !randomDirection;
            hasLanded = true;
        }
    }
}
