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
    AudioManager audioManager;
    GameObject Mario;
    CinemachineVirtualCamera cm;
    public Collider2D PoleCollider;
    Text PauseText;
    public bool revived = false;
    public bool isPaused;
    public bool hasStarted;
    public bool stageEnded = false;
    public FloatReference remainingTime;
    public IntReference scoreCount;
    public IntReference lifeCount;
	  public IntReference sceneNumber;

    void Awake()
    {
        Time.timeScale = 0f;
        isPaused = true;
        hasStarted = false;
        stageEnded = false;
        audioManager = FindObjectOfType<AudioManager>();
        Mario = Instantiate(MarioPreFab, MarioPreFab.transform.position, Quaternion.identity);
        MarioControl = Mario.GetComponent<MarioController>();
        cm = FindObjectOfType<CinemachineVirtualCamera>();
        cm.Follow = Mario.transform;
        PauseText = PauseScreen.GetComponentsInChildren<Text>()[0];
        PoleCollider = GameObject.FindGameObjectWithTag("Pole").GetComponent<Collider2D>();
    }
    // Update is called once per frame
    void Update()
    {
      
      LifeText.text = lifeCount.Value.ToString();
      ScoreText.text = scoreCount.Value.ToString();
      TimeText.text = Mathf.RoundToInt(remainingTime.Value).ToString();
      if(remainingTime.Value <=0){
        stageEnded = true;
        Debug.Log("gameEnded");
        StartCoroutine(EndGame());
      }
      if(!hasStarted && isPaused){
        PauseScreen.SetActive(true);
        PauseText.text = "Press any key to start";
        Time.timeScale = 0f;
      }else if(hasStarted && isPaused){
        PauseScreen.SetActive(true);
        PauseText.text = "Press any key to resume";
        Time.timeScale = 0f;
        audioManager.playThemeSound(false);
      }else if(!isPaused){
        PauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
        if(MarioControl.isAlive && !stageEnded){
          audioManager.playThemeSound(true);
        }        
      }
      if(Input.anyKeyDown && isPaused){
        isPaused = false;
        hasStarted = true;
      }  
      if(Input.GetKeyDown(KeyCode.Escape) && !isPaused){
        Debug.Log("P is pressed");
        isPaused = true;
      }
      if(Mario){
        revived = false;
      }
    }
    IEnumerator Respawn(){
      revived = true;
      yield return new WaitForSeconds(2f);
      SceneManager.LoadScene(sceneNumber.Value);
      yield return null;
    }
    IEnumerator EndGame(){
      Mario.gameObject.SetActive(false);
      yield return new WaitForSeconds(2f);
      SceneManager.LoadScene(0);
    }
    public void ClearStage(){
      PoleCollider.isTrigger = true;
      audioManager.PlayClearSound();
    }
    IEnumerator NextLevel(){
      Mario.gameObject.SetActive(false);
      yield return new WaitForSeconds(3f);
      SceneManager.LoadScene(sceneNumber.Value);
    }

    public void EndTheGame(){
      stageEnded = true;
      Debug.Log(lifeCount.Value);
      if(lifeCount.Value>1){
        StartCoroutine(Respawn());
      }else{
        Debug.Log("gameEnded");
        StartCoroutine(EndGame());
      }
    }

    public void toNextStage(){
      stageEnded = true;
      StartCoroutine(NextLevel());
    }
    

}
