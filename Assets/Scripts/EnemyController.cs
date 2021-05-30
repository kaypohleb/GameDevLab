using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAlive = true;
    private int moveLeft = 1;
    public float enemySpeed = 7.0f;
    public float checkRadius = 0.3f;
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
        if(isGrounded && enemyBody.velocity.magnitude <enemySpeed){
            enemyBody.AddForce(new Vector2(moveLeft*enemySpeed,0),ForceMode2D.Force);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag=="Foreground"){
            moveLeft *= -1;
        } 
    }
}
