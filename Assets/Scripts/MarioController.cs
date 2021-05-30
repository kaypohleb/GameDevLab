using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour
{
    Rigidbody2D marioBody;
    SpriteRenderer marioSpriteRenderer;
    BoxCollider2D marioBoxCollider;
    Animator marioAnimator;
    Transform feetPos;
    [SerializeField] LayerMask whatisGround;
    AudioManager audioManager;
    GameController gameController;
    bool isAlive = true;
    bool grow = false;
    public float MAXSPEED = 6f;
    public float upSpeed = 9f;
    public float bounceSpeed = 10f;
    public float upBoost = 0.7f;
    public float deathBoost = 3f;
    public float speed = 15f;
    public float jumpTime = 0.15f;
    float jumpTimeCounter = 0;
    public bool isGrounded = true;
    public bool isJumping = false;
    public float checkRadius = 0.3f;
    private bool faceRightState = true;
    private float moveInput;
    private bool hurtRecently;
    public bool touchedPole = false;
    public bool touchedEnding = false;
    private bool preventMovement = false;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate =  30;
        feetPos = gameObject.transform.GetChild(0);
        marioBody = GetComponent<Rigidbody2D>();
        marioSpriteRenderer = GetComponent<SpriteRenderer>();
        marioBoxCollider = GetComponent<BoxCollider2D>();
        gameController = FindObjectOfType<GameController>();
        audioManager = FindObjectOfType<AudioManager>();
        marioAnimator = GetComponent<Animator>();
        marioAnimator.SetBool("Alive", true);
        Physics2D.queriesStartInColliders = true;
    }

    // Update is called once per frame

    void Update(){
        if(isAlive){
            if (Input.GetKeyDown(KeyCode.A) && faceRightState && !preventMovement){
                faceRightState = false;
                marioSpriteRenderer.flipX = true;
            }
            if (Input.GetKeyDown(KeyCode.D) && !faceRightState && !preventMovement){
                faceRightState = true;
                marioSpriteRenderer.flipX = false;
            }
            // Jumping Mechanic
            if(Input.GetKeyUp(KeyCode.Space)){
                isJumping = false;
            }
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping && !preventMovement){
                audioManager.playJumpSound(grow);
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                isGrounded = false;
                isJumping = true;
                jumpTimeCounter = jumpTime; 
                marioAnimator.SetBool("Jumping", true);
            }
            //add Jump height if space is held down
            if(Input.GetKey(KeyCode.Space) && isJumping && !preventMovement){
                if(jumpTimeCounter>0){
                    marioBody.AddForce(Vector2.up * upBoost, ForceMode2D.Impulse);
                    jumpTimeCounter-=Time.deltaTime;
                }else{
                    isJumping = false;
                }
            }

            if(marioBody.velocity.x < 0.05f && marioBody.velocity.x > -0.05f && isGrounded){
                marioAnimator.SetBool("Running", false);
            }else if(marioBody.velocity.x < 1f && marioBody.velocity.x > -1f && isGrounded){
                marioAnimator.SetBool("Running", false);
            }

            if(Mathf.Abs(marioBody.velocity.x) > 0 && isGrounded == true){
                marioAnimator.SetBool("Running", true);
            }
            if(isGrounded){
                marioAnimator.SetBool("Jumping", false);
            } 

            //slowing down
            if((moveInput > 0 && marioBody.velocity.x < 0) || (moveInput < 0 && marioBody.velocity.x > 0)){
                marioAnimator.SetBool("Skidding", true);
            }else{
                marioAnimator.SetBool("Skidding", false);
            }
        }
       
    }
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatisGround);
        moveInput = Input.GetAxis("Horizontal");
        if(Mathf.Abs(moveInput) > 0 && !preventMovement){
            if (marioBody.velocity.magnitude < MAXSPEED){
               marioBody.AddForce(new Vector2(moveInput * speed,0));
            }
        }
        if(touchedPole && isGrounded && preventMovement){
            if (marioBody.velocity.magnitude < MAXSPEED){
               Debug.Log("Pushing Mario");
               marioBody.AddForce(new Vector2(1 * speed,0));
            }
        }
    }

    private bool IsDescending() {
        return marioBody.velocity.y < 0;
    }

    private void BounceAfterKill(){
        Debug.Log("Bounced");
        audioManager.playStompSound();
        marioBody.AddForce(Vector2.up * bounceSpeed,  ForceMode2D.Impulse);
    }

    private void OnDeath(){
        Debug.Log("I died");
        audioManager.playDieSound();
        marioBoxCollider.isTrigger = true;
        marioBody.constraints = RigidbodyConstraints2D.FreezePositionX;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y,transform.rotation.z);
        marioBody.AddForce(Vector2.up * deathBoost,  ForceMode2D.Impulse);
        
    }

    private void GrowControl(bool g){
        if(g){
                Vector2 prevVel = marioBody.velocity;
                transform.position= new Vector2(transform.position.x,transform.position.y + 0.5f);
                marioBoxCollider.offset = new Vector2(marioBoxCollider.offset.x,-0.1f);
                marioBoxCollider.size = new Vector2(marioBoxCollider.size.x,1.8f);
                StartCoroutine(growing());
                marioAnimator.SetBool("Growth", true);
                marioBody.velocity = prevVel;
            }else{
                Vector2 prevVel = marioBody.velocity;
                marioBoxCollider.size = new Vector2(marioBoxCollider.size.x,0.9f);
                marioBoxCollider.offset = new Vector2(marioBoxCollider.offset.x,-0.05f);
                StartCoroutine(hurting());
                marioAnimator.SetBool("Growth", false);
                transform.position= new Vector2(transform.position.x,transform.position.y -0.5f);
                marioBody.velocity = prevVel;
            }
            audioManager.playPowerSound(g);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Collision: " + other.gameObject.tag);
        switch(other.gameObject.tag){
            case "Pole":
                touchedPole = true;
                audioManager.playPoleSound();
                StartCoroutine(slideDown());
                break;

        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Trigger: " + other.gameObject.tag);
        switch (other.gameObject.tag)
        {
            case "HitBox":
                if(isAlive){
                    if(grow){
                        grow = false;
                        GrowControl(false);  
                    }else if(!hurtRecently){
                        isAlive = false;
                        marioAnimator.SetBool("Alive", false);
                        OnDeath();
                    }
                }
                
                break;
            case "HurtBox":
                if(isAlive && IsDescending() && !hurtRecently){
                    BounceAfterKill();
                    Destroy(other.transform.parent.gameObject);
                    gameController.killedEnemyScore();
                }
                break;
            case "Grow":
                if(!grow && isAlive){
                    grow = true;
                    GrowControl(true);
                    gameController.growScore();
                    Destroy(other.gameObject);
                }
                break;
            case "Destroy":
                isAlive = false;
                Destroy(gameObject);
                break;
            case "End":
                touchedEnding = true;
                break;
        }
        
    }

    IEnumerator hurting()  //  <-  its a standalone method
    {
        preventMovement = true;
        hurtRecently = true;
        marioBody.constraints = RigidbodyConstraints2D.FreezeAll;
        feetPos.position = new Vector3(feetPos.position.x, feetPos.position.y+0.5f, feetPos.position.z);
        marioAnimator.SetBool("ChangingSize", true);
        yield return new WaitForSeconds(0.5f);
        hurtRecently = false;
        marioAnimator.SetBool("ChangingSize", false);
        marioBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.rotation = Quaternion.Euler(transform.rotation.x,transform.eulerAngles.y,transform.rotation.z);
        preventMovement = false;
    }

    IEnumerator growing()  //  <-  its a standalone method
    {
        preventMovement = true;
        marioBody.constraints = RigidbodyConstraints2D.FreezeAll;
        feetPos.position = new Vector3(feetPos.position.x, feetPos.position.y-0.5f, feetPos.position.z);
        marioAnimator.SetBool("ChangingSize", true);
        yield return new WaitForSeconds(0.5f);
        marioAnimator.SetBool("ChangingSize", false);
        marioBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.rotation = Quaternion.Euler(transform.rotation.x,transform.eulerAngles.y,transform.rotation.z);
        preventMovement = false;
    }
    IEnumerator slideDown()  //  <-  its a standalone method
    {   
        marioBody.gravityScale = marioBody.gravityScale/3;
        while(!isGrounded){
            
            marioBody.constraints = RigidbodyConstraints2D.FreezePositionX;
            transform.rotation = Quaternion.Euler(transform.rotation.x,transform.eulerAngles.y,transform.rotation.z);
            marioAnimator.SetBool("Pole", true);
            yield return new WaitForSeconds(0.5f);
            
        }
        Debug.Log("Out of Slide");
        marioBody.gravityScale = marioBody.gravityScale*3;
        marioBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y,transform.rotation.z);
        marioAnimator.SetBool("Pole", false);
        preventMovement = true;
    }
    
}
