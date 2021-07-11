using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameConstants gameConstants;
    public bool isAlive = true;
    private int moveLeft = -1;
    public float enemySpeed = 6.0f;
    public float checkRadius = 0.35f;
    Transform feetPos;
    [SerializeField] LayerMask whatisGround;
    [SerializeField] GameObject prefab;
    [SerializeField] float stuckLife = 0.1f;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    private SpriteRenderer enemySpriteRenderer;
    bool isGrounded = false;
    
    private float stuckTimer = 0.1f;
    private void Start(){
      enemyBody = GetComponent<Rigidbody2D>();
      feetPos = gameObject.transform.GetChild(0);
      enemySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyBody.velocity.x < 0.01f && enemyBody.velocity.x > -0.01f && isGrounded){
            if(stuckTimer>0){
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
        gameObject.SetActive(false);
    }
    public void  EnemyRejoice(){
        if(gameObject){
            StartCoroutine(rejoice());
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Destroy"){
            onDeath();
        }
        if(other.gameObject.tag == "Killable"){
            moveLeft *= -1;
        }
    } 
    IEnumerator rejoice(){
        enemyBody.constraints = RigidbodyConstraints2D.FreezeAll;
        for(int i = 0; i< 4; i++){
            enemySpriteRenderer.flipY = !enemySpriteRenderer.flipY;
            yield return new WaitForSeconds(0.5f);
        }
        
        enemyBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.rotation = Quaternion.Euler(transform.rotation.x,transform.eulerAngles.y,transform.rotation.z);
        }
}
