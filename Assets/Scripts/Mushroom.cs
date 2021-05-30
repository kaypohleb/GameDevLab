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
    // Start is called before the first frame update
    void Start()
    {
        randomDirection = Random.value > 0.5f;
        mushroomBody = GetComponent<Rigidbody2D>(); 
        feetPos = gameObject.transform.GetChild(0);
        mushroomBody.AddForce(Vector2.up * MAXSPEED,  ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatisGround);
        //Debug.Log(mushroomBody.velocity.magnitude);
        float speed = MAXSPEED;
        if(isGrounded && mushroomBody.velocity.magnitude <MAXSPEED){
            if(mushroomBody.velocity.magnitude>0.1){
                speed = MAXSPEED/mushroomBody.velocity.magnitude;
            }
            if(randomDirection){
                mushroomBody.AddForce(new Vector2(speed,0));
            }else{
                mushroomBody.AddForce(new Vector2(-speed,0));
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag=="Foreground"){
            randomDirection = !randomDirection;
        } 
    }
}
