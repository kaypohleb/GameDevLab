using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this has methods callable by players
public  class CentralManager : MonoBehaviour
{
	
	public GameConstants gameConstants;
    public IntReference scoreCount;
    public FloatReference remainingTime;
	public  GameObject gameManagerObject;
	private  GameController gameManager;
	bool touchedPole;
	private void Start() {
		touchedPole = false;
		gameManager  = FindObjectOfType<GameController>();
    }
	private void Update() {
		if(!touchedPole){
			remainingTime.Variable.ApplyChange(-Time.deltaTime);
		}
	}
	public void touchPole(){
		touchedPole = true;
	}
    public void TimetoScore(){
        Debug.Log("TOS called");
        StartCoroutine(TimetoScoreRoller());
    }
    IEnumerator TimetoScoreRoller(){
       while(Mathf.RoundToInt(remainingTime.Value)>0){
            remainingTime.Variable.ApplyChange(-1);
            scoreCount.Variable.ApplyChange(1);
            yield return new WaitForEndOfFrame();
        }
    }
}