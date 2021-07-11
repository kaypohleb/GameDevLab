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
    BoxCollider2D hitBox;
    BoxCollider2D hurtBox;
    [SerializeField] LayerMask whatisGround;
    ParticleSystem marioParticles;
    public bool isAlive = true;
    public BoolReference grow;
    public bool boosting = false;
    [SerializeField] float MAXSPEED = 6f;
    [SerializeField] float upSpeed = 9f;
    [SerializeField] float bounceSpeed = 10f;
    [SerializeField] float upBoost = 0.7f;
    [SerializeField] float deathBoost = 3f;
    [SerializeField] float speed = 15f;
    [SerializeField] float jumpTime = 0.15f;
    float jumpTimeCounter = 0;
    [SerializeField] bool isGrounded = true;
    [SerializeField] bool isJumping = false;
    [SerializeField] float checkRadius = 0.3f;
    private bool faceRightState = true;
    private float moveInput;
    private bool hurtRecently;
    public bool touchedPole = false;
    public bool touchedEnding = false;
    private bool preventMovement = false;
    public bool keepStatus = false;
    public GameEvent onPlayerDeath;
    public GameEvent onPlayerTouchedPole;
    public GameEvent onPlayerTouchedEnding;
    public GameEvent onPlayerKill;
    public GameEvent onPlayerGrow;
    public GameEvent playerJump;
    public GameEvent playerPowerdown;
    public PowerUpEvent OnPlayerGetPowerup;
    public KeyCodeEvent onPlayerConsumePowerup;


    // Start is called before the first frame update
  
    private void OnEnable() {
        Application.targetFrameRate =  30;
        feetPos = gameObject.transform.GetChild(0);
        marioBody = GetComponent<Rigidbody2D>();
        marioSpriteRenderer = GetComponent<SpriteRenderer>();
        marioBoxCollider = GetComponent<BoxCollider2D>();
        marioAnimator = GetComponent<Animator>();
        hitBox = gameObject.transform.GetChild(1).GetComponent<BoxCollider2D>();
        hurtBox = gameObject.transform.GetChild(2).GetComponent<BoxCollider2D>();
        Physics2D.queriesStartInColliders = true;
        marioParticles = GetComponent<ParticleSystem>();
        isAlive = true;
        preventMovement = false;
        marioAnimator.SetBool("Alive", true);
        marioBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        marioBoxCollider.isTrigger = false;
    }

    // Update is called once per frame

    void Update(){
        if(isAlive){
            if (Input.GetKeyDown(KeyCode.A) && faceRightState){
                faceRightState = false;
                marioSpriteRenderer.flipX = true;
            }
            if (Input.GetKeyDown(KeyCode.D) && !faceRightState){
                faceRightState = true;
                marioSpriteRenderer.flipX = false;
            }
            // Jumping Mechanic
            if(Input.GetKeyUp(KeyCode.Space)){
                isJumping = false;
            }
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping && !preventMovement){
                playerJump.Raise();
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

            if (Input.GetKeyDown(KeyCode.Q)){
	            onPlayerConsumePowerup.Raise(KeyCode.Q);
            }

            if (Input.GetKeyDown(KeyCode.E)){
	            onPlayerConsumePowerup.Raise(KeyCode.E);
            }

            if(marioBody.velocity.x < 0.05f && marioBody.velocity.x > -0.05f && isGrounded){
                marioAnimator.SetBool("Running", false);
            }else if(marioBody.velocity.x < 1f && marioBody.velocity.x > -1f && isGrounded){
                marioAnimator.SetBool("Running", false);
            }

            if(Mathf.Abs(marioBody.velocity.x) > 0.1f && isGrounded == true){
                marioAnimator.SetBool("Running", true);
            }
            if(isGrounded){
                marioAnimator.SetBool("Jumping", false);
            } 
            else{
                if(marioParticles.isPlaying){
                    marioParticles.Stop();
                }
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
        if(touchedPole && isGrounded && preventMovement &&!touchedEnding){
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
        //playStompSound();
        marioBody.AddForce(Vector2.up * bounceSpeed,  ForceMode2D.Impulse);
    }

    private void OnDeath(){
        Debug.Log("I died");
        isAlive = false;
        onPlayerDeath.Raise();
        //playDieSound();
        marioBoxCollider.isTrigger = true;
        marioBody.constraints = RigidbodyConstraints2D.FreezePositionX;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y,transform.rotation.z);
        marioBody.AddForce(Vector2.up * deathBoost,  ForceMode2D.Impulse);
    
    }
    

    private void GrowControl(bool g){
        if(g){
                Vector2 prevVel = marioBody.velocity;
                transform.position= new Vector2(transform.position.x,transform.position.y + 0.5f);
                marioBoxCollider.offset = new Vector2(marioBoxCollider.offset.x,-0.15f);
                marioBoxCollider.size = new Vector2(marioBoxCollider.size.x,1.8f);
                hitBox.offset = new Vector2(hitBox.offset.x, hurtBox.offset.y*2+0.1f);
                hurtBox.size = new Vector2(hurtBox.size.x, hurtBox.size.y*2+0.1f);
                StartCoroutine(growing());
                marioAnimator.SetBool("Growth", true);
                marioBody.velocity = prevVel;
            }else{
                Vector2 prevVel = marioBody.velocity;
                marioBoxCollider.size = new Vector2(marioBoxCollider.size.x,0.9f);
                marioBoxCollider.offset = new Vector2(marioBoxCollider.offset.x,-0.05f);
                marioAnimator.SetBool("ChangingSize", false);
                hitBox.offset = new Vector2(hitBox.offset.x, (hurtBox.offset.y-0.1f)/2);
                hurtBox.size = new Vector2(hurtBox.size.x, (hurtBox.size.y-0.1f)/2);
                StartCoroutine(hurting());
                marioAnimator.SetBool("Growth", false);
                transform.position= new Vector2(transform.position.x,transform.position.y -0.5f);
                marioBody.velocity = prevVel;
            }
            //playPowerSound(g);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        //Debug.Log("Collision: " + other.gameObject.tag);
        switch(other.gameObject.tag){
            case "Pole":
                touchedPole = true;
                onPlayerTouchedPole.Raise();
                //playPoleSound();
                StartCoroutine(slideDown());
                break;
            case "Ground":
                marioParticles.Play();
                break;
            case "Foreground":
                marioParticles.Play();
                break;
            case "Bricks":
                if(grow.Value){
                    //Debug.Log("Bricks:" + other.gameObject.name);
                    other.gameObject.GetComponent<BrickEventManager>().onKill();
                }
                break;  
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Trigger: " + other.gameObject.tag);
        switch (other.gameObject.tag)
        {
            case "HitBox":
                
                if(isAlive && other.gameObject.activeSelf){
                    if(grow.Value){
                        playerPowerdown.Raise();
                        grow.Variable.SetValue(false);
                        GrowControl(false);  
                    }else if(!hurtRecently){
                        Debug.Log(other.gameObject.transform.parent.transform.position);
                        isAlive = false;
                        marioAnimator.SetBool("Alive", false);
                        OnDeath();
                    }
                }
                
                break;
            case "HurtBox":
                if(isAlive && IsDescending() && !hurtRecently && other.gameObject.activeSelf){
                    other.transform.parent.GetComponent<EnemyController>().onDeath();
                    onPlayerKill.Raise();
                    BounceAfterKill();
                }
                break;
            case "Grow":
                OnPlayerGetPowerup.Raise(other.GetComponent<Mushroom>().powerUp);
                
                Destroy(other.gameObject);
                break;
            case "JumpBoost":
                OnPlayerGetPowerup.Raise(other.GetComponent<Mushroom>().powerUp);
                Destroy(other.gameObject);
                break;
            case "Destroy":
                if(isAlive){
                    onPlayerDeath.Raise();
                }
                isAlive = false;
                Destroy(gameObject,0.25f);
                break;
            case "End":
                touchedEnding = true;
                onPlayerTouchedEnding.Raise();
                gameObject.SetActive(false);
                break;
        }
        
    }
    public void UsePowerUp(int i){
        switch(i){
            case 0:
                if(!boosting && isAlive){
                    StartCoroutine(jumpBooster());
                }
                break;
            case 1:
                if(!grow.Value && isAlive){
                    grow.Variable.SetValue(true);
                    GrowControl(true);
                    onPlayerGrow.Raise();
                    
                }
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
    IEnumerator jumpBooster(){
        boosting = true;
        upBoost = 1f;
        upSpeed = 11f;
        yield return new WaitForSeconds(5f);
        boosting = false;
        upBoost = 0.7f;
        upSpeed = 9f;
    }

    
    void ResetSize(){
        marioBoxCollider.size = new Vector2(marioBoxCollider.size.x,0.9f);
        marioBoxCollider.offset = new Vector2(marioBoxCollider.offset.x,-0.05f);
        marioAnimator.SetBool("ChangingSize", false);
    }
}
