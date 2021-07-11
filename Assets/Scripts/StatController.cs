using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StatController : MonoBehaviour
{
    public GameConstants gameConstants;
    public IntReference scoreCount;
    public IntReference lifeCount;
    public FloatReference remainingTime;
    public IntReference sceneNumber;
    public BoolReference gameOver;
    public BoolReference grow;

    public void nextStage(){
        if(sceneNumber.Value<SceneManager.sceneCountInBuildSettings-1){
            sceneNumber.Variable.ApplyChange(1);
        }else{
            sceneNumber.Variable.SetValue(0);
        }
        
        
    }

    private void Awake() {
        if(gameOver.Value){
            gameOver.Variable.SetValue(false);
            sceneNumber.Variable.SetValue(SceneManager.GetActiveScene().buildIndex);
            Reset();
        }
       
    }

    public  void increaseKillScore(){
		addScore(gameConstants.killScore);
	}
    public  void increaseGrowScore(){
		addScore(gameConstants.growScore);
	}
    public void Reset(){
        remainingTime.Variable.SetValue(gameConstants.startingTime);
        lifeCount.Variable.SetValue(gameConstants.startingLives);
        scoreCount.Variable.SetValue(gameConstants.startingScore);
    }
    public void updateTime(float rtime){
        remainingTime.Variable.ApplyChange(-rtime);
    }
    public void KillLife(){
        lifeCount.Variable.SetValue(lifeCount.Value-1);
    }
    public void addScore(int score){
        scoreCount.Variable.ApplyChange(score);
    }

}
