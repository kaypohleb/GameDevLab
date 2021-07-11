using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameConstants gameConstants;
    public IntReference sceneNumber;
    public IntReference scoreCount;
    public FloatReference remainingTime;
    public IntReference lifeCount;
    public BoolReference gameOver;
    public void PlayGame(){
        gameOver.Variable.SetValue(true);
        remainingTime.Variable.SetValue(gameConstants.startingTime);
        lifeCount.Variable.SetValue(gameConstants.startingLives);
        scoreCount.Variable.SetValue(gameConstants.startingScore);
        sceneNumber.Variable.SetValue(1);
        SceneManager.LoadScene(1);
    }
}
