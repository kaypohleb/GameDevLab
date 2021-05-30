using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAlive = true;
    private int moveLeft = -1;
    public float enemySpeed = 4.0f;
    public float checkRadius = 0.35f;
    Transform feetPos;
    [SerializeField] LayerMask whatisGround;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    public bool isGrounded = false;
    void Start()
    {
      enemyBody = GetComponent<Rigidbody2D>();
      feetPos = gameObject.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatisGround);
        if(!isGrounded){
            enemyBody.constraints = RigidbodyConstraints2D.FreezePositionX;
            transform.rotation = Quaternion.Euler(transform.rotation.x,transform.eulerAngles.y,transform.rotation.z);
        }
        if(isGrounded && enemyBody.velocity.magnitude <enemySpeed){
            enemyBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y,transform.rotation.z);
            enemyBody.MovePosition(enemyBody.position + new Vector2(moveLeft*enemySpeed,0) * Time.fixedDeltaTime);
        }      
    
    }
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag=="Foreground" && isGrounded){
            moveLeft *= -1;
        } 
    }
}
