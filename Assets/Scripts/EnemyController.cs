using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAlive = true;
    private int moveLeft = -1;
    public float enemySpeed = 6.0f;
    public float checkRadius = 0.35f;
    Transform feetPos;
    [SerializeField] LayerMask whatisGround;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    public bool isGrounded = false;
    public GameObject prefab;
    public float stuckLife = 0.1f;
    private float stuckTimer = 0.1f;
    void Start()
    {
      enemyBody = GetComponent<Rigidbody2D>();
      feetPos = gameObject.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyBody.velocity.x < 0.01f && enemyBody.velocity.x > -0.01f && isGrounded){
            if(stuckTimer>0){
                Debug.Log("Stuck");
                stuckTimer -= Time.fixedDeltaTime;
            }else{
                stuckTimer = stuckLife;
                moveLeft *= -1;
            }
        }
        else{
            stuckTimer = stuckLife;
        }
         
    
    }
    private void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatisGround);
        if(!isGrounded){
            enemyBody.constraints = RigidbodyConstraints2D.FreezePositionX;
            transform.rotation = Quaternion.Euler(transform.rotation.x,transform.eulerAngles.y,transform.rotation.z);
        }
        if(isGrounded && enemyBody.velocity.magnitude <enemySpeed){
            enemyBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y,transform.rotation.z);
            enemyBody.AddForce(new Vector2(moveLeft*enemySpeed,0));
        }     
    }
    public void onDeath() {
        if(prefab != null){
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}
