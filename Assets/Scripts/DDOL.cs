using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    public int startingLives = 3;
    public float startingTime = 300f;
    public int startingScore = 0;
    public int scoreCount;
    public float remainingTime;
    public int lifeCount;
    static DDOL instance;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        }else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        remainingTime = startingTime;
        lifeCount = startingLives;
        scoreCount = startingScore;
    }
    public void Reset(){
        remainingTime = startingTime;
        lifeCount = startingLives;
        scoreCount = startingScore;
    }
    public void updateTime(float rtime){
        remainingTime-=rtime;
    }
    public void KillLife(int life){
        lifeCount--;
    }
    public void addScore(int score){
        scoreCount+=score;
    }
}