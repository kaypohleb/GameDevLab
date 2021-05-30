using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
public class GameController : MonoBehaviour
{
    [SerializeField] Text ScoreText;
    [SerializeField] Text TimeText;
    [SerializeField] Text LifeText;
    [SerializeField] GameObject PauseScreen;
    [SerializeField] GameObject MarioPreFab;
    MarioController MarioControl;
    GameObject Mario;
    public Collider2D PoleCollider;
    Text PauseText;
    CinemachineVirtualCamera cm;
    public bool revived = false;
    public bool isPaused;
    public bool hasStarted;
    public bool touchedPole;
    public bool gameEnded = false;
    private DDOL gameState;

    void Awake()
    {
        Time.timeScale = 0f;
        isPaused = true;
        hasStarted = false;
        gameEnded = false;
        gameState = GameObject.FindGameObjectWithTag("DDOL").GetComponent<DDOL>();
        cm = GameObject.FindGameObjectWithTag("VCamera").GetComponent<CinemachineVirtualCamera>();
        Mario = Instantiate(MarioPreFab, new  Vector3(0.5f,1.5f, this.transform.position.z), Quaternion.identity);
        MarioControl = Mario.GetComponent<MarioController>();
        cm.Follow = Mario.transform;
        PauseText = PauseScreen.GetComponentsInChildren<Text>()[0];
        PoleCollider = GameObject.FindGameObjectWithTag("Pole").GetComponent<Collider2D>();
    }
    // Update is called once per frame
    void Update()
    {
      LifeText.text = gameState.lifeCount.ToString();
      ScoreText.text = gameState.scoreCount.ToString();
      TimeText.text = Mathf.RoundToInt(gameState.remainingTime).ToString();
      if(!MarioControl.touchedPole){
        gameState.updateTime(Time.fixedDeltaTime);
      }else if(!gameEnded){
        PoleCollider.isTrigger = true;
      }
      
      if(!hasStarted && isPaused){
        PauseScreen.SetActive(true);
        PauseText.text = "Press any key to start";
        Time.timeScale = 0f;
      }else if(hasStarted && isPaused){
        PauseScreen.SetActive(true);
        PauseText.text = "Press any key to resume";
        Time.timeScale = 0f;
      }else if(!isPaused){
        PauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
      }
      if(Input.anyKeyDown && isPaused){
        isPaused = false;
        hasStarted = true;
      }  
      if(Input.GetKeyDown(KeyCode.Escape) && !isPaused){
        Debug.Log("P is pressed");
        isPaused = true;
      }
      if(gameState.lifeCount <=0 || gameState.remainingTime <=0 || MarioControl.touchedEnding){
          EndGame();
      }
      if(!Mario && !revived){
        StartCoroutine(Respawn());
      }
      if(Mario){
        revived = false;
      }
    }
    void EndGame(){
      gameEnded = true;
      SceneManager.LoadScene(1);
      
    }
    IEnumerator Respawn(){
      revived = true;
      gameState.KillLife(1);
      yield return new WaitForSeconds(2f);
      SceneManager.LoadScene(0);
      yield return null;
    }
}
